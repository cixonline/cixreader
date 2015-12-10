// *****************************************************
// CIXReader
// SettingsRules.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/12/2015 14:30
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CIXClient;

namespace CIXReader.Forms
{
    public sealed partial class SettingsRules : Form
    {
        public SettingsRules()
        {
            InitializeComponent();
        }

        private void SettingsRules_Load(object sender, EventArgs e)
        {
            LoadRulesList();

            settingsDeleteRule.Enabled = false;
            settingsEditRule.Enabled = false;
        }

        /// <summary>
        /// Called when the user selects a rule from the list.
        /// </summary>
        private void settingsRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRuleButtons();
        }

        /// <summary>
        /// Update the state of the rule editing buttons
        /// </summary>
        private void UpdateRuleButtons()
        {
            ListView.SelectedIndexCollection index = settingsRulesList.SelectedIndices;
            bool hasSelection = (index.Count > 0);
            settingsEditRule.Enabled = hasSelection;
            settingsDeleteRule.Enabled = hasSelection;
        }

        /// <summary>
        /// Create a new rule.
        /// </summary>
        private void settingsNewRule_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Delete the selected rule.
        /// </summary>
        private void settingsDeleteRule_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selections = settingsRulesList.SelectedItems;
            if (selections.Count == 1)
            {
                ListViewItem lvItem = selections[0];
                CIX.RuleCollection.DeleteRule(lvItem.Text);
                CIX.RuleCollection.Save();
            }
            LoadRulesList();
            UpdateRuleButtons();
        }

        /// <summary>
        /// Load the signatures list.
        /// </summary>
        private void LoadRulesList()
        {
            IEnumerable<string> rules = CIX.RuleCollection.RuleTitles;

            settingsRulesList.Items.Clear();
            foreach (string title in rules)
            {
                ListViewItem lvItem = new ListViewItem { Text = title };
                settingsRulesList.Items.Add(lvItem);
            }
        }

        private void settingsEditRule_Click(object sender, EventArgs e)
        {
            EditRule();
        }

        /// <summary>
        /// Double-click edits the selected item.
        /// </summary>
        private void settingsRules_DoubleClick(object sender, EventArgs e)
        {
            EditRule();
        }

        private void EditRule()
        {
            ListView.SelectedListViewItemCollection selections = settingsRulesList.SelectedItems;
            if (selections.Count == 1)
            {
                ListViewItem lvItem = selections[0];
                string ruleBeingEdited = lvItem.Text;
            }
        }
    }
}