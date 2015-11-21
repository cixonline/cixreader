// *****************************************************
// CIXReader
// MessageEditorCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 21/03/2014 18:20
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;
using System.Linq;
using CIXClient.Tables;
using CIXReader.Forms;

namespace CIXReader.Utilities
{
    internal static class MessageEditorCollection
    {
        private static List<CIXMessageEditor> _modelessList;

        /// <summary>
        /// Find an existing message editor that matches the requested message.
        /// </summary>
        public static CIXMessageEditor Get(CIXMessage message)
        {
            if (_modelessList == null)
            {
                _modelessList = new List<CIXMessageEditor>();
                return null;
            }
            return _modelessList.FirstOrDefault(editor => editor.Matches(message));
        }

        /// <summary>
        /// Add the specified editor to the list.
        /// </summary>
        public static void Add(CIXMessageEditor editor)
        {
            if (_modelessList == null)
            {
                _modelessList = new List<CIXMessageEditor>();
            }
            _modelessList.Add(editor);
        }

        /// <summary>
        /// Remove the specified editor from the list.
        /// </summary>
        public static void Remove(CIXMessageEditor editor)
        {
            if (_modelessList != null)
            {
                _modelessList.Remove(editor);
            }
        }

        /// <summary>
        /// Close all open message editor windows.
        /// </summary>
        public static bool Close()
        {
            return _modelessList == null || _modelessList.All(editor => editor.PreClose());
        }
    }
}