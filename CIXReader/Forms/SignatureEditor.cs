// *****************************************************
// CIXReader
// SignatureEditor.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 16/01/2015 13:03
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class SignatureEditor : Form
    {
        public SignatureEditor(string title)
        {
            InitializeComponent();
            SignatureTitle = title;
        }

        public string SignatureTitle { get; set; }

        public string SignatureText { get; set; }

        private void SignatureEditor_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SignatureTitle))
            {
                sigTitle.Text = SignatureTitle;
                sigText.Text = Signatures.DefaultSignatures.SignatureForTitle(SignatureTitle);
            }
            UpdateButtons();
        }

        private void sigEditorSave_Click(object sender, EventArgs e)
        {
            SignatureTitle = sigTitle.Text;
            SignatureText = sigText.Text;
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void sigTitle_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void sigText_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            sigEditorSave.Enabled = !string.IsNullOrEmpty(sigTitle.Text) && !string.IsNullOrEmpty(sigText.Text);
        }
    }
}