// *****************************************************
// CIXReader
// Forum.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 18/06/2014 20:21
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX Forum structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class Forum
    {
        /// <remarks />
        [DataMember]
        public string Category { get; set; }

        /// <remarks />
        [DataMember]
        public string Description { get; set; }

        /// <remarks />
        [DataMember]
        public string Name { get; set; }

        /// <remarks />
        [DataMember]
        public string SubCategory { get; set; }

        /// <remarks />
        [DataMember]
        public string Title { get; set; }

        /// <remarks />
        [DataMember]
        public string Type { get; set; }
    }
}