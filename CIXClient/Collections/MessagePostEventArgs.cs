// *****************************************************
// CIXReader
// MessagePostEventArgs.cs
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
    /// Class that encapsulates the message post event arguments
    /// </summary>
    public sealed class MessagePostEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message affected
        /// </summary>
        public CIXMessage Message { get; set; }
    }
}