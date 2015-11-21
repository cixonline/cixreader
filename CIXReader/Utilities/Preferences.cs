// *****************************************************
// CIXReader
// Preferences.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 19/05/2015 12:24
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using CIXReader.Properties;
using IniParser;
using IniParser.Model;

namespace CIXReader.Utilities
{
    internal sealed class Preferences
    {
        private static volatile Preferences _defaultPreferences;
        private static readonly object syncRoot = new Object();

        private bool _groupByConv;
        private bool _collapseConv;
        private bool _showAllTopics;
        private bool _checkSpellAsYouType;
        private bool _ignoreMarkup;
        private bool _downloadInlineImages;
        private bool _showIgnored;
        private bool _showFullDate;
        private bool _viewStatusBar;
        private bool _viewMenuBar;
        private bool _showToolBar;
        private bool _startOffline;
        private bool _useBeta;
        private bool _startInHomePage;
        private bool _enableLogFile;
        private bool _archiveLogFile;
        private bool _cumulativeLogFile;
        private bool _enableSmartFolderCounts;
        private readonly bool _useBetaAPI;
        private string _lastAddress;
        private string _defaultSearchEngine;
        private string _defaultSignature;
        private string _currentTheme;
        private readonly int _maxCount;
        private int _cacheCleanUpFrequency;
        private DateTime _lastCacheCleanUp;
        private float _folderSplitPosition;
        private float _threadSplitPosition;

        // Setting strings
        public const string MAPref_GroupByConv = "Conversations";
        public const string MAPref_CollapseConv = "CollapseConversations";
        public const string MAPref_ShowAllTopics = "ShowAllTopics";
        public const string MAPref_CheckSpellAsYouType = "DisableSpellAsYouType"; // Backward compat
        public const string MAPref_IgnoreMarkup = "DisableMarkup";
        public const string MAPref_DownloadInlineImages = "NoDownloadImages"; // Backward compat
        public const string MAPref_ShowIgnored = "HideIgnored"; // Backward compat
        public const string MAPref_ShowFullDate = "ShowFullDate";
        public const string MAPref_ViewStatusBar = "ViewStatusBar";
        public const string MAPref_ViewMenuBar = "ShowMenuBar";
        public const string MAPref_ShowToolBar = "ShowToolBar";
        public const string MAPref_StartOffline = "StartOffline";
        public const string MAPref_LastAddress = "LastAddress";
        public const string MAPref_CacheCleanUpFrequency = "CacheCleanUpFrequency";
        public const string MAPref_LastCacheCleanUp = "LastCacheCleanUp";
        public const string MAPref_UseBeta = "UseBeta";
        public const string MAPref_DefaultSearchEngine = "DefaultSearchEngine";
        public const string MAPref_DefaultSignature = "DefaultSignature";
        public const string MAPref_CurrentTheme = "CurrentTheme";
        public const string MAPref_MaxCount = "MaxCount";
        public const string MAPref_UseBetaAPI = "UseBetaAPI";
        public const string MAPref_FolderSplitPosition = "SplitPosition2";
        public const string MAPref_ThreadSplitPosition = "SplitPosition";
        public const string MAPref_StartInHomePage = "StartInHomePage";
        public const string MAPref_EnableLogFile = "EnableLogFile";
        public const string MAPref_ArchiveLogFile = "ArchiveLogFile";
        public const string MAPref_CumulativeLogFile = "CumulativeLogFile";
        public const string MAPref_EnableSmartFolderCounts = "MAPref_EnableSmartFolderCounts";

        // Preferences file variables
        private static FileIniDataParser _parser;
        private static IniData _data;
        private static string _path;

        private const string _mainSection = "Main";

        /// <summary>
        /// Event handler for notifying a delegate that forum has been updated.
        /// </summary>
        public static event PreferencesChangedHandler PreferencesChanged;
        public delegate void PreferencesChangedHandler(object sender, PreferencesChangedEventArgs e);

