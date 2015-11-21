// *****************************************************
// CIXReader
// Authenticate.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 07/11/2013 3:14 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Implements the Authenticate dialog box.
    /// </summary>
    public sealed partial class Authenticate : Form
    {
        /// <summary>
        /// Instantiate the Authenticate dialog class.
        /// </summary>
        public Authenticate()
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

            fldOK.Enabled = fldUsernameLength > 0 && fldPasswordLength > 0;
        }

        /// <summary>
        /// Handle the Login button and cache the username and password entered. If the
        /// Remember option was checked, save the username in the global settings.
        /// </summary>
        private void fldOK_Click(object sender, EventArgs e)
        {
            Username = fldUsername.Text;
            Password = fldPassword.Text;

            Settings.CurrentUser.SetString("LastUser", Username);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Authenticate_Load(object sender, EventArgs e)
        {
            if (Username != null)
            {
                fldUsername.Text = Username;
                ActiveControl = fldPassword;
            }
            UpdateLoginState();
        }
        
        /// <summary>
        /// Invoke the CIX web page to recover a password.
        /// </summary>
        private void fldForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.ForgottenPasswordURL);
        }
    }
}