// *****************************************************
// CIXReader
// SendMail.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/2/2015 8:19 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX SendMail structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class SendMail
    {
        /// <remarks />
        [DataMember]
        public string Text { get; set; }

        /// <remarks />
        [DataMember]
        public string HTML { get; set; }
    }
}