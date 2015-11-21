// *****************************************************
// CIXReader
// Mugshot.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 4:58 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using CIXClient.Database;
using CIXClient.Properties;

namespace CIXClient.Tables
{
    /// <summary>
    /// The Mugshot table caches all mugshots.
    /// </summary>
    public sealed class Mugshot
    {
        private Image _realImage;
        private byte[] _image;

        /// <summary>
        /// The CIX username of the person whose mugshot this identifies.
        /// </summary>
        [PrimaryKey, Indexed]
        public string Username { get; set; }

        /// <summary>
        /// The actual mugshot bytes.
        /// </summary>
        public byte[] Image
        {
            get { return _image; } 
            set { 
                _image = value;
                _realImage = null;
            }
        }

        /// <summary>
        /// A flag which indicates if the mugshot is pending uploading.
        /// </summary>
        public bool Pending { get; set; }

        /// <summary>
        /// The date and time when this mugshot was created.
        /// </summary>
        public DateTime CreationTime { get; set; }

        // Maximum mugshot dimensions
        private const int MaxMugshotWidth = 100;
        private const int MaxMugshotHeight = 100;

        private static bool _loadedExternalMugshots;

        // Mugshot cache, for performance
        private static readonly Dictionary<string, Mugshot> _cache = new Dictionary<string, Mugshot>();

        /// <summary>
        /// Return the mugshot image as an Image object
        /// </summary>
        [Ignore]
        public Image RealImage
        {
            get
            {
                if (_realImage == null)
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(typeof (Image));
                    _realImage = (Image) tc.ConvertFrom(Image);
                }
                return _realImage;
            }
        }

        /// <summary>
        /// Retrieve the mugshot image for the specified user.
        /// 
        /// If no mugshot is stored locally then we will attempt to retrieve one from the server
        /// asynchronously. The caller will need to add to the MugshotUpdated to be notified when
        /// the image is retrieved. A default image is returned by this function in the meantime.
        /// 
        /// If the specified user has no mugshot then the default image is returned.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="refresh">True if we force refresh the mugshot</param>
        /// <returns>An image for the mugshot to be used</returns>
        public static Mugshot MugshotForUser(string username, bool refresh)
        {
            Mugshot mugshot = null;

            if (username != null)
            {
                if (!_loadedExternalMugshots)
                {
                    LoadExternalMugshots();
                    _loadedExternalMugshots = true;
                }

                if (_cache.ContainsKey(username))
                {
                    mugshot = _cache[username];
                }
                else
                {
                    username = username.ToLower();
                    mugshot = CIX.DB.Table<Mugshot>().SingleOrDefault(c => c.Username == username);

                    if (mugshot == null || mugshot.Image.Length == 0)
                    {
                        ImageConverter converter = new ImageConverter();
                        Image mugshotImage = GetDefaultMugshot();
                        mugshot = new Mugshot
                        {
                            Username = username,
                            Image = (byte[]) converter.ConvertTo(mugshotImage, typeof (byte[]))
                        };
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Insert(mugshot);
                        }
                        if (refresh && username != "cix")
                        {
                            mugshot.Refresh();
                        }
                    }
                    _cache[username] = mugshot;
                }
            }
            return mugshot;
        }

        /// <summary>
        /// Set the mugshot for the current user. First change in the database and if we
        /// have network access, change on the server. If there's no network access we
        /// mark this as needing updating in the configuration.
        /// </summary>
        /// <param name="newMugshot">An image specifying the new mugshot</param>
        public void Update(Image newMugshot)
        {
            // Make sure the input image is the maximum size allowed
            newMugshot = newMugshot.ResizeImage(MaxMugshotWidth, MaxMugshotHeight);

            ImageConverter converter = new ImageConverter();

            Mugshot mugshot = CIX.DB.Table<Mugshot>().SingleOrDefault(c => c.Username == CIX.Username);
            if (mugshot != null)
            {
                mugshot.Image = (byte[])converter.ConvertTo(newMugshot, typeof(byte[]));
                mugshot.CreationTime = DateTime.Now;

                lock (CIX.DBLock)
                {
                    CIX.DB.Update(mugshot);
                }
            }
            else
            {
                mugshot = new Mugshot
                {
                    Username = CIX.Username,
                    CreationTime = DateTime.Now,
                    Image = (byte[])converter.ConvertTo(newMugshot, typeof(byte[]))
                };

                lock (CIX.DBLock)
                {
                    CIX.DB.Insert(mugshot);
                }
            }

            _cache.Remove(CIX.Username);
            _realImage = null;

            if (!CIX.Online)
            {
                Pending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(mugshot);
                }
            }
            else
            {
                Sync();
            }
            CIX.NotifyMugshotUpdated(this);
        }

        /// <summary>
        /// Execution task to retrieve a mugshot for the specified username from the server.
        /// </summary>
        public void Refresh()
        {
            if (CIX.Online)
            {
                LogFile.WriteLine("Mugshot for {0} requested from server", Username);

                try
                {
                    HttpWebRequest wrGeturl = APIRequest.Get("user/" + Username + "/mugshot", APIRequest.APIFormat.XML);
                    HttpWebResponse response = (HttpWebResponse)wrGeturl.GetResponse();

                    Stream responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        Byte[] lnByte;

                        using (BinaryReader reader = new BinaryReader(responseStream))
                        {
                            lnByte = reader.ReadBytes((int)response.ContentLength);
                        }

                        // Crop the user mugshot and scale it down so we're only storing a centered
                        // square of the required dimensions.
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Image));
                        Image mugshotImage = (Image)tc.ConvertFrom(lnByte);

                        mugshotImage = mugshotImage.ResizeImage(MaxMugshotWidth, MaxMugshotHeight);

                        ImageConverter converter = new ImageConverter();
                        lnByte = (byte[])converter.ConvertTo(mugshotImage, typeof(byte[]));

                        Image = lnByte;
                        CreationTime = DateTime.Now;

                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(this);
                        }

                        _cache[Username] = this;

                        LogFile.WriteLine("Mugshot for {0} updated from server", Username);
                        CIX.NotifyMugshotUpdated(this);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("Mugshot.Refresh", e);
                }
                Debug.Flush();
            }
        }

        /// <summary>
        /// Sync the user's mugshot as stored in the database with the server.
        /// </summary>
        public void Sync()
        {
            if (CIX.Online && Username == CIX.Username)
            {
                LogFile.WriteLine("Uploading mugshot for {0} to server", Username);
                using (Image img = System.Drawing.Image.FromStream(new MemoryStream(Image)))
                {
                    try
                    {
                        HttpWebRequest wrPosturl = APIRequest.Post("user/setmugshot", APIRequest.APIFormat.XML, img);
                        string responseString = APIRequest.ReadResponseString(wrPosturl);

                        if (responseString == "Success")
                        {
                            LogFile.WriteLine("Mugshot successfully uploaded");
                            Pending = false;
                            CreationTime = DateTime.Now;
                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(this);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CIX.ReportServerExceptions("Mugshot.Sync", e);
                        Pending = true;
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load all external mugshots into the cache.
        /// </summary>
        private static void LoadExternalMugshots()
        {
            string mugshotFolder = Path.Combine(CIX.HomeFolder, "Mugshots");
            if (Directory.Exists(mugshotFolder))
            {
                string[] allFiles = Directory.GetFiles(mugshotFolder);
                foreach (string filename in allFiles)
                {
                    try
                    {
                        string username = Path.GetFileNameWithoutExtension(filename);
                        Image mugshotImage = new Bitmap(filename);
                        mugshotImage = mugshotImage.ResizeImage(MaxMugshotWidth, MaxMugshotHeight);

                        if (username != null && mugshotImage != null)
                        {
                            Mugshot mugshot = new Mugshot();

                            ImageConverter converter = new ImageConverter();
                            mugshot.Image = (byte[])converter.ConvertTo(mugshotImage, typeof(byte[]));
                            mugshot.Username = username;

                            _cache[username] = mugshot;
                        }
                    }
                    catch (Exception e)
                    {
                        LogFile.WriteLine("Error loading {0} : {1}", filename, e.Message);
                    }
                }
            }
        }


        /// <summary>
        /// Return the default Mugshot image.
        /// </summary>
        public static Image GetDefaultMugshot()
        {
            return Resources.DefaultUser;
        }
    }
}