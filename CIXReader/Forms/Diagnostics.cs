// *****************************************************
// CIXReader
// Diagnostics.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 27/02/2014 12:50
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Reflection;
using CIXClient;
using CIXReader.Properties;
using TheArtOfDev.HtmlRenderer;

namespace CIXReader.Forms
{
    /// <summary>
    /// Declares a class that displays the Diagnostics dialog.
    /// </summary>
    public sealed partial class Diagnostics : Form
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Diagnostics"/> class.
        /// </summary>
        public Diagnostics()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the Close button.
        /// </summary>
        private void diagClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static string GetFileVersionInfo(string filename)
        {
            try
            {
                string asmPath = AppDomain.CurrentDomain.BaseDirectory;

                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Path.Combine(asmPath, filename));
                if (!string.IsNullOrEmpty(ver.FileDescription) && !string.IsNullOrEmpty(ver.FileVersion))
                {
                    return string.Format("{0} {1}", ver.FileDescription, ver.FileVersion);
                }
                return string.Format("{0} {1}.{2}.{3}", filename, ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart);
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Diagnostics: Error retrieving version info for {0} : {1}", filename, e.Message);
            }
            return string.Format("{0} {1}", filename, "Not Found");
        }

        /// <summary>
        /// Initialise the diagnostics dialog with detailed info.
        /// </summary>
        private void Diagnostics_Load(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("{0} {1}\r\n", Resources.AppTitle, Program.VersionString);
            str.AppendLine();

            str.AppendLine("Support Libraries:");
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"CIXClient.dll"));
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"CIXMarkup.dll"));
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"HtmlRenderer.dll"));
            if (!MonoHelper.IsMono)
            {
                str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"NHunspell.dll"));
                str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"NHunspellExtender.dll"));
            }
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"lua52.dll"));
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"NLua.dll"));
            str.AppendFormat(" {0}\r\n", GetFileVersionInfo(@"KeraLua.dll"));

            str.AppendLine();

            if (!MonoHelper.IsMono)
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                    foreach (var queryObj in searcher.Get().Cast<ManagementObject>())
                    {
                        str.AppendFormat("Operating System: {0} {1}\r\n", queryObj["Caption"], queryObj["Version"]);
                        str.AppendFormat("Service Pack: {0}.{1}\r\n", queryObj["ServicePackMajorVersion"], queryObj["ServicePackMinorVersion"]);
                        str.AppendFormat("Architecture: {0}\r\n", queryObj["OSArchitecture"]);
                    }
                }
                catch (ManagementException)
                {
                }
                str.AppendLine();
            }
            else
            {
                Process process = new Process();
                string kernelname = "Unknown";
                string osname = "Linux";

                process.StartInfo.FileName = "uname";
                process.StartInfo.Arguments = "-mr";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (s, args) => {
                    if (args.Data != null)
                    {
                        kernelname = args.Data;
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                string osReleaseFile = "/etc/os-release";
                if (File.Exists(osReleaseFile))
                {
                    string [] releaseInfo = File.ReadAllLines(osReleaseFile);
                    foreach (string line in releaseInfo)
                    {
                        string [] tokens = line.Split(new [] { '=' });
                        if (tokens.Length == 2 && tokens[0] == "PRETTY_NAME")
                        {
                            osname = tokens[1].Trim(new char [] { '"'});
                            break;
                        }
                    }
                }

                str.AppendFormat("Operating System: {0}\r\n", osname);
                str.AppendFormat("Kernel Version: {0}\r\n", kernelname);
                str.AppendLine();
            }
            
            str.AppendFormat("User: {0}\r\n", CIX.Username);
            str.AppendFormat("Home: {0}\r\n", CIX.HomeFolder);
            str.AppendFormat("Config: {0}\r\n", CIX.UIConfigFolder);
            str.AppendFormat("Online: {0}\r\n", CIX.Online);
            str.AppendLine();

            string settingsPath = Path.Combine(CIX.HomeFolder, CIX.Username + ".ini");
            if (File.Exists(settingsPath))
            {
                string settingsData = File.ReadAllText(settingsPath);
                str.Append(settingsData);
                str.AppendLine();
            }

            diagText.Text = str.ToString();
            diagText.SelectionLength = 0;
            diagText.SelectionStart = 0;
        }

        /// <summary>
        /// Copy the text to the clipboard.
        /// </summary>
        private void diagCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(diagText.Text);
        }
    }
}