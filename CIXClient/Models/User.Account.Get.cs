// *****************************************************
// CIXReader
// User.Account.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 04/07/2014 12:19
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
    public sealed class Account
    {
        private string typeField;

        /// <remarks />
        public string Type
        {
            get { return typeField; }
            set { typeField = value; }
        }
    }
}