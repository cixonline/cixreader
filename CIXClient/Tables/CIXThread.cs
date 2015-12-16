// *****************************************************
// CIXReader
// CIXThread.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/06/2015 17:25
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;

namespace CIXClient.Tables
{
    /// <summary>
    /// A class that represents a CIX message.
    /// </summary>
    public sealed class CIXThread
    {
        /// <summary>
        /// Gets or sets the author of the message.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the body of the message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the name of the forum containing this message.
        /// </summary>
        public string Forum { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic within the forum.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this message was posted.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets  unique ID of this message within the topic.
        /// </summary>
        public int RemoteID { get; set; }
    }
}