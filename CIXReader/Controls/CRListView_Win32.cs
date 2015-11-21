// *****************************************************
// CIXReader
// CRListView_Win32.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 18/06/2015 15:01
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// Win32 specific portions of CRListView
    /// </summary>
    public partial class CRListView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            if (!SystemInformation.TerminalServerSession)
            {
                DoubleBuffered = true;
            }
            base.OnHandleCreated(e);
        }

        public void ScrollHorizontal(int newPos)
        {
            int pos = NativeMethods.GetScrollPos(Handle, NativeMethods.SBS_HORZ);
            NativeMethods.SendMessage(Handle, NativeMethods.LVM_SCROLL, (IntPtr)(newPos - pos), IntPtr.Zero);
        }

        public void SetSortIcon(int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = NativeMethods.SendMessage(Handle, NativeMethods.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
            for (int columnNumber = 0; columnNumber <= Columns.Count - 1; columnNumber++)
            {
                var columnPtr = new IntPtr(columnNumber);
                var item = new NativeMethods.HDITEM
                {
                    mask = NativeMethods.HDITEM.Mask.Format
                };

                if (NativeMethods.SendMessage(columnHeader, NativeMethods.HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                if (order != SortOrder.None && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            item.fmt &= ~NativeMethods.HDITEM.Format.SortDown;
                            item.fmt |= NativeMethods.HDITEM.Format.SortUp;
                            break;
                        case SortOrder.Descending:
                            item.fmt &= ~NativeMethods.HDITEM.Format.SortUp;
                            item.fmt |= NativeMethods.HDITEM.Format.SortDown;
                            break;
                    }
                }
                else
                {
                    item.fmt &= ~NativeMethods.HDITEM.Format.SortDown & ~NativeMethods.HDITEM.Format.SortUp;
                }

                if (NativeMethods.SendMessage(columnHeader, NativeMethods.HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
            }
        }
    }
}