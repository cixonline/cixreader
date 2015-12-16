// *****************************************************
// CIXReader
// CRTextBox.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 06/03/2014 10:20
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    /// <summary>
    /// Subclass of the standard TextBox that supports placeholders.
    /// </summary>
    internal sealed class CRTextBox : TextBox
    {
        const string DEFAULT_PLACEHOLDER = "<Input>";
        string _placeholderText = DEFAULT_PLACEHOLDER;
        bool avoidTextChanged;

        /// <summary>
        /// Initialises a new instance of the <see cref="CRTextBox"/> class.
        /// </summary>
        public CRTextBox()
        {
            SubscribeEvents();
            IsPlaceholderActive = true;
        }

        /// <summary>
        /// Inserts placeholder, assigns placeholder style and sets cursor to first position.
        /// </summary>
        public void Reset()
        {
            IsPlaceholderActive = true;

            ActionWithoutTextChanged(() => Text = PlaceholderText);
            Select(0, 0);
        }

        /// <summary>
        /// Gets a value indicating whether the Placeholder is active.
        /// </summary>
        [Browsable(false)]
        public bool IsPlaceholderActive { get; private set; }

        /// <summary>
        /// Gets or sets the placeholder in the PlaceholderTextBox.
        /// </summary>
        [Description("The placeholder associated with the control."), Category("Placeholder"), DefaultValue(DEFAULT_PLACEHOLDER)]
        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value;
                if (IsPlaceholderActive)
                {
                    Text = value;
                }
            }
        }

        /// <summary>
        /// Gets the length of the text in the TextBox.
        /// </summary>
        [Browsable(false)]
        public override int TextLength
        {
            get
            {
                if (IsPlaceholderActive && base.Text == PlaceholderText)
                {
                    return 0;
                }
                return base.TextLength;
            }
        }

        /// <summary>
        /// Gets or sets the current text in the TextBox.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                // Check 'IsPlaceholderActive' to avoid this if-clause when the text is the same as the placeholder but actually it's not the placeholder.
                // Check 'base.Text == this.Placeholder' because in some cases IsPlaceholderActive changes too late although it isn't the placeholder anymore.
                // If you want to get the Text Property and it still contains the placeholder, an empty string will return.
                if (IsPlaceholderActive && base.Text == PlaceholderText)
                {
                    return String.Empty;
                }
                return base.Text;
            }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets or sets the foreground colour of the control.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                // We have to differentiate whether the system is asking for the ForeColor to draw it
                // or the developer is asking for the color.
                if (IsPlaceholderActive && Environment.StackTrace.Contains("System.Windows.Forms.Control.InitializeDCForWmCtlColor(IntPtr dc, Int32 msg)"))
                {
                    return Color.Gray;
                }
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Run an action with avoiding the TextChanged event.
        /// </summary>
        /// <param name="act">Specifies the action to run.</param>
        private void ActionWithoutTextChanged(Action act)
        {
            avoidTextChanged = true;
            act.Invoke();
            avoidTextChanged = false;
        }

        /// <summary>
        /// Subscribe necessary Events.
        /// </summary>
        private void SubscribeEvents()
        {
            TextChanged += PlaceholderTextBox_TextChanged;
        }

        private void PlaceholderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (avoidTextChanged)
            {
                return;
            }

            // Run code with avoiding recursive call
            ActionWithoutTextChanged(delegate {
                // If the Text is empty, insert placeholder and set cursor to to first position
                if (String.IsNullOrEmpty(Text))
                {
                    Reset();
                    return;
                }

                // If the placeholder is active, revert state to a usual TextBox
                if (IsPlaceholderActive)
                {
                    IsPlaceholderActive = false;

                    // Throw away the placeholder but leave the new typed char
                    Text = Text.Replace(PlaceholderText, String.Empty);

                    // Set Selection to last position
                    Select(TextLength, 0);
                }
            });
            Font = Font;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (IsPlaceholderActive)
            {
                Select(0, 0);
            }
            base.OnGotFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsPlaceholderActive)
            {
                Reset();
            }
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Prevents that the user can go through the placeholder with arrow keys
            if (IsPlaceholderActive && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Override the Ctrl+Backspace to delete the previous character.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Back | Keys.Control))
            {
                for (int i = SelectionStart - 1; i > 0; i--)
                {
                    switch (Text.Substring(i, 1))
                    {
                        case " ":
                        case ";":
                        case ",":
                        case "/":
                        case "\\":
                            Text = Text.Remove(i, SelectionStart - i);
                            SelectionStart = i;
                            return true;
                        case "\n":
                            Text = Text.Remove(i - 1, SelectionStart - i);
                            SelectionStart = i;
                            return true;
                    }
                }
                Clear();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}