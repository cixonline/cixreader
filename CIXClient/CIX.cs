// *****************************************************
// CIXReader
// CIX.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 12:09 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;
using CIXClient.Tables;

namespace CIXClient
{
    /// <summary>
    /// The Config class encapsulates global CIXClient data.
    /// </summary>
    public static class CIX
    {
        /// <summary>
        /// A lock object for synchronising DB access.
        /// </summary>
        internal static readonly object DBLock = new object();

        private const string PassPhrase = "@#$%^&*";

        /// <summary>
        /// Number of milliseconds between sync.
        /// Minutes x 60 seconds x 1000 milliseconds.
        /// </summary>
        private const int RefreshInterval = 1 * 60 * 1000;

        private static ProfileCollection _profileCollection;
        private static DirectoryCollection _directoryCollection;
        private static ImageRequestorTask _imageRequestorTask;
        private static ConversationCollection _conversationCollection;
        private static RuleCollection _ruleCollection;
        private static FolderCollection _folderCollection;

        private static string _homeFolder;
        private static int _inReport;
        private static bool _onlineState;
        private static bool _initialised;

        private static volatile bool _syncRunning;

        private static Timer _timer;

        private static Globals _globals;
        private static string _unencryptedPassword;

        /// <summary>
        /// Defines the delegate for ProfileUpdated event notifications.
        /// </summary>
        /// <param name="sender">The ProfileTasks object</param>
        /// <param name="e">Additional profile update data</param>
        public delegate void AuthenticationFailedHandler(object sender, EventArgs e);

        /// <summary>
        /// Defines the delegate for MugshotUpdated event notifications.
        /// </summary>
        /// <param name="mugshot">The mugshot that was updated</param>
        public delegate void MugshotUpdatedHandler(Mugshot mugshot);

        /// <summary>
        /// Event handler for notifying when CIX authentication fails.
        /// </summary>
        public static event AuthenticationFailedHandler AuthenticationFailed;

        /// <summary>
        /// Event handler for notifying a delegate of mugshot updates.
        /// </summary>
        public static event MugshotUpdatedHandler MugshotUpdated;

        /// <summary>
        /// Possible responses from the Authenticate call.
        /// </summary>
        public enum AuthenticateResponse
        {
            /// <summary>
            /// Authentication succeeded.
            /// </summary>
            Success,

            /// <summary>
            /// Authentication failed.
            /// </summary>
            Failure,

            /// <summary>
            /// Account is valid but inactivated.
            /// </summary>
            Inactivated,

            /// <summary>
            /// No connection, so cannot authenticate
            /// </summary>
            Unconnected
        }

