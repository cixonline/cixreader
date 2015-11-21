// *****************************************************
// CIXReader
// Platform.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 18/06/2015 14:09
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXReader.Controls;

namespace CIXReader.Utilities
{
    /// <summary>
    /// Platform specific routines for Mono.
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// Ensure code is run on the UI thread.
        /// </summary>
        internal static void UIThread(Control form, MethodInvoker code)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(code);
                return;
            }
            code.Invoke();
        }

        /// <summary>
        /// Switch to another running instance of CIXReader.
        /// </summary>
        internal static void SwitchToExistingCIXReaderInstance()
        {
        }

        /// <summary>
        /// Animate the specified window into view
        /// </summary>
        /// <param name="Handle">Window handle</param>
        internal static void AnimateWindowIn(IntPtr Handle)
        {
        }

        /// <summary>
        /// Animate the specified window out of view
        /// </summary>
        /// <param name="Handle">Window handle</param>
        internal static void AnimateWindowOut(IntPtr Handle)
        {
        }

        /// <summary>
        /// Return the control that currently has the focus.
        /// </summary>
        internal static Control GetFocusedControl()
        {
            return null;
        }

        /// <summary>
        /// Send a WM_VSCROLL message to the specified window with a given direction.
        /// </summary>
        /// <param name="Handle">Window handle</param>
        /// <param name="direction">Non-zero to scroll down, zero to scroll up.</param>
        internal static void ScrollWindow(IntPtr Handle, int direction)
        {
        }
    }
}