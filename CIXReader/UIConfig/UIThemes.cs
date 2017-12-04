// *****************************************************
// CIXReader
// UIThemes.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 17/12/2013 16:23
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CIXClient;

namespace CIXReader.UIConfig
{
    /// <summary>
    /// Class that encapsulates a collection of themes.
    /// </summary>
    public sealed class UIThemes : IEnumerable<UITheme>
    {
        private List<UITheme> _allThemes;

        /// <summary>
        /// Return a collection of all themes.
        /// </summary>
        private IEnumerable<UITheme> Themes
        {
            get
            {
                if (_allThemes == null)
                {
                    _allThemes = new List<UITheme>();

                    string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    string themesFolder = Path.Combine(appPath, "Themes");
                    foreach (string themeName in Directory.EnumerateDirectories(themesFolder))
                    {
                        UITheme theme = new UITheme
                        {
                            IsCustom = false,
                            Name = Path.GetFileName(themeName)
                        };
                        _allThemes.Add(theme);
                    }
                    if (CIX.HomeFolder != null)
                    {
                        themesFolder = Path.Combine(CIX.HomeFolder, "Themes");
                        if (Directory.Exists(themesFolder))
                        {
                            foreach (string themeName in Directory.EnumerateDirectories(themesFolder))
                            {
                                UITheme theme = new UITheme
                                {
                                    IsCustom = true,
                                    Name = Path.GetFileName(themeName)
                                };
                                _allThemes.Add(theme);
                            }
                        }
                    }
                }
                return _allThemes;
            }
        }

        /// <summary>
        /// Returns an enumerator for iterating over the themes.
        /// </summary>
        /// <returns>An enumerator for UITheme</returns>
        public IEnumerator<UITheme> GetEnumerator()
        {
            return Themes.GetEnumerator();
        }

        /// <summary>
        /// Return the UITheme with the given name.
        /// </summary>
        public UITheme this[string s]
        {
            get
            {
                return Themes.FirstOrDefault(thm => thm.Name == s);
            }
        }

        /// <summary>
        /// Add a new custom theme.
        /// </summary>
        /// <param name="custom">Name of the new theme</param>
        public void Add(string custom)
        {
            if (this[custom] == null)
            {
                UITheme theme = new UITheme
                {
                    IsCustom = true,
                    Name = custom
                };
                _allThemes.Add(theme);
            }
        }

        /// <summary>
        /// Remove a custom theme.
        /// </summary>
        /// <param name="custom">Name of the theme to remove</param>
        public void Remove(string custom)
        {
            _allThemes.Remove(this[custom]);
        }

        /// <summary>
        /// Returns an enumerator for iterating over the conversations.
        /// </summary>
        /// <returns>A generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}