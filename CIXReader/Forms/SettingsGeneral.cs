// *****************************************************
// CIXReader
// SettingsGeneral.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/03/2015 14:27
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public sealed partial class SettingsGeneral : Form
    {
        private bool _isInitialising;

        private readonly Dictionary<int, string> _cacheListOptions = new Dictionary<int, string>
        {
            {0, Resources.Never},
            {1, Resources.Daily},
            {7, Resources.Weekly}
        };

        public SettingsGeneral()
        {
            InitializeComponent();
        }

        private void SettingsGeneral_Load(object sender, EventArgs e)
        {
            _isInitialising = true;

            settingsStartOffline.Checked = Preferences.StandardPreferences.StartOffline;
            settingsStartInHomePage.Checked = Preferences.StandardPreferences.StartInHomePage;
            settingsDebugLog.Checked = Preferences.StandardPreferences.EnableLogFile;
            settingsArchiveLogs.Checked = Preferences.StandardPreferences.ArchiveLogFile;

            // List of search engines
            settingsSearchEngines.Items.Clear();
            foreach (string entry in SearchEngines.AllSites)
            {
                settingsSearchEngines.Items.Add(entry);
            }
            settingsSearchEngines.SelectedIndex = settingsSearchEngines.Items.IndexOf(Preferences.StandardPreferences.DefaultSearchEngine);

            // List of cache clean options
            settingsCleanUpCacheList.Items.Clear();
            foreach (string entry in _cacheListOptions.Values)
            {
                settingsCleanUpCacheList.Items.Add(entry);
            }
            int cacheCleanupFrequency = Preferences.StandardPreferences.CacheCleanUpFrequency;
            string selectedOption = _cacheListOptions[cacheCleanupFrequency];
            settingsCleanUpCacheList.SelectedIndex = settingsCleanUpCacheList.Items.IndexOf(selectedOption);

            _isInitialising = false;
        }

        /// <summary>
        /// Toggle the setting that specifies whether or not we start offline.
        /// </summary>
        private void settingsStartOffline_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.StartOffline = settingsStartOffline.Checked;
            }
        }

        /// <summary>
        /// Toggle the setting that specifies whether or not we maintain a debug log.
        /// </summary>
        private void settingsDebugLog_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.EnableLogFile = settingsDebugLog.Checked;
                LogFile.Enabled = settingsDebugLog.Checked;
            }
        }

        /// <summary>
        /// Toggle the setting that specifies whether or not we archive debug logs.
        /// </summary>
        private void settingsArchiveLogs_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.ArchiveLogFile = settingsArchiveLogs.Checked;
            }
        }

        /// <summary>
        /// Toggle the setting which specifies whether CIXReader starts with the home page.
        /// </summary>
        private void settingsStartInHomePage_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.StartInHomePage = settingsStartInHomePage.Checked;
            }
        }

        private void settingsSearchEngines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newSearchEngine = (string) settingsSearchEngines.SelectedItem;
            Preferences.StandardPreferences.DefaultSearchEngine = newSearchEngine;
        }

        /// <summary>
        /// Open the log file.
        /// </summary>
        private void settingsOpenLogFile_Click(object sender, EventArgs e)
        {
            string debugFilePath = Path.Combine(CIX.HomeFolder, "cixreader.debug.log");
            Process.Start(debugFilePath);
        }

        /// <summary>
        /// User changed a cache cleanup value
        /// </summary>
        private void settingsCleanUpCacheList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                string selectedFrequency = (string) settingsCleanUpCacheList.SelectedItem;
                foreach (int frequency in _cacheListOptions.Keys.Where(frequency => _cacheListOptions[frequency] == selectedFrequency))
                {
                    Preferences.StandardPreferences.CacheCleanUpFrequency = frequency;
                    break;
                }
            }
        }

        /// <summary>
        /// Mark all messages in the database as read.
        /// </summary>
        private void settingsMarkAllRead_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.MarkAllRead, Resources.MarkAllReadPrompt, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CIX.FolderCollection.MarkAllRead();
            }
        }
    }
}