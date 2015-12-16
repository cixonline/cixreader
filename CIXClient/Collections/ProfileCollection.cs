// *****************************************************
// CIXReader
// ProfileCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/10/2013 11:03 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// The ProfileCollection class manages a collection of profiles.
    /// </summary>
    public sealed class ProfileCollection : IEnumerable<Profile>
    {
        private List<Profile> _profiles;

        /// <summary>
        /// Initialises a new instance of the <see cref="ProfileCollection"/> class 
        /// with a set of profiles.
        /// </summary>
        public ProfileCollection()
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        /// <summary>
        /// Defines the delegate for ProfileUpdated event notifications.
        /// </summary>
        /// <param name="sender">The ProfileTasks object</param>
        /// <param name="e">Additional profile update data</param>
        public delegate void ProfileUpdatedHandler(object sender, ProfileEventArgs e);

        /// <summary>
        /// Event handler for notifying a delegate of profile updates.
        /// </summary>
        public event ProfileUpdatedHandler ProfileUpdated;

        /// <summary>
        /// Gets a list of all profiles.
        /// </summary>
        public List<Profile> Profiles
        {
            get { return _profiles ?? (_profiles = new List<Profile>(CIX.DB.Table<Profile>().ToList())); }
        }

        /// <summary>
        /// Returns the Profile with the specified user name.
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>A Profile, or null</returns>
        public Profile this[string name]
        {
            get { return Profiles.SingleOrDefault(c => c.Username == name); }
        }

        /// <summary>
        /// Synchronise the profile collection, updating any changes to the local
        /// profile and resume that was made offline to the server.
        /// </summary>
        public void Sync()
        {
            if (CIX.Online)
            {
                try
                {
                    Profile selfProfile = Profile.ProfileForUser(CIX.Username);
                    if (selfProfile.Pending)
                    {
                        selfProfile.Sync();
                    }
                    Mugshot selfMugshot = Mugshot.MugshotForUser(CIX.Username, false);
                    if (selfMugshot.Pending)
                    {
                        selfMugshot.Sync();
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("ProfileCollection.Sync", e);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for iterating over the profile collection.
        /// </summary>
        /// <returns>An enumerator for Profile</returns>
        public IEnumerator<Profile> GetEnumerator()
        {
            return (Profiles != null) ? Profiles.GetEnumerator() : new List<Profile>.Enumerator();
        }

        /// <summary>
        /// Send a notification that a profile was updated.
        /// </summary>
        /// <param name="profile">The profile that was updated</param>
        public void NotifyProfileUpdated(Profile profile)
        {
            if (ProfileUpdated != null)
            {
                ProfileEventArgs args = new ProfileEventArgs { Profile = profile };
                ProfileUpdated(this, args);
            }
        }

        /// <summary>
        /// Add the new profile to the collection.
        /// </summary>
        /// <param name="newProfile">A completed profile object to be added</param>
        public void Add(Profile newProfile)
        {
            Profiles.Add(newProfile);
        }

        /// <summary>
        /// Look up a profile for the specified user.
        /// </summary>
        /// <param name="username">The username to be looked up</param>
        /// <returns>The profile matching the user, or null if it does not exist</returns>
        public Profile Get(string username)
        {
            return Profiles.FirstOrDefault(profile => profile.Username == username);
        }

        /// <summary>
        /// Returns an enumerator for iterating over the profile collection.
        /// </summary>
        /// <returns>A generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add a profile to the collection.
        /// </summary>
        /// <param name="profile">A new profile to add</param>
        internal void AddInternal(Profile profile)
        {
            lock (CIX.DBLock)
            {
                CIX.DB.Insert(profile);
            }
            Profiles.Add(profile);
        }

        /// <summary>
        /// Respond to network state change by triggering an update to download profiles if there
        /// are any pending requests that were blocked due to being offline.
        /// </summary>
        /// <param name="sender">The network monitor, which CAN be on a different thread</param>
        /// <param name="e">Network availability event information</param>
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                Sync();
            }
        }
    }
}