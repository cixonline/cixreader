// *****************************************************
// CIXReader
// FoldersAddedEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/07/2015 03:05
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Folders added event arguments
    /// </summary>
    public sealed class FoldersAddedEventArgs : EventArgs
    {
        /// <summary>
        /// List of added folders
        /// </summary>
        public List<Folder> Folders { get; set; }
    }
}