        /// <summary>
        /// Gets or sets a value indicating whether we're in online or
        /// offline mode.
        /// </summary>
        public static bool Online
        {
            get { return _onlineState && _initialised; }
            set
            {
                if (value != _onlineState)
                {
                    _onlineState = value;

                    Thread t = new Thread(() => Sync(null));
                    t.Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value which specifies the application's home folder, where all data files
        /// are kept.
        /// </summary>
        public static string HomeFolder
        {
            #if __MonoCS__
            get { return _homeFolder ?? (_homeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CIXReader")); }
            #else
            get { return _homeFolder ?? (_homeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CIXReader")); }
            #endif
            set { _homeFolder = value; }
        }

        /// <summary>
        /// Gets or sets the users login username.
        /// </summary>
        public static string Username
        {
            get { return _globals.Username;  }
            set
            {
                _globals.Username = value;
                DB.InsertOrReplace(_globals);
            }
        }

        /// <summary>
        /// Gets or sets the users login password.
        /// </summary>
        public static string Password
        {
            get
            {
                if (_unencryptedPassword == null)
                {
                    if (_globals.Password != null)
                    {
                        _unencryptedPassword = StringCipher.Decrypt(_globals.Password, PassPhrase);
                    }
                }
                return _unencryptedPassword;
            }
            set
            {
                _globals.Password = StringCipher.Encrypt(value, PassPhrase);
                _unencryptedPassword = value;
                DB.InsertOrReplace(_globals);
            }
        }

        /// <summary>
        /// Gets or sets the folder where the UI configuration files are stored
        /// </summary>
        public static string UIConfigFolder { get; set; }

        /// <summary>
        /// Gets the global count of unread messages.
        /// </summary>
        public static int TotalUnread
        {
            get { return ConversationCollection.TotalUnread + FolderCollection.TotalUnread; }
        }

        /// <summary>
        /// Gets the global count of unread priority messages.
        /// </summary>
        public static int TotalUnreadPriority
        {
            get { return ConversationCollection.TotalUnreadPriority + FolderCollection.TotalUnreadPriority; }
        }

        /// <summary>
        /// Gets a FolderCollection object for managing forum items.
        /// </summary>
        public static FolderCollection FolderCollection
        {
            get { return _folderCollection ?? (_folderCollection = new FolderCollection()); }
        }

        /// <summary>
        /// Gets a ProfileCollection object for managing profile items.
        /// </summary>
        public static ProfileCollection ProfileCollection
        {
            get { return _profileCollection ?? (_profileCollection = new ProfileCollection()); }
        }

        /// <summary>
        /// Gets a DirectoryCollection object for querying the directory.
        /// </summary>
        public static DirectoryCollection DirectoryCollection
        {
            get { return _directoryCollection ?? (_directoryCollection = new DirectoryCollection()); }
        }

        /// <summary>
        /// Gets a RuleCollection object for querying the rules.
        /// </summary>
        public static RuleCollection RuleCollection
        {
            get { return _ruleCollection ?? (_ruleCollection = new RuleCollection()); }
        }

        /// <summary>
        /// Gets a InboxTasks object for querying the inbox.
        /// </summary>
        public static ConversationCollection ConversationCollection
        {
            get { return _conversationCollection ?? (_conversationCollection = new ConversationCollection()); }
        }

        /// <summary>
        /// Gets a ImageRequestorTask object for managing image requests.
        /// </summary>
        public static ImageRequestorTask ImageRequestorTask
        {
            get
            {
                if (_imageRequestorTask == null)
                {
                    _imageRequestorTask = new ImageRequestorTask();
                    _imageRequestorTask.Load();
                }
                return _imageRequestorTask;
            }
        }

        /// <summary>
        /// Gets the current version of this client.
        /// </summary>
        internal static int CurrentVersion
        {
            get { return 8; }
        }

        /// <summary>
        /// Gets the global SQLite database handle.
        /// </summary>
        internal static SQLiteConnection DB { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether tasks are running
        /// </summary>
        internal static bool IsTasksRunning { get; set; }

        /// <summary>
        /// Gets or sets the date of the last sync
        /// </summary>
        internal static DateTime LastSyncDate
        {
            get { return _globals.LastSyncDate; }
            set
            {
                _globals.LastSyncDate = value;
                DB.Update(_globals);
            }
        }

        /// <summary>
        /// Compact the database.
        /// </summary>
        public static void CompactDatabase()
        {
            if (DB != null)
            {
                DB.Execute("vacuum");
            }
        }

        /// <summary>
        /// Suspend sync activities.
        /// </summary>
        public static void SuspendTasks()
        {
            IsTasksRunning = false;
        }

        /// <summary>
        /// Resume sync activities.
        /// </summary>
        public static void ResumeTasks()
        {
            IsTasksRunning = true;
        }

        /// <summary>
        /// Start the periodic sync tasks
        /// </summary>
        public static void StartTask()
        {
            _timer = new Timer(Sync, null, 200, RefreshInterval);
        }

        /// <summary>
        /// Initialise CIXClient.
        /// </summary>
        /// <param name="databasePath">The path to the database to use</param>
        /// <returns>True if we succeeded initialisation, false otherwise.</returns>
        public static bool Init(string databasePath)
        {
            DB = new SQLiteConnection(databasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
            if (DB == null)
            {
                return false;
            }
            NativeMethods.Config(SQLite3.ConfigOption.Serialized);

            DB.RunInTransaction(() =>
            {
                DB.CreateTable<Globals>();
                DB.CreateTable<InboxConversation>();
                DB.CreateTable<InboxMessage>();
                DB.CreateTable<Mugshot>();
                DB.CreateTable<DirCategory>();
                DB.CreateTable<DirForum>();
                DB.CreateTable<Profile>();
                DB.CreateTable<Folder>();
                DB.CreateTable<CIXMessage>();
            });

            _onlineState = true;
            _initialised = true;

            IsTasksRunning = true;

            // Persist any globals from the database
            _globals = DB.Table<Globals>().FirstOrDefault() ?? new Globals();

            // Upgrade from v0 database
            if (_globals.Version < 1)
            {
                DB.DeleteAll<Folder>();
            }

            // Upgrade from v1 database
            if (_globals.Version < 2)
            {
                DB.DeleteAll<CIXMessage>();
            }

            // Upgrade from v2 database
            if (_globals.Version < 3)
            {
                _globals.Password = null;
            }

            // Upgrade from v3 database
            if (_globals.Version < 4)
            {
                DB.Execute("DROP TABLE IF EXISTS DirForumMap");
                DB.DropTable<DirCategory>();
                DB.DropTable<DirForum>();
                DB.CreateTable<DirCategory>();
                DB.CreateTable<DirForum>();
            }

            // Upgrade from v5 database
            // (No upgrade from v4 needed as the same table has changed.)
            if (_globals.Version < 6)
            {
                DB.Execute("DROP TABLE IF EXISTS DirForums");
                DB.Execute("DROP TABLE IF EXISTS Moderator");
                DB.Execute("DROP TABLE IF EXISTS Participant");
                DB.CreateTable<DirForum>();
            }

            // Upgrade from v6 database
            // Replace old Flags scheme with distinct columns for each flag for improved performance.
            if (_globals.Version < 7)
            {
                // Replace Flags with individual state columns. Migrate existing non-zero flags to their
                // appropriate state columns and update the folder column accordingly for unread messages
                List<OldCIXMessage> cixMessages = DB.Query<OldCIXMessage>("SELECT * FROM CIXMessage WHERE Flags<>0 ORDER BY TopicID");
                List<Folder> folders = new List<Folder>(DB.Table<Folder>().ToArray());
                List<Folder> allFolders = new List<Folder>();
                Folder folder = null;

                DB.BeginTransaction();
                foreach (OldCIXMessage message in cixMessages)
                {
                    CIXMessage cixMessage = new CIXMessage
                    {
                        ID = message.ID,
                        RemoteID = message.RemoteID,
                        CommentID = message.CommentID,
                        TopicID = message.TopicID,
                        RootID = message.RootID,
                        Body = message.Body,
                        Author = message.Author,
                        Date = message.Date
                    };

                    if (message.Flags.HasFlag(CIXMessageFlags.Priority))
                    {
                        cixMessage.Priority = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.Unread))
                    {
                        cixMessage.Unread = true;
                        if (folder == null || folder.ID != message.TopicID)
                        {
                            folder = folders[message.TopicID];
                            allFolders.Add(folder);
                        }
                        folder.Unread += 1;
                        if (cixMessage.Priority)
                        {
                            folder.UnreadPriority += 1;
                        }
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.Starred))
                    {
                        cixMessage.Starred = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.ReadLocked))
                    {
                        cixMessage.ReadLocked = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.Ignored))
                    {
                        cixMessage.Ignored = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.ReadPending))
                    {
                        cixMessage.ReadPending = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.PostPending))
                    {
                        cixMessage.PostPending = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.StarPending))
                    {
                        cixMessage.StarPending = true;
                    }
                    if (message.Flags.HasFlag(CIXMessageFlags.WithdrawPending))
                    {
                        cixMessage.WithdrawPending = true;
                    }
                    DB.Update(cixMessage);
                }
                DB.UpdateAll(allFolders);
                DB.Commit();
            }

            // Set new version
            if (_globals.Version < CurrentVersion)
            {
                _globals.Version = CurrentVersion;
                DB.Update(_globals);
            }

            return true;
        }

        /// <summary>
        /// Close the configuration
        /// </summary>
        public static void Close()
        {
            _initialised = false;
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            _profileCollection = null;
            _folderCollection = null;
            _directoryCollection = null;
            _conversationCollection = null;

            if (_imageRequestorTask != null)
            {
                _imageRequestorTask.Unload();
                _imageRequestorTask = null;
            }

            // Wait for a stray sync to finish.
            while (_syncRunning)
            {
                Thread.Sleep(250);
            }

            // Clear the username cache.
            _globals.Username = null;
            _globals.Password = null;

            if (DB != null)
            {
                try
                {
                    DB.Close();
                }
                catch (SQLiteException e)
                {
                    LogFile.WriteLine("Config.Close: SQLite exception {0}", e.Message);
                }
                DB = null;
            }
        }

        /// <summary>
        /// Authenticate the given username and password by making a simple API call
        /// and validating the results.
        /// This is static because at the point we're called, neither the database nor
        /// the full ForumsTask is initialised. So it needs to be lightweight and fast.
        /// </summary>
        /// <param name="username">A CIX username</param>
        /// <param name="password">The password for the CIX username</param>
        /// <returns>True if we're authenticated, false otherwise</returns>
        public static AuthenticateResponse Authenticate(string username, string password)
        {
            AuthenticateResponse response = AuthenticateResponse.Unconnected;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    HttpWebRequest request = APIRequest.GetWithCredentials("user/account", username, password, APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(request);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Account));
                            Account accountDetails = (Account)serializer.Deserialize(reader);

                            response = (accountDetails.Type == "activate")
                                ? AuthenticateResponse.Inactivated
                                : AuthenticateResponse.Success;
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpStatusCode statusCode = ((HttpWebResponse)e.Response).StatusCode;
                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            response = AuthenticateResponse.Failure;
                        }
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// Retrieve the authenticated user's account details.
        /// </summary>
        public static void RefreshUserAccount()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    LogFile.WriteLine("Retrieving user account details");

                    HttpWebRequest wrGeturl = APIRequest.Get("user/account", APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Account));
                            Account accountDetails = (Account)serializer.Deserialize(reader);

