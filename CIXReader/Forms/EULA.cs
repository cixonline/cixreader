// *****************************************************
// CIXReader
// EULA.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/11/2013 9:53 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    /// <summary>
    /// Class that displays the end user license agreement dialog.
    /// </summary>
    public sealed partial class EULA : Form
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EULA"/> class.
        /// </summary>
        public EULA()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Display the EULA form from the resource.
        /// </summary>
        private void EULA_Load(object sender, EventArgs e)
        {
            eulaText.Rtf = Resources.EULA;
        }

        /// <summary>
        /// User accepted the EULA so return OK
        /// </summary>
        private void eulaAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// User rejected the EULA so return Cancel.
        /// </summary>
        private void eulaReject_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}