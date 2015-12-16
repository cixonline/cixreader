// *****************************************************
// CIXReader
// DirectoryEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 13/06/2015 09:52
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that communicates the directory change
    /// </summary>
    public sealed class DirectoryEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the name of the category that was updated
        /// </summary>
        public string CategoryName { get; internal set; }
    }
}