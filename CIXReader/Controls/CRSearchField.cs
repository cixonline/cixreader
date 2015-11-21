// *****************************************************
// CIXReader
// CRSearchField.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 31/05/2014 18:49
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXReader.Properties;

namespace CIXReader.Controls
{
    /// <summary>
    /// Implements a search field control.
    /// </summary>
    public sealed partial class CRSearchField : UserControl
    {
        /// <summary>
        /// Trigger on action in the search field.
        /// </summary>
        public event EventHandler<EventArgs> Trigger;

        /// <summary>
        /// Invoked when the search field is cleared.
        /// </summary>
        public event EventHandler<EventArgs> Cleared;

        /// <summary>
        /// Instantiates an instance of the search field control.
        /// </summary>
        public CRSearchField()
        {
            InitializeComponent();

            searchInputField.PlaceholderText = Resources.SearchForText;
        }

        /// <summary>
        /// Override the Text property to return the search input
        /// field value.
        /// </summary>
        public override string Text
        {
            get { return searchInputField.Text; }
            set { searchInputField.Text = value; }
        }

        /// <summary>
        /// Get or set the placeholder text.
        /// </summary>
        public string PlaceholderText
        {
            get { return searchInputField.PlaceholderText; }
            set { searchInputField.PlaceholderText = value; }
        }

        /// <summary>
        /// Invoked when text changed in the control.
        /// </summary>
        private void searchInputField_TextChanged(object sender, EventArgs e)
        {
            searchClose.Visible = !searchInputField.IsPlaceholderActive;
        }

        /// <summary>
        /// When the search field is activated, put the focus on the search input.
        /// </summary>
        private void CRSearchField_Enter(object sender, EventArgs e)
        {
            // ReSharper disable RedundantCheckBeforeAssignment
            if (ActiveControl != searchInputField)
            // ReSharper restore RedundantCheckBeforeAssignment
            {
                ActiveControl = searchInputField;
            }
        }

        /// <summary>
        /// Look for and trigger on the Enter key.
        /// </summary>
        private void searchInputField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Trigger != null)
            {
                Trigger(this, new EventArgs());
            }
        }

        /// <summary>
        /// Trigger a click on the search icon.
        /// </summary>
        private void searchIcon_Click(object sender, EventArgs e)
        {
            if (Trigger != null)
            {
                Trigger(this, new EventArgs());
            }
        }

        /// <summary>
        /// Trigger a click on the close icon.
        /// </summary>
        private void searchClose_Click(object sender, EventArgs e)
        {
            if (Cleared != null)
            {
                Cleared(this, new EventArgs());
            }
        }

        private void CRSearchField_Load(object sender, EventArgs e)
        {
            searchIcon.BackColor = searchContainer.BackColor;
            searchInputField.BackColor = searchContainer.BackColor;
            searchInputField.ForeColor = SystemColors.ControlText;
            searchClose.BackColor = searchContainer.BackColor;
        }
    }
}