// *****************************************************
// CIXReader
// ScriptPackage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 04/09/2014 10:36
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Windows.Forms;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class ScriptPackage : Form
    {
        private readonly ScriptManifest _manifest;

        /// <summary>
        /// Instantiates the ScriptPackage dialog class.
        /// </summary>
        public ScriptPackage(ScriptManifest manifest)
        {
            InitializeComponent();
            _manifest = manifest;
        }

        /// <summary>
        /// Initialise the dialog with the parameters specified by the caller
        /// </summary>
        private void InstallScriptPackage_Load(object sender, System.EventArgs e)
        {
            instName.Text = string.IsNullOrWhiteSpace(_manifest.Name) ? Resources.None : _manifest.Name;
            instDescription.Text = string.IsNullOrWhiteSpace(_manifest.Description) ? Resources.None : _manifest.Description;
            instAuthor.Text = string.IsNullOrWhiteSpace(_manifest.Author) ? Resources.None : _manifest.Author;
            instToolbar.Text = _manifest.InstallToToolbar ? Resources.Yes : Resources.No;
        }

        /// <summary>
        /// User clicked the Install button.
        /// </summary>
        private void instOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// User clicked the Cancel button.
        /// </summary>
        private void instCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}