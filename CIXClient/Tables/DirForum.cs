﻿// *****************************************************
// CIXReader
// DirForum.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 11:30 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;
using CIXClient.Properties;

namespace CIXClient.Tables
{
    /// <summary>
    /// The DirForum table stores a single Forum from the
    /// directory.
    /// </summary>
    public sealed class DirForum
    {
        private int isParticipantsRefreshing;
        private int isModeratorsRefreshing;

        /// <summary>
        /// Gets or sets the auto-generated ID of this Forum.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the unique Forum name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short title of the Forum.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short description of the Forum.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Gets or sets the type of the Forum ("o" is open, "c" is closed).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the category to which this Forum belongs.
        /// </summary>
        public string Cat { get; set; }

        /// <summary>
        /// Gets or sets the sub-category to which this Forum belongs.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Gets or sets the recent number of messages in the Forum.
        /// </summary>
        public int Recent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether changes to the Forum details are pending update.
        /// </summary>
        public bool DetailsPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a refresh of Forum details are pending.
        /// </summary>
        public bool RefreshPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a join is pending.
        /// </summary>
        public bool JoinPending { get; set; }

        /// <summary>
        /// Gets or sets the list of participants of this Forum
        /// </summary>
        public string Parts { get; set; }

        /// <summary>
        /// Gets or sets the list of moderators of this Forum
        /// </summary>
        public string Mods { get; set; }

        /// <summary>
        /// Gets or sets the list of moderators being added to this Forum
        /// </summary>
        public string AddedMods { get; set; }

        /// <summary>
        /// Gets or sets the list of participants being added to this Forum
        /// </summary>
        public string AddedParts { get; set; }

        /// <summary>
        /// Gets or sets the list of moderators being removed from this Forum
        /// </summary>
        public string RemovedMods { get; set; }

