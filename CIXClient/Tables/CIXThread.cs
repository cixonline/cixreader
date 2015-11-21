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
        /// The author of the message.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The name of the forum containing this message.
        /// </summary>
        public string Forum { get; set; }

        /// <summary>
        /// The name of the topic within the forum.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The date and time when this message was posted.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The unique ID of this message within the topic.
        /// </summary>
        public int RemoteID { get; set; }
    }
}