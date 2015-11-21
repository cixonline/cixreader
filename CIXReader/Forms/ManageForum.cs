// *****************************************************
// CIXReader
// ManageForum.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 27/02/2015 5:13 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXClient.Tables;

namespace CIXReader.Forms
{
    public sealed partial class ManageForum : Form
    {
        private readonly ManageForumGeneral _general;
        private readonly ManageParticipants _participants;
        private readonly ManageModerators _moderators;
        private readonly DirForum _forum;

        private Form _activeView;
        private Button _activeButton;

        public ManageForum(DirForum forum)
        {
            InitializeComponent();
            _forum = forum;

            _general = new ManageForumGeneral(forum);
            _moderators = new ManageModerators(forum);
            _participants = new ManageParticipants(forum);
        }

        /// <summary>
        /// Display the General tab to start with.
        /// </summary>
        private void ManageForum_Load(object sender, EventArgs e)
        {
            SelectButton(manageGeneral);
            LoadPage(_general);
        }

        /// <summary>
        /// Load and make the specified page active
        /// </summary>
        /// <param name="page">Page to show</param>
        private void LoadPage(Form page)
        {
            if (page != _activeView)
            {
                if (_activeView != null)
                {
                    _activeView.Hide();
                }
                _activeView = page;
                _activeView.TopLevel = false;
                _activeView.Parent = managePanel;
                _activeView.Size = managePanel.Size;
                _activeView.Show();
            }
        }

        /// <summary>
        /// Select the specified tab button.
        /// </summary>
        private void SelectButton(Button tabButton)
        {
            if (tabButton != _activeButton)
            {
                if (_activeButton != null)
                {
                    _activeButton.ForeColor = SystemColors.ControlText;
                    _activeButton.BackColor = manageToolbar.BackColor;
                }
                _activeButton = tabButton;
                _activeButton.BackColor = SystemColors.Highlight;
                _activeButton.ForeColor = SystemColors.HighlightText;
            }
        }

        /// <summary>
        /// Resize the active view when the main form is resized.
        /// </summary>
        private void ManageForum_Resize(object sender, EventArgs e)
        {
            if (_activeView != null)
            {
                _activeView.Size = managePanel.Size;
            }
        }

        /// <summary>
        /// Activate the General tab.
        /// </summary>
        private void manageGeneral_Click(object sender, EventArgs e)
        {
            SelectButton(manageGeneral);
            LoadPage(_general);
        }

        /// <summary>
        /// Activate the Participants tab.
        /// </summary>
        private void manageParts_Click(object sender, EventArgs e)
        {
            SelectButton(manageParts);
            LoadPage(_participants);
        }

        /// <summary>
        /// Activate the Moderators tab.
        /// </summary>
        private void manageMods_Click(object sender, EventArgs e)
        {
            SelectButton(manageMods);
            LoadPage(_moderators);
        }

        /// <summary>
        /// Save button clicked - invoke Save on each tab then close the
        /// form.
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            _general.CloseView();
            _moderators.CloseView();
            _participants.CloseView();
            _forum.Update();
            Close();
        }

        /// <summary>
        /// Cancel button clicked. Close the form without saving changes.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}