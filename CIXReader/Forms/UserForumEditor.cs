// *****************************************************
// CIXReader
// UserForumEditor.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/02/2015 10:49
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CIXClient.Tables;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    public class UserForumEditor : Form
    {
        private ImageList _imageList;
        private List<string> _fullList; 

        protected string [] UserList;

        protected List<string> AddList;

        protected List<string> RemoveList;

        protected bool ShowMugshots { get; set; }

        protected virtual ListView ListControl { get { return null; } }

        protected virtual Control RemoveButton { get { return null; } }

        /// <summary>
        /// Update the list with the sorted combination of the user list
        /// plus any names added or removed.
        /// </summary>
        protected void UpdateList()
        {
            LoadList();

            _imageList = new ImageList();

            ListControl.SmallImageList = _imageList;
            ListControl.VirtualListSize = _fullList.Count();

            ReloadData();
        }

        protected void LoadList()
        {
            _fullList = new List<string>();
            _fullList.AddRange(UserList);
            foreach (string name in AddList.Where(name => !_fullList.Contains(name)))
            {
                _fullList.Add(name);
            }
            foreach (string name in RemoveList.Where(name => !_fullList.Contains(name)))
            {
                _fullList.Add(name);
            }
            _fullList.Sort();
        }

        /// <summary>
        /// Redraw the list after it has been updated.
        /// </summary>
        protected void ReloadData()
        {
            if (_fullList.Any())
            {
                ListControl.VirtualListSize = _fullList.Count();
                ListControl.RedrawItems(0, _fullList.Count() - 1, false);
            }
        }

        protected void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int index = -1;
            string name = _fullList[e.ItemIndex];

            if (AddList.Contains(name))
            {
                name = name + " (added)";
            }
            if (RemoveList.Contains(name))
            {
                name = name + " (removed)";
            }

            if (ShowMugshots)
            {
                index = _imageList.Images.IndexOfKey(name);
                if (index < 0)
                {
                    _imageList.Images.Add(name, Mugshot.MugshotForUser(name, false).RealImage);
                }
            }

            e.Item = new ListViewItem("") { ImageIndex = index };
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, name));
        }

        /// <summary>
        /// Handle selection changing in the list.
        /// </summary>
        protected void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            UpdateRemoveButton();
        }

        /// <summary>
        /// Handle the Add button
        /// </summary>
        protected void OnAddButtonClicked(object sender, EventArgs e)
        {
            AddUserInput addUser = new AddUserInput();
            if (addUser.ShowDialog() == DialogResult.OK)
            {
                if (!AddList.Contains(addUser.UserName))
                {
                    AddList.Add(addUser.UserName);
                }
                LoadList();
                ReloadData();
                UpdateRemoveButton();

                ListViewItem addedIndex = ListControl.FindItemWithText(addUser.UserName);
                if (addedIndex != null)
                {
                    addedIndex.Selected = true;
                }
            }
        }

        /// <summary>
        /// Handle the Remove button
        /// </summary>
        protected void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection selectedItems = ListControl.SelectedIndices;
            if (selectedItems.Count == 1)
            {
                string name = _fullList[selectedItems[0]];
                if (AddList.Contains(name))
                {
                    AddList.Remove(name);
                }
                else if (RemoveList.Contains(name))
                {
                    RemoveList.Remove(name);
                }
                else
                {
                    RemoveList.Add(name);
                }
                LoadList();
                ReloadData();
                UpdateRemoveButton();
            }
        }

        /// <summary>
        /// Update the state of the Remove button
        /// </summary>
        private void UpdateRemoveButton()
        {
            ListView.SelectedIndexCollection selectedItems = ListControl.SelectedIndices;
            if (selectedItems.Count == 1 && RemoveList.Contains(_fullList[selectedItems[0]]))
            {
                RemoveButton.Text = Resources.RestoreButton;
            }
            else
            {
                RemoveButton.Text = Resources.RemoveButton;
            }
            RemoveButton.Enabled = selectedItems.Count > 0;
        }
    }
}