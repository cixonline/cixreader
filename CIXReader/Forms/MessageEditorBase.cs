// *****************************************************
// CIXReader
// MessageEditorBase.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 20/05/2014 12:18
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Base class for all message editors.
    /// </summary>
    public class MessageEditorBase : Form
    {
        private string _currentSignature;

        /// <summary>
        /// Specifies whether a signature is to be added to the message
        /// </summary>
        protected bool AddSignature { get; set; }

        protected virtual TextBox Editor { get; set; }

        protected string CurrentSignatureTitle { get; set; }

        /// <summary>
        /// Copy the selected text to the clipboard.
        /// </summary>
        protected void Copy()
        {
            Editor.Copy();
        }

        /// <summary>
        /// Cut the selected text to the clipboard.
        /// </summary>
        protected void Cut()
        {
            Editor.Cut();
        }

        /// <summary>
        /// Delete the selected text.
        /// </summary>
        protected void Delete()
        {
            Editor.SelectedText = string.Empty;
        }

        /// <summary>
        /// Paste the clipboard to the insertion position.
        /// </summary>
        protected void Paste()
        {
            Editor.Paste();
        }

        /// <summary>
        /// Select all the text.
        /// </summary>
        protected void SelectAll()
        {
            Editor.SelectAll();
        }

        /// <summary>
        /// Undo the last editor action.
        /// </summary>
        protected void Undo()
        {
            Editor.Undo();
        }

        /// <summary>
        /// Load the message text box with any existing text and append a signature
        /// if one is found.
        /// </summary>
        protected void LoadMessage(string forumName, string message)
        {
            Editor.Text = message;

            int textLength = Editor.TextLength;

            string defaultSignature = (forumName != string.Empty) && Signatures.DefaultSignatures.SignatureForTitle(forumName) != null 
                ? forumName
                : Preferences.StandardPreferences.DefaultSignature;
            if (defaultSignature != Signatures.NoSignatureString && AddSignature)
            {
                InsertSignature(defaultSignature);
            }

            Editor.SelectionStart = textLength;
            Editor.SelectionLength = 0;
            Editor.Modified = false;
        }

        protected void ReloadSignaturesList(ComboBox nmSignatureList)
        {
            IEnumerable<string> signatures = Signatures.DefaultSignatures.SignatureTitles;
            nmSignatureList.Items.Clear();
            nmSignatureList.Items.Add(Signatures.NoSignatureString);

            foreach (string signature in signatures)
            {
                nmSignatureList.Items.Add(signature);
            }

            nmSignatureList.SelectedItem = AddSignature ? Preferences.StandardPreferences.DefaultSignature : Signatures.NoSignatureString;
        }

        protected void InsertSignature(string signatureTitle)
        {
            StringBuilder msgText = new StringBuilder(Editor.Text);
            string newSignature = "";
            bool doAppend = true;

            if (signatureTitle != Signatures.NoSignatureString)
            {
                newSignature = string.Format("\r\n\r\n{0}", Signatures.DefaultSignatures.ExpandSignatureForTitle(signatureTitle));
            }
            if (!string.IsNullOrEmpty(_currentSignature))
            {
                int location = msgText.ToString().IndexOf(_currentSignature, StringComparison.Ordinal);
                if (location >= 0)
                {
                    msgText.Replace(_currentSignature, newSignature);
                    Editor.Text = msgText.ToString();
                    doAppend = false;
                }
            }
            if (doAppend)
            {
                msgText.Append(newSignature);
                Editor.Text = msgText.ToString();
            }

            _currentSignature = newSignature;
            CurrentSignatureTitle = signatureTitle;
        }

        /// <summary>
        /// Load all preset custom words from the custom file.
        /// </summary>
        /// <returns></returns>
        protected string [] LoadCustomWords()
        {
            string [] customWords = Resources.CustomWords.Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return customWords;
        }
    }
}