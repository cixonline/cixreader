// *****************************************************
// CIXReader
// CRInfoBar.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 21/05/2014 21:11
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// Defines the type of icon shown in the info bar
    /// </summary>
    public enum CRInfoBarIcon
    {
        /// <summary>
        /// Indicates no icon is displayed
        /// </summary>
        None,

        /// <summary>
        /// Specifies an error icon.
        /// </summary>
        CRInfoBarIconError
    };

    /// <summary>
    /// The CRInfoBar class defines a custom informational bar control.
    /// </summary>
    public sealed partial class CRInfoBar : UserControl
    {
        private CRInfoBarIcon _currentIcon;
        private readonly Timer _infoBarTimer;

        /// <summary>
        /// Initialises a new instance of the <see cref="CRInfoBar"/> class.
        /// </summary>
        public CRInfoBar()
        {
            InitializeComponent();

            _currentIcon = CRInfoBarIcon.None;

            _infoBarTimer = new Timer();
            _infoBarTimer.Tick += InfoBarTimerOnTick;
        }

        /// <summary>
        /// Get or set the informational bar text.
        /// </summary>
        public string InfoText
        {
            set { text.Text = value; }
            get { return text.Text; }
        }

        /// <summary>
        /// Get or set the informational bar icon
        /// </summary>
        public CRInfoBarIcon InfoIcon
        {
            set
            {
                if (value != _currentIcon)
                {
                    switch (value)
                    {
                        case CRInfoBarIcon.None:
                            icon.Visible = false;
                            text.Left = icon.Left;
                            break;

                        case CRInfoBarIcon.CRInfoBarIconError:
                            icon.Visible = true;
                            icon.Image = Properties.Resources.Error1;
                            text.Left = icon.Left + icon.Image.Width + 8;
                            break;
                    }
                    _currentIcon = value;
                }
            }
            get { return _currentIcon; }
        }

        /// <summary>
        /// Display the info bar for the specified period of time.
        /// </summary>
        /// <param name="delay">Duration in milliseconds</param>
        public void Show(int delay)
        {
            // The following code is a hack intended to force the infobar contents to
            // render before AnimateWindow. Otherwise the icon and text will be invisible until
            // after the animation.
            int infoBarHeight = Height;
            Height = 0;
            Show();
            Hide();
            Height = infoBarHeight;

            _infoBarTimer.Interval = delay;
            _infoBarTimer.Start();

            Platform.AnimateWindowIn(Handle);
            Show();
        }

        /// <summary>
        /// Hide the info bar when the timer tick expires.
        /// </summary>
        private void InfoBarTimerOnTick(object sender, EventArgs eventArgs)
        {
            _infoBarTimer.Stop();
            Platform.AnimateWindowOut(Handle);
            Hide();
        }
    }
}