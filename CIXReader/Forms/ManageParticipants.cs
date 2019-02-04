// *****************************************************
// CIXReader
// ManageParticipants.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/02/2015 10:35
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
    public sealed partial class ManageParticipants : UserForumEditor
    {
        private readonly DirForum _forum;

        public ManageParticipants(DirForum forum)
        {
            InitializeComponent();
            _forum = forum;
        }

        /// <summary>
        /// Called when the Manage Forum dialog is closed.
        /// </summary>
        public void CloseView()
        {
            _forum.RemovedParticipants = RemoveList;
            _forum.AddedParticipants = AddList;
        }

        protected override ListView ListControl { get { return forumParList; } }

        protected override Control RemoveButton { get { return forumRemovePart; } }

        private void ManageParticipants_Load(object sender, System.EventArgs e)
        {
            CIX.DirectoryCollection.ParticipantsUpdated += OnParticipantsUpdated;

            forumParList.RetrieveVirtualItem += OnRetrieveVirtualItem;
            forumParList.SelectedIndexChanged += OnSelectedIndexChanged;
            forumAddPart.Click += OnAddButtonClicked;
            forumRemovePart.Click += OnRemoveButtonClicked;

            UserList = _forum.Participants();
            AddList = new List<string>(_forum.AddedParticipants);
            RemoveList = new List<string>(_forum.RemovedParticipants);
            UpdateList();

            forumRemovePart.Enabled = false;
        }

        private void OnParticipantsUpdated(object sender, DirForum forum)
        {
            Platform.UIThread(this, delegate
            {
                UserList = _forum.Participants();
                AddList = new List<string>(_forum.AddedParticipants);
                RemoveList = new List<string>(_forum.RemovedParticipants);
                UpdateList();
            });
        }
    }
}