// *****************************************************
// CIXReader
// SubViewBase.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/10/2013 2:25 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Windows.Forms;
using CIXReader.CanvasItems;
using CIXReader.Controls;
using CIXReader.SpecialFolders;
using CIXReader.Utilities;

namespace CIXReader.SubViews
{
    /// <summary>
    /// The SubViewBase class implements the base functionality for a subview.
    /// </summary>
    public partial class ViewBaseView : UserControl
    {
        private readonly CRSortOrder _ordering;

        /// <summary>
        /// Initialises a new instance of the <see cref="ViewBaseView"/> class.
        /// </summary>
        protected ViewBaseView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ViewBaseView"/> class with
        /// the specified view name.
        /// </summary>
        protected ViewBaseView(string name)
        {
            _ordering = new CRSortOrder(name);
            InitializeComponent();
        }

        /// <summary>
        /// Display this view with the specified folder and options
        /// </summary>
        public virtual bool ViewFromFolder(FolderBase folder, Address address, FolderOptions flags)
        {
            return true;
        }

        /// <summary>
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Return the MessageItem for the actual displayed message. This must always be
        /// overridden.
        /// </summary>
        public virtual MessageItem DisplayedItem
        {
            get { return null; }
        }

        /// <summary>
        /// Get or set the ordering of messages within the view
        /// </summary>
        public CRSortOrder SortOrderBase
        {
            get { return _ordering; }
        }

        /// <summary>
        /// Return the sort menu for this view.
        /// </summary>
        public virtual ContextMenuStrip SortMenu
        {
            get { return null; }
        }

        public virtual void SetFocus() {}

        /// <summary>
        /// Override to return whether the subview can handle the specified action.
        /// </summary>
        /// <param name="id">The ID of the action</param>
        public virtual bool CanAction(ActionID id)
        {
            return false;
        }

        /// <summary>
        /// Override to return the title for the specified action.
        /// </summary>
        public virtual string TitleForAction(ActionID id) { return null; }

        /// <summary>
        /// Override to handle the specified action.
        /// </summary>
        /// <param name="id">The ID of the action</param>
        public virtual void Action(ActionID id) {}

        /// <summary>
        /// Override to handle the search filter string
        /// </summary>
        /// <param name="searchString"></param>
        public virtual void FilterViewByString(string searchString)
        {
        }

        /// <summary>
        /// Override to report whether the view handles the given scheme.
        /// </summary>
        /// <param name="scheme">A scheme name</param>
        /// <returns>Overrides should return true if they handle the scheme, false otherwise</returns>
        public virtual bool Handles(string scheme)
        {
            return false;
        }
    }
}