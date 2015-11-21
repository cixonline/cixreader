// *****************************************************
// CIXReader
// GoToMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 07/05/2014 11:16
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;

namespace CIXReader.Forms
{
    /// <summary>
    /// Implements the GoTo dialog box.
    /// </summary>
    public sealed partial class GoToMessage : Form
    {
        /// <summary>
        /// Instantiates the GoTo dialog class.
        /// </summary>
        public GoToMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the message number from the input field.
        /// </summary>
        public int MessageNumber { get; set; }

        /// <summary>
        /// Handle the OK button.
        /// </summary>
        private void gotoOK_Click(object sender, EventArgs e)
        {
            MessageNumber = Int32.Parse(gotoInput.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Handle the Cancel button.
        /// </summary>
        private void gotoCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Limit text box input to numbers.
        /// </summary>
        private void gotoInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void gotoInput_TextChanged(object sender, EventArgs e)
        {
            int messageNumber;
            gotoOK.Enabled = !string.IsNullOrEmpty(gotoInput.Text) && Int32.TryParse(gotoInput.Text, out messageNumber);
        }

        /// <summary>
        /// Initialise dialog when it is loaded.
        /// </summary>
        private void GoToMessage_Load(object sender, EventArgs e)
        {
            gotoOK.Enabled = false;
        }
    }
}