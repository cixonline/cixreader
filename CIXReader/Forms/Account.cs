// *****************************************************
// CIXReader
// Account.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 03/01/2015 3:09 PM
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class Account : Form
    {
        private readonly PictureBox _dropThumbnail;
        private string _dropFilename = String.Empty;
        private Thread _getDropImageThread;
        private bool _hasValidDropData;
        private int _lastDropX;
        private int _lastDropY;
        private Image image;
        private Image nextImage;

        /// <summary>
        /// Initialises a new instance of the <see cref="Account"/> class.
        /// </summary>
        public Account()
        {
            InitializeComponent();

            // Thumbnail will be filled with a small image of what is being
            // dropped on the Mugshot control.
            _dropThumbnail = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = false
            };
            actMugshot.Controls.Add(_dropThumbnail);

            // Ensure user can drag an image onto the mugshot.
            actMugshot.AllowDrop = true;
        }

        private void Account_Load(object sender, EventArgs e)
        {
            CIX.ProfileCollection.ProfileUpdated += OnProfileUpdated;
            CIX.MugshotUpdated += OnMugshotUpdated;

            CIX.FolderCollection.AccountUpdated += OnAccountUpdated;
            CIX.RefreshUserAccount();

            RefreshAccount(Profile.ProfileForUser(CIX.Username), true);
            CIX.ProfileCollection.Get(CIX.Username).Refresh();
        }

        /// <summary>
        /// Invoked when we have user account details.
        /// </summary>
        private void OnAccountUpdated(object sender, AccountEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                // Translate account type to a more user-friendly name.
                string accountTypeName;
                bool enableUpgrade = false;
                switch (args.Account.Type)
                {
                    case "ICA-SUT":
                    case "ICA-OUT":
                    case "OUT":
                    case "CCA":
                        accountTypeName = Resources.Full;
                        break;

                    case "BASIC":
                        accountTypeName = Resources.Basic;
                        enableUpgrade = true;
                        break;

                    default:
                        accountTypeName = args.Account.Type;
                        break;
                }

                actAccountUpgrade.Visible = enableUpgrade;
                actAccountType.Text = accountTypeName;
            });
        }

        /// <summary>
        /// This notification is triggered if the profile was updated from the
        /// server. We're only interested if the profile is for the current user.
        /// </summary>
        /// <param name="sender">The profiles task</param>
        /// <param name="args">Profile event arguments specifying the profile affected</param>
        private void OnProfileUpdated(object sender, ProfileEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (args.Profile.Username == CIX.Username)
                {
                    RefreshAccount(args.Profile, false);
                }
            });
        }

        /// <summary>
        /// This notification is triggered if a mugshot is updated from the server. We're
        /// only interested if the current user's mugshot changes.
        /// </summary>
        /// <param name="mugshot">The mugshot</param>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, delegate
            {
                if (mugshot.Username == CIX.Username)
                {
                    actMugshot.Image = mugshot.RealImage;
                }
            });
        }

        /// <summary>
        /// Refresh the Account field from the server. The profile structure specifies
        /// the updated data. Note that some fields may legally be null in the case where
        /// no server side profile has been retrieved. The only field that is guaranteed
        /// not to be null is the username.
        /// </summary>
        /// <param name="profile">A Profile object with the account profile data</param>
        /// <param name="force">Whether to force refresh the mugshot</param>
        private void RefreshAccount(Profile profile, bool force)
        {
            actUsername.Text = CIX.Username;

            Mugshot mugshot = Mugshot.MugshotForUser(profile.Username, force);
            actMugshot.Image = mugshot.RealImage;

            if (force)
            {
                mugshot.Refresh();
            }

            actEmail.Text = profile.EMailAddress;
            actFullname.Text = profile.FullName;
            actLocation.Text = profile.Location;

            string sexCode = "u";
            if (!string.IsNullOrEmpty(profile.Sex))
            {
                sexCode = profile.Sex.ToLower();
            }
            actSexDontSay.Checked = sexCode == "" || sexCode == "u";
            actSexFemale.Checked = sexCode == "f";
            actSexMale.Checked = sexCode == "m";

            actAbout.Text = profile.About;

            actNotifyPM.Checked = profile.Flags.HasFlag(Profile.NotificationFlags.PMNotification);
            actNotifyTag.Checked = profile.Flags.HasFlag(Profile.NotificationFlags.TagNotification);

            actSave.Enabled = false;
        }

        /// <summary>
        /// Save the account changes and upload to the server
        /// </summary>
        private void SaveAccountChanges()
        {
            actSave.Enabled = false;

            // Resize the image to 100x100 before uploading
            Mugshot mugshot = Mugshot.MugshotForUser(CIX.Username, false);
            mugshot.Update(actMugshot.Image);

            Profile myProfile = Profile.ProfileForUser(CIX.Username);
            bool isModified = false;

            string sexCode = "";
            if (actSexMale.Checked)
            {
                sexCode = "M";
            }
            if (actSexFemale.Checked)
            {
                sexCode = "F";
            }
            if (sexCode != myProfile.Sex)
            {
                myProfile.Sex = sexCode;
                isModified = true;
            }
            if (actEmail.Text != myProfile.EMailAddress)
            {
                myProfile.EMailAddress = actEmail.Text;
                isModified = true;
            }
            if (actFullname.Text != myProfile.FullName)
            {
                myProfile.FullName = actFullname.Text;
                isModified = true;
            }
            if (actLocation.Text != myProfile.Location)
            {
                myProfile.Location = actLocation.Text;
                isModified = true;
            }
            if (actAbout.Text != myProfile.About)
            {
                myProfile.About = actAbout.Text;
                isModified = true;
            }
            Profile.NotificationFlags flags = 0;
            if (actNotifyPM.Checked)
            {
                flags |= Profile.NotificationFlags.PMNotification;
            }
            if (actNotifyTag.Checked)
            {
                flags |= Profile.NotificationFlags.TagNotification;
            }
            if (flags != myProfile.Flags)
            {
                myProfile.Flags = flags;
                isModified = true;
            }
            if (isModified)
            {
                myProfile.Update();
            }
        }

        /// <summary>
        /// Handle an image being dragged and dropped over the mugshot.
        /// </summary>
        private void actMugshot_DragDrop(object sender, DragEventArgs e)
        {
            if (_hasValidDropData)
            {
                while (_getDropImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                _dropThumbnail.Visible = false;
                image = nextImage;
                if ((actMugshot.Image != null) && (actMugshot.Image != nextImage))
                {
                    actMugshot.Image.Dispose();
                }
                actMugshot.Image = image;
                OnAccountChanged(this, new EventArgs());
            }
        }

        private void actMugshot_DragEnter(object sender, DragEventArgs e)
        {
            string filename;

            _hasValidDropData = GetFilename(out filename, e);
            if (_hasValidDropData)
            {
                _dropThumbnail.Image = null;
                _dropThumbnail.Visible = false;
                _dropFilename = filename;
                _getDropImageThread = new Thread(LoadImage);
                _getDropImageThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private static bool GetFilename(out string filename, DragEventArgs e)
        {
            bool success = false;
            filename = String.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = e.Data.GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[]) data)[0];
                        var extension = Path.GetExtension(filename);
                        if (extension != null)
                        {
                            string ext = extension.ToLower();
                            if ((ext == ".jpg") || (ext == ".png") || (ext == ".gif"))
                            {
                                success = true;
                            }
                        }
                    }
                }
            }
            return success;
        }

        private void LoadImage()
        {
            nextImage = Image.FromFile(_dropFilename);
            Invoke(new AssignImageDlgt(AssignImage));
        }

        private void AssignImage()
        {
            _dropThumbnail.Width = 100;
            _dropThumbnail.Height = nextImage.Height*100/nextImage.Width;
            SetThumbnailLocation(PointToClient(new Point(_lastDropX, _lastDropY)));
            _dropThumbnail.Image = nextImage;
        }

        private void actMugshot_DragOver(object sender, DragEventArgs e)
        {
            if (_hasValidDropData)
            {
                if ((e.X != _lastDropX) || (e.Y != _lastDropY))
                {
                    SetThumbnailLocation(PointToClient(new Point(e.X, e.Y)));
                    _lastDropX = e.X;
                    _lastDropY = e.Y;
                }
            }
        }

        private void SetThumbnailLocation(Point p)
        {
            if (_dropThumbnail.Image == null)
            {
                _dropThumbnail.Visible = false;
            }
            else
            {
                p.X -= _dropThumbnail.Width/2;
                p.Y -= _dropThumbnail.Height/2;
                _dropThumbnail.Location = p;
                _dropThumbnail.Visible = true;
            }
        }

        private void actMugshot_DragLeave(object sender, EventArgs e)
        {
            _dropThumbnail.Visible = false;
        }

        /// <summary>
        /// Handle the user clicking the Upload button to upload an image via the
        /// file picker dialog.
        /// </summary>
        /// <param name="sender">The form control</param>
        /// <param name="e">Event arguments</param>
        private void actUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = Resources.MugshotFileTypes,
                Title = Resources.SelectMugshot
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                actMugshot.Image = Image.FromFile(openFileDialog1.FileName);
                OnAccountChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Replace the mugshot image with the default image.
        /// </summary>
        /// <param name="sender">The form control</param>
        /// <param name="e">Event arguments</param>
        private void actRemove_Click(object sender, EventArgs e)
        {
            actMugshot.Image = Mugshot.GetDefaultMugshot();
            OnAccountChanged(this, new EventArgs());
        }

        /// <summary>
        /// Save button was clicked. Save the changes on the page to the
        /// server.
        /// </summary>
        /// <param name="sender">The form</param>
        /// <param name="e">Event arguments</param>
        private void actSave_Click(object sender, EventArgs e)
        {
            SaveAccountChanges();
            Close();
        }

        /// <summary>
        /// Cancel button was clicked. Overwrite all changes with what is
        /// currently on the server.
        /// </summary>
        /// <param name="sender">The form</param>
        /// <param name="e">Event arguments</param>
        private void actCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Indicate that the account information has been altered by the
        /// user so show the Save and Cancel buttons to allow them to either save
        /// the changes or cancel out altogether.
        /// </summary>
        /// <param name="sender">The form</param>
        /// <param name="e">Event arguments</param>
        private void OnAccountChanged(object sender, EventArgs e)
        {
            actSave.Enabled = true;
        }

        /// <summary>
        /// Handle the Upgrade link click.
        /// </summary>
        private void actAccountUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.AccountUpgradeURL);
        }

        private delegate void AssignImageDlgt();
    }
}