// *****************************************************
// CIXReader
// NativeMethods.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 18/12/2013 16:52
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Runtime.InteropServices;

namespace CIXReader.Utilities
{
    /// <summary>
    /// This class just wraps some Win32 stuff that we're going to use
    /// </summary>
    internal static class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;

        public const int WM_VSCROLL = 0x0115;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_THEMECHANGED = 0x031A;

        public const int AW_ACTIVATE = 0x20000;
        public const int AW_HIDE = 0x10000;
        public const int AW_SLIDE = 0X40000;
        public const int AW_VER_POSITIVE = 0x4;
        public const int AW_VER_NEGATIVE = 0x8;

        public const int LVM_FIRST = 0x1000;
        public const int LVM_SCROLL = LVM_FIRST + 20;
        public const int LVM_GETHEADER = LVM_FIRST + 31;

        public const int SBS_HORZ = 0;

        public const int HDM_FIRST = 0x1200;
        public const int HDM_GETITEM = HDM_FIRST + 11;
        public const int HDM_SETITEM = HDM_FIRST + 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public Mask mask;
            public readonly int cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public readonly string pszText;
            public IntPtr hbm;
            public readonly int cchTextMax;
            public Format fmt;
            public IntPtr lParam;
            // _WIN32_IE >= 0x0300 
            public readonly int iImage;
            public readonly int iOrder;
            // _WIN32_IE >= 0x0500
            public readonly uint type;
            public IntPtr pvFilter;
            // _WIN32_WINNT >= 0x0600
            public readonly uint state;

            [Flags]
            public enum Mask
            {
                Format = 0x4,       // HDI_FORMAT
            };

            [Flags]
            public enum Format
            {
                SortDown = 0x200,   // HDF_SORTDOWN
                SortUp = 0x400,     // HDF_SORTUP
            };
        };

        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

        /// <summary>
        /// Standard Win32 POINT structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// X coordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// Y coordinate.
            /// </summary>
            public int Y;
        }

        // P/Invoke declarations

        // Suppress error 1901 which is a false positive. There's no planned fix from Microsoft for this.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0"), DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(POINT pt);

        [DllImport("user32")]
        internal static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref HDITEM lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        internal static extern int GetScrollPos(IntPtr hWnd, int nBar);
    }
}