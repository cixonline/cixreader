// *****************************************************
// CIXReader
// User.UserProfile.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/09/2013 1:38 PM
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
    public sealed class ProfileSmall
    {
        private string emailField;

        private string firstOnField;

        private string fnameField;

        private string lastOnField;

        private string lastPostField;

        private string locationField;

        private string sexField;

        private string snameField;

        private string unameField;

        private int flagsField;

        /// <remarks />
        public string Email
        {
            get { return emailField; }
            set { emailField = value; }
        }

        /// <remarks />
        public string FirstOn
        {
            get { return firstOnField; }
            set { firstOnField = value; }
        }

        /// <remarks />
        public int Flags
        {
            get { return flagsField; }
            set { flagsField = value; }
        }

        /// <remarks />
        public string Fname
        {
            get { return fnameField; }
            set { fnameField = value; }
        }

        /// <remarks />
        public string LastOn
        {
            get { return lastOnField; }
            set { lastOnField = value; }
        }

        /// <remarks />
        public string LastPost
        {
            get { return lastPostField; }
            set { lastPostField = value; }
        }

        /// <remarks />
        public string Location
        {
            get { return locationField; }
            set { locationField = value; }
        }

        /// <remarks />
        public string Sex
        {
            get { return sexField; }
            set { sexField = value; }
        }

        /// <remarks />
        public string Sname
        {
            get { return snameField; }
            set { snameField = value; }
        }

        /// <remarks />
        public string Uname
        {
            get { return unameField; }
            set { unameField = value; }
        }
    }
}