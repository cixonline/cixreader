// *****************************************************
// CIXReader
// AddUserInput.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/03/2015 11:34
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;

namespace CIXReader.Forms
{
    public sealed partial class AddUserInput : Form
    {
        public AddUserInput()
        {
            InitializeComponent();
        }

        public string UserName { get; set; }

        /// <summary>
        /// Add button was clicked.
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
        {
            UserName = inputField.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel button was clicked.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Don't allow whitespace in user name
        /// </summary>
        private void inputField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}