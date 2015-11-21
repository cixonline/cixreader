// *****************************************************
// CIXReader
// CRToolbar.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 19/05/2015 16:40
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    /// <summary>
    /// Defines the CIXReader toolbar control
    /// </summary>
    internal sealed partial class CRToolbar : Panel
    {
        private ContextMenuStrip _toolbarMenu;
        private Form _customPanel;
        private CRToolbarItem _draggedButton;
        private CRToolbarItem _dragSource;
        private bool _hasDragItem;

        const int marginSize = 8;
        const int searchWidth = 280;
        const int interButtonSpacing = 3;
        const int fixedSpaceWidth = 16;

        /// <summary>
        /// Event handler for toolbar state.
        /// </summary>
        public event ValidateToolbarItemHandler ValidateToolbarItem;
        public delegate void ValidateToolbarItemHandler(object sender, CRToolbarItemEventArgs e);

        /// <summary>
        /// Event handler for toolbar action.
        /// </summary>
        public event ActionToolbarItemHandler ActionToolbarItem;
        public delegate void ActionToolbarItemHandler(object sender, CRToolbarItemEventArgs e);

        public CRToolbar()
        {
            InitializeComponent();

            // Set panel properties that are common to a toolbar
            AutoSize = true;
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            BackColor = UI.System.ToolbarColour;
        }

        /// <summary>
        /// Get or set the flag which controls whether the toolbar can be
        /// customised.
        /// </summary>
        public bool CanCustomise { get; set; }

        /// <summary>
        /// Get or set the flag which indicates whether we're customising.
        /// </summary>
        public bool Customising { get; private set; }

        /// <summary>
        /// Close the toolbar control
        /// </summary>
        public void Close()
        {
            CRToolbarItemCollection.DefaultCollection.Clear();
        }

        /// <summary>
        /// Load and initialise the toolbar.
        /// </summary>
        public void Load()
        {
            bool hasSearchField = false;

            int buttonHeight = Height - 12;
            int buttonWidth = Height - 6;

            SuspendLayout();
            Controls.Clear();

            foreach (CRToolbarItem button in CRToolbarItemCollection.DefaultCollection.Buttons)
            {
                switch (button.Type)
                {
                    case CRToolbarItemType.Search:
                        if (!hasSearchField)
                        {
                            CRSearchField searchField = new CRSearchField
                            {
                                Size = new Size(buttonWidth, buttonHeight),
                                Text = String.Empty,
                                Visible = true
                            };
                            button.Control = searchField;
                            searchField.Tag = button;

                            searchField.MouseDown += ToolbarOnMouseDown;
                            searchField.MouseMove += CustomisationOnMouseMove;

                            Controls.Add(searchField);

                            // Can only have one search field
                            hasSearchField = true;
                        }
                        break;

                    case CRToolbarItemType.Button:
                        {
                            CRRoundButton newButton = new CRRoundButton
                            {
                                Image = button.Image,
                                Name = button.Name,
                                Text = String.Empty,
                                Size = new Size(buttonWidth, buttonHeight),
                                ImageScaling = true,
                                Enabled = (button.ID == ActionID.Script || Customising)
                            };
                            newButton.Click += OnToolbarItemClick;

                            newButton.MouseDown += ToolbarOnMouseDown;
                            newButton.MouseMove += CustomisationOnMouseMove;

                            button.Control = newButton;
                            Controls.Add(newButton);

                            newButton.Tag = button;

                            // Add a tooltip for this button
                            ToolTip newToolTip = new ToolTip();
                            newToolTip.SetToolTip(newButton, button.Tooltip);
                            break;
                        }

                    case CRToolbarItemType.FlexibleSpace:
                    case CRToolbarItemType.Space:
                        {
                            PictureBox pictureBox = new PictureBox
                            {
                                Name = button.Name,
                                Size = new Size(buttonWidth, buttonHeight),
                                Visible = Customising
                            };

                            button.Control = pictureBox;
                            pictureBox.Tag = button;

                            pictureBox.MouseDown += ToolbarOnMouseDown;
                            pictureBox.MouseMove += CustomisationOnMouseMove;

                            Controls.Add(pictureBox);
                            break;
                        }
                }
            }
            Relayout();
            ResumeLayout(true);
        }

        /// <summary>
        /// Invoke the toolbar customisation UI.
        /// </summary>
        public void CustomiseToolbar()
        {
            if (!Customising)
            {
                InternalCustomiseToolbar();
            }
        }

        /// <summary>
        /// Return the toolbar button that corresponds to the specified Action ID or
        /// null if no such button exists.
        /// </summary>
        public int IndexOfItemWithPoint(Point pos)
        {
            CRToolbarItemCollection collection = CRToolbarItemCollection.DefaultCollection;
            for (int index = 0; index < collection.Buttons.Count; ++index)
            {
                CRToolbarItem item = collection.Buttons[index];
                if (item.Control != null)
                {
                    int halfWidth = item.Control.Width/2;
                    Rectangle leftPosition = new Rectangle(item.Control.Left - halfWidth, item.Control.Top, item.Control.Width, item.Control.Height);
                    Rectangle rightPosition = new Rectangle(item.Control.Right - halfWidth, item.Control.Top, item.Control.Width, item.Control.Height);
                    if (leftPosition.Contains(pos))
                    {
                        return index;
                    }
                    if (rightPosition.Contains(pos))
                    {
                        return index + 1;
                    }
                }
            }
            return collection.Buttons.Count;
        }

        /// <summary>
        /// Return the toolbar button that corresponds to the specified one or
        /// null if no such button exists.
        /// </summary>
        public CRToolbarItem MatchingItem(CRToolbarItem match)
        {
            return (from CRToolbarItem item in CRToolbarItemCollection.DefaultCollection.Buttons where item.Name == match.Name && item.Tooltip == match.Tooltip select item).FirstOrDefault();
        }

        /// <summary>
        /// Return the toolbar button that corresponds to the specified Action ID or
        /// null if no such button exists.
        /// </summary>
        public CRToolbarItem ItemWithID(ActionID id)
        {
            return (from CRToolbarItem item in CRToolbarItemCollection.DefaultCollection.Buttons where item.ID == id select item).FirstOrDefault();
        }

        /// <summary>
        /// Force a refresh of the toolbar button states
        /// </summary>
        public void RefreshButtons()
        {
            if (ValidateToolbarItem != null && !Customising)
            {
                foreach (CRToolbarItem item in CRToolbarItemCollection.DefaultCollection.Buttons.
                            Where(btn => btn.Type == CRToolbarItemType.Button).
                            Where(item => item.ID != ActionID.Script))
                {
                    ValidateToolbarItem(this, new CRToolbarItemEventArgs {Item = item});
                }
            }
        }

        /// <summary>
        /// Internal customisation logic
        /// </summary>
        private void InternalCustomiseToolbar()
        {
            const int formWidth = 600;
            const int formHeight = 200;

            Point pos = PointToScreen(new Point((Width - formWidth) / 2, Bottom - Top));
            _customPanel = new Form
            {
                Size = new Size(formWidth, formHeight),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MinimizeBox = false,
                MaximizeBox = false,
                ControlBox = false,
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Window,
                AutoScaleDimensions = new SizeF(6F, 13F),
                AutoScaleMode = AutoScaleMode.Font,
                StartPosition = FormStartPosition.Manual,
                ShowIcon = false,
                ShowInTaskbar = false,
                Text = String.Empty,
                Location = pos
            };

            CRToolbarItemCollection collection = CRToolbarItemCollection.DefaultCollection;

            // Populate the panel with all possible buttons.
            IEnumerable<ToolbarDataItem> allButtons = collection.AllButtons;
            int xPosition = 10;
            int yPosition = 20;
            int nextYPosition = 20;
            int buttonHeight = Height - 12;
            int buttonWidth = Height - 6;
            const int spacing = 14;

            foreach (ToolbarDataItem item in allButtons)
            {
                int itemWidth = (item.Type == CRToolbarItemType.Search) ? (item.Image.Width + 12) : buttonWidth;
                SizeF sizeF = CreateGraphics().MeasureString(item.label, Font);
                int realWidth = Math.Max((int)sizeF.Width, itemWidth);

                CRRoundButton newButton = new CRRoundButton
                {
                    Image = item.Image,
                    Location = new Point(xPosition + (realWidth - itemWidth) / 2, yPosition),
                    Name = item.name,
                    Text = String.Empty,
                    Enabled = true,
                    Parent = _customPanel,
                    Size = new Size(itemWidth, buttonHeight),
                    Tag = item,
                    ImageScaling = true
                };
                newButton.MouseDown += CustomizePanelOnMouseDown;
                newButton.MouseMove += CustomisationOnMouseMove;
                newButton.MouseUp += CustomisationOnMouseUp;

                _customPanel.Controls.Add(newButton);

                Label newButtonLabel = new Label
                {
                    Text = item.label,
                    AutoSize = true,
                    Location = new Point((realWidth - (int)sizeF.Width) / 2 + xPosition, yPosition + buttonHeight + 4)
                };
                _customPanel.Controls.Add(newButtonLabel);

                xPosition += realWidth + spacing;
                nextYPosition = Math.Max(nextYPosition, yPosition + newButton.Height + newButtonLabel.Height + spacing + 4);

                if (xPosition > _customPanel.ClientRectangle.Right - itemWidth)
                {
                    xPosition = spacing;
                    yPosition = nextYPosition;
                }
            }

            const int mainButtonWidth = 80;
            const int mainButtonHeight = 23;
 
            // Adjust height if necessary
            _customPanel.Height = nextYPosition + mainButtonHeight + 8;

            // Close button in bottom right
            Button closeButton = new Button
            {
                Size = new Size(mainButtonWidth, mainButtonHeight),
                Text = Resources.Close,
                Visible = true,
                Enabled = true,
                Parent = _customPanel,
                BackColor = SystemColors.ButtonFace,
                Location = new Point((_customPanel.ClientRectangle.Width - mainButtonWidth) - 4, (_customPanel.ClientRectangle.Height - mainButtonHeight) - 4)
            };
            closeButton.Click += CloseButtonOnClick;
            _customPanel.Controls.Add(closeButton);

            // Reset button in bottom right
            Button resetButton = new Button
            {
                Size = new Size(mainButtonWidth, mainButtonHeight),
                Text = Resources.Reset,
                Visible = true,
                Enabled = true,
                Parent = _customPanel,
                BackColor = SystemColors.ButtonFace,
                Location = new Point((closeButton.Left - (mainButtonWidth + 4)) - 4, (_customPanel.ClientRectangle.Height - mainButtonHeight) - 4)
            };
            resetButton.Click += ResetButtonOnClick;
            _customPanel.Controls.Add(resetButton);

            // Enable all the toolbar buttons so they're draggable
            foreach (CRRoundButton button in (from item in collection.Buttons where item.Type == CRToolbarItemType.Button select item.Control).OfType<CRRoundButton>())
            {
                button.Enabled = true;
            }

            // Make sure the separators are visible
            foreach (CRToolbarItem item in collection.Buttons.Where(item => item.Type == CRToolbarItemType.Space || item.Type == CRToolbarItemType.FlexibleSpace))
            {
                item.Control.Visible = true;
            }

            // Animate the customise bar into view. The following two lines are to ensure that the
            // dialog controls are rendered before they appear.
            _customPanel.Show();
            _customPanel.Hide();
            Platform.AnimateWindowIn(_customPanel.Handle);
            _customPanel.Show(this);

            // Flag we're in customisation mode
            Customising = true;
        }

        /// <summary>
        /// Handle mouse down over a button on the customisation panel.
        /// </summary>
        private void CustomizePanelOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            CRRoundButton button = sender as CRRoundButton;
            if (button != null)
            {
                // Make a copy of the item being dragged.
                ToolbarDataItem item = (ToolbarDataItem)button.Tag;
                _draggedButton = new CRToolbarItem(item)
                {
                    Control = button
                };
                _hasDragItem = false;
                _dragSource = null;
            }
        }

        /// <summary>
        /// Handle mouse down over a button on the toolbar when we're customising.
        /// </summary>
        private void ToolbarOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            if (Customising)
            {
                // Find the corresponding button on the customisation panel and use
                // that as the drag source control.
                Control dragButton = (Control)sender;
                CRToolbarItem dragitem = (CRToolbarItem)dragButton.Tag;

                Control button = (from control in _customPanel.Controls.Cast<Control>().Where(control => control is CRRoundButton) 
                                  let item = (ToolbarDataItem) control.Tag 
                                  where item.name == dragitem.Name 
                                  select control).FirstOrDefault();

                if (button != null)
                {
                    _draggedButton = new CRToolbarItem(dragitem.DataItem)
                    {
                        Control = button
                    };
                }
                _hasDragItem = false;
                _dragSource = dragitem;
            }
        }

        /// <summary>
        /// Dropping a button during customisation.
        /// </summary>
        private void CustomisationOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_draggedButton != null)
            {
                InternalHandleCustomDrop();
                _customPanel.Cursor = DefaultCursor;
                Cursor = DefaultCursor;
            }
        }

        /// <summary>
        /// Handle mouse movement over a button being dragged.
        /// </summary>
        private void CustomisationOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_draggedButton != null && Customising)
            {
                if (!_hasDragItem)
                {
                    Control control = _draggedButton.Control;
                    using (Bitmap buttonImage = new Bitmap(control.Size.Width, control.Size.Height))
                    {
                        control.DrawToBitmap(buttonImage, new Rectangle(new Point(0, 0), control.Size));
                        Cursor = new Cursor(buttonImage.GetHicon());
                        _customPanel.Cursor = Cursor;
                    }
                    control.Capture = true;
                    _hasDragItem = true;
                }
            }
        }

        /// <summary>
        /// Return the control at the given screen coordinates.
        /// </summary>
        /// <param name="container">Container form</param>
        /// <param name="pos">Point in screen coordinates</param>
        /// <returns>The control at the specified location</returns>
        private static Control WindowFromPoint(Control container, Point pos)
        {
            return (from Control c in container.Controls
                where c.Visible && c.Bounds.Contains(pos)
                let child = WindowFromPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top))
                select child ?? c).FirstOrDefault();
        }

        /// <summary>
        /// Handle a toolbar button being dropped, either from the customisation panel onto the
        /// toolbar or from the toolbar itself.
        /// </summary>
        private void InternalHandleCustomDrop()
        {
            Point screenDropLocation = Cursor.Position;
            Control form = TopLevelControl;
            if (form == null || !form.Bounds.Contains(screenDropLocation))
            {
                return;
            }
            Control dropControl = WindowFromPoint(form, form.PointToClient(screenDropLocation));
            if (dropControl != null)
            {
                // Change parent if we dropped on the search field's text box or icon.
                if (dropControl.Parent is CRSearchField)
                {
                    dropControl = dropControl.Parent;
                }

                // If we dropped on a toolbar button, separator control or the search field, switch to
                // the parent for consistency.
                if (dropControl is CRRoundButton || dropControl is PictureBox || dropControl is CRSearchField)
                {
                    dropControl = dropControl.Parent;
                }

                // Dropping on the toolbar at this point means the new button is being inserted.
                // Otherwise it is being deleted.
                Point dropLocation = dropControl.PointToClient(screenDropLocation);
                CRToolbarItemCollection collection = CRToolbarItemCollection.DefaultCollection;

                if (dropControl is CRToolbar)
                {
                    if (_dragSource != null)
                    {
                        collection.Remove(_dragSource);
                        Controls.Remove(_dragSource.Control);
                        Relayout();
                    }
                    else
                    {
                        CRToolbarItem button = MatchingItem(_draggedButton);
                        if (button != null && button.Type != CRToolbarItemType.Space && button.Type != CRToolbarItemType.FlexibleSpace)
                        {
                            collection.Remove(button);
                            Relayout();
                        }
                    }

                    // Find the index of where to insert the button.
                    int index = IndexOfItemWithPoint(dropLocation);

                    // Add to the collection at the index.
                    collection.Insert(index, _draggedButton);

                    // Reload the toolbar.
                    Load();
                    Update();
                }
                else if (_dragSource != null)
                {
                    collection.Remove(_dragSource);
                    Controls.Remove(_dragSource.Control);
                    Relayout();
                    Update();
                }
            }

            _draggedButton.Control.Capture = false;

            _hasDragItem = false;
            _draggedButton = null;
        }

        /// <summary>
        /// Relayout the button after items have been added or removed.
        /// </summary>
        private void Relayout()
        {
            if (Width > 0 && Height > 0)
            {
                CRToolbarItemCollection collection = CRToolbarItemCollection.DefaultCollection;
                AnchorStyles horizontalAnchor = AnchorStyles.Left;

                int buttonHeight = Height - 12;
                int buttonWidth = Height - 6;

                int searchFieldWidth = collection.Buttons.Count(item => item.Type == CRToolbarItemType.Search)*
                                       (searchWidth + interButtonSpacing) - interButtonSpacing;
                int allButtonWidths = collection.Buttons.Count(item => item.Type == CRToolbarItemType.Button)*
                                      (buttonWidth + interButtonSpacing) - interButtonSpacing;
                int allFixedSpacesWidth = collection.Buttons.Count(item => item.Type == CRToolbarItemType.Space)*
                                          fixedSpaceWidth;
                int countOfFlexSpaces = collection.Buttons.Count(item => item.Type == CRToolbarItemType.FlexibleSpace);
                int flexSpaceWidth = (countOfFlexSpaces > 0)
                    ? ((Width - (marginSize*2)) - (searchFieldWidth + allButtonWidths + allFixedSpacesWidth))/
                      countOfFlexSpaces
                    : 0;

                int xPosition = marginSize;
                int yPosition = (Height - buttonHeight)/2;
                foreach (CRToolbarItem button in collection.Buttons)
                {
                    switch (button.Type)
                    {
                        case CRToolbarItemType.Search:
                            button.Control.Anchor = AnchorStyles.Top | horizontalAnchor;
                            button.Control.Location = new Point(xPosition, yPosition);
                            button.Control.Size = new Size(searchWidth, buttonHeight);

                            xPosition += button.Control.Width;
                            break;

                        case CRToolbarItemType.Button:
                            button.Control.Anchor = AnchorStyles.Top | horizontalAnchor;
                            button.Control.Location = new Point(xPosition, yPosition);
                            button.Control.Size = new Size(buttonWidth, buttonHeight);

                            xPosition += button.Control.Width + interButtonSpacing;
                            break;

                        case CRToolbarItemType.FlexibleSpace:
                        case CRToolbarItemType.Space:
                        {
                            int width = (button.Type == CRToolbarItemType.FlexibleSpace)
                                ? flexSpaceWidth
                                : fixedSpaceWidth;
                            Bitmap image = new Bitmap(width - interButtonSpacing, buttonHeight);
                            using (var graphics = Graphics.FromImage(image))
                            {
                                using (Brush backBrush = new SolidBrush(BackColor))
                                {
                                    graphics.FillRectangle(backBrush, 0, 0, image.Width, image.Height);
                                }
                                using (Pen borderPen = new Pen(Color.White))
                                {
                                    graphics.DrawRectangle(borderPen, 0, 0, image.Width - 1, image.Height - 1);
                                }
                            }
                            ((PictureBox) button.Control).Image = image;
                            button.Control.Anchor = AnchorStyles.Top | horizontalAnchor;
                            button.Control.Location = new Point(xPosition, yPosition);
                            button.Control.Size = new Size(width, buttonHeight);
                            if (button.Type == CRToolbarItemType.FlexibleSpace)
                            {
                                horizontalAnchor = AnchorStyles.Right;
                            }

                            xPosition += width;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reset the toolbar back to the default.
        /// </summary>
        private void ResetButtonOnClick(object sender, EventArgs e)
        {
            CRToolbarItemCollection.DefaultCollection.Reset();
            Load();
        }

        /// <summary>
        /// Close the customisation panel.
        /// </summary>
        private void CloseButtonOnClick(object sender, EventArgs eventArgs)
        {
            Platform.AnimateWindowOut(_customPanel.Handle);
            _customPanel.Close();

            CRToolbarItemCollection collection = CRToolbarItemCollection.DefaultCollection;

            collection.Save();

            Customising = false;

            // Hide the separators and restore the buttons state back
            foreach (CRToolbarItem item in collection.Buttons.Where(item => item.Type == CRToolbarItemType.Space || item.Type == CRToolbarItemType.FlexibleSpace))
            {
                item.Control.Visible = false;
            }
            RefreshButtons();
        }

        /// <summary>
        /// Handle right-click on the toolbar to display a context menu.
        /// </summary>
        private void OnMouseClick(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Right && CanCustomise && !Customising)
            {
                if (_toolbarMenu == null)
                {
                    _toolbarMenu = new ContextMenuStrip();

                    ToolStripMenuItem customiseMenuItem = new ToolStripMenuItem
                    {
                        Text = Resources.CustomiseToolbar
                    };
                    customiseMenuItem.Click += OnCustomiseToolbarClick;
                    _toolbarMenu.Items.Add(customiseMenuItem);
                }
                _toolbarMenu.Show(this, args.Location, ToolStripDropDownDirection.BelowRight);
            }
        }

        /// <summary>
        /// Invoke customisation.
        /// </summary>
        private void OnCustomiseToolbarClick(object sender, EventArgs eventArgs)
        {
            InternalCustomiseToolbar();
        }

        /// <summary>
        /// Handles a toolbar button click.
        /// </summary>
        private void OnToolbarItemClick(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null && ActionToolbarItem != null && !Customising)
            {
                CRToolbarItem item = control.Tag as CRToolbarItem;
                if (item != null)
                {
                    ActionToolbarItem(this, new CRToolbarItemEventArgs {Item = item});
                }
            }
        }
    }

    /// <summary>
    /// Class that encapsulates the preferences change event arguments
    /// </summary>
    internal sealed class CRToolbarItemEventArgs : EventArgs
    {
        /// <summary>
        /// The toolbar item
        /// </summary>
        public CRToolbarItem Item { get; set; }
    }
}