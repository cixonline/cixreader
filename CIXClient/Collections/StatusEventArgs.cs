// *****************************************************
// CIXReader
// StatusEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/01/2019 14:58
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the status event arguments
    /// </summary>
    public sealed class StatusEventArgs
    {
        /// <summary>
        /// Gets or sets the status bar message
        /// </summary>
        public string Message { get; set; }
    }
}
