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
using System.Windows.Forms;

namespace CIXReader.Utilities
{
    /// <summary>
    /// Platform specific routines for Windows.
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// Ensure code is run on the UI thread.
        /// </summary>
        public static void UIThread(Control form, MethodInvoker code)
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
            NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Animate the specified window into view
        /// </summary>
        /// <param name="Handle">Window handle</param>
        internal static void AnimateWindowIn(IntPtr Handle)
        {
            NativeMethods.AnimateWindow(Handle, 300, NativeMethods.AW_ACTIVATE | NativeMethods.AW_VER_POSITIVE | NativeMethods.AW_SLIDE);
        }

        /// <summary>
        /// Animate the specified window out of view
        /// </summary>
        /// <param name="Handle">Window handle</param>
        internal static void AnimateWindowOut(IntPtr Handle)
        {
            NativeMethods.AnimateWindow(Handle, 300, NativeMethods.AW_HIDE | NativeMethods.AW_VER_NEGATIVE | NativeMethods.AW_SLIDE);
        }

        /// <summary>
        /// Return the control that currently has the focus.
        /// </summary>
        internal static Control GetFocusedControl()
        {
            Control focusedControl = null;
            IntPtr focusedHandle = NativeMethods.GetFocus();
            if (focusedHandle != IntPtr.Zero)
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        /// <summary>
        /// Send a WM_VSCROLL message to the specified window with a given direction.
        /// </summary>
        /// <param name="Handle">Window handle</param>
        /// <param name="direction">Non-zero to scroll down, zero to scroll up.</param>
        internal static void ScrollWindow(IntPtr Handle, int direction)
        {
            NativeMethods.SendMessage(Handle, NativeMethods.WM_VSCROLL, (IntPtr)direction, (IntPtr)0);
        }
    }
}