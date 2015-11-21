// *****************************************************
// CIXReader
// PMessageReply.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 06/09/2013 9:14 AM
//  
//  Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
//  *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// CIX PMessageReply structure
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class PMessageReply
    {
        /// <remarks />
        [DataMember]
        public string Body { get; set; }

        /// <remarks />
        [DataMember]
        public int ConID { get; set; }
    }
}