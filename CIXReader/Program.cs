// *****************************************************
// CIXReader
// Program.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/08/2013 11:25 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CIXClient;
using CIXReader.Forms;
using CIXReader.Properties;
using CIXReader.Utilities;
using Microsoft.Win32;
using TheArtOfDev.HtmlRenderer;

namespace CIXReader
{
    /// <summary>
    /// The CIXReader program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// CIXReader shut down reason types
        /// </summary>
        public enum ShutdownReasonType
        {
            /// <summary>
            /// Indicates that CIXReader is being shut down
            /// </summary>
            Close,

            /// <summary>
            /// Indicates that the user has logged out of CIXReader
            /// and will log in again as a new user.
            /// </summary>
            Logout
        }

        /// <summary>
        /// Get or set the flag which specifies the application main
        /// form shutdown reason.
        /// </summary>
        public static ShutdownReasonType ShutdownReason { private get; set; }

        /// <summary>
        /// Flag indicating whether this is a first run
        /// </summary>
        public static bool IsFirstRun { get; set; }

        // Mutex used to control access to running instances
        static readonly Mutex mutex = new Mutex(true, "{16042BB8-B234-4992-9CE4-BE88B4EC6E1D}");

        /// Command line settings
        private static bool _forceOffline;
        private static string _address;
        private static string _databasePath;
        private static string _versionString;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(true);

                // Save any command line arguments here because we need to
                // ignore them if we log out.
                string[] args = Environment.GetCommandLineArgs();
                string commandLine = args.Length > 1 ? args[1] : Settings.CurrentUser.GetString("LastCommandLine");
                if (!string.IsNullOrWhiteSpace(commandLine))
                {
                    if (!ParseCommandLine(commandLine))
                    {
                        return;
                    }
                    Settings.CurrentUser.SetString("LastCommandLine", commandLine);
                }
                LastUser = Settings.CurrentUser.GetString("LastUser");

                ShutdownReason = ShutdownReasonType.Close;

                if (Initialise())
                {
                    Application.Run(new MainForm());
                }

                LogFile.WriteLine("{0} {1} shut down", Resources.AppTitle, VersionString);
                LogFile.Close();

                // NetSparkle shuts down the app forcibly so the last command line is
                // preserved. For a normal shutdown, clear it instead.
                Settings.CurrentUser.SetString("LastCommandLine", string.Empty);
                Preferences.Save();

