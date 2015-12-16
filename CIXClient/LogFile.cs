// *****************************************************
// CIXReader
// Debug.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 31/08/2013 3:16 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.IO;

namespace CIXClient
{
    /// <summary>
    /// LogFile implements simple log file functionality.
    /// </summary>
    public static class LogFile
    {
        private static string _debugFilePath;
        private static StreamWriter _file;

        private const int ArchiveMaximum = 9;

        /// <summary>
        /// Gets or sets a value indicating whether or not the debug log is enabled.
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the debug log accumulates between
        /// sessions.
        /// </summary>
        public static bool Cumulative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the we archive earlier copies of
        /// the debug log.
        /// </summary>
        public static bool Archive { get; set; }

        /// <summary>
        /// Open the debug log file.
        /// </summary>
        /// <param name="debugFilePath">Path to the debug log file</param>
        public static void Create(string debugFilePath)
        {
            _file = null;
            _debugFilePath = debugFilePath;
        }

        /// <summary>
        /// Close the debug log file.
        /// </summary>
        public static void Close()
        {
            if (_file != null)
            {
                _file.Close();
                _file = null;
            }
        }

        /// <summary>
        /// Writes a formatted string to the debug file preceded by the date and time of
        /// the event in UK format.
        /// </summary>
        /// <param name="formatString">The format string</param>
        /// <param name="args">Optional variable arguments used by the format string</param>
        public static void WriteLine(string formatString, params object[] args)
        {
            if (Enabled)
            {
                if (_file == null)
                {
                    if (!Cumulative && Archive)
                    {
                        ArchiveOldLogs();
                    }
                    FileMode fileMode = Cumulative ? FileMode.Append : FileMode.Create;
                    _file = new StreamWriter(File.Open(_debugFilePath, fileMode, FileAccess.Write, FileShare.Read));
                }
                if (_file == null)
                {
                    Enabled = false;
                    return;
                }
                _file.WriteLine("{0} : {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), string.Format(formatString, args));
                if (_file != null)
                {
                    _file.Flush();
                }
            }
        }

        /// <summary>
        /// Archive old log files.
        /// </summary>
        private static void ArchiveOldLogs()
        {
            for (int extMax = ArchiveMaximum - 1; extMax >= 0; --extMax)
            {
                string archiveNew = Path.ChangeExtension(_debugFilePath, (extMax + 1).ToString("000"));
                string archiveCurrent = (extMax > 0)
                    ? Path.ChangeExtension(_debugFilePath, extMax.ToString("000"))
                    : _debugFilePath;

                if (File.Exists(archiveNew))
                {
                    File.Delete(archiveNew);
                }
                if (File.Exists(archiveCurrent))
                {
                    File.Move(archiveCurrent, archiveNew);
                }
            }
        }
    }
}