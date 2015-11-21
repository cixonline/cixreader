// *****************************************************
// CIXReader
// JoinForumInput.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/01/2015 11:17
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;

namespace CIXReader.Forms
{
    public sealed partial class JoinForumInput : Form
    {
        public JoinForumInput()
        {
            InitializeComponent();
        }

        public string ForumName { get; set; }

        /// <summary>
        /// Handle the Join button.
        /// </summary>
        private void inputJoinButton_Click(object sender, EventArgs e)
        {
            ForumName = inputText.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Handle the Cancel button.
        /// </summary>
        private void inputCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Limit text box input to non-space characters.
        /// </summary>
        private void inputText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Disable the Join button unless we have text.
        /// </summary>
        private void inputText_TextChanged(object sender, EventArgs e)
        {
            inputJoinButton.Enabled = !string.IsNullOrWhiteSpace(inputText.Text);
        }

        private void JoinForumInput_Load(object sender, EventArgs e)
        {
            inputJoinButton.Enabled = false;
        }
    }
}