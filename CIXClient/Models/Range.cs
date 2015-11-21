// *****************************************************
// CIXReader
// Range.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/05/2015 12:23
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX Range structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class Range
    {
        /// <remarks />
        [DataMember]
        public int End { get; set; }

        /// <remarks />
        [DataMember]
        public string ForumName { get; set; }

        /// <remarks />
        [DataMember]
        public int Start { get; set; }

        /// <remarks />
        [DataMember]
        public string TopicName { get; set; }
    }
}