// *****************************************************
// CIXReader
// MainForm_Win32.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 18/06/2015 16:50
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using AppLimit.NetSparkle;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    public partial class MainForm : IMessageFilter
    {
        private Sparkle _sparkle;

        /// <summary>
        /// Secondary MainForm initialisation.
        /// </summary>
        public void Initialise()
        {
            Application.AddMessageFilter(this);

            bool useBeta = Preferences.StandardPreferences.UseBeta;

            _sparkle = new Sparkle(useBeta ? Constants.BetaAppCastURL : Constants.AppCastURL);
            _sparkle.installAndRelaunch += _sparkle_installAndRelaunch;

            _sparkle.StartLoop(true);
        }

        /// <summary>
        /// Pre-filter incoming Windows messages to redirect mouse wheel messages to the window under
        /// the cursor regardless of whether or not it has focus.
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_MOUSEWHEEL)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                NativeMethods.POINT pos;

                pos.X = (m.LParam.ToInt32() & 0xffff);
                pos.Y = (m.LParam.ToInt32() >> 16);
                IntPtr hWnd = NativeMethods.WindowFromPoint(pos);

                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && FromHandle(hWnd) != null)
                {
                    NativeMethods.SendMessage(hWnd, (uint)m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            switch (m.Msg)
            {
                case NativeMethods.WM_THEMECHANGED:
                    UI.InvokeThemeChanged();
                    break;

                case NativeMethods.WM_MOUSEWHEEL:
                case NativeMethods.WM_LBUTTONDOWN:
                case NativeMethods.WM_KEYDOWN:
                    _toolbar.RefreshButtons();
                    break;
            }
            return false;
        }

        /// <summary>
        /// Intercept Windows procedure messages to trap the WM_SHOWME from a second
        /// running instance.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }

                bool top = TopMost;
                TopMost = true;
                TopMost = top;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Check for product updates.
        /// </summary>
        private void mainCheckForUpdates_Click(object sender, EventArgs e)
        {
            bool useBeta = Preferences.StandardPreferences.UseBeta;
            _sparkle = new Sparkle(useBeta ? Constants.BetaAppCastURL : Constants.AppCastURL);

            // Make sure INI file changes get preserved
            Preferences.Save();

            NetSparkleConfiguration config = _sparkle.GetApplicationConfig();
            NetSparkleAppCastItem latestVersion;
            Boolean bUpdateRequired = _sparkle.IsUpdateRequired(config, out latestVersion);

            _sparkle.installAndRelaunch += _sparkle_installAndRelaunch;

            if (bUpdateRequired)
            {
                _sparkle.ShowUpdateNeededUI(latestVersion);
            }
            else
            {
                MessageBox.Show(String.Format(Resources.LatestVersion, Program.VersionString), Resources.LatestVersionTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Do stuff to prepare for the app being restarted by Sparkle.
        /// </summary>
        private void _sparkle_installAndRelaunch(object sender, EventArgs args)
        {
            HandleFormClosure(true);
        }
    }
}