        /// <summary>
        /// Gets or sets the list of participants being removed from this Forum
        /// </summary>
        public string RemovedParts { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this Forum is closed or private
        /// </summary>
        [Ignore]
        public bool IsClosed
        {
            get { return Type != "o"; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the authenticated user is a moderator of
        /// this forum.
        /// </summary>
        [Ignore]
        public bool IsModerator
        {
            get { return Moderators().Contains(CIX.Username); }
        }

        /// <summary>
        /// Gets a value indicating whether there are some pending actions on the forum
        /// </summary>
        [Ignore]
        public bool HasPending
        {
            get { return JoinPending || DetailsPending || RefreshPending; }
        }

        /// <summary>
        /// Gets or sets a string array of added participants
        /// </summary>
        /// <returns>String array of added participants</returns>
        [Ignore]
        public IEnumerable<string> AddedParticipants
        {
            get
            {
                return string.IsNullOrEmpty(AddedParts) ? new string[0] : AddedParts.Split(',');
            }
            set
            {
                AddedParts = value == null ? string.Empty : string.Join(",", value);
            }
        }

        /// <summary>
        /// Gets or sets a string array of removed participants
        /// </summary>
        /// <returns>String array of removed participants</returns>
        [Ignore]
        public IEnumerable<string> RemovedParticipants
        {
            get
            {
                return string.IsNullOrEmpty(RemovedParts) ? new string[0] : RemovedParts.Split(',');
            }
            set
            {
                RemovedParts = value == null ? string.Empty : string.Join(",", value);
            }
        }

        /// <summary>
        /// Gets or sets a string array of added moderators
        /// </summary>
        /// <returns>String array of added moderators</returns>
        [Ignore]
        public IEnumerable<string> AddedModerators
        {
            get
            {
                return string.IsNullOrEmpty(AddedMods) ? new string[0] : AddedMods.Split(',');
            }
            set
            {
                AddedMods = value == null ? string.Empty : string.Join(",", value);
            }
        }

        /// <summary>
        /// Gets or sets a string array of removed moderators
        /// </summary>
        /// <returns>String array of removed moderators</returns>
        [Ignore]
        public IEnumerable<string> RemovedModerators
        {
            get
            {
                return string.IsNullOrEmpty(RemovedMods) ? new string[0] : RemovedMods.Split(',');
            }
            set
            {
                RemovedMods = value == null ? string.Empty : string.Join(",", value);
            }
        }

        /// <summary>
        /// Join the specified forum on the server. Forum may already exist locally in which
        /// case this is a re-join action.
        /// </summary>
        public void Join()
        {
            if (!CIX.Online)
            {
                JoinPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            else
            {
                Thread t = new Thread(InternalJoin);
                t.Start();
            }
        }

        /// <summary>
        /// Retrieve the participants list
        /// </summary>
        /// <returns>A string array of participants</returns>
        public string[] Participants()
        {
            if (string.IsNullOrEmpty(Parts) && CIX.Online && Interlocked.CompareExchange(ref isParticipantsRefreshing, 1, 0) == 0)
            {
                RefreshParticipants();
            }
            return string.IsNullOrEmpty(Parts) ? new string[0] : Parts.Split(',');
        }

        /// <summary>
        /// Retrieve the moderators list
        /// </summary>
        /// <returns>A string array of moderators</returns>
        public string[] Moderators()
        {
            if (string.IsNullOrEmpty(Mods) && CIX.Online && Interlocked.CompareExchange(ref isModeratorsRefreshing, 1, 0) == 0)
            {
                RefreshModerators();
            }
            return string.IsNullOrEmpty(Mods) ? new string[0] : Mods.Split(',');
        }

        private void RefreshParticipants() 
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    List<string> participants = new List<string>();

                    LogFile.WriteLine("Updating list of participants for {0}", Name);

                    string urlFormat = string.Format("forums/{0}/participants", FolderCollection.EncodeForumName(Name));

                    HttpWebRequest wrGeturl = APIRequest.GetWithQuery(urlFormat, APIRequest.APIFormat.XML, "maxresults=10000");
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Parts));
                            Parts allParticipants = (Parts)serializer.Deserialize(reader);

                            participants.AddRange(allParticipants.Users.Select(part => part.Name));

                            Parts = string.Join(",", participants);
                                                       
                            LogFile.WriteLine("List of participants for {0} updated", Name);

                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(this);
                            }
                        }
                    }
                    CIX.DirectoryCollection.NotifyParticipantsUpdated(this);
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirForum.Participants", e);
                }
                isParticipantsRefreshing = 0;
            });
            t.Start();
        }

        /// <summary>
        /// Refresh the list of moderators
        /// </summary>
        private void RefreshModerators() 
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    List<string> moderators = new List<string>();

                    LogFile.WriteLine("Updating list of moderators for {0}", Name);

                    string urlFormat = string.Format("forums/{0}/moderators", FolderCollection.EncodeForumName(Name));

                    HttpWebRequest wrGeturl = APIRequest.GetWithQuery(urlFormat, APIRequest.APIFormat.XML, "maxresults=10000");
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ForumMods));
                            ForumMods allModerators = (ForumMods)serializer.Deserialize(reader);

                            moderators.AddRange(allModerators.Mods.Select(mod => mod.Name));

                            Mods = string.Join(",", moderators);

                            LogFile.WriteLine("List of moderators for {0} updated", Name);

                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(this);
                            }
                        }
                    }
                    CIX.DirectoryCollection.NotifyModeratorsUpdated(this);
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirForum.Moderators", e);
                }
                isModeratorsRefreshing = 0;
            });
            t.Start();
        }

        /// <summary>
        /// Request admittance to this forum
        /// </summary>
        public void RequestAdmittance()
        {
            if (CIX.Online)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        StringBuilder textTemplate = new StringBuilder(Resources.AdmissionRequestTemplate1);
                        textTemplate.Replace("$username$", CIX.Username);
                        textTemplate.Replace("$forum$", Name);

                        StringBuilder htmlTemplate = new StringBuilder(Resources.AdmissionRequestTemplate);
                        htmlTemplate.Replace("$username$", CIX.Username);
                        htmlTemplate.Replace("$forum$", Name);

                        SendMail sendMail = new SendMail
                        {
                            Text = textTemplate.ToString(),
                            HTML = htmlTemplate.ToString()
                        };

                        LogFile.WriteLine("Requesting admission to forum {0}", Name);

                        string url = string.Format("moderator/{0}/sendmessage", Name);
                        HttpWebRequest postUrl = APIRequest.Post(url, APIRequest.APIFormat.XML, sendMail);
                        string responseString = APIRequest.ReadResponseString(postUrl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("Successfully sent admittance request for forum {0}", Name);
                        }
                    }
                    catch (Exception e)
                    {
                        CIX.ReportServerExceptions("DirForum.RequestAdmittance", e);
                    }
                });
                t.Start();
            }
        }

        /// <summary>
        /// Refresh the list of moderators and participants for this forum.
        /// </summary>
        public void Refresh()
        {
            if (!CIX.Online)
            {
                RefreshPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            else
            {
                Mods = string.Empty;
                Parts = string.Empty;

                RefreshParticipants();
                RefreshModerators();
            }
        }

        /// <summary>
        /// Update this forum in the database and also sync with the server if
        /// we're online.
        /// </summary>
        public void Update()
        {
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.DirectoryCollection.NotifyForumUpdated(this);
            if (!CIX.Online)
            {
                DetailsPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            else
            {
                SyncDetails();
                SyncParticipantsAndModerators();
            }
        }

        /// <summary>
        /// Called to sync this forum if any details are
        /// pending.
        /// </summary>
        internal void Sync()
        {
            if (RefreshPending)
            {
                Mods = string.Empty;
                Parts = string.Empty;

                CIX.DirectoryCollection.RefreshForum(Name);
                RefreshParticipants();
                RefreshModerators();

                RefreshPending = false;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            if (DetailsPending)
            {
                SyncDetails();
            }
            if (JoinPending)
            {
                InternalJoin();
            }
            SyncParticipantsAndModerators();
        }

        /// <summary>
        /// Join the specified forum.
        /// </summary>
        private void InternalJoin()
        {
            try
            {
                LogFile.WriteLine("Joining forum {0}", Name);

                HttpWebRequest request = APIRequest.GetWithQuery("forums/" + FolderCollection.EncodeForumName(Name) + "/join", APIRequest.APIFormat.XML, "mark=true");

                string responseString = APIRequest.ReadResponseString(request);
                if (responseString == "Success")
                {
                    LogFile.WriteLine("Successfully joined forum {0}", Name);

                    Folder folder = CIX.FolderCollection.Get(-1, Name);
                    if (folder == null)
                    {
                        folder = new Folder { Name = Name, Flags = FolderFlags.Recent, ParentID = -1 };
                        CIX.FolderCollection.Add(folder);
                    }

                    folder.Flags &= ~FolderFlags.Resigned;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(folder);
                    }

                    CIX.DirectoryCollection.NotifyForumJoined(folder);

                    CIX.FolderCollection.Refresh(false);
                }
                JoinPending = false;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("DirForum.Join", e);
            }
        }

        /// <summary>
        /// Sync the additions or removal of moderators and participants
        /// </summary>
        private void SyncParticipantsAndModerators()
        {
            if (!string.IsNullOrWhiteSpace(AddedParts) || !string.IsNullOrWhiteSpace(RemovedParts))
            {
                SyncParticipants();
            }
            if (!string.IsNullOrWhiteSpace(AddedMods) || !string.IsNullOrWhiteSpace(RemovedMods))
            {
                SyncModerators();
            }
        }

        /// <summary>
        /// Sync forum details with the server.
        /// </summary>
        private void SyncDetails()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    LogFile.WriteLine("Updating forum {0} to server", Name);
                    Forum newForum = new Forum
                    {
                        Title = Title,
                        Name = Name,
                        Description = Desc,
                        Category = Cat,
                        SubCategory = Sub,
                        Type = Type
                    };

                    HttpWebRequest postUrl = APIRequest.Post("moderator/forumupdate", APIRequest.APIFormat.XML, newForum);
                    string responseString = APIRequest.ReadResponseString(postUrl);

                    if (responseString == "Success")
                    {
                        LogFile.WriteLine("Forum {0} successfully updated", Name);

                        DetailsPending = false;
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(this);
                        }
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirForum.SyncDetails", e);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Synchronise participants list changes with the server
        /// </summary>
        private void SyncParticipants()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    string encodedForumName = FolderCollection.EncodeForumName(Name);
                    foreach (string part in AddedParticipants)
                    {
                        string url = string.Format("moderator/{0}/{1}/partadd", encodedForumName, part);
                        HttpWebRequest getUrl = APIRequest.Get(url, APIRequest.APIFormat.XML);
                        string responseString = APIRequest.ReadResponseString(getUrl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("User {0} successfully added to {1}", part, Name);
                        }
                    }
                    foreach (string part in RemovedParticipants)
                    {
                        string url = string.Format("moderator/{0}/{1}/partrem", encodedForumName, part);
                        HttpWebRequest getUrl = APIRequest.Get(url, APIRequest.APIFormat.XML);
                        string responseString = APIRequest.ReadResponseString(getUrl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("User {0} successfully removed from {1}", part, Name);
                        }
                    }

                    // Clear out the current participant lists to force a refresh from the server
                    AddedParts = string.Empty;
                    RemovedParts = string.Empty;
                    Parts = string.Empty;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(this);
                    }

                    // Notify interested parties that the participant list has changed
                    CIX.DirectoryCollection.NotifyParticipantsUpdated(this);
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirForum.SyncParticipants", e);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Synchronise moderators list changes with the server
        /// </summary>
        private void SyncModerators()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    string encodedForumName = FolderCollection.EncodeForumName(Name);
                    foreach (string part in AddedModerators)
                    {
                        string url = string.Format("moderator/{0}/{1}/modadd", encodedForumName, part);
                        HttpWebRequest getUrl = APIRequest.Get(url, APIRequest.APIFormat.XML);
                        string responseString = APIRequest.ReadResponseString(getUrl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("Moderator {0} successfully added to {1}", part, Name);
                        }
                    }
                    foreach (string part in RemovedModerators)
                    {
                        string url = string.Format("moderator/{0}/{1}/modrem", encodedForumName, part);
                        HttpWebRequest getUrl = APIRequest.Get(url, APIRequest.APIFormat.XML);
                        string responseString = APIRequest.ReadResponseString(getUrl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("Moderator {0} successfully removed from {1}", part, Name);
                        }
                    }

                    // Clear out the current moderators lists to force a refresh from the server
                    AddedMods = string.Empty;
                    RemovedMods = string.Empty;
                    Mods = string.Empty;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(this);
                    }

                    // Notify interested parties that the moderators list has changed
                    CIX.DirectoryCollection.NotifyModeratorsUpdated(this);
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirForum.SyncModerators", e);
                }
            });
            t.Start();
        }
    }
}