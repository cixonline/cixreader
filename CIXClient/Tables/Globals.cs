// *****************************************************
// CIXReader
// Settings.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 2:28 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Database;

namespace CIXClient.Tables
{
    /// <summary>
    /// The UserSettings table encapsulates database global information.
    /// </summary>
    public sealed class Globals
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Globals"/> class.
        /// </summary>
        public Globals()
        {
            ID = 0;
            Version = CIX.CurrentVersion;
            LastSyncDate = default(DateTime);
        }

        /// <summary>
        /// Gets or sets the ID of this configuration row.
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the version of this database
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last sync.
        /// </summary>
        public DateTime LastSyncDate { get; set; }
    }
}