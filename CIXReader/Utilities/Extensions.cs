// *****************************************************
// CIXReader
// Extensions.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 06/11/2013 9:51 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Utilities
{
    /// <summary>
    /// The Extensions class provides extensions to other, generally .NET Framework, classes
    /// to provide additional functionality.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Save the Windows Form state to the settings.
        /// </summary>
        /// <param name="winForm">Form to save</param>
        public static void SaveToSettings(this Form winForm)
        {
            string state = winForm.Name + "State";
            string location = winForm.Name + "Location";
            string size = winForm.Name + "Size";

            Preferences.WriteFormWindowState(state, winForm.WindowState);

            switch (winForm.WindowState)
            {
                default:
                    Preferences.WritePoint(location, winForm.RestoreBounds.Location);
                    Preferences.WriteSize(size, winForm.RestoreBounds.Size);
                    break;

                case FormWindowState.Normal:
                    Preferences.WritePoint(location, winForm.Location);
                    Preferences.WriteSize(size, winForm.Size);
                    break;
            }
        }

        /// <summary>
        /// Restore the Windows Form to the state saved in the settings.
        /// </summary>
        /// <param name="winForm">Form to restore</param>
        public static void RestoreFromSettings(this Form winForm)
        {
            string state = winForm.Name + "State";
            string location = winForm.Name + "Location";
            string size = winForm.Name + "Size";

            Point savedLocation = Preferences.ReadPoint(location);
            Size savedSize = Preferences.ReadSize(size);

            int minX = SystemInformation.VirtualScreen.X;
            int minY = SystemInformation.VirtualScreen.Y;
            int maxWidth = SystemInformation.VirtualScreen.Width;
            int maxHeight = SystemInformation.VirtualScreen.Height;

            // Constrain the window location and size to fit within the actual desktop
            // Note that if the app was last run on a secondary display that is positioned to
            // the left or above the primary display then the X and Y coordinates can legally
            // be negative.
            if (savedLocation.X + savedSize.Width > maxWidth)
            {
                savedLocation.X = maxWidth - savedSize.Width;
            }
            if (savedLocation.Y + savedSize.Height > maxHeight)
            {
                savedLocation.Y = maxHeight - savedSize.Height;
            }
            if (savedLocation.X < minX)
            {
                savedLocation.X = minX;
                if (savedLocation.X + savedSize.Width > maxWidth)
                {
                    savedSize.Width = maxWidth;
                }
            }
            if (savedLocation.Y < minY)
            {
                savedLocation.Y = minY;
                if (savedLocation.Y + savedSize.Height > maxHeight)
                {
                    savedSize.Height = maxHeight;
                }
            }

            if (savedSize.Width > 10 && savedSize.Height > 10)
            {
                winForm.Location = savedLocation;
                winForm.Size = savedSize;
            }
            winForm.WindowState = Preferences.ReadWindowState(state);
        }
    }
}