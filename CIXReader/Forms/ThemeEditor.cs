// *****************************************************
// CIXReader
// ThemeEditor.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/11/2013 9:14 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CIXReader.Properties;
using CIXReader.UIConfig;

namespace CIXReader.Forms
{
    /// <summary>
    /// Implements the Theme editor class.
    /// </summary>
    public sealed partial class ThemeEditor : Form
    {
        private bool _isInit;
        private static FontFamily[] _familyList;

        /// <summary>
        /// Initialises a new instance of the <see cref="ThemeEditor"/> class.
        /// </summary>
        public ThemeEditor()
        {
            InitializeComponent();
        }

        private void ThemeEditor_Load(object sender, EventArgs e)
        {
            _isInit = true;
            _familyList = new System.Drawing.Text.InstalledFontCollection().Families.Where(fml => fml.IsStyleAvailable(FontStyle.Regular)).ToArray();

            // Make sure the UI reflects whatever we have as custom
            UI.CurrentTheme = Resources.Custom;

            LoadFontDropDown(menuFontList);
            LoadFontDropDown(systemFontList);
            LoadFontDropDown(forumsFontList);
            LoadFontDropDown(forumsFoldersFontList);
            LoadFontDropDown(forumsRootFontList);
            LoadFontDropDown(forumsMessageFontList);
            LoadFontSizeDropDown(forumsFontSizeList);
            LoadFontSizeDropDown(forumsFoldersFontSizeList);
            LoadFontSizeDropDown(forumsRootFontSizeList);
            LoadFontSizeDropDown(forumsMessageFontSizeList);
            UpdateThemeState();

            _isInit = false;
        }

        /// <summary>
        /// Fill the specified combo box with a list of all font families.
        /// </summary>
        private static void LoadFontDropDown(ComboBox control)
        {
            foreach (FontFamily family in _familyList)
            {
                control.Items.Add(family.Name);
            }
        }

