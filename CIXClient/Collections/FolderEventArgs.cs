// *****************************************************
// CIXReader
// FolderEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/06/2015 12:18
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the folders event arguments
    /// </summary>
    public sealed class FolderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this folder is being
        /// refreshed as part of a call to Fixup(). This is used by the
        /// delegate to avoid recursive fixup calls.
        /// </summary>
        public bool Fixup { get; set; }

        /// <summary>
        /// Gets or sets a the folder affected
        /// </summary>
        public Folder Folder { get; set; }
    }
}