                if (ShutdownReason == ShutdownReasonType.Logout)
                {
                    Settings.CurrentUser.SetString("LastUser", string.Empty);
                    RelaunchProcess();
                }
            }
            else
            {
                // Another instance is running so switch access to it.
                Platform.SwitchToExistingCIXReaderInstance();
            }
        }

        /// <summary>
        /// Return the underlying OS name.
        /// </summary>
        public static string OSName
        {
            get
            {
                int p = (int) Environment.OSVersion.Platform;
                return ((p == 4) || (p == 6) || (p == 128)) ? "Linux" : "Windows";
            }
        }

        /// <summary>
        /// Get the application version string
        /// </summary>
        public static string VersionString
        {
            get
            {
                if (_versionString == null)
                {
                    Version ver = Assembly.GetEntryAssembly().GetName().Version;
                    string betaString = string.Empty;

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    object[] attributes = assembly.GetCustomAttributes(true);
                    object configRaw = attributes.FirstOrDefault(a => a is AssemblyConfigurationAttribute);
                    if (configRaw != null)
                    {
                        AssemblyConfigurationAttribute config = (AssemblyConfigurationAttribute) configRaw;
                        if (!string.IsNullOrWhiteSpace(config.Configuration))
                        {
                            betaString = string.Format(" {0}", config.Configuration);
                        }
                    }
                    _versionString = string.Format("{0}.{1}.{2}{3}", ver.Major, ver.Minor, ver.Build, betaString);
                }
                return _versionString;
            }
        }

        /// <summary>
        /// Get or set the flag which indicates whether we start online.
        /// </summary>
        public static bool StartupOnline { get; set; }

        /// <summary>
        /// Get or set the address of the message first displayed when CIXReader starts.
        /// </summary>
        public static string StartupAddress { get; set; }

        /// <summary>
        /// Get or set the last user name.
        /// </summary>
        private static string LastUser { get; set; }

        /// <summary>
        /// Return the CIXReader logo
        /// </summary>
        public static Image CIXReaderLogo
        {
            get
            {
                DateTime today = DateTime.Now;
                if (today.Month == 12 && today.Day >= 18 && today.Day <= 27)
                {
                    return Resources.ChristmasLogo;
                }
                return Resources.CIXReaderLogo;
            }
        }

        /// <summary>
        /// Do all application pre-run initialisation.
        /// </summary>
        /// <returns>Return true if initialisation succeeded, or false if any failed</returns>
        private static bool Initialise()
        {
            return InitialiseFirstRun() && InitializeDatabase();
        }

        /// <summary>
        /// Exit and restart CIXReader.
        /// </summary>
        private static void RelaunchProcess()
        {
            string cmdLine = Environment.CommandLine;
            string workingDir = Environment.CurrentDirectory;
            string command;
            string arguments;

            // generate the batch file path
            if (MonoHelper.IsMono)
            {
                string cmd = Environment.ExpandEnvironmentVariables("/tmp/" + Guid.NewGuid() + ".sh");
                command = "/bin/sh";
                arguments = cmd;

                StreamWriter write = new StreamWriter(cmd);
                write.WriteLine("#!sh");
                write.WriteLine("mono " + cmdLine);
                write.Close();
            }
            else
            {
                string cmd = Environment.ExpandEnvironmentVariables("%temp%\\" + Guid.NewGuid() + ".cmd");
                command = cmd;
                arguments = string.Empty;

                StreamWriter write = new StreamWriter(cmd);
                write.WriteLine(cmdLine);
                write.Close();
            }

            // start the installer helper
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    WorkingDirectory = workingDir,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            process.Start();

            // quit the app
            Environment.Exit(0);
        }

        /// <summary>
        /// Parse the command line argument passed. The syntax so far is:
        /// 
        /// [path_to_database]
        /// /cix=[address]
        /// /offline
        /// 
        /// </summary>
        /// <param name="commandLine"></param>
        private static bool ParseCommandLine(string commandLine)
        {
            string [] args = commandLine.Split(' ');
            foreach (string arg in args)
            {
                if (arg.Equals("/offline"))
                {
                    _forceOffline = true;
                    continue;
                }
                if (arg.StartsWith("/cix=", StringComparison.Ordinal))
                {
                    _address = arg.Substring(5).Trim('\"');
                    continue;
                }
                if (!File.Exists(arg))
                {
                    MessageBox.Show(@"Invalid database file name specified: " + arg);
                    return false;
                }
                CIX.HomeFolder = Path.GetDirectoryName(arg);
                _databasePath = arg;
            }
            return true;
        }

        /// <summary>
        /// Do first-run initialisation.
        /// </summary>
        /// <returns>True if we initialised successfully, false otherwise</returns>
        private static bool InitialiseFirstRun()
        {
            if (Settings.CurrentUser.GetBoolean("FirstRun", true))
            {
                int firstRunLicense = 0;

                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CIXOnline Ltd\CIXReader");
                if (regKey != null)
                {
                    firstRunLicense = (int)regKey.GetValue("FirstRunLicense", 0);
                    regKey.Close();
                }

                IsFirstRun = true;

                Settings.CurrentUser.SetBoolean("FirstRun", false);
            }

            return true;
        }

        /// <summary>
        /// Do database initialisation.
        /// </summary>
        /// <returns>True if we initialised successfully, false otherwise</returns>
        private static bool InitializeDatabase()
        {
            string username = LastUser;
            string databasePath = _databasePath;
            string password = null;

            if (databasePath != null)
            {
                username = Path.GetFileNameWithoutExtension(databasePath);
            }
            else
            {
                string docFolder = CIX.HomeFolder;

                if (!string.IsNullOrEmpty(username))
                {
                    databasePath = Path.Combine(docFolder, username + ".cixreader");
                    if (!File.Exists(databasePath))
                    {
                        // The lastuser was mysteriously deleted so clear it and start
                        // again by requesting login.
                        username = null;
                    }
                }

                if (string.IsNullOrEmpty(username))
                {
                    Settings.CurrentUser.SetString("LastUser", string.Empty);

                    Login loginDialog = new Login();
                    if (loginDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        Application.Exit();
                        return false;
                    }
                    username = loginDialog.Username;
                    password = loginDialog.Password;
                }

                databasePath = Path.Combine(docFolder, username + ".cixreader");

                if (!Directory.Exists(docFolder))
                {
                    Directory.CreateDirectory(docFolder);
                }
            }

            // Ensure all tables exist
            if (!CIX.Init(databasePath))
            {
                MessageBox.Show(@"Cannot open database: " + databasePath);
                return false;
            }

            // If a cached username was specified and it matches the username we have then we
            // use that to indicate that the password in the DB is to be used. Otherwise we need
            // the user to authenticate against the database password.
            if (CIX.Password == null || CIX.Username == null)
            {
                // Prompt for credentials if we don't have them and ensure they match
                // what is in the database.
                if (username == null || password == null) {
                    Login loginDialog = new Login {
                        Username = CIX.Username,
                        Password = CIX.Password
                    };

                    if (loginDialog.ShowDialog() == DialogResult.Cancel) {
                        Application.Exit();
                        return false;
                    }

                    username = loginDialog.Username;
                    password = loginDialog.Password;
                }

                CIX.Username = username;
                CIX.Password = password;
            }

            string settingsPath = Path.Combine(CIX.HomeFolder, username + ".ini");
            Preferences.Open(settingsPath);

            if (IsFirstRun) {
                Preferences.StandardPreferences.StartOffline = true;
            }

            InitializeLogFile();

            LogFile.WriteLine("{0} {1} started", Resources.AppTitle, VersionString);
            LogFile.WriteLine("Opened database {0}", databasePath);

            // Compact the database if the time has come
            int cleanupFrequency = Preferences.StandardPreferences.CacheCleanUpFrequency;
            if (cleanupFrequency > 0)
            {
                DateTime cleanupDate = Preferences.StandardPreferences.LastCacheCleanUp;
                cleanupDate = cleanupDate.AddDays(cleanupFrequency);
                if (cleanupDate < DateTime.Now)
                {
                    CIX.CompactDatabase();
                    Preferences.StandardPreferences.LastCacheCleanUp = DateTime.Now;
                }
            }

            // Set initial state
            StartupOnline = (!_forceOffline) && !Preferences.StandardPreferences.StartOffline;
            StartupAddress = _address ?? Preferences.StandardPreferences.LastAddress;

            // Use the beta API if the user has opted into beta.
            APIRequest.UseBetaAPI = Preferences.StandardPreferences.UseBeta;
            return true;
        }

        /// <summary>
        /// Create the log file in the CIXReader home folder.
        /// </summary>
        private static void InitializeLogFile()
        {
            string logFilePath = Path.Combine(CIX.HomeFolder, "cixreader.debug.log");
            try
            {
                if (!Directory.Exists(CIX.HomeFolder))
                {
                    Directory.CreateDirectory(CIX.HomeFolder);
                }
                LogFile.Create(logFilePath);
                LogFile.Enabled = Preferences.StandardPreferences.EnableLogFile;
                LogFile.Archive = Preferences.StandardPreferences.ArchiveLogFile;
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Cannot create log file. " + e.Message);
            }
        }
    }
}