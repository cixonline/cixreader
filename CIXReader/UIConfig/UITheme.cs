// *****************************************************
// CIXReader
// UITheme.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 17/12/2013 18:01
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Windows.Forms;
using CIXClient;
using CIXReader.Properties;

namespace CIXReader.UIConfig
{
    /// <summary>
    /// Class that encapsulates a single UI theme
    /// </summary>
    public sealed class UITheme
    {
        /// <summary>
        /// Get or set a flag which indicates whether this is a custom or
        /// built-in theme.
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Get a flag which indicates whether this is the default theme.
        /// </summary>
        public bool IsDefault
        {
            get { return Name == Resources.Default; }
        }

        /// <summary>
        /// Get or set the theme's friendly display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the theme's installation path.
        /// </summary>
        public string Path
        {
            get
            {
                if (!IsCustom)
                {
                    string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                    if (appPath != null)
                    {
                        string themesFolder = System.IO.Path.Combine(appPath, "Themes");
                        return System.IO.Path.Combine(themesFolder, Name, "ui.xml");
                    }
                }
                else
                {
                    string themesFolder = System.IO.Path.Combine(CIX.HomeFolder, "Themes");
                    return System.IO.Path.Combine(themesFolder, Name, "ui.xml");
                }
                return null;
            }
        }
    }
}