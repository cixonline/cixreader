// *****************************************************
// CIXReader
// Participants.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 27/01/2015 15:31
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class Participants : Form
    {
        private readonly MainForm _mainForm;
        private string [] _participants;

        private ImageList _imageList;
        private readonly bool _showMugshots;
        private DirForum _forum;

        public Participants(MainForm mainForm, string forumName)
        {
            InitializeComponent();

            CIX.DirectoryCollection.ForumUpdated += OnForumUpdated;
            CIX.DirectoryCollection.ParticipantsUpdated += OnParticipantsUpdated;

            _forum = CIX.DirectoryCollection.ForumByName(forumName);
            _mainForm = mainForm;
            _showMugshots = false;

            if (_forum == null)
            {
                CIX.DirectoryCollection.RefreshForum(forumName);
            }
        }

        private void Participants_Load(object sender, EventArgs e)
        {
            if (_forum != null)
            {
                LoadList();
            }
        }

        /// <summary>
        /// Callback after the forum details are refreshed from the server, whether or not they
        /// succeed. The e.Forum parameter is null if the forum doesn't exist.
        /// </summary>
        private void OnForumUpdated(object sender, DirForum forum)
        {
            if (forum != null)
            {
                _forum = forum;
                LoadList();
            }
        }

        private void LoadList()
        {
            Text = string.Format(Resources.ParticipantsTitle, _forum.Name);

            _participants = _forum.Participants();

            _imageList = new ImageList();

            parList.SmallImageList = _imageList;
            parList.VirtualListSize = _participants.Length;
            if (_participants.Any())
            {
                parList.RedrawItems(0, _participants.Length - 1, false);
            }

            parCount.Text = string.Format(Resources.ParticipantsCount, _participants.Length);

            UpdateButtons();
        }

        private void OnParticipantsUpdated(object sender, DirForum forum)
        {
            Platform.UIThread(this, delegate
            {
                if (forum == _forum)
                {
                    LoadList();
                }
            });
        }

        private void parList_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            int index = -1;
            string name = _participants[e.ItemIndex];

            if (_showMugshots)
            {
                index = _imageList.Images.IndexOfKey(name);
                if (index < 0)
                {
                    _imageList.Images.Add(name, Mugshot.MugshotForUser(name, false).RealImage);
                }
            }

            e.Item = new ListViewItem("") {ImageIndex = index};
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, name));
        }

        private void parList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        void UpdateButtons()
        {
            parViewProfile.Enabled = parList.SelectedIndices.Count > 0;
        }

        private void parClose_Click(object sender, EventArgs e)
        {
            CIX.DirectoryCollection.ForumUpdated -= OnForumUpdated;
            Close();
        }

        private void parViewProfile_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection selectedItems = parList.SelectedIndices;
            if (selectedItems.Count == 1)
            {
                string profileName = _participants[selectedItems[0]];
                _mainForm.Address = string.Format("cixuser:{0}", profileName);
            }
        }

        // Set the column widths so that the name column adjusts based on the window width and
        // the mugshot column remains fixed.
        private void UpdateColumnWidths()
        {
            int mugshotWidth = _showMugshots ? 24 : 0;
            parList.Columns[0].Width = mugshotWidth;
            parList.Columns[1].Width = parList.Width - mugshotWidth;
        }

        private void parList_Resize(object sender, EventArgs e)
        {
            UpdateColumnWidths();
        }

        private void parList_DoubleClick(object sender, EventArgs e)
        {
            parViewProfile_Click(sender, e);
        }
    }
}