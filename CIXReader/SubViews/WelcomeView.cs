// *****************************************************
// CIXReader
// WelcomeView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/06/2015 16:17
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Models;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.CanvasItems;
using CIXReader.Forms;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.SubViews
{
    internal sealed partial class WelcomeView : ViewBaseView
    {
        private Whos _onlineUsers = new Whos();
        private List<CIXThread> _threads = new List<CIXThread>();

        private WelcomePage _welcomePage;
 
        public WelcomeView(FoldersTree foldersTree) : base("Welcome")
        {
            FoldersTree = foldersTree;
            InitializeComponent();
        }

        /// <summary>
        /// Display this view with the specified folder and options
        /// </summary>
        public override bool ViewFromFolder(FolderBase folder, Address address, FolderOptions flags)
        {
            if (_welcomePage == null)
            {
                FillCanvas();
            }
            CIX.RefreshOnlineUsers();
            FolderCollection.RefreshInterestingThreads();
            return true;
        }

        /// <summary>
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public override string Address
        {
            get { return "welcome"; }
        }

        /// <summary>
        /// Indicate that we handle the welcome scheme.
        /// </summary>
        public override bool Handles(string scheme)
        {
            return scheme == "welcome";
        }

        /// <summary>
        /// Called when the form is loaded for the first time.
        /// </summary>
        private void WelcomeView_Load(object sender, EventArgs e)
        {
            CIX.FolderCollection.OnlineUsersUpdated += OnlineUsersUpdated;
            CIX.FolderCollection.InterestingThreadsUpdated += OnInterestingThreadsUpdated;
            CIX.MugshotUpdated += OnMugshotUpdated;

            wlcTopPosts.LinkClicked += OnLinkClicked;
            wlcTopPosts.CanvasItemAction += OnItemAction;

            UI.ThemeChanged += UI_ThemeChanged;
        }

        /// <summary>
        /// Invoked when the theme changes. Change the font on the page.
        /// </summary>
        void UI_ThemeChanged(object sender, EventArgs e)
        {
            if (_welcomePage != null)
            {
                _welcomePage.Font = UI.Forums.Font;
            }
        }

        /// <summary>
        /// Get notified when the list of interesting threads changes.
        /// </summary>
        private void OnInterestingThreadsUpdated(object sender, InterestingThreadsEventArgs e)
        {
            Platform.UIThread(this, delegate
            {
                _threads = e.Threads;
                if (_welcomePage == null)
                {
                    FillCanvas();
                }
                else
                {
                    _welcomePage.Threads = _threads;
                }
            });
        }

        /// <summary>
        /// Get and set the associated folders tree
        /// </summary>
        private FoldersTree FoldersTree { get; set; }

        /// <summary>
        /// Get notified when the list of online users is updated.
        /// </summary>
        private void OnlineUsersUpdated(object sender, OnlineUsersEventArgs e)
        {
            Platform.UIThread(this, delegate
            {
                _onlineUsers = e.Users;
                if (_welcomePage == null)
                {
                    FillCanvas();
                }
                else
                {
                    _welcomePage.OnlineUsers = _onlineUsers;
                }
            });
        }

        /// <summary>
        /// Handle mugshot change events. We pass this through to the moderator component to
        /// refresh any that are affected.
        /// </summary>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, () => _welcomePage.Refresh(mugshot.Username));
        }

        /// <summary>
        /// Respond to links being clicked by sending them to the main form to be processed.
        /// </summary>
        private void OnLinkClicked(object sender, LinkClickedEventArgs args)
        {
            FoldersTree.MainForm.Address = args.LinkText;
        }

        /// <summary>
        /// This event is triggered when the user performs an action on a canvas item.
        /// </summary>
        /// <param name="sender">The thread view control</param>
        /// <param name="args">A CanvasItemArgs object that contains details of the action</param>
        private void OnItemAction(object sender, CanvasItemArgs args)
        {
            switch (args.Item)
            {
                case ActionID.GoToSource:
                {
                    CIXThread thread = args.Tag as CIXThread;
                    if (thread != null)
                    {
                        FoldersTree.MainForm.Address = string.Format("cix:{0}/{1}:{2}", thread.Forum, thread.Topic,
                            thread.RemoteID);
                    }
                    break;
                }

                case ActionID.AuthorImage:
                    FoldersTree.MainForm.Address = string.Format("cixuser:{0}", args.Tag);
                    break;
            }
        }

        /// <summary>
        /// Add all the messages in the specified list to the current thread being displayed.
        /// </summary>
        public void FillCanvas()
        {
            wlcTopPosts.BeginUpdate();
            wlcTopPosts.Items.Clear();

            _welcomePage = new WelcomePage(wlcTopPosts)
            {
                Font = UI.System.Font,
                OnlineUsers = _onlineUsers,
                Threads = _threads
            };
            wlcTopPosts.Items.Add(_welcomePage);

            wlcTopPosts.EndUpdate(null);
        }
    }
}