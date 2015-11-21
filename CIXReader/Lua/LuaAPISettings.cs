// *****************************************************
// CIXReader
// LuaAPISettings.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/05/2014 11:42
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Lua
{
    /// <summary>
    /// Implements a class that controls access to CIXReader settings
    /// </summary>
    // ReSharper disable UnusedMember.Global
    public sealed class LuaAPISettings
    {
        /// <summary>
        /// Get or set the current theme.
        /// </summary>
        public string CurrentTheme
        {
            get { return UI.CurrentTheme; }
            set { UI.CurrentTheme = value; }
        }

        /// <summary>
        /// Get or set a flag that controls markup
        /// </summary>
        public bool DisableMarkup
        {
            get { return Preferences.StandardPreferences.IgnoreMarkup; }
            set { Preferences.StandardPreferences.IgnoreMarkup = value; }
        }

        /// <summary>
        /// Get or set a flag that controls whether we start offline
        /// </summary>
        public bool StartOffline
        {
            get { return Preferences.StandardPreferences.StartOffline; }
            set { Preferences.StandardPreferences.StartOffline = value; }
        }

        /// <summary>
        /// Get or set a flag that controls whether we download images
        /// </summary>
        public bool ExpandInlineImages
        {
            get { return Preferences.StandardPreferences.DownloadInlineImages; }
            set { Preferences.StandardPreferences.DownloadInlineImages = value; }
        }

        /// <summary>
        /// Get or set a flag that controls whether the log file is enabled
        /// </summary>
        public bool EnableLogFile
        {
            get { return Preferences.StandardPreferences.EnableLogFile; }
            set { Preferences.StandardPreferences.EnableLogFile = value; }
        }

        /// <summary>
        /// Get or set a flag that controls whether the status bar is visible
        /// </summary>
        public bool ViewStatusBar
        {
            get { return Preferences.StandardPreferences.ViewStatusBar; }
            set { Preferences.StandardPreferences.ViewStatusBar = value; }
        }
    }
}