// *****************************************************
// CIXReader
// Login.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 5:17 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXClient;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Implements the Login dialog box.
    /// </summary>
    internal sealed partial class Login : Form
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Username property from the login dialog
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password property from the login dialog
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Called when the username is edited.
        /// </summary>
        private void fldUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginState();
        }

        /// <summary>
        /// Called when the password field is edited.
        /// </summary>
        private void fldPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginState();
        }

        /// <summary>
        /// Enable or disable the Login button depending on whether we have
        /// both a username and a password.
        /// </summary>
        private void UpdateLoginState()
        {
            int fldUsernameLength = fldUsername.TextLength;
            int fldPasswordLength = fldPassword.TextLength;

            fldError.Visible = false;
            fldOK.Enabled = fldUsernameLength > 0 && fldPasswordLength > 0;
        }

        /// <summary>
        /// Handle the Login button and cache the username and password entered. If the
        /// Remember option was checked, save the username in the global settings.
        /// </summary>
        private void fldOK_Click(object sender, EventArgs e)
        {
            if (Username != null && fldUsername.Text != Username)
            {
                fldError.Visible = true;
                return;
            }

            if (Password != null && fldPassword.Text != Password)
            {
                fldError.Visible = true;
                return;
            }

            // Authenticate online if possible
            // Disabling the buttons is symbolic anyway. Just to show something is happening.
            fldOK.Enabled = false;
            fldCancel.Enabled = false;
            CIX.AuthenticateResponse success = CIX.Authenticate(fldUsername.Text, fldPassword.Text);
            fldOK.Enabled = true;
            fldCancel.Enabled = true;

            if (success == CIX.AuthenticateResponse.Unconnected)
            {
                MessageBox.Show(Properties.Resources.NoConnection, Properties.Resources.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (success == CIX.AuthenticateResponse.Inactivated)
            {
                fldError.Text = Properties.Resources.InactivatedAccount;
                fldError.Visible = true;
                return;
            }

            if (success == CIX.AuthenticateResponse.Failure)
            {
                fldError.Text = Properties.Resources.BadUsernameOrPassword;
                fldError.Visible = true;
                return;
            }

            Username = fldUsername.Text;
            Password = fldPassword.Text;

            Settings.CurrentUser.SetString("LastUser", Username);

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Initialise the Login dialog and display the CIXReader name and version number.
        /// </summary>
        private void Login_Load(object sender, EventArgs e)
        {
            fldTitle.Text = Properties.Resources.AppTitle;

            if (Username != null)
            {
                fldUsername.Text = Username;
                ActiveControl = fldPassword;
            }
            UpdateLoginState();
        }

        /// <summary>
        /// Event called when the user clicks on the sign-in link.
        /// </summary>
        private void fldSignupLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.SignUpURL);
        }

        /// <summary>
        /// Handle the Cancel button.
        /// </summary>
        private void fldCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}