        public Preferences()
        {
            _groupByConv = ReadBoolean(MAPref_GroupByConv, true);
            _enableLogFile = ReadBoolean(MAPref_EnableLogFile, true);
            _archiveLogFile = ReadBoolean(MAPref_ArchiveLogFile);
            _cumulativeLogFile = ReadBoolean(MAPref_CumulativeLogFile);
            _collapseConv = ReadBoolean(MAPref_CollapseConv);
            _showAllTopics = ReadBoolean(MAPref_ShowAllTopics);
            _checkSpellAsYouType = !ReadBoolean(MAPref_CheckSpellAsYouType);
            _ignoreMarkup = ReadBoolean(MAPref_IgnoreMarkup);
            _downloadInlineImages = !ReadBoolean(MAPref_DownloadInlineImages);
            _showIgnored = !ReadBoolean(MAPref_ShowIgnored);
            _showFullDate = ReadBoolean(MAPref_ShowFullDate);
            _viewStatusBar = ReadBoolean(MAPref_ViewStatusBar);
            _viewMenuBar = ReadBoolean(MAPref_ViewMenuBar, true);
            _showToolBar = ReadBoolean(MAPref_ShowToolBar, true);
            _startOffline = ReadBoolean(MAPref_StartOffline);
            _cacheCleanUpFrequency = ReadInteger(MAPref_CacheCleanUpFrequency);
            _lastCacheCleanUp = ReadDate(MAPref_LastCacheCleanUp, DateTime.MinValue);
            _useBeta = ReadBoolean(MAPref_UseBeta);
            _defaultSearchEngine = ReadString(MAPref_DefaultSearchEngine, "Google");
            _defaultSignature = ReadString(MAPref_DefaultSignature, Signatures.NoSignatureString);
            _currentTheme = ReadString(MAPref_CurrentTheme, Resources.Default);
            _maxCount = ReadInteger(MAPref_MaxCount, 999);
            _useBetaAPI = ReadBoolean(MAPref_UseBetaAPI);
            _folderSplitPosition = ReadFloat(MAPref_FolderSplitPosition);
            _threadSplitPosition = ReadFloat(MAPref_ThreadSplitPosition);
            _lastAddress = ReadString(MAPref_LastAddress);
            _enableSmartFolderCounts = ReadBoolean(MAPref_EnableSmartFolderCounts);
            _startInHomePage = ReadBoolean(MAPref_StartInHomePage, true);
        }

        /// <summary>
        /// Open an INI file at the specified path.
        /// </summary>
        /// <param name="INIPath">Path to the INI file</param>
        public static void Open(string INIPath)
        {
            _parser = new FileIniDataParser();
            _parser.Parser.Configuration.AllowDuplicateKeys = true;
            if (File.Exists (INIPath)) 
            {
                _data = _parser.ReadFile(INIPath);
            }
            else
            {
                _data = new IniData();
                _data.Sections.AddSection (_mainSection);
            }
            _path = INIPath;
        }

        /// <summary>
        /// Save the INI file changes.
        /// </summary>
        public static void Save()
        {
            if (_path != null && _data != null)
            {
                _parser.WriteFile(_path, _data);
            }
        }

        /// <summary>
        /// Return the default preferences object.
        /// </summary>
        public static Preferences StandardPreferences 
        {
            get
            {
                if (_defaultPreferences == null)
                {
                    lock (syncRoot)
                    {
                        if (_defaultPreferences == null)
                        {
                            _defaultPreferences = new Preferences();
                        }
                    }
                }
                return _defaultPreferences;
            }
        }

        /// <summary>
        /// Return whether conversations are grouped.
        /// </summary>
        public bool GroupByConv
        {
            get { return _groupByConv; }
            set
            {
                if (_groupByConv != value)
                {
                    _groupByConv = value;
                    WriteBoolean(MAPref_GroupByConv, value);
                }
            }
        }

        /// <summary>
        /// Return whether conversations are collapsed by default
        /// </summary>
        public bool CollapseConv
        {
            get { return _collapseConv; }
            set
            {
                if (_collapseConv != value)
                {
                    _collapseConv = value;
                    WriteBoolean(MAPref_CollapseConv, value);
                }
            }
        }

        /// <summary>
        /// Return whether the folder list shows all or just recent topics.
        /// </summary>
        public bool ShowAllTopics
        {
            get { return _showAllTopics; }
            set
            {
                if (_showAllTopics != value)
                {
                    _showAllTopics = value;
                    WriteBoolean(MAPref_ShowAllTopics, value);
                }
            }
        }