                            // Notify about the account update.
                            FolderCollection.NotifyAccountUpdated(accountDetails);
                        }
                    }
                }
                catch (Exception e)
                {
                    ReportServerExceptions("CIX.UserAccountDetails", e);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Return an array of online users.
        /// </summary>
        public static void RefreshOnlineUsers()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    LogFile.WriteLine("Retrieving list of online users");

                    HttpWebRequest wrGeturl = APIRequest.Get("user/who", APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Whos));
                            Whos onlineList = (Whos)serializer.Deserialize(reader);

                            // Notify about the account update.
                            FolderCollection.NotifyOnlineUsersUpdated(onlineList);
                        }
                    }
                }
                catch (Exception e)
                {
                    ReportServerExceptions("CIX.RefreshOnlineUsers", e);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Notify that mugshot has been updated.
        /// </summary>
        /// <param name="mugshot">The mugshot that was updated</param>
        internal static void NotifyMugshotUpdated(Mugshot mugshot)
        {
            if (MugshotUpdated != null)
            {
                MugshotUpdated(mugshot);
            }
        }

        /// <summary>
        /// Handle server exceptions and log these with details complete with the name of the
        /// folder in which the exception occurred. For HTTP authentication errors we also
        /// invoke the authentication failed event handler.
        /// </summary>
        /// <param name="source">The name of the function raising the exception</param>
        /// <param name="folder">The folder in which the exception occurred</param>
        /// <param name="e">The exception details</param>
        internal static void ReportServerExceptions(string source, Folder folder, Exception e)
        {
            if (folder != null)
            {
                source = string.Format("{0} in {1}", source, folder.Name);
            }
            ReportServerExceptions(source, e);
        }

        /// <summary>
        /// Handle server exceptions and log these with details. For HTTP authentication
        /// errors we also invoke the authentication failed event handler.
        /// </summary>
        /// <param name="source">The name of the function raising the exception</param>
        /// <param name="e">The exception details</param>
        internal static void ReportServerExceptions(string source, Exception e)
        {
            WebException webException = e as WebException;
            if (webException != null)
            {
                if (Interlocked.CompareExchange(ref _inReport, 1, 0) == 0)
                {
                    if (webException.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpStatusCode statusCode = ((HttpWebResponse)webException.Response).StatusCode;
                        if (statusCode == HttpStatusCode.Unauthorized && AuthenticationFailed != null)
                        {
                            LogFile.WriteLine("Authentication failed.Requesting new credentials from the client.");
                            AuthenticationFailed(null, new EventArgs());
                            _inReport = 0;
                            return;
                        }
                    }
                    _inReport = 0;
                }
            }
            LogFile.WriteLine("{0} : Caught exception {1}", source, e.Message);
        }

        /// <summary>
        /// Sync the list of forums and topics to which the user is joined. Does
        /// nothing if the user is offline.
        /// </summary>
        /// <param name="obj">The timer object</param>
        private static void Sync(object obj)
        {
            if (Online && IsTasksRunning)
            {
                _syncRunning = true;
                FolderCollection.Sync();
                ConversationCollection.Sync();
                DirectoryCollection.Sync();
                _syncRunning = false;
            }
        }
    }
}