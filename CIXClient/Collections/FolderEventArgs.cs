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
        /// Sets whether this folder was fixed-upped.
        /// </summary>
        public bool Fixup { get; set; }

        /// <summary>
        /// A copy of the folder affected
        /// </summary>
        public Folder Folder { get; set; }
    }
}