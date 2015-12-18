// *****************************************************
// CIXReader
// About.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 31/08/2013 7:54 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    /// <summary>
    /// Implements the About dialog box that shows the CIXReader version number.
    /// </summary>
    public sealed partial class About : Form
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            abtVersion.Text = string.Format("{0} {1}", Resources.AppTitle, Program.VersionString);

            var companyAttr = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            string companyString = ((AssemblyCompanyAttribute) companyAttr[0]).Company;

            var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
            string copyrightString = ((AssemblyCopyrightAttribute) attributes[0]).Copyright;

            DateTime today = DateTime.Now;
            if (today.Month == 12 && today.Day >= 18 && today.Day <= 27)
            {
                abtLogo.Image = Resources.ChristmasLogo;
            }

            abtCopyright.Text = string.Format("{0} {1}", copyrightString, companyString);
        }

        /// <summary>
        /// Display the open source license file.
        /// </summary>
        private void abtLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string currentAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (currentAssemblyPath != null)
            {
                string licenseFilePath = Path.Combine(currentAssemblyPath, "Acknowledgements.html");
                if (File.Exists(licenseFilePath))
                {
                    Process.Start(licenseFilePath);
                }
            }
        }
    }
}