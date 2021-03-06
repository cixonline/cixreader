﻿// *****************************************************
// CIXReader
// Constants_Mono.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 21/06/2015 11:02
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

namespace CIXReader.Utilities
{
    /// <summary>
    /// Constants specific to the Linux version.
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// URL to the appcast link.
        /// </summary>
        public const string AppCastURL = "http://cixreader.cixhosting.co.uk/cixreader/linux/release/appcast.xml";

        /// <summary>
        /// URL to the beta appcast link.
        /// </summary>
        public const string BetaAppCastURL = "http://cixreader.cixhosting.co.uk/cixreader/linux/beta/appcast.xml";

        /// <summary>
        /// Change log file link.
        /// </summary>
        public const string ChangeLogURL = "http://cixreader.cixhosting.co.uk/cixreader/linux/{0}/changes.html";

        /// <summary>
        /// Support forum for this version of CIXReader
        /// </summary>
        public const string SupportForum = "cix.support/cixreader";

        /// <summary>
        /// Beta forum for this version of CIXReader
        /// </summary>
        public const string BetaForum = "cix.beta/cixreaderlinux";
    }
}