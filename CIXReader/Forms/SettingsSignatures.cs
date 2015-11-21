// *****************************************************
// CIXReader
// SettingsSignatures.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/03/2015 14:30
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class SettingsSignatures : Form
    {
        public SettingsSignatures()
        {
            InitializeComponent();
        }

        private void SettingsSignatures_Load(object sender, EventArgs e)
        {
            LoadSignaturesList();

            settingsDeleteSignature.Enabled = false;
            settingsEditSignature.Enabled = false;
        }

        /// <summary>
        /// Called when the user selects a signature from the list.
        /// </summary>
        private void settingsSignatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSignatureButtons();
        }

        /// <summary>
        /// Update the state of the signature editing buttons
        /// </summary>
        private void UpdateSignatureButtons()
        {
            ListView.SelectedIndexCollection index = settingsSignaturesList.SelectedIndices;
            bool hasSelection = (index.Count > 0);
            settingsEditSignature.Enabled = hasSelection;
            settingsDeleteSignature.Enabled = hasSelection;
        }

        /// <summary>
        /// Create a new signature.
        /// </summary>
        private void settingsNewSignature_Click(object sender, EventArgs e)
        {
            SignatureEditor editor = new SignatureEditor("");
            if (editor.ShowDialog() == DialogResult.OK)
            {
                Signatures.DefaultSignatures.AddSignature(editor.SignatureTitle, editor.SignatureText);
                LoadSignaturesList();

                ListViewItem lvItem = settingsSignaturesList.FindItemWithText(editor.SignatureTitle);
                if (lvItem != null)
                {
                    lvItem.Selected = true;
                    settingsSignaturesList.Select();
                }

                UpdateSignatureButtons();
            }
        }

        /// <summary>
        /// Delete the selected signature.
        /// </summary>
        private void settingsDeleteSignature_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selections = settingsSignaturesList.SelectedItems;
            if (selections.Count == 1)
            {
                ListViewItem lvItem = selections[0];
                Signatures.DefaultSignatures.RemoveSignature(lvItem.Text);
            }
            LoadSignaturesList();
            UpdateSignatureButtons();
        }

        /// <summary>
        /// The default signature has changed.
        /// </summary>
        private void settingsDefaultSignature_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newDefault = (string)settingsDefaultSignature.SelectedItem;
            Preferences.StandardPreferences.DefaultSignature = newDefault;
        }

        /// <summary>
        /// Load the signatures list.
        /// </summary>
        private void LoadSignaturesList()
        {
            IEnumerable<string> signatures = Signatures.DefaultSignatures.SignatureTitles;
            settingsSignaturesList.Items.Clear();
            settingsDefaultSignature.Items.Clear();
            foreach (string signature in signatures)
            {
                ListViewItem lvItem = new ListViewItem { Text = signature };
                settingsSignaturesList.Items.Add(lvItem);
                settingsDefaultSignature.Items.Add(signature);
            }
            settingsDefaultSignature.SelectedItem = Preferences.StandardPreferences.DefaultSignature;
        }

        private void settingsEditSignature_Click(object sender, EventArgs e)
        {
            EditSignature();
        }

        /// <summary>
        /// Double-click edits the selected item.
        /// </summary>
        private void settingsSignatures_DoubleClick(object sender, EventArgs e)
        {
            EditSignature();
        }

        private void EditSignature()
        {
            ListView.SelectedListViewItemCollection selections = settingsSignaturesList.SelectedItems;
            if (selections.Count == 1)
            {
                ListViewItem lvItem = selections[0];
                string signatureBeingEdited = lvItem.Text;

                SignatureEditor editor = new SignatureEditor(signatureBeingEdited);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    if (editor.SignatureTitle != signatureBeingEdited)
                    {
                        Signatures.DefaultSignatures.RemoveSignature(signatureBeingEdited);
                    }
                    Signatures.DefaultSignatures.AddSignature(editor.SignatureTitle, editor.SignatureText);
                    LoadSignaturesList();

                    lvItem = settingsSignaturesList.FindItemWithText(editor.SignatureTitle);
                    if (lvItem != null)
                    {
                        lvItem.Selected = true;
                        settingsSignaturesList.Select();
                    }
                }
            }
        }
    }
}