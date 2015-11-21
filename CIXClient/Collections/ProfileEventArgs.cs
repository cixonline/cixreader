// *****************************************************
// CIXReader
// ProfileEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/06/2015 12:19
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the profile event arguments
    /// </summary>
    public sealed class ProfileEventArgs : EventArgs
    {
        /// <summary>
        /// A copy of the profile that was recently added.
        /// </summary>
        public Profile Profile { get; internal set; }
    }
}