// *****************************************************
// CIXReader
// PostMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 17/09/2013 1:20 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// CIX PostMessage structure
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class PostMessage
    {
        /// <remarks />
        [DataMember]
        public string Body { get; set; }

        /// <remarks />
        [DataMember]
        public string Forum { get; set; }

        /// <remarks />
        [DataMember]
        public string MarkRead { get; set; }

        /// <remarks />
        [DataMember]
        public string MsgID { get; set; }

        /// <remarks />
        [DataMember]
        public string Topic { get; set; }

        /// <remarks />
        [DataMember]
        public string WrapColumn { get; set; }
    }
}