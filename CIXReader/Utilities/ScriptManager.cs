// *****************************************************
// CIXReader
// ScriptManager.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 04/09/2014 11:55
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXReader.Controls;
using CIXReader.Forms;
using CIXReader.Lua;
using CIXReader.Properties;
using ICSharpCode.SharpZipLib.Zip;
using IniParser;
using IniParser.Model;
using NLua;
using NLua.Exceptions;

namespace CIXReader.Utilities
{
    /// <summary>
    /// The ScriptManifest documents details of a script
    /// package and is parsed from the install.ini file in
    /// the package.
    /// </summary>
    public sealed class ScriptManifest
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string Author;
        public readonly string IconFile;
        public readonly string ScriptFile;
        public readonly bool InstallToToolbar;

        /// <summary>
        /// Instantiate a manifest from the specified manifest file.
        /// </summary>
        /// <param name="manifestFile">Manifest file name</param>
        public ScriptManifest(string manifestFile)
        {
            const string cixScriptSection = "CIXReader Script";
            string manifestFolder = Path.GetDirectoryName(manifestFile) ?? ".";

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(manifestFile);

            bool tempValue;

            Name = data[cixScriptSection]["Name"];
            Description = data[cixScriptSection]["Description"];
            ScriptFile = data[cixScriptSection]["Script"];
            IconFile = Path.Combine(manifestFolder, data[cixScriptSection]["Icon"]);
            InstallToToolbar = Boolean.TryParse(data[cixScriptSection]["Toolbar"], out tempValue) && tempValue;
            Author = data[cixScriptSection]["Author"];
        }
    }

    internal sealed class ScriptManager
    {
        private readonly LuaAPI _luaAPI;
        private readonly Dictionary<string, NLua.Lua> _scriptObjects;

        private const string ManifestFileName = "install.ini";

        public ScriptManager(MainForm mainForm)
        {
            _luaAPI = new LuaAPI(mainForm);
            _scriptObjects = new Dictionary<string, NLua.Lua>();
        }

        /// <summary>
        /// Locate and, if found, load all scripts in the scripts folder in the home directory.
        /// Then call the initialisation procedure for each script to allow them to register to
        /// receive events.
        /// </summary>
        public void LoadScripts()
        {
            string scriptsFolder = Path.Combine(CIX.HomeFolder, "scripts");
            if (Directory.Exists(scriptsFolder))
            {
                LoadScriptsFromPath(scriptsFolder);
            }
        }

        /// <summary>
        /// Run the specified script.
        /// </summary>
        /// <param name="scriptFile">Script file name</param>
        public void RunScript(string scriptFile)
        {
            if (File.Exists(scriptFile))
            {
                try
                {
                    using (NLua.Lua lua = new NLua.Lua())
                    {
                        lua.LoadCLRPackage();
                        _luaAPI.RegisterFunctions(lua);
                        lua.DoFile(scriptFile);
                        LuaFunction fnc = lua.GetFunction("on_Run");
                        if (fnc != null)
                        {
                            fnc.Call();
                        }
                    }
                }
                catch (LuaScriptException e)
                {
                    MessageBox.Show(e.Message, Resources.ScriptException, MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Run the specified event in all installed scripts.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="data">Optional data passed to event handler</param>
        /// <returns>True if events return success, False if any returned failure</returns>
        public bool RunEvents(string eventName, object data)
        {
            try
            {
                return (from lua in _scriptObjects.Values
                        select lua.GetFunction(eventName) into fnc
                        where fnc != null
                        select fnc.Call(data) into res
                        where res != null && res.Length == 1
                        select Convert.ToBoolean(res[0])).All(thisSuccess => thisSuccess);
            }
            catch (LuaScriptException e)
            {
                MessageBox.Show(e.Message, Resources.ScriptException, MessageBoxButtons.OK);
            }
            return true;
        }

        /// <summary>
        /// Install the script package at the specified path. The script package should be a valid
        /// ZIP file that contains an install.ini file with a "CIXReader Script" section.
        /// </summary>
        /// <param name="installerPath">Path to the installation package</param>
        public void InstallScriptPackage(string installerPath)
        {
            bool success = false;

            ScriptManifest manifest = ReadPackageManifest(installerPath);
            if (manifest == null)
            {
                LogFile.WriteLine("Bad script package: missing install.ini in {0}", installerPath);
            }
            else
            {
                // Can accept blanks for everything apart from Script
                if (string.IsNullOrWhiteSpace(manifest.ScriptFile))
                {
                    LogFile.WriteLine("Bad script package: missing Script entry in install.ini in {0}", installerPath);
                }
                else
                {
                    success = true;
                    ScriptPackage isp = new ScriptPackage(manifest);
                    if (isp.ShowDialog() == DialogResult.OK)
                    {
                        // All OK, so unpack the lot and install
                        string packageName = Path.GetFileNameWithoutExtension(installerPath);
                        if (packageName != null)
                        {
                            string scriptFolder = Path.Combine(CIX.HomeFolder, "scripts", packageName);

                            if (!Directory.Exists(scriptFolder))
                            {
                                Directory.CreateDirectory(scriptFolder);
                            }
                            if (UnzipPackage(installerPath, scriptFolder))
                            {
                                manifest = new ScriptManifest(Path.Combine(scriptFolder, ManifestFileName));
                                LoadSingleScript(scriptFolder, manifest);
                            }
                        }
                    }
                }
            }

            if (!success)
            {
                MessageBox.Show(Resources.ScriptInstallError, Resources.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Load all script files in the specified directory.
        /// </summary>
        private void LoadScriptsFromPath(string path)
        {
            try
            {
                // Look in direcories under the folder first.
                foreach (string directory in Directory.EnumerateDirectories(path))
                {
                    LoadScriptsFromPath(directory);
                }

                foreach (string filename in Directory.EnumerateFiles(path))
                {
                    string baseFilename = Path.GetFileName(filename);
                    if (baseFilename != null && baseFilename.ToLower() == ManifestFileName)
                    {
                        LoadSingleScript(path, filename);
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error initialising Lua: {0}", e.Message);
            }
        }

        /// <summary>
        /// Load a single script from the specified manifest.
        /// </summary>
        private void LoadSingleScript(string path, string manifestFile)
        {
            LoadSingleScript(path, new ScriptManifest(manifestFile));
        }

        /// <summary>
        /// Load a single script from the specified manifest.
        /// </summary>
        private void LoadSingleScript(string path, ScriptManifest manifest)
        {
            string scriptFile = Path.Combine(path, manifest.ScriptFile);

            NLua.Lua lua = new NLua.Lua();
            lua.LoadCLRPackage();
            _luaAPI.RegisterFunctions(lua);
            bool success = true;
            try
            {
                lua.DoFile(scriptFile);
                LuaFunction fnc = lua.GetFunction("on_load");
                if (fnc != null)
                {
                    object[] res = fnc.Call();
                    if (res != null && res.Length == 1)
                    {
                        success = Convert.ToBoolean(res[0]);
                    }
                }

                // Cache this script object for event callbacks if the
                // init function returns success.
                if (success)
                {
                    if (_scriptObjects.ContainsKey(scriptFile))
                    {
                        // BUGBUG: What if we have scripts that register events? We need to tell
                        // them to unregister first. Add an interface for this.
                        NLua.Lua oldScript = _scriptObjects[scriptFile];
                        oldScript.Dispose();

                        _scriptObjects.Remove(scriptFile);
                    }
                    _scriptObjects.Add(scriptFile, lua);
                }

                if (manifest.InstallToToolbar)
                {
                    ToolbarDataItem item = new ToolbarDataItem
                    {
                        type = "button",
                        name = "Script",
                        label = manifest.Name,
                        tooltip = manifest.Description,
                        data = scriptFile,
                        image = manifest.IconFile
                    };
                    CRToolbarItemCollection.DefaultCollection.Add(item);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error loading script {0} : {1}", scriptFile, e.Message);
                success = false;
            }
            if (success)
            {
                LogFile.WriteLine("Loaded and initialised script {0}", scriptFile);
            }
            else
            {
                lua.Dispose();
            }
        }

        /// <summary>
        /// Read the script package manifest
        /// </summary>
        /// <param name="installerPath">Path to the script package</param>
        private static ScriptManifest ReadPackageManifest(string installerPath)
        {
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(installerPath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (fileName == ManifestFileName)
                        {
                            string temporaryManifestFile = Path.GetTempFileName();
                            using (FileStream streamWriter = File.Create(temporaryManifestFile))
                            {
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    int size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            ScriptManifest manifest = new ScriptManifest(temporaryManifestFile);
                            File.Delete(temporaryManifestFile);

                            return manifest;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot read zip file {0} : {1}", installerPath, e.Message);
            }
            return null;
        }

        /// <summary>
        /// Uncompress a ZIP file to the specified directory.
        /// </summary>
        /// <returns>True if compression succeeded, false otherwise</returns>
        private static bool UnzipPackage(string installerPath, string destPath)
        {
            bool success = true;
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(installerPath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            string targetDirectory = Path.Combine(destPath, directoryName);
                            Directory.CreateDirectory(targetDirectory);
                        }

                        if (fileName != String.Empty)
                        {
                            string targetFilename = Path.Combine(destPath, theEntry.Name);
                            using (FileStream streamWriter = File.Create(targetFilename))
                            {
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    int size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot uncompress zip file {0} : {1}", installerPath, e.Message);
                success = false;
            }
            return success;
        }
    }
}