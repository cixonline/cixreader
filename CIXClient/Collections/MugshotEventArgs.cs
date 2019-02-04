// *****************************************************
// CIXReader
// MugshotEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 1/02/2019 13:38
//  
// Copyright (C) 2013-2019 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the mugshot event arguments
    /// </summary>
    public sealed class MugshotEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the mugshot that was updated.
        /// </summary>
        public Mugshot Mugshot { get; internal set; }
    }
}