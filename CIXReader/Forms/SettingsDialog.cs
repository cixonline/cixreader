// *****************************************************
// CIXReader
// SettingsDialog.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/03/2015 14:33
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class SettingsDialog : Form
    {
        private readonly SettingsGeneral _general;
        private readonly SettingsSignatures _signatures;
        private readonly SettingsUpdates _updates;
        private readonly SettingsViewing _viewing;
        private readonly SettingsRules _rules;

        private Button _activeButton;
        private Form _activeView;

        public SettingsDialog()
        {
            InitializeComponent();

            _general = new SettingsGeneral();
            _viewing = new SettingsViewing();
            _signatures = new SettingsSignatures();
            _updates = new SettingsUpdates();
            _rules = new SettingsRules();
        }

        /// <summary>
        /// Display the General tab to start with.
        /// </summary>
        private void NewSettingsDialog_Load(object sender, EventArgs e)
        {
            // On Linux, Check for Updates is not supported
            #if __MonoCS__
            settingsUpdates.Visible = false;
            #endif
            SelectButton(settingsGeneral);
            LoadPage(_general);
        }

        /// <summary>
        /// Load and make the specified page active
        /// </summary>
        /// <param name="page">Page to show</param>
        private void LoadPage(Form page)
        {
            if (page != _activeView)
            {
                if (_activeView != null)
                {
                    _activeView.Hide();
                }
                _activeView = page;
                _activeView.TopLevel = false;
                _activeView.Parent = settingsPanel;
                _activeView.Size = settingsPanel.Size;
                _activeView.Show();
            }
        }

        /// <summary>
        /// Select the specified tab button.
        /// </summary>
        private void SelectButton(Button tabButton)
        {
            if (tabButton != _activeButton)
            {
                if (_activeButton != null)
                {
                    _activeButton.ForeColor = SystemColors.ControlText;
                    _activeButton.BackColor = settingsToolbar.BackColor;
                }
                _activeButton = tabButton;
                _activeButton.BackColor = SystemColors.Highlight;
                _activeButton.ForeColor = SystemColors.HighlightText;
            }
        }

        /// <summary>
        /// Resize the active view when the main form is resized.
        /// </summary>
        private void NewSettingsDialog_Resize(object sender, EventArgs e)
        {
            if (_activeView != null)
            {
                _activeView.Size = settingsPanel.Size;
            }
        }

        /// <summary>
        /// Activate the General tab.
        /// </summary>
        private void settingsGeneral_Click(object sender, EventArgs e)
        {
            SelectButton(settingsGeneral);
            LoadPage(_general);
        }

        /// <summary>
        /// Activate the Viewings tab.
        /// </summary>
        private void settingsViewing_Click(object sender, EventArgs e)
        {
            SelectButton(settingsViewing);
            LoadPage(_viewing);
        }

        /// <summary>
        /// Activate the Signatures tab.
        /// </summary>
        private void settingsSignatures_Click(object sender, EventArgs e)
        {
            SelectButton(settingsSignatures);
            LoadPage(_signatures);
        }

        /// <summary>
        /// Activate the Rules tab.
        /// </summary>
        private void settingsRules_Click(object sender, EventArgs e)
        {
            SelectButton(settingsRules);
            LoadPage(_rules);
        }

        /// <summary>
        /// Activate the Updates tab.
        /// </summary>
        private void settingsUpdates_Click(object sender, EventArgs e)
        {
            SelectButton(settingsUpdates);
            LoadPage(_updates);
        }

        /// <summary>
        /// Save the settings when the form is closing.
        /// </summary>
        private void SettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Preferences.Save();
        }
    }
}