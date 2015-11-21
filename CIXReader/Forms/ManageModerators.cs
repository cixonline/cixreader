// *****************************************************
// CIXReader
// ManageModerators.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/02/2015 11:32
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public partial class ManageModerators : UserForumEditor
    {
        private readonly DirForum _forum;

        public ManageModerators(DirForum forum)
        {
            InitializeComponent();
            _forum = forum;
        }

        /// <summary>
        /// Called when the Manage Forum dialog is closed.
        /// </summary>
        public void CloseView()
        {
            _forum.RemovedModerators = RemoveList;
            _forum.AddedModerators = AddList;
        }

        protected override ListView ListControl { get { return forumModList; } }

        protected override Control RemoveButton { get { return forumRemoveMod; } }

        private void ManageModerators_Load(object sender, System.EventArgs e)
        {
            CIX.DirectoryCollection.ModeratorsUpdated += OnModeratorsUpdated;

            forumModList.RetrieveVirtualItem += OnRetrieveVirtualItem;
            forumModList.SelectedIndexChanged += OnSelectedIndexChanged;
            forumAddMod.Click += OnAddButtonClicked;
            forumRemoveMod.Click += OnRemoveButtonClicked;

            ShowMugshots = true;
            UserList = _forum.Moderators();
            AddList = new List<string>(_forum.AddedModerators);
            RemoveList = new List<string>(_forum.RemovedModerators);
            UpdateList();

            forumRemoveMod.Enabled = false;
        }

        private void OnModeratorsUpdated(DirForum forum)
        {
            Platform.UIThread(this, delegate
            {
                UserList = _forum.Moderators();
                AddList = new List<string>(_forum.AddedModerators);
                RemoveList = new List<string>(_forum.RemovedModerators);
                UpdateList();
            });
        }
    }
}