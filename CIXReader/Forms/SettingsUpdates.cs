// *****************************************************
// CIXReader
// SettingsUpdates.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/03/2015 14:31
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.Utilities;
using Microsoft.Win32;

namespace CIXReader.Forms
{
    public sealed partial class SettingsUpdates : Form
    {
        private bool _isInitialising;
        private bool _checkForUpdate;

        public SettingsUpdates()
        {
            InitializeComponent();
        }

        private void SettingsUpdates_Load(object sender, EventArgs e)
        {
            _isInitialising = true;

            settingsUseBeta.Checked = Preferences.StandardPreferences.UseBeta;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CIXOnline Ltd\CIXReader\AutoUpdate");
            if (regKey != null)
            {
                _checkForUpdate = Convert.ToBoolean(regKey.GetValue("CheckForUpdate", 0));
                regKey.Close();
            }
            settingsCheckForUpdates.Checked = _checkForUpdate;

            _isInitialising = false;
        }

        /// <summary>
        /// Include beta releases in checks for updates.
        /// </summary>
        private void settingsUseBeta_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                Preferences.StandardPreferences.UseBeta = settingsUseBeta.Checked;
            }
        }

        private void settingsCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isInitialising)
            {
                _checkForUpdate = !_checkForUpdate;
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CIXOnline Ltd\CIXReader\AutoUpdate", true);
                if (regKey != null)
                {
                    regKey.SetValue("CheckForUpdate", _checkForUpdate.ToString());
                    regKey.Close();
                }
            }
        }
    }
}