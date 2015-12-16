// *****************************************************
// CIXReader
// AccountEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/06/2015 12:19
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Models;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the account event arguments
    /// </summary>
    public sealed class AccountEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the account details
        /// </summary>
        public Account Account { get; set; }
    }
}