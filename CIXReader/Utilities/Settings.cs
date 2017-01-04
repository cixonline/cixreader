// *****************************************************
// CIXReader
// Settings.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 23/06/2015 11:51
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using Microsoft.Win32;

namespace CIXReader.Utilities
{
    internal interface ISimpleCollection {}

    /// <summary>
    /// Stores non-volatile name/value settings. These persist between sessions of the application.
    /// </summary>
    /// <remarks>
    /// On Windows, this class uses the registry.
    /// </remarks>
    public sealed class Settings : ISimpleCollection
    {
        private const string hkcuKey = @"SOFTWARE\CIXReader";

        public static readonly Settings CurrentUser = new Settings(Registry.CurrentUser);

        private readonly RegistryKey rootKey;

        private Settings(RegistryKey rootKey)
        {
            this.rootKey = rootKey;
        }

        private RegistryKey CreateSettingsKey(bool writable)
        {
            RegistryKey softwareKey;

            try
            {
                softwareKey = rootKey.OpenSubKey(hkcuKey, writable);
            }

            catch (Exception)
            {
                softwareKey = null;
            }

            return softwareKey ?? (rootKey.CreateSubKey(hkcuKey));
        }

        /// <summary>
        ///     Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public object GetObject(string key)
        {
            using (RegistryKey pdnKey = CreateSettingsKey(false))
            {
                return pdnKey.GetValue(key);
            }
        }

        /// <summary>
        ///     Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public object GetObject(string key, object defaultValue)
        {
            try
            {
                using (RegistryKey pdnKey = CreateSettingsKey(false))
                {
                    return pdnKey.GetValue(key, defaultValue);
                }
            }

            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetObject(string key, object value)
        {
            using (RegistryKey pdnKey = CreateSettingsKey(true))
            {
                pdnKey.SetValue(key, value);
            }
        }

        /// <summary>
        ///     Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <returns>The value of the key.</returns>
        public string GetString(string key)
        {
            return (string) GetObject(key);
        }

        /// <summary>
        ///     Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public string GetString(string key, string defaultValue)
        {
            return (string) GetObject(key, defaultValue);
        }

        /// <summary>
        ///     Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetString(string key, string value)
        {
            SetObject(key, value);
        }

        /// <summary>
        ///     Retrieves the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to retrieve.</param>
        /// <param name="defaultValue">The default value to use if the key doesn't exist.</param>
        /// <returns>The value of the key, or defaultValue if it didn't exist.</returns>
        public bool GetBoolean(string key, bool defaultValue)
        {
            return bool.Parse(GetString(key, defaultValue.ToString()));
        }

        /// <summary>
        ///     Sets the value of a settings key.
        /// </summary>
        /// <param name="key">The name of the key to set.</param>
        /// <param name="value">The new value of the key.</param>
        public void SetBoolean(string key, bool value)
        {
            SetString(key, value.ToString());
        }
    }
}