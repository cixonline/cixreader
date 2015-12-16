// *****************************************************
// CIXReader
// InterestingThreadsEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/06/2015 10:17
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the interesting threads event arguments.
    /// </summary>
    public sealed class InterestingThreadsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the list of interesting threads.
        /// </summary>
        public List<CIXThread> Threads { get; set; }
    }
}