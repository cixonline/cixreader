// *****************************************************
// CIXReader
// ProfileSet.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/09/2013 10:08 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Runtime.Serialization;

namespace CIXClient.Models
{
    /// <summary>
    /// CIX ProfileSet structure
    /// </summary>
    [DataContract(Namespace = "http://cixonline.com")]
    public sealed class ProfileSet
    {
        /// <remarks />
        [DataMember]
        public string Email { get; set; }

        /// <remarks />
        [DataMember]
        public int Flags { get; set; }

        /// <remarks />
        [DataMember]
        public string Fname { get; set; }

        /// <remarks />
        [DataMember]
        public string Location { get; set; }

        /// <remarks />
        [DataMember]
        public string Sex { get; set; }

        /// <remarks />
        [DataMember]
        public string Sname { get; set; }
    }
}