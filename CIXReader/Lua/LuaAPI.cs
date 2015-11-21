// *****************************************************
// CIXReader
// LuaAPI.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 13/05/2014 11:04
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXReader.Forms;

namespace CIXReader.Lua
{
    /// <summary>
    /// Implements the Lua API functions
    /// </summary>
    public sealed class LuaAPI
    {
        private readonly MainForm _mainForm;
        private readonly LuaAPISettings _userSettings;

        /// <summary>
        /// Initialise an instance of the Lua API interface
        /// </summary>
        /// <param name="mainForm">Main form control</param>
        public LuaAPI(MainForm mainForm)
        {
            _mainForm = mainForm;
            _userSettings = new LuaAPISettings();
        }

        /// <summary>
        /// Register API functions with the Lua instance specified.
        /// </summary>
        /// <param name="lua">Lua instance</param>
        public void RegisterFunctions(NLua.Lua lua)
        {
            lua["Settings"] = _userSettings;
            lua["Application"] = _mainForm;
        }
    }
}