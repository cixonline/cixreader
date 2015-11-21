// *****************************************************
// CIXReader
// SpellEditorBase.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 20/06/2015 17:40
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    internal class SpellEditorBase : MessageEditorBase
    {
        private NHunspellExtender.NHunspellTextBoxExtender nmSpellChecker;
        private TextBox _editor;

        protected override TextBox Editor
        {
            get { return _editor; }
            set
            {
                _editor = value;

                if (Preferences.StandardPreferences.CheckSpellAsYouType)
                {
                    if (nmSpellChecker == null)
                    {
                        nmSpellChecker = new NHunspellExtender.NHunspellTextBoxExtender
                        {
                            Language = "UK English",
                            MaintainUserChoice = true,
                            ShortcutKey = Shortcut.F7,
                            SpellAsYouType = true
                        };
                    }

                    nmSpellChecker.SetSpellCheckEnabled(_editor, nmSpellChecker.SpellAsYouType);
                    nmSpellChecker.AddCustomWords(LoadCustomWords());
                }
            }
        }
    }
}