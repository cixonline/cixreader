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
using CIXClient.Collections;
using CIXClient.Models;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    public sealed partial class SettingsRules : Form
    {
        private List<RuleGroup> _arrayOfRules;
        private bool _isInitialising;

        public SettingsRules()
        {
            InitializeComponent();
        }

        private void SettingsRules_Load(object sender, EventArgs e)
        {
            _arrayOfRules = CIX.RuleCollection.AllRules;

            ReloadRules(0);
        }

        /// <summary>
        /// Update the state of the rule editing buttons
        /// </summary>
        private void UpdateRuleButtons()
        {
            bool hasSelection = (settingsRulesList.SelectedIndex >= 0);
            settingsEditRule.Enabled = hasSelection;
            settingsDeleteRule.Enabled = hasSelection;
        }

        /// <summary>
        /// Create a new rule.
        /// </summary>
        private void settingsNewRule_Click(object sender, EventArgs e)
        {
            RuleGroup newRuleGroup = new RuleGroup
            {
                title = "",
                active = true,
                type = RuleGroupType.Any,
                actionCode = RuleActionCodes.Unread | RuleActionCodes.Clear,
                rule = new[]
                {
                    new Rule
                    {
                        property = "Subject",
                        value = string.Empty,
                        op = PredicateBuilder.Op.Equals
                    }
                }
            };
            RuleEditor ruleEditor = new RuleEditor(newRuleGroup);
            if (ruleEditor.ShowDialog() == DialogResult.OK)
            {
                CIX.RuleCollection.AddRule(newRuleGroup);

                _arrayOfRules = CIX.RuleCollection.AllRules;
                ReloadRules(0);
            }
        }

        /// <summary>
        /// Delete the selected rule.
        /// </summary>
        private void settingsDeleteRule_Click(object sender, EventArgs e)
        {
            int index = settingsRulesList.SelectedIndex;
            if (index >= 0)
            {
                CIX.RuleCollection.DeleteRule(_arrayOfRules[index]);
                CIX.RuleCollection.Save();

                _arrayOfRules = CIX.RuleCollection.AllRules;
                ReloadRules(index);
            }
        }

        /// <summary>
        /// Load the rules list.
        /// </summary>
        private void ReloadRules(int initialSelection)
        {
            settingsRulesList.Items.Clear();

            _isInitialising = true;

            foreach (RuleGroup ruleGroup in _arrayOfRules)
            {
                int index = settingsRulesList.Items.Add(ruleGroup.title);
                settingsRulesList.SetItemChecked(index, ruleGroup.active);
            }
            if (initialSelection >= settingsRulesList.Items.Count)
            {
                initialSelection = settingsRulesList.Items.Count - 1;
            }
            settingsRulesList.SelectedIndex = initialSelection;

            _isInitialising = false;

            UpdateRuleButtons();
        }

        /// <summary>
        /// Edit button edits the selected item.
        /// </summary>
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

        /// <summary>
        /// Invoke the rule editor to edit the selected item and save it back
        /// if the user OK's the editor dialog.
        /// </summary>
        private void EditRule()
        {
            int index = settingsRulesList.SelectedIndex;
            if (index >= 0)
            {
                RuleEditor ruleEditor = new RuleEditor(_arrayOfRules[index]);
                if (ruleEditor.ShowDialog() == DialogResult.OK)
                {
                    CIX.RuleCollection.Save();
                }
            }
        }

        /// <summary>
        /// Reset rules back to the default.
        /// </summary>
        private void settingsResetRules_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.ResetRules, Resources.ResetRulesTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CIX.RuleCollection.Reset();
                _arrayOfRules = CIX.RuleCollection.AllRules;
                ReloadRules(0);
            }
        }

        /// <summary>
        /// Selection changed - update the button states.
        /// </summary>
        private void settingsRulesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRuleButtons();
        }

        /// <summary>
        /// User enabled or disabled a specific rule. This can get invoked during
        /// SetItemChecked so we need to filter that scenario out by ensuring that
        /// _isInitialising is set while programmatically changing the state.
        /// </summary>
        private void settingsRulesList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_isInitialising)
            {
                if (e.Index >= 0 && e.Index < _arrayOfRules.Count)
                {
                    _arrayOfRules[e.Index].active = e.NewValue == CheckState.Checked;
                    CIX.RuleCollection.Save();
                }
            }
        }
    }
}