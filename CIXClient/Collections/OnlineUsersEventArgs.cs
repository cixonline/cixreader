// *****************************************************
// CIXReader
// OnlineUsersEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 25/06/2015 17:45
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Models;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the online users event arguments.
    /// </summary>
    public sealed class OnlineUsersEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the list of users online.
        /// </summary>
        public Whos Users { get; set; }
    }
}