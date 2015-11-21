// *****************************************************
// CIXReader
// PMessageAdd.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/09/2013 8:43 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX PMessageAdd structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class PMessageAdd
    {
        /// <remarks />
        [DataMember]
        public string Body { get; set; }

        /// <remarks />
        [DataMember]
        public string Recipient { get; set; }

        /// <remarks />
        [DataMember]
        public string Subject { get; set; }
    }
}