        /// <summary>
        /// Fill the specified combo box with a list of all font sizes.
        /// </summary>
        private static void LoadFontSizeDropDown(ComboBox control)
        {
            for (int size = 8; size <= 24; ++size)
            {
                control.Items.Add(size.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void UpdateThemeState()
        {
            menuForeColour.BackColor = UI.Menu.ForegroundColour;
            menuBackColour.BackColor = UI.Menu.BackgroundColour;
            menuInactiveColour.BackColor = UI.Menu.InactiveColour;
            menuFontList.SelectedIndex = systemFontList.Items.IndexOf(UI.Menu.font);

            systemFontList.SelectedIndex = systemFontList.Items.IndexOf(UI.System.font);
            systemForeColour.BackColor = UI.System.ForegroundColour;
            systemLinkColour.BackColor = UI.System.LinkColour;
            systemSelectionColour.BackColor = UI.System.SelectionColour;
            systemSelectionTextColour.BackColor = UI.System.SelectionTextColour;
            systemCountColour.BackColor = UI.System.CountColour;
            systemCountTextColour.BackColor = UI.System.CountTextColour;
            systemInfobarColour.BackColor = UI.System.InfoBarColour;
            systemInfobarTextColour.BackColor = UI.System.InfoBarTextColour;
            systemEdgeColour.BackColor = UI.System.EdgeColour;
            systemSplitterBarColour.BackColor = UI.System.SplitterBarColour;

            forumsCollapsedColour.BackColor = UI.Forums.CollapsedColour;
            forumsCommentsColour.BackColor = UI.Forums.CommentColour;
            forumsRootColour.BackColor = UI.Forums.RootColour;
            forumsBodyColour.BackColor = UI.Forums.BodyColour;
            forumsHeaderColour.BackColor = UI.Forums.HeaderFooterColour;
            forumsIgnoredColour.BackColor = UI.Forums.IgnoredColour;
            forumsPriorityColour.BackColor = UI.Forums.PriorityColour;
            forumsDraftColour.BackColor = UI.Forums.DraftColour;
            forumsSelectionColour.BackColor = UI.Forums.SelectionColour;
            forumsSelectionTextColour.BackColor = UI.Forums.SelectionTextColour;
            forumsFontList.SelectedIndex = forumsFontList.Items.IndexOf(UI.Forums.font);
            forumsFontSizeList.SelectedIndex = forumsFontSizeList.Items.IndexOf(UI.Forums.FontSize.ToString(CultureInfo.InvariantCulture));
            forumsFoldersFontList.SelectedIndex = forumsFoldersFontList.Items.IndexOf(UI.Forums.listfont);
            forumsFoldersFontSizeList.SelectedIndex = forumsFoldersFontSizeList.Items.IndexOf(UI.Forums.ListFontSize.ToString(CultureInfo.InvariantCulture));
            forumsRootFontList.SelectedIndex = forumsRootFontList.Items.IndexOf(UI.Forums.rootfont);
            forumsRootFontSizeList.SelectedIndex = forumsRootFontSizeList.Items.IndexOf(UI.Forums.RootFontSize.ToString(CultureInfo.InvariantCulture));
            forumsMessageFontList.SelectedIndex = forumsMessageFontList.Items.IndexOf(UI.Forums.messagefont);
            forumsMessageFontSizeList.SelectedIndex = forumsMessageFontSizeList.Items.IndexOf(UI.Forums.MessageFontSize.ToString(CultureInfo.InvariantCulture));
            forumsToolbarColour.BackColor = UI.System.ToolbarColour;
        }

        private void menuForeColor_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Menu.ForegroundColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Menu.ForegroundColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void menuBackColor_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Menu.BackgroundColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Menu.BackgroundColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void menuInactiveColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Menu.InactiveColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Menu.InactiveColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && systemFontList.SelectedIndex >= 0)
            {
                UI.System.Font = UI.GetFont((string)systemFontList.Items[systemFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void menuFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && menuFontList.SelectedIndex >= 0)
            {
                UI.Menu.Font = UI.GetFont((string)menuFontList.Items[menuFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void systemForeColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.ForegroundColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.ForegroundColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemLinkColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.LinkColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.LinkColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemSelectionColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.SelectionColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.SelectionColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemSelectionTextColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.SelectionTextColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.SelectionTextColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemCountColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.CountColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.CountColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemCountTextColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.CountTextColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.CountTextColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemInfobarColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.InfoBarColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.InfoBarColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemInfobarTextColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.InfoBarTextColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.InfoBarTextColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemEdgeColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.EdgeColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.EdgeColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemSplitterBarColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.SplitterBarColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.SplitterBarColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void systemToolbarColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.System.ToolbarColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.System.ToolbarColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsCommentsColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.CommentColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.CommentColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsRootColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.RootColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.RootColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsBodyColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.BodyColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.BodyColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsPriorityColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.PriorityColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.PriorityColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsCollapsedColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.CollapsedColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.CollapsedColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsIgnoredColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.IgnoredColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.IgnoredColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsHeaderColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.HeaderFooterColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.HeaderFooterColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsDraftColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.DraftColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.DraftColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsSelectionColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.SelectionColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.SelectionColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsSelectionTextColour_Click(object sender, EventArgs e)
        {
            thmColourPicker.Color = UI.Forums.SelectionTextColour;
            if (thmColourPicker.ShowDialog() == DialogResult.OK)
            {
                UI.Forums.SelectionTextColour = thmColourPicker.Color;
                UpdateThemeState();
            }
        }

        private void forumsFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsFontList.SelectedIndex >= 0)
            {
                UI.Forums.Font = UI.GetFont((string)forumsFontList.Items[forumsFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void forumsFontSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsFontSizeList.SelectedIndex >= 0)
            {
                UI.Forums.FontSize = Int32.Parse((string)forumsFontSizeList.Items[forumsFontSizeList.SelectedIndex]);
                UpdateThemeState();
            }
        }

        private void forumsFoldersFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsFoldersFontList.SelectedIndex >= 0)
            {
                UI.Forums.ListFont = UI.GetFont((string)forumsFoldersFontList.Items[forumsFoldersFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void forumsFoldersFontSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsFoldersFontSizeList.SelectedIndex >= 0)
            {
                UI.Forums.ListFontSize = Int32.Parse((string)forumsFoldersFontSizeList.Items[forumsFoldersFontSizeList.SelectedIndex]);
                UpdateThemeState();
            }
        }

        private void forumsRootFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsRootFontList.SelectedIndex >= 0)
            {
                UI.Forums.RootFont = UI.GetFont((string)forumsRootFontList.Items[forumsRootFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void forumsRootFontSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsRootFontSizeList.SelectedIndex >= 0)
            {
                UI.Forums.RootFontSize = Int32.Parse((string)forumsRootFontSizeList.Items[forumsRootFontSizeList.SelectedIndex]);
                UpdateThemeState();
            }
        }

        private void forumsMessageFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsMessageFontList.SelectedIndex >= 0)
            {
                UI.Forums.MessageFont = UI.GetFont((string)forumsMessageFontList.Items[forumsMessageFontList.SelectedIndex], 10);
                UpdateThemeState();
            }
        }

        private void forumsMessageFontSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit && forumsMessageFontSizeList.SelectedIndex >= 0)
            {
                UI.Forums.MessageFontSize = Int32.Parse((string)forumsMessageFontSizeList.Items[forumsMessageFontSizeList.SelectedIndex]);
                UpdateThemeState();
            }
        }

        /// <summary>
        /// If the user closes the Theme Editor window, just hide it rather than
        /// dispose of it.
        /// </summary>
        private void ThemeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// Revert back to the CIXReader built-in themes.
        /// </summary>
        private void revertDefaultButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.RevertThemeToDefault, Resources.AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UI.RevertConfig(false);
                UpdateThemeState();
            }
        }

        /// <summary>
        /// Revert back to the current saved custom theme.
        /// </summary>
        private void revertButton_Click(object sender, EventArgs e)
        {
            UI.RevertConfig(true);
            UpdateThemeState();
        }

        /// <summary>
        /// Save the current customisations to a custom theme file.
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (UI.SaveConfig())
            {
                string messageString = string.Format("Themes saved to {0}", UI.CustomThemeFile);
                MessageBox.Show(messageString, Resources.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}