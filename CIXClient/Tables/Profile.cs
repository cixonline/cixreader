// *****************************************************
// CIXReader
// Profile.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/09/2013 1:40 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;

namespace CIXClient.Tables
{
    /// <summary>
    /// The Profile table stores the profile for an individual user.
    /// </summary>
    public sealed class Profile
    {
        /// <summary>
        /// Notification flags
        /// </summary>
        [Flags]
        public enum NotificationFlags
        {
            /// <summary>
            /// Notify when a private message is sent to the user
            /// </summary>
            PMNotification = 1,

            /// <summary>
            /// Notify when the user is tagged in a message
            /// </summary>
            TagNotification = 2
        };

        /// <summary>
        /// An unique ID that identifies this profile.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// The username for which this profile.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's e-mail address. May be empty.
        /// </summary>
        public string EMailAddress { get; set; }

        /// <summary>
        /// The user's full name, which may be empty.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The date and time when the user was last on the server.
        /// </summary>
        public DateTime LastOn { get; set; }

        /// <summary>
        /// The users geographical location, typically a home town. This
        /// may be blank.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The user's gender. A blank value equates to "Don't want to say".
        /// A string beginning with 'M' is male and one beginning with 'F' is female.
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Some descriptive details about the user. This may be blank if they have
        /// not provided any information.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Specifies whether this user profile and resume are pending upload.
        /// </summary>
        public bool Pending { get; set; }

        /// <summary>
        /// Get or set the user's notification flags.
        /// </summary>
        public NotificationFlags Flags { get; set; }

        /// <summary>
        /// Return the user's full name if it is known, or the CIX username
        /// name otherwise.
        /// </summary>
        [Ignore]
        public string FriendlyName
        {
            get { return (string.IsNullOrWhiteSpace(FullName)) ? Username : FullName; }
        }

        /// <summary>
        /// Retrieve the profile for the specified user. If the profile is cached in the database
        /// it returns the full details immediately. Otherwise it creates and returns an empty
        /// profile.
        /// </summary>
        /// <param name="username">The user whose profile is requested</param>
        /// <returns>A profile for the specified user. May be incomplete if the full profile
        /// is not cached in the database.</returns>
        public static Profile ProfileForUser(string username)
        {
            // Enforce lowercase usernames
            username = username.ToLower();

            ProfileCollection prc = CIX.ProfileCollection;
            Profile profile = prc.Get(username);
            if (profile == null)
            {
                profile = new Profile
                {
                    Username = username,
                };
                prc.Add(profile);
            }

            return profile;
        }

        /// <summary>
        /// Update this profile on the server.
        /// </summary>
        public void Update()
        {
            Pending = true;

            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }

            CIX.ProfileCollection.NotifyProfileUpdated(this);
            Sync();
        }

        /// <summary>
        /// Refresh this profile from the server.
        /// </summary>
        public void Refresh()
        {
            if (CIX.Online)
            {
                LogFile.WriteLine("Profile for {0} requested from server", Username);

                try
                {
                    // First get the basic profile information
                    string profileUrl = Username == CIX.Username ? "user/profile" : "user/" + Username + "/profile";
                    HttpWebRequest wrGeturl = APIRequest.Get(profileUrl, APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof (ProfileSmall));
                            ProfileSmall profileSet = (ProfileSmall) serializer.Deserialize(reader);

                            Location = profileSet.Location;
                            FullName = profileSet.Fname + " " + profileSet.Sname;
                            EMailAddress = profileSet.Email;
                            LastOn = DateTime.Parse(profileSet.LastOn);
                            Sex = profileSet.Sex;
                            Flags = (NotificationFlags)profileSet.Flags;
                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(this);
                            }
                            LogFile.WriteLine("Profile for {0} updated from server", Username);
                        }
                    }

                    wrGeturl = APIRequest.Get("user/" + Username + "/resume", APIRequest.APIFormat.XML);
                    string resumeText = APIRequest.ReadResponseString(wrGeturl);

                    if (!string.IsNullOrEmpty(resumeText) && CIX.DB != null)
                    {
                        About = resumeText.UnescapeXml();
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(this);
                        }

                        LogFile.WriteLine("Resume for {0} updated from server", Username);

                        CIX.ProfileCollection.NotifyProfileUpdated(this);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("Profile.Refresh", e);
                }
                Debug.Flush();
            }
        }

        /// <summary>
        /// Sync this profile with the server.
        /// </summary>
        public void Sync()
        {
            if (CIX.Online && Username == CIX.Username)
            {
                string[] splitName = FullName.Split(new[] { ' ' }, 2);

                LogFile.WriteLine("Uploading profile for {0} to server", CIX.Username);

                ProfileSet newProfileSmall = new ProfileSet
                {
                    Fname = splitName[0],
                    Sname = (splitName.Length > 1) ? splitName[1] : "",
                    Location = Location,
                    Email = EMailAddress,
                    Flags = (int)Flags,
                    Sex = Sex
                };

                try
                {
                    HttpWebRequest wrPosturl = APIRequest.Post("user/setprofile", APIRequest.APIFormat.XML, newProfileSmall);
                    string responseString = APIRequest.ReadResponseString(wrPosturl);

                    if (responseString == "Success")
                    {
                        Pending = false;
                        LogFile.WriteLine("Profile successfully uploaded");
                    }

                    wrPosturl = APIRequest.Post("user/setresume", APIRequest.APIFormat.XML, About);
                    responseString = APIRequest.ReadResponseString(wrPosturl);

                    if (responseString == "True")
                    {
                        Pending = false;
                        LogFile.WriteLine("Resume successfully uploaded");
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("Profile.Sync", e);
                }
            }
        }
    }
}