        public bool EnableSmartFolderCounts
        {
            get { return _enableSmartFolderCounts; }
            set
            {
                if (_enableSmartFolderCounts != value)
                {
                    _enableSmartFolderCounts = value;
                    WriteBoolean(MAPref_EnableSmartFolderCounts, value);
                }
            }
        }

        /// <summary>
        /// Get or set the value which specifies whether spell checking is enabled
        /// </summary>
        public bool CheckSpellAsYouType
        {
            get { return _checkSpellAsYouType; }
            set
            {
                if (_checkSpellAsYouType != value)
                {
                    _checkSpellAsYouType = value;
                    WriteBoolean(MAPref_CheckSpellAsYouType, !value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether markup is disabled in messages
        /// </summary>
        public bool IgnoreMarkup
        {
            get { return _ignoreMarkup; }
            set
            {
                if (_ignoreMarkup != value)
                {
                    _ignoreMarkup = value;
                    WriteBoolean(MAPref_IgnoreMarkup, value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether inline images are expanded.
        /// </summary>
        public bool DownloadInlineImages
        {
            get { return _downloadInlineImages; }
            set
            {
                if (_downloadInlineImages != value)
                {
                    _downloadInlineImages = value;
                    WriteBoolean(MAPref_DownloadInlineImages, !value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether ignored messages are displayed
        /// </summary>
        public bool ShowIgnored
        {
            get { return _showIgnored; }
            set
            {
                if (_showIgnored != value)
                {
                    _showIgnored = value;
                    WriteBoolean(MAPref_ShowIgnored, !value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which enables log files.
        /// </summary>
        public bool EnableLogFile
        {
            get { return _enableLogFile; }
            set
            {
                if (_enableLogFile != value)
                {
                    _enableLogFile = value;
                    WriteBoolean(MAPref_EnableLogFile, value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which enables log file archiving.
        /// </summary>
        public bool ArchiveLogFile
        {
            get { return _archiveLogFile; }
            set
            {
                if (_archiveLogFile != value)
                {
                    _archiveLogFile = value;
                    WriteBoolean(MAPref_ArchiveLogFile, value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether log files accumulate
        /// in the same file.
        /// </summary>
        public bool CumulativeLogFile
        {
            get { return _cumulativeLogFile; }
            set
            {
                if (_cumulativeLogFile != value)
                {
                    _cumulativeLogFile = value;
                    WriteBoolean(MAPref_CumulativeLogFile, value);
                }
            }
        }

        /// <summary>
        /// Get or set the flag which specifies whether messages show the full or friendly date.
        /// </summary>
        public bool ShowFullDate
        {
            get { return _showFullDate; }
            set
            {
                if (_showFullDate != value)
                {
                    _showFullDate = value;
                    WriteBoolean(MAPref_ShowFullDate, value);
                }
            }
        }

        /// <summary>
        /// Get or set the flag which indicates whether the status bar is visible.
        /// </summary>
        public bool ViewStatusBar
        {
            get { return _viewStatusBar; }
            set
            {
                if (_viewStatusBar != value)
                {
                    _viewStatusBar = value;
                    WriteBoolean(MAPref_ViewStatusBar, value);
                }
            }
        }

        /// <summary>
        /// Get or set the flag which indicates whether the menu bar is visible.
        /// </summary>
        public bool ViewMenuBar
        {
            get { return _viewMenuBar; }
            set
            {
                if (_viewMenuBar != value)
                {
                    _viewMenuBar = value;
                    WriteBoolean(MAPref_ViewMenuBar, value);
                }
            }
        }

        /// <summary>
        /// Get or set the flag which indicates whether the toolbar bar is visible.
        /// </summary>
        public bool ShowToolBar
        {
            get { return _showToolBar; }
            set
            {
                if (_showToolBar != value)
                {
                    _showToolBar = value;
                    WriteBoolean(MAPref_ShowToolBar, value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether we start up offline
        /// </summary>
        public bool StartOffline
        {
            get { return _startOffline; }
            set
            {
                if (_startOffline != value)
                {
                    _startOffline = value;
                    WriteBoolean(MAPref_StartOffline, value);
                }
            }
        }

        /// <summary>
        /// Get or set a flag which specifies whether we always show the home
        /// page when CIXReader starts.
        /// </summary>
        public bool StartInHomePage
        {
            get { return _startInHomePage; }
            set
            {
                if (_startInHomePage != value)
                {
                    _startInHomePage = value;
                    WriteBoolean(MAPref_StartInHomePage, value);
                }
            }
        }

        /// <summary>
        /// Get or set the address of the last displayed message
        /// </summary>
        public string LastAddress
        {
            get { return _lastAddress; }
            set
            {
                if (_lastAddress != value)
                {
                    _lastAddress = value;
                    WriteValue(MAPref_LastAddress, value);
                }
            }
        }

        /// <summary>
        /// Get or set the cache cleanup frequency in days
        /// </summary>
        public int CacheCleanUpFrequency
        {
            get { return _cacheCleanUpFrequency; }
            set
            {
                if (_cacheCleanUpFrequency != value)
                {
                    _cacheCleanUpFrequency = value;
                    WriteInteger(MAPref_CacheCleanUpFrequency, value);
                }
            }
        }

        /// <summary>
        /// Get or set the date of the last cache cleanup.
        /// </summary>
        public DateTime LastCacheCleanUp
        {
            get { return _lastCacheCleanUp; }
            set
            {
                if (_lastCacheCleanUp != value)
                {
                    _lastCacheCleanUp = value;
                    WriteDate(MAPref_LastCacheCleanUp, value);
                }
            }
        }

        /// <summary>
        /// Get or set the value which indicates we can update to beta releases.
        /// </summary>
        public bool UseBeta
        {
            get { return _useBeta; }
            set
            {
                if (_useBeta != value)
                {
                    _useBeta = value;
                    WriteBoolean(MAPref_UseBeta, value);
                }
            }
        }

        /// <summary>
        /// Get or set the default search engine
        /// </summary>
        public string DefaultSearchEngine
        {
            get { return _defaultSearchEngine; }
            set
            {
                if (_defaultSearchEngine != value)
                {
                    _defaultSearchEngine = value;
                    WriteValue(MAPref_DefaultSearchEngine, value);
                }
            }
        }

        /// <summary>
        /// Get or set the default signature
        /// </summary>
        public string DefaultSignature
        {
            get { return _defaultSignature; }
            set
            {
                if (_defaultSignature != value)
                {
                    _defaultSignature = value;
                    WriteValue(MAPref_DefaultSignature, value);
                }
            }
        }

        /// <summary>
        /// Get or set the name of the current UI theme.
        /// </summary>
        public string CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    WriteValue(MAPref_CurrentTheme, value);
                }
            }
        }

        /// <summary>
        /// Return the maximum count that can be shown in the count button
        /// </summary>
        public int MaxCount
        {
            get { return _maxCount; }
        }

        /// <summary>
        /// Return whether we use the beta API server.
        /// </summary>
        public bool UseBetaAPI
        {
            get { return _useBetaAPI; }
        }

        /// <summary>
        /// Get or set the folder pane split bar position.
        /// </summary>
        public float FolderSplitPosition
        {
            get { return _folderSplitPosition; }
            set
            {
                _folderSplitPosition = value;
                WriteFloat(MAPref_FolderSplitPosition, value);
            }
        }

        /// <summary>
        /// Get or set the thread pane split bar position.
        /// </summary>
        public float ThreadSplitPosition
        {
            get { return _threadSplitPosition; }
            set
            {
                _threadSplitPosition = value;
                WriteFloat(MAPref_ThreadSplitPosition, value);
            }
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        public static void WriteValue(string Key, string Value)
        {
            _data[_mainSection][Key] = Value;
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write a boolean value to the INI file.
        /// </summary>
        public static void WriteBoolean(string Key, bool value)
        {
            _data[_mainSection][Key] = value.ToString();
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write an integer value to the INI file.
        /// </summary>
        public static void WriteInteger(string Key, int value)
        {
            _data[_mainSection][Key] = value.ToString(CultureInfo.InvariantCulture);
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write an floating point value to the INI file.
        /// </summary>
        public static void WriteFloat(string Key, float value)
        {
            _data[_mainSection][Key] = value.ToString(CultureInfo.InvariantCulture);
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write a WindowState value to the INI file.
        /// </summary>
        public static void WriteFormWindowState(string Key, FormWindowState value)
        {
            _data[_mainSection][Key] = value.ToString();
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write a Point value to the INI file.
        /// </summary>
        public static void WritePoint(string Key, Point value)
        {
            _data[_mainSection][Key] = string.Format("{0},{1}", value.X, value.Y);
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write a Size value to the INI file.
        /// </summary>
        public static void WriteSize(string Key, Size value)
        {
            _data[_mainSection][Key] = string.Format("{0},{1}", value.Width, value.Height);
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Write a date value to the INI file.
        /// </summary>
        public static void WriteDate(string Key, DateTime value)
        {
            _data[_mainSection][Key] = value.ToString("dd-MM-yyyy HH:mm:ss");
            RaisePreferencesChangedEvent(Key);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        public static string ReadString(string Key, string defaultValue = "")
        {
            string value = _data[_mainSection][Key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Read a boolean value from the INI file.
        /// </summary>
        public static bool ReadBoolean(string Key, bool defaultValue = false)
        {
            bool tempValue;
            string temp = _data[_mainSection][Key] ?? string.Empty;
            return Boolean.TryParse(temp, out tempValue) ? tempValue : defaultValue;
        }

        /// <summary>
        /// Read an integer value from the INI file.
        /// </summary>
        public static int ReadInteger(string Key, int defaultValue = 0)
        {
            int tempValue;

            string temp = _data[_mainSection][Key] ?? string.Empty;
            return int.TryParse(temp, out tempValue) ? tempValue : defaultValue;
        }

        /// <summary>
        /// Read a floating point value from the INI file.
        /// </summary>
        public static float ReadFloat(string Key)
        {
            float tempValue;

            string temp = _data[_mainSection][Key] ?? string.Empty;
            return float.TryParse(temp, out tempValue) ? tempValue : 0;
        }

        /// <summary>
        /// Read a Point value from the INI file.
        /// </summary>
        public static Point ReadPoint(string Key)
        {
            string temp = _data[_mainSection][Key] ?? string.Empty;

            string[] points = temp.Split(',');
            int X, Y;

            if (points.Length == 2 && Int32.TryParse(points[0], out X) && Int32.TryParse(points[1], out Y))
            {
                return new Point(X, Y);
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Read a Size value from the INI file.
        /// </summary>
        public static Size ReadSize(string Key)
        {
            string temp = _data[_mainSection][Key] ?? string.Empty;

            string[] points = temp.Split(',');
            int width, height;

            if (points.Length == 2 && Int32.TryParse(points[0], out width) && Int32.TryParse(points[1], out height))
            {
                return new Size(width, height);
            }
            return new Size(0, 0);
        }

        /// <summary>
        /// Read a WindowState value from the INI file.
        /// </summary>
        public static FormWindowState ReadWindowState(string Key)
        {
            FormWindowState tempValue;

            string temp = _data[_mainSection][Key] ?? string.Empty;
            if (!Enum.TryParse(temp, out tempValue))
            {
                tempValue = FormWindowState.Normal;
            }
            return tempValue;
        }

        /// <summary>
        /// Read a date value from the INI file.
        /// </summary>
        public static DateTime ReadDate(string Key, DateTime defaultValue)
        {
            DateTime tempValue;

            string temp = _data[_mainSection][Key] ?? string.Empty;
            if (!DateTime.TryParse(temp, out tempValue))
            {
                tempValue = defaultValue;
            }
            return tempValue;
        }
 
        /// <summary>
        /// Raise the event that a preference has changed, assuming that someone has
        /// subscribed to that event.
        /// </summary>
        /// <param name="Key">The name of the key that changed</param>
        private static void RaisePreferencesChangedEvent(string Key)
        {
            if (PreferencesChanged != null)
            {
                PreferencesChanged(null, new PreferencesChangedEventArgs { Name = Key });
            }
        }
    }

    /// <summary>
    /// Class that encapsulates the preferences change event arguments
    /// </summary>
    public sealed class PreferencesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the preference that changed
        /// </summary>
        public string Name { get; set; }
    }
}