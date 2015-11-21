// *****************************************************
// CIXReader
// DirectoryView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/06/2015 10:05
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Controls;
using CIXReader.Forms;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.SubViews
{
    public sealed partial class DirectoryView : ViewBaseView
    {
        private string _currentFilterString;
        private List<DirForum> _items;
        private readonly ImageList _imageList = new ImageList();
        private CategoryFolder _currentCategory;

        private const int _widthNameColumn = 125;
        private const int _widthPopularityColumn = 100;
        private const int _widthSubCategoryColumn = 125;

        public DirectoryView(FoldersTree foldersTree) : base("Directory")
        {
            InitializeComponent();

            FoldersTree = foldersTree;
            SortOrderBase.DefaultSortOrder = CRSortOrder.SortOrder.Popularity;
            SortOrderBase.Ascending = false;

            _imageList.Images.Add(Resources.ReadLock);

            dvForumsList.SmallImageList = _imageList;
        }

        /// <summary>
        /// Display the directory for the specified CategoryFolder
        /// </summary>
        public override bool ViewFromFolder(FolderBase folder, Address address, FolderOptions flags)
        {
            CategoryFolder category = folder as CategoryFolder;
            if (category != null)
            {
                if (flags.HasFlag(FolderOptions.ClearFilter))
                {
                    _currentFilterString = null;
                }
                _currentCategory = category;
                _items = ItemsForView();
                SortItems();
            }
            return true;
        }

        /// <summary>
        /// Display a directory of forums filtered by the given search string. If the search string is
        /// empty then all forums are displayed.
        /// </summary>
        /// <param name="searchString">A search string</param>
        public override void FilterViewByString(string searchString)
        {
            string lowerCaseSearchString = searchString.Trim().ToLower();
            if (lowerCaseSearchString != _currentFilterString)
            {
                _items = ItemsForView().Select(frm => frm).Where(frm =>
                            frm.Name.ToLower().Contains(lowerCaseSearchString) ||
                            frm.Title.ToLower().Contains(lowerCaseSearchString)).ToList();
                SortItems();
                _currentFilterString = lowerCaseSearchString;
            }
        }

        /// <summary>
        /// Return the sort menu for this view.
        /// </summary>
        public override ContextMenuStrip SortMenu
        {
            get { return FoldersTree.MainForm.DirectorySortMenu; }
        }

        /// <summary>
        /// Indicate that we handle the cixmailbox scheme.
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public override bool Handles(string scheme)
        {
            return scheme == "cixdirectory";
        }

        /// <summary>
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public override string Address
        {
            get
            {
                return _currentCategory.Address;
            }
        }

        /// <summary>
        /// Return whether or not we can process the specified action ID.
        /// </summary>
        public override bool CanAction(ActionID id)
        {
            switch (id)
            {
                case ActionID.JoinForum:
                case ActionID.Refresh:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Process the specified Action ID.
        /// </summary>
        public override void Action(ActionID id)
        {
            switch (id)
            {
                case ActionID.JoinForum:
                    {
                        ListView.SelectedIndexCollection selectedItems = dvForumsList.SelectedIndices;
                        if (selectedItems.Count == 1)
                        {
                            DirForum selectedForum = _items[selectedItems[0]];
                            string forumName = selectedForum.Name;

                            JoinForum joinForum = new JoinForum(forumName);
                            if (joinForum.ShowDialog() == DialogResult.OK)
                            {
                                FoldersTree.MainForm.Address = string.Format("cix:{0}", forumName);
                            }
                        }
                        break;
                    }

                case ActionID.Refresh:
                    _currentCategory.Refresh();
                    break;
            }
        }

        /// <summary>
        /// Called when the directory view is first loaded.
        /// </summary>
        private void DirectoryView_Load(object sender, EventArgs e)
        {
            UI.ThemeChanged += OnThemeChanged;

            CIX.DirectoryCollection.DirectoryChanged += OnDirectoryChanged;

            SortOrderBase.OrderingChanged += OnOrderingChanged;
        }

        /// <summary>
        /// Handle theme changed events to change the directory view theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            dvForumsList.Invalidate();
        }

        /// <summary>
        /// Callback from the directory task update when new directory data has been loaded
        /// from the server. In this case we're just interested in updating the progress bar until
        /// we get to 100% at which point we stop and update the UI.
        /// </summary>
        private void OnDirectoryChanged(object sender, DirectoryEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (args.CategoryName != null && args.CategoryName == _currentCategory.Name)
                {
                    _items = new List<DirForum>(CIX.DirectoryCollection.AllForumsInCategory(_currentCategory.Name));
                    SortItems();
                }
            });
        }

        /// <summary>
        /// Return the collection of items for this view.
        /// </summary>
        private List<DirForum> ItemsForView()
        {
            return new List<DirForum>(_currentCategory.Name == CategoryFolder.AllCategoriesName
                    ? CIX.DirectoryCollection.Forums
                    : CIX.DirectoryCollection.AllForumsInCategory(_currentCategory.Name));
        }

        // Set the column widths so that the title column adjusts based on the window width and
        // the other columns remain fixed.
        private void UpdateColumnWidths()
        {
            dvForumsList.Columns[0].Width = _widthNameColumn;
            dvForumsList.Columns[1].Width = _widthPopularityColumn;
            dvForumsList.Columns[2].Width = dvForumsList.Width - (_widthNameColumn + _widthPopularityColumn + _widthSubCategoryColumn);
            dvForumsList.Columns[3].Width = _widthSubCategoryColumn;
        }

        /// <summary>
        /// Get or set the associated folders tree.
        /// </summary>
        private FoldersTree FoldersTree { get; set; }

        /// <summary>
        /// Sort the items in the list by the current sort series
        /// </summary>
        private void SortItems()
        {
            int directionIndex = SortOrderBase.Ascending ? 1 : -1;
            SortOrder sortOrder = SortOrderBase.Ascending ? SortOrder.Ascending : SortOrder.Descending;
            switch (SortOrderBase.Order)
            {
                case CRSortOrder.SortOrder.Popularity:
                    _items.Sort((p1, p2) => (p1.Recent - p2.Recent) * directionIndex);
                    dvForumsList.SetSortIcon(1, sortOrder);
                    break;

                case CRSortOrder.SortOrder.Name:
                    _items.Sort((p1, p2) => (String.Compare(p1.Name, p2.Name, StringComparison.Ordinal)) * directionIndex);
                    dvForumsList.SetSortIcon(0, sortOrder);
                    break;

                case CRSortOrder.SortOrder.Title:
                    _items.Sort((p1, p2) => (String.Compare(p1.Title, p2.Title, StringComparison.Ordinal)) * directionIndex);
                    dvForumsList.SetSortIcon(2, sortOrder);
                    break;

                case CRSortOrder.SortOrder.SubCategory:
                    _items.Sort((p1, p2) => (String.Compare(p1.Sub, p2.Sub, StringComparison.Ordinal)) * directionIndex);
                    dvForumsList.SetSortIcon(3, sortOrder);
                    break;
            }
            dvForumsList.VirtualListSize = _items.Count();
            if (_items.Any())
            {
                dvForumsList.RedrawItems(0, _items.Count - 1, false);
            }
        }

        private void dvForumsList_Resize(object sender, EventArgs e)
        {
            UpdateColumnWidths();
        }

        private void dvForumsList_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            DirForum forum = _items[e.ItemIndex];
            e.Item = new ListViewItem(forum.Name);
            if (forum.IsClosed)
            {
                e.Item.ImageIndex = 0;
            }

            int popularityValue = Math.Min((forum.Recent + 100) / 100, 5);
            string popularity = string.Empty;

            switch (popularityValue)
            {
                case 1: popularity = "★"; break;
                case 2: popularity = "★★"; break;
                case 3: popularity = "★★★"; break;
                case 4: popularity = "★★★★"; break;
                case 5: popularity = "★★★★★"; break;
            }

            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, popularity));
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, forum.Title));
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, forum.Sub));
        }

        // Handle a click on the column to sort by that column. A click on the column which
        // is currently sorted just reverses the order of the sort.
        private void dvForumsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            CRSortOrder.SortOrder newOrder;
            switch (e.Column)
            {
                case 0:
                    newOrder = CRSortOrder.SortOrder.Name;
                    break;

                case 1:
                    newOrder = CRSortOrder.SortOrder.Popularity;
                    break;

                case 2:
                    newOrder = CRSortOrder.SortOrder.Title;
                    break;

                case 3:
                    newOrder = CRSortOrder.SortOrder.SubCategory;
                    break;

                default:
                    throw new NotImplementedException();
            }
            if (SortOrderBase.Order == newOrder)
            {
                SortOrderBase.Ascending = !SortOrderBase.Ascending;
            }
            else
            {
                SortOrderBase.Order = newOrder;
            }
        }

        private void OnOrderingChanged(object sender, EventArgs eventArgs)
        {
            SortItems();
        }

        /// <summary>
        /// Handle double-click to do a Join
        /// </summary>
        private void dvForumsList_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection selectedItems = dvForumsList.SelectedIndices;
            if (selectedItems.Count == 1)
            {
                DirForum selectedForum = _items[selectedItems[0]];
                string forumName = selectedForum.Name;
                FoldersTree.MainForm.Address = string.Format("cix:{0}", forumName);
            }
        }

        // Don't let the user change the column size.
        private void dvForumsList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (dvForumsList.Columns[e.ColumnIndex].Width == 0)
            {
                e.Cancel = true;
                e.NewWidth = dvForumsList.Columns[e.ColumnIndex].Width;
            }
        }
    }
}