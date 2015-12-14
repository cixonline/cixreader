// *****************************************************
// CIXReader
// RuleEditor.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 13/12/2015 13:30
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Models;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    public sealed partial class RuleEditor : Form
    {
        private readonly RuleGroup _ruleGroup;

        private readonly List<string> propertyNames = new List<string>
        {
            "Author",
            "Body",
            "Subject",
            "Parent.Author",
            "Parent.Body",
            "Parent.Subject",
            "Topic.Name",
            "Forum.Name",
            "Priority",
            "Ignored",
            "Parent.Priority",
            "Parent.Ignored",
            "IsMine",
            "IsWithdrawn"
        };
        private readonly Dictionary<PredicateBuilder.Op, string> operatorNames = new Dictionary<PredicateBuilder.Op, string>
        {
            { PredicateBuilder.Op.Equals, "Equals" },
            { PredicateBuilder.Op.NotEquals, "Not Equals" },
            { PredicateBuilder.Op.Contains, "Contains" },
            { PredicateBuilder.Op.StartsWith, "Starts With" },
            { PredicateBuilder.Op.EndsWith, "Ends With" }
        };

        public RuleEditor(RuleGroup ruleGroup)
        {
            InitializeComponent();
            _ruleGroup = ruleGroup;
        }

        /// <summary>
        /// Initialise the rule editor dialog from the settings in the _ruleGroup.
        /// </summary>
        private void RuleEditor_Load(object sender, EventArgs e)
        {
            if (_ruleGroup != null)
            {
                titleField.Text = _ruleGroup.title;
                typeList.SelectedItem = _ruleGroup.type == RuleGroupType.All ? "All" : "Any";

                markMessageRead.Checked = _ruleGroup.actionCode.HasFlag(RuleActionCodes.Unread | RuleActionCodes.Clear);
                markMessagePriority.Checked = _ruleGroup.actionCode.HasFlag(RuleActionCodes.Priority);
                markMessageIgnored.Checked = _ruleGroup.actionCode.HasFlag(RuleActionCodes.Ignored);
                markMessageFlag.Checked = _ruleGroup.actionCode.HasFlag(RuleActionCodes.Flag);

                dataGrid.Columns.Add(new DataGridViewComboBoxColumn
                {
                    DataSource = propertyNames,
                    HeaderText = Resources.Property,
                    DataPropertyName = "Property",
                    Resizable = DataGridViewTriState.False,
                    Width = 130
                });
                dataGrid.Columns.Add(new DataGridViewComboBoxColumn
                {
                    DataSource = operatorNames.Values.ToList(),
                    HeaderText = Resources.Operator,
                    DataPropertyName = "Operator",
                    Resizable = DataGridViewTriState.False,
                    Width = 130
                });
                dataGrid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = Resources.Value,
                    DataPropertyName = "Value",
                    Resizable = DataGridViewTriState.False,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });

                foreach (Rule rule in _ruleGroup.rule)
                {
                    if (!operatorNames.ContainsKey(rule.op))
                    {
                        continue;
                    }
                    DataGridViewComboBoxCell propertyCell = new DataGridViewComboBoxCell
                    {
                        DataSource = propertyNames,
                        Value = rule.property
                    };
                    DataGridViewComboBoxCell operatorCell = new DataGridViewComboBoxCell
                    {
                        DataSource = operatorNames.Values.ToList(),
                        Value = operatorNames[rule.op]
                    };
                    DataGridViewCell valueCell = CellForProperty(rule.property, rule.value);

                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(propertyCell);
                    row.Cells.Add(operatorCell);
                    row.Cells.Add(valueCell);
                    dataGrid.Rows.Add(row);
                }
            }
            UpdateButtons();
        }

        /// <summary>
        /// User clicked the Cancel button.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// User clicked the Save button.
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            _ruleGroup.title = titleField.Text.Trim();
            _ruleGroup.type = (string)typeList.SelectedItem == "All" ? RuleGroupType.All : RuleGroupType.Any;

            List<Rule> rules = new List<Rule>();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                DataGridViewCell propertyCell = row.Cells[0];
                DataGridViewCell operatorCell = row.Cells[1];
                DataGridViewCell valueCell = row.Cells[2];

                // Empty, incomplete, row
                if (propertyCell.Value == null)
                {
                    break;
                }

                rules.Add(new Rule
                {
                    property = propertyCell.Value as string,
                    op = OpByName(operatorCell.Value as string),
                    value = valueCell.Value
                });
            }
            _ruleGroup.rule = rules.ToArray();

            _ruleGroup.actionCode = 0;
            if (markMessageRead.Checked)
            {
                _ruleGroup.actionCode |= RuleActionCodes.Unread | RuleActionCodes.Clear;
            }
            if (markMessagePriority.Checked)
            {
                _ruleGroup.actionCode |= RuleActionCodes.Priority;
            }
            if (markMessageIgnored.Checked)
            {
                _ruleGroup.actionCode |= RuleActionCodes.Ignored;
            }
            if (markMessageFlag.Checked)
            {
                _ruleGroup.actionCode |= RuleActionCodes.Flag;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private PredicateBuilder.Op OpByName(string name)
        {
            return (from key in operatorNames.Keys 
                    let value = operatorNames[key] 
                    where value == name 
                    select key).FirstOrDefault();
        }

        /// <summary>
        /// Disable the Save button if the title field is empty.
        /// </summary>
        private void ruleEditorTitle_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        /// <summary>
        /// Update the buttons. The Save button requires a non-empty title field,
        /// one full row in the data editor and none of the rows empty.
        /// </summary>
        private void UpdateButtons()
        {
            string titleText = titleField.Text;
            bool enabled = !string.IsNullOrWhiteSpace(titleText) && dataGrid.RowCount > 1;
            for (int row = 0; enabled && row < dataGrid.RowCount - 1; row++)
            {
                DataGridViewCell propertyCell = dataGrid.Rows[row].Cells[0];
                DataGridViewCell operatorCell = dataGrid.Rows[row].Cells[1];
                DataGridViewCell valueCell = dataGrid.Rows[row].Cells[2];
                if (propertyCell.Value == null || operatorCell.Value == null || valueCell.Value == null)
                {
                    enabled = false;
                }
            }
            saveButton.Enabled = enabled;
        }

        /// <summary>
        /// Invoked when a datagrid cell value changes. Use this to change the type of the
        /// value field depending on the property.
        /// </summary>
        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCell propertyCell = dataGrid.Rows[e.RowIndex].Cells[0];
                if (propertyCell.Value != null)
                {
                    string propertyName = propertyCell.Value as string;
                    dataGrid.Rows[e.RowIndex].Cells[2] = CellForProperty(propertyName, null);
                }
            }
            UpdateButtons();
        }

        /// <summary>
        /// Return the cell object that corresponds to the specified property.
        /// </summary>
        /// <returns>A DataGridViewCell object</returns>
        private static DataGridViewCell CellForProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "Priority":
                case "Ignored":
                case "Parent.Priority":
                case "Parent.Ignored":
                case "IsMine":
                case "IsWithdrawn":
                    return new DataGridViewCheckBoxCell
                    {
                        Value = value ?? false
                    };

                default:
                    return new DataGridViewTextBoxCell
                    {
                        Value = value ?? string.Empty
                    };
            }
        }

        private void dataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGrid.IsCurrentCellDirty)
            {
                dataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Remove selected rows.
        /// </summary>
        private void deleteRow_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGrid.SelectedRows)
            {
                dataGrid.Rows.RemoveAt(item.Index);
            }
            UpdateButtons();
        }

        /// <summary>
        /// Need to re-enable the Save button if there's a valid full row.
        /// </summary>
        private void dataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateButtons();
        }
    }
}