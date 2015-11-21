// *****************************************************
// CIXReader
// JoinForum.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/02/2015 13:00
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class JoinForum : Form
    {
        private readonly string _forumName;
        private bool isResizing;
        private DirForum _forum;

        public JoinForum(string forumName)
        {
            InitializeComponent();
            _forumName = forumName;
        }

        private void JoinForum_Load(object sender, EventArgs e)
        {
            CIX.DirectoryCollection.ForumUpdated += OnForumUpdated;

            statusField.Text = Resources.JoinForumStatusText;

            _forum = CIX.DirectoryCollection.ForumByName(_forumName);
            if (_forum != null && !string.IsNullOrEmpty(_forum.Desc))
            {
                RefreshForum(_forum);
            }
            else
            {
                if (_forum != null)
                {
                    forumTitle.Text = _forum.Title;
                }
                forumName.Text = _forumName;
                forumImage.Image = CategoryFolder.IconForCategory("CIX");
                CIX.DirectoryCollection.RefreshForum(_forumName);
            }
            ResizeForm();
        }

        /// <summary>
        /// Callback after the forum details are refreshed from the server, whether or not they
        /// succeed. The e.Forum parameter is null if the forum doesn't exist.
        /// </summary>
        private void OnForumUpdated(DirForum forum)
        {
            Platform.UIThread(this, delegate
            {
                _forum = forum;
                if (_forum != null)
                {
                    RefreshForum(_forum);
                }
                else
                {
                    forumName.Text = _forumName;
                    forumImage.Image = CategoryFolder.IconForCategory("CIX");
                    statusField.Text = Resources.JoinNoSuchForum;
                    cancelButton.Text = Resources.Close;
                    joinButton.Visible = false;
                    joinButton.Enabled = false;

                    ResizeForm();
                }
            });
        }

        private void RefreshForum(DirForum forum)
        {
            if (forum != null)
            {
                forumName.Text = forum.Name;
                forumImage.Image = CategoryFolder.IconForCategory(forum.Cat);
                forumTitle.Text = forum.Title;

                string descText = string.Format("<font face=\"Arial\" size=\"11px\"><div style=\"color:rgb({0});\">{1}</div></font>", UI.ToString(SystemColors.ControlText), forum.Desc);
                descText = descText.Replace("\n", "<br/>");
                forumDescription.Text = descText;

                if (forum.IsClosed)
                {
                    statusField.Text = Resources.JoinClosedForum;
                    joinButton.Text = Resources.RequestAdmittance;
                }

                ResizeForm();
            }
        }

        /// <summary>
        /// User clicked the Join button.
        /// </summary>
        private void joinButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            if (_forum != null)
            {
                if (_forum.IsClosed)
                {
                    _forum.RequestAdmittance();
                }
                else
                {
                    _forum.Join();
                    DialogResult = DialogResult.OK;
                }
            }
            CIX.DirectoryCollection.ForumUpdated -= OnForumUpdated;
            Close();
        }

        /// <summary>
        /// User clicked the Cancel button.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            CIX.DirectoryCollection.ForumUpdated -= OnForumUpdated;
            Close();
        }

        private void crPanel1_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        /// <summary>
        /// Resize the form based on the forum title, description and status text. FlowLayoutControl
        /// does a pretty awkward job of this so the manual approach feels easier and safer.
        /// </summary>
        private void ResizeForm()
        {
            if (!isResizing)
            {
                isResizing = true;

                SuspendLayout();

                const int margin = 10;

                SizeF nameSize = CreateGraphics().MeasureString(forumName.Text, forumName.Font, forumPanel.Width - (forumName.Left + margin));
                forumName.Top = margin;
                forumName.Width = (int)nameSize.Width + margin;
                forumName.Height = (int) nameSize.Height;

                SizeF titleSize = CreateGraphics().MeasureString(forumTitle.Text, forumTitle.Font, forumPanel.Width - (forumTitle.Left + margin));
                forumTitle.Top = (int)(forumName.Top + nameSize.Height + margin);
                forumTitle.Width = (int) titleSize.Width + 10;
                forumTitle.Height = (int) titleSize.Height;

                SizeF descSize = CreateGraphics().MeasureString(forumDescription.Text, forumDescription.Font, forumPanel.Width - (forumDescription.Left + margin));
                forumDescription.Top = (int)(forumTitle.Top + titleSize.Height + margin);
                forumDescription.Width = (int) descSize.Width;
                forumDescription.Height = (int) descSize.Height;

                forumLine.Top = forumDescription.Bottom + margin;
                forumLine.Width = (forumPanel.Width - forumLine.Left) - margin;

                SizeF statusSize = CreateGraphics().MeasureString(statusField.Text, statusField.Font, forumPanel.Width - statusField.Left);
                statusField.Top = forumLine.Bottom + margin + 2;
                statusField.Width = (int)statusSize.Width + margin;
                statusField.Height = (int)statusSize.Height;

                int forumPanelHeight = statusField.Bottom + (margin * 2);
                forumPanel.Height = forumPanelHeight;

                int padding = joinButton.Height/2;

                joinButton.Top = forumPanelHeight + padding/2;
                cancelButton.Top = forumPanelHeight + padding/2;

                Height = forumPanelHeight + joinButton.Height + (2 * padding) + SystemInformation.CaptionHeight + SystemInformation.BorderSize.Height * 2;
                Top = (Screen.PrimaryScreen.Bounds.Height - Height) / 2;
                ResumeLayout(true);

                isResizing = false;
            }
        }
    }
}