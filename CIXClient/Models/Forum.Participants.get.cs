﻿// *****************************************************
// CIXReader
// Forum.Participants.get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 26/10/2013 4:27 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CIXClient.Models
{
    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    [XmlRoot(Namespace = "http://cixonline.com", IsNullable = false)]
    public sealed class Parts
    {
        private string countField;

        private string startField;

        private PartsUser[] usersField;

        /// <remarks />
        public string Count
        {
            get { return countField; }
            set { countField = value; }
        }

        /// <remarks />
        public string Start
        {
            get { return startField; }
            set { startField = value; }
        }

        /// <remarks />
        [XmlArrayItem("Part", typeof (PartsUser), IsNullable = false)]
        public PartsUser[] Users
        {
            get { return usersField; }
            set { usersField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class PartsUser
    {
        private string nameField;

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}
