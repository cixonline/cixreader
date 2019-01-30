// *****************************************************
// CIXReader
// DirectoryCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 12:36 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Models;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// The DirectoryCollection class encapsulates functionality for working with the CIX directory
    /// list of forums.
    /// </summary>
    public sealed class DirectoryCollection
    {
        private Dictionary<string, List<DirCategory>> _allCategories;
        private Dictionary<int, DirForum> _allForums;

        /// <summary>
        /// Defines the delegate for ForumJoined event notifications.
        /// </summary>
        /// <param name="forum">The Folder object</param>
        public delegate void ForumJoinedHandler(Folder forum);

        /// <summary>
        /// Defines the delegate for ParticipantsUpdated event notifications.
        /// </summary>
        /// <param name="forum">The DirForum object</param>
        public delegate void ParticipantsUpdatedHandler(DirForum forum);

        /// <summary>
        /// Defines the delegate for ModeratorsUpdated event notifications.
        /// </summary>
        /// <param name="forum">The DirForum object</param>
        public delegate void ModeratorsUpdatedHandler(DirForum forum);

        /// <summary>
        /// Defines the delegate for DirectoryChanged event notifications.
        /// </summary>
        /// <param name="sender">The DirectoryCollection object</param>
        /// <param name="e">Additional event data for the directory update</param>
        public delegate void DirectoryChangedHandler(object sender, DirectoryEventArgs e);

        /// <summary>
        /// Defines the delegate for ForumUpdated event notifications.
        /// </summary>
        /// <param name="forum">The DirForum object</param>
        public delegate void ForumUpdatedHandler(DirForum forum);

        /// <summary>
        /// Event handler for notifying a delegate that the directory has been updated.
        /// </summary>
        public event DirectoryChangedHandler DirectoryChanged;

        /// <summary>
        /// Event handler for notifying a delegate that forum has been updated.
        /// </summary>
        public event ForumUpdatedHandler ForumUpdated;

        /// <summary>
        /// Event handler for notifying a delegate that the participant list has been updated.
        /// </summary>
        public event ForumJoinedHandler ForumJoined;

        /// <summary>
        /// Event handler for notifying a delegate that the participant list has been updated.
        /// </summary>
        public event ParticipantsUpdatedHandler ParticipantsUpdated;

        /// <summary>
        /// Event handler for notifying a delegate that the moderators list has been updated.
        /// </summary>
        public event ModeratorsUpdatedHandler ModeratorsUpdated;

        /// <summary>
        /// Gets the list of categories from the database.
        /// </summary>
        public IEnumerable<string> Categories
        {
            get
            {
                if (_allCategories == null)
                {
                    List<DirCategory> results = CIX.DB.Table<DirCategory>().ToList();
                    _allCategories = new Dictionary<string, List<DirCategory>>();

                    foreach (DirCategory category in results)
                    {
                        List<DirCategory> subCategories = SubCategoriesByCategoryName(category.Name);
                        if (subCategories == null)
                        {
                            subCategories = new List<DirCategory>();
                            _allCategories[category.Name] = subCategories;
                        }
                        subCategories.Add(category);
                    }
                }
                return new List<string>(_allCategories.Keys);
            }
        }

        /// <summary>
        /// Gets the list of forums from the database.
        /// </summary>
        public IEnumerable<DirForum> Forums
        {
            get
            {
                if (_allForums == null)
                {
                    _allForums = new Dictionary<int, DirForum>();
                    List<DirForum> results = CIX.DB.Table<DirForum>().ToList();

                    foreach (DirForum forum in results)
                    {
                        _allForums[forum.ID] = forum;
                    }
                }
                return new List<DirForum>(_allForums.Values);
            }
        }

        /// <summary>
        /// Read the list of categories from the server.
        /// </summary>
        public void Refresh()
        {
            Thread t = new Thread(() =>
            {
                if (CIX.Online)
                {
                    HttpWebRequest wrGeturl = APIRequest.Get("directory/categories", APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(CategoryResultSet));
                            CategoryResultSet inboxSet = (CategoryResultSet)serializer.Deserialize(reader);

                            List<DirCategory> newCategories = new List<DirCategory>();

                            foreach (CategoryResult conv in inboxSet.Categories)
                            {
                                DirCategory newCategory = CategoryByName(conv.Name, conv.Sub);
                                if (newCategory == null)
                                {
                                    newCategory = new DirCategory
                                    {
                                        Name = conv.Name,
                                        Sub = conv.Sub
                                    };

                                    List<DirCategory> subCategories;
                                    if (!_allCategories.TryGetValue(conv.Name, out subCategories))
                                    {
                                        subCategories = new List<DirCategory>();
                                        _allCategories[conv.Name] = subCategories;
                                    }
                                    subCategories.Add(newCategory);
                                    newCategories.Add(newCategory);
                                }
                            }

                            if (newCategories.Count > 0)
                            {
                                lock (CIX.DBLock)
                                {
                                    CIX.DB.InsertAll(newCategories);
                                }
                                LogFile.WriteLine(@"Directory refreshed with {0} new categories", newCategories.Count);

                                if (DirectoryChanged != null)
                                {
                                    DirectoryChanged(this, new DirectoryEventArgs { CategoryName = null });
                                }
                            }

                            RefreshCategories();
                        }
                    }
                }
            });
            t.Start();
        }

        /// <summary>
        /// Refresh the details of the forum, including title and description.
        /// </summary>
        /// <param name="forumName">The forum name</param>
        public void RefreshForum(string forumName)
        {
            Thread t = new Thread(() =>
            {
                DirForum forum = null;
                try
                {
                    string encodedForumName = FolderCollection.EncodeForumName(forumName);
                    LogFile.WriteLine("Updating directory for {0}", forumName);

                    HttpWebRequest wrGeturl = APIRequest.Get("forums/" + encodedForumName + "/details", APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ForumDetails));
                            ForumDetails forumDetails = (ForumDetails)serializer.Deserialize(reader);

                            bool isNewForum = false;

                            forum = ForumByName(forumDetails.Name);
                            if (forum == null)
                            {
                                forum = new DirForum();
                                isNewForum = true;
                            }
                            forum.Name = forumDetails.Name;
                            forum.Title = forumDetails.Title;
                            forum.Desc = forumDetails.Description;
                            forum.Cat = forumDetails.Category;
                            forum.Sub = forumDetails.SubCategory;
                            forum.Recent = forumDetails.Recent;
                            forum.Type = forumDetails.Type;

                            lock (CIX.DBLock)
                            {
                                if (isNewForum)
                                {
                                    CIX.DB.Insert(forum);
                                    _allForums[forum.ID] = forum;
                                }
                                else
                                {
                                    CIX.DB.Update(forum);
                                }
                            }

                            LogFile.WriteLine("Directory for {0} updated", forum.Name);
                        }
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("DirectoryCollection.RefreshForum", e);
                }
                NotifyForumUpdated(forum);
            });
            t.Start();
        }

        /// <summary>
        /// Return the list of all forums in the named category.
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <returns>A list of DirForum</returns>
        public IEnumerable<DirForum> AllForumsInCategory(string categoryName)
        {
            return Forums.Where(forum => forum.Cat == categoryName).ToList();
        }

        /// <summary>
        /// Return the DirForum corresponding to the given name
        /// </summary>
        /// <param name="forumName">Forum name</param>
        /// <returns>DirForum with the given name, or null if not found</returns>
        public DirForum ForumByName(string forumName)
        {
            return Forums.FirstOrDefault(forum => forum.Name == forumName);
        }

        /// <summary>
        /// Given a category name, return the list of all sub-categories in that category
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <returns>An enumerable list of all sub-categories</returns>
        public List<DirCategory> SubCategoriesByCategoryName(string categoryName)
        {
            List<DirCategory> subCategories;
            _allCategories.TryGetValue(categoryName, out subCategories);
            return subCategories;
        }

        /// <summary>
        /// Call the synchronisation on the directory.
        /// </summary>
        internal void Sync()
        {
            foreach (DirForum forum in Forums.Where(forum => forum.HasPending))
            {
                forum.Sync();
            }
        }

        /// <summary>
        /// Notify that a forum details has been updated.
        /// </summary>
        /// <param name="dirForum">The forum for which the list was updated</param>
        internal void NotifyForumUpdated(DirForum dirForum)
        {
            if (ForumUpdated != null)
            {
                ForumUpdated(dirForum);
            }
        }

        /// <summary>
        /// Notify that the moderator list has been updated.
        /// </summary>
        /// <param name="dirForum">The forum for which the list was updated</param>
        internal void NotifyModeratorsUpdated(DirForum dirForum)
        {
            if (ModeratorsUpdated != null)
            {
                ModeratorsUpdated(dirForum);
            }
        }

        /// <summary>
        /// Notify that the participants list has been updated.
        /// </summary>
        /// <param name="dirForum">The forum for which the list was updated</param>
        internal void NotifyParticipantsUpdated(DirForum dirForum)
        {
            if (ParticipantsUpdated != null)
            {
                ParticipantsUpdated(dirForum);
            }
        }

        /// <summary>
        /// Notify that the specified forum has been joined on the server
        /// </summary>
        /// <param name="forum">The forum that was joined</param>
        internal void NotifyForumJoined(Folder forum)
        {
            if (ForumJoined != null)
            {
                ForumJoined(forum);
            }
        }

        /// <summary>
        /// Return the DirCategory corresponding to the given category and subcategory name.
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="subCategoryName">Subcategory name</param>
        /// <returns>DirCategory for the category and subcategory</returns>
        private DirCategory CategoryByName(string categoryName, string subCategoryName)
        {
            IEnumerable<DirCategory> subCategories = SubCategoriesByCategoryName(categoryName);
            return (subCategories != null)
                ? subCategories.FirstOrDefault(subCategory => subCategory.Sub == subCategoryName)
                : null;
        }

        private void RefreshCategories()
        {
            foreach (string categoryName in _allCategories.Keys)
            {
                string safeCategoryName = categoryName.Replace("&", "+and+");

                HttpWebRequest request = APIRequest.Get("directory/" + safeCategoryName + "/forums", APIRequest.APIFormat.XML);
                Stream objStream = APIRequest.ReadResponse(request);
                if (objStream != null)
                {
                    using (XmlReader reader = XmlReader.Create(objStream))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(DirListings));
                        DirListings inboxSet = (DirListings)serializer.Deserialize(reader);

                        int countOfNewForums = 0;

                        lock (CIX.DBLock)
                        {
                            CIX.DB.RunInTransaction(() =>
                            {
                                foreach (DirListing conv in inboxSet.Forums)
                                {
                                    // Forums can appear in multiple categories, and forum names are unique so see if this
                                    // forum is already known.
                                    DirForum forum = ForumByName(conv.Forum) ?? new DirForum
                                    {
                                        Name = conv.Forum
                                    };

                                    // Find the ID of the category and sub-category to which this belongs.
                                    // At this point, both will be in the AllCategories table. If not then something
                                    // has corrupted on the remote end.
                                    forum.Title = conv.Title;
                                    forum.Type = conv.Type;
                                    forum.Recent = conv.Recent;
                                    forum.Cat = conv.Cat;
                                    forum.Sub = conv.Sub;
                                    forum.DetailsPending = false;

                                    if (forum.ID == 0)
                                    {
                                        CIX.DB.Insert(forum);
                                    }
                                    else
                                    {
                                        CIX.DB.Update(forum);
                                    }
                                    _allForums[forum.ID] = forum;

                                    ++countOfNewForums;
                                }
                            });
                        }

                        if (countOfNewForums > 0)
                        {
                            LogFile.WriteLine("{0} new forums added to category {1}", countOfNewForums, categoryName);

                            if (DirectoryChanged != null)
                            {
                                DirectoryChanged(this, new DirectoryEventArgs { CategoryName = categoryName });
                            }
                        }
                    }
                }
            }
        }
    }
}