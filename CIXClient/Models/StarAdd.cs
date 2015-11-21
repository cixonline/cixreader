// *****************************************************
// CIXReader
// StarAdd.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/10/2013 12:34 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX StarAdd structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class StarAdd
    {
        /// <remarks />
        [DataMember]
        public string Forum { get; set; }

        /// <remarks />
        [DataMember]
        public string MsgID { get; set; }

        /// <remarks />
        [DataMember]
        public string Topic { get; set; }
    }
}