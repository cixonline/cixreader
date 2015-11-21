// *****************************************************
// CIXReader
// SettingsViewing.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/03/2015 14:29
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class SettingsViewing : Form
    {
        private bool _isInitialising;

        static private ThemeEditor _themeEditor;

        public SettingsViewing()
        {
            InitializeComponent();
        }

        private void SettingsViewing_Load(object sender, EventArgs e)
        {
            _isInitialising = true;

            settingsDontDownloadImages.Checked = !Preferences.StandardPreferences.DownloadInlineImages;
            settingsViewStatusBar.Checked = Preferences.StandardPreferences.ViewStatusBar;
            settingsDisableMarkup.Checked = Preferences.StandardPreferences.IgnoreMarkup;
            settingsShowMenuBar.Checked = Preferences.StandardPreferences.ViewMenuBar;
            settingsShowToolbar.Checked = Preferences.StandardPreferences.ShowToolBar;
            settingsShowFullDate.Checked = Preferences.StandardPreferences.ShowFullDate;
            settingsShowCounts.Checked = Preferences.StandardPreferences.EnableSmartFolderCounts;

            settingsTheme.Items.Clear();
            foreach (UITheme theme in UI.Themes)
            {
                settingsTheme.Items.Add(theme.Name);
            }
            settingsTheme.SelectedIndex = settingsTheme.Items.IndexOf(UI.CurrentTheme);

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;

            // Disable Editor options on Linux
            #if __MonoCS__
            settingsSeparator2.Visible = false;
            settingsSpellAsYouType.Visible = false;
            settingsEditorLabel.Visible = false;
            #else
            settingsSpellAsYouType.Checked = Preferences.StandardPreferences.CheckSpellAsYouType;
            #endif

            _isInitialising = false;
        }

        /// <summary>
        /// Respond to changes to the custom theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                settingsTheme.SelectedItem = UI.CurrentTheme;
            }
        }

        /// <summary>
        /// Toggle the setting that specifies whether or not we download inline images.
        /// </summary>
        private void settingsDontDownloadImages_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
               Preferences.StandardPreferences.DownloadInlineImages = !settingsDontDownloadImages.Checked;
            }
        }

        /// <summary>
        /// Change the current theme.
        /// </summary>
        private void settingsTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                UI.CurrentTheme = (string)settingsTheme.SelectedItem;
            }
        }

        /// <summary>
        /// Invoke the theme editor.
        /// </summary>
        private void settingsCustomiseTheme_Click(object sender, EventArgs e)
        {
            if (_themeEditor == null)
            {
                _themeEditor = new ThemeEditor();
            }
            _themeEditor.Show();
            _themeEditor.BringToFront();
        }

        /// <summary>
        /// Toggle spell as you type.
        /// </summary>
        private void settingsSpellAsYouType_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.CheckSpellAsYouType = settingsSpellAsYouType.Checked;
            }
        }

        /// <summary>
        /// Toggle display of the status bar.
        /// </summary>
        private void settingsViewStatusBar_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.ViewStatusBar = settingsViewStatusBar.Checked;
            }
        }

        /// <summary>
        /// Toggle whether markup is rendered in messages.
        /// </summary>
        private void settingsDisableMarkup_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.IgnoreMarkup = settingsDisableMarkup.Checked;
            }
        }

        private void settingsShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.ViewMenuBar = settingsShowMenuBar.Checked;
            }
        }

        /// <summary>
        /// Toggle display of short or full dates in messages.
        /// </summary>
        private void settingsShowFullDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.ShowFullDate = settingsShowFullDate.Checked;
            }
        }

        /// <summary>
        /// Toggle display of the toolbar.
        /// </summary>
        private void settingsShowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.ShowToolBar = settingsShowToolbar.Checked;
            }
        }

        /// <summary>
        /// Toggle whether counts are shown on smart folders.
        /// </summary>
        private void settingsShowCounts_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.EnableSmartFolderCounts = settingsShowCounts.Checked;
            }
        }
    }
}