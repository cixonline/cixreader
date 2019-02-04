// *****************************************************
// CIXReader
// ProfileView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/10/2013 10:13 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.CanvasItems;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Profile viewer modeless form.
    /// </summary>
    internal sealed partial class ProfileView : Form
    {
        private Profile _thisProfile;
        private ProfileItem _profileItem;
        private readonly Font _font;

        /// <summary>
        /// Initialises a new instance of the <see cref="ProfileView"/> class.
        /// </summary>
        public ProfileView(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();

            _font = UI.GetFont(UI.System.font, 10);
        }

        /// <summary>
        /// Get or set the associated main form.
        /// </summary>
        private MainForm MainForm { get; set; }

        /// <summary>
        /// Set the profile displayed in the viewer
        /// </summary>
        public Profile Profile
        {
            private get { return _thisProfile; }
            set
            {
                if (_thisProfile != value)
                {
                    _thisProfile = value;
                    DisplayProfile();
                }
            }
        }

        /// <summary>
        /// Do first time initialisation.
        /// </summary>
        private void ProfileView_Load(object sender, EventArgs e)
        {
            prvDetails.LinkClicked += OnLinkClicked;
            prvDetails.CanvasItemAction += OnItemAction;
            prvDetails.KeyPressed += OnKeyPressed;

            CIX.ProfileCollection.ProfileUpdated += OnProfileUpdated;
            CIX.MugshotUpdated += OnMugshotUpdated;
        }

        /// <summary>
        /// Handle the event that is sent when a profile is updated. If the profile is the
        /// one we're displaying here, update what is displayed in the viewer.
        /// </summary>
        private void OnProfileUpdated(object sender, ProfileEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (_thisProfile != null && args.Profile.Username == _thisProfile.Username)
                {
                    DisplayProfile();
                }
            });
        }

        /// <summary>
        /// Display the currently set profile
        /// </summary>
        private void DisplayProfile()
        {
            if (_thisProfile == null)
            {
                _profileItem = null;
            }
            else
            {
                if (_profileItem != null)
                {
                    _profileItem.Image = Mugshot.MugshotForUser(_thisProfile.Username, true).RealImage;
                    _profileItem.ProfileData = _thisProfile;
                }
                else
                {
                    Mugshot mugshot = Mugshot.MugshotForUser(_thisProfile.Username, true);
                    _profileItem = new ProfileItem(prvDetails, false)
                    {
                        NameColour = UI.System.SelectionColour,
                        Image = mugshot.RealImage,
                        Font = _font,
                        ProfileData = _thisProfile
                    };
                    mugshot.Refresh();
                    prvDetails.Items.Add(_profileItem);
                }
            }
        }

        /// <summary>
        /// This event is triggered when the user performs an action on a thread item.
        /// </summary>
        /// <param name="sender">The thread view control</param>
        /// <param name="args">A CanvasItemArgs object that contains details of the action</param>
        private void OnItemAction(object sender, CanvasItemArgs args)
        {
            ProfileItem profileItem = (ProfileItem)args.Control;
            switch (args.Item)
            {
                case ActionID.Profile:
                case ActionID.AuthorImage:
                    MainForm.Address = string.Format("cixmail:{0}", profileItem.ProfileData.Username);
                    break;

                case ActionID.Email:
                    MainForm.Address = string.Format("mailto:{0}", profileItem.ProfileData.EMailAddress);
                    break;

                case ActionID.Location:
                    MainForm.Address = string.Format(Constants.MapLink, profileItem.ProfileData.Location);
                    break;
            }
        }

        /// <summary>
        /// Respond to links being clicked by sending them to the main form to be processed.
        /// </summary>
        private void OnLinkClicked(object sender, LinkClickedEventArgs args)
        {
            MainForm.Address = args.LinkText;
        }

        /// <summary>
        /// Close the view when the Esc key is pressed.
        /// </summary>
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || (e.KeyCode == Keys.F4 && e.Control))
            {
                Close();
            }
        }

        /// <summary>
        /// Handle a mugshot updated. If this mugshot appears in any friend profile then we update
        /// the friend panel shown here.
        /// </summary>
        private void OnMugshotUpdated(object sender, MugshotEventArgs e)
        {
            Platform.UIThread(this, delegate
            {
                if (Profile != null && Profile.Username == e.Mugshot.Username)
                {
                    ProfileItem item = (ProfileItem) prvDetails.Items[0];
                    item.Image = e.Mugshot.RealImage;
                    item.UpdateImage();
                }
            });
        }

        /// <summary>
        /// Hide the window when the user clicks away from it.
        /// </summary>
        private void ProfileView_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Trap the form close and convert it to a hide.
        /// </summary>
        private void ProfileView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}