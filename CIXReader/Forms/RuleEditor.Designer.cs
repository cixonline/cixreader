namespace CIXReader.Forms
{
    sealed partial class RuleEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleEditor));
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleField = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.matchLabel = new System.Windows.Forms.Label();
            this.typeList = new System.Windows.Forms.ComboBox();
            this.conditionLabel = new System.Windows.Forms.Label();
            this.actionLabel = new System.Windows.Forms.Label();
            this.markMessageRead = new System.Windows.Forms.CheckBox();
            this.markMessagePriority = new System.Windows.Forms.CheckBox();
            this.markMessageIgnored = new System.Windows.Forms.CheckBox();
            this.markMessageFlag = new System.Windows.Forms.CheckBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.deleteRow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(19, 16);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(30, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "&Title:";
            // 
            // titleField
            // 
            this.titleField.Location = new System.Drawing.Point(56, 12);
            this.titleField.Name = "titleField";
            this.titleField.Size = new System.Drawing.Size(225, 20);
            this.titleField.TabIndex = 1;
            this.titleField.TextChanged += new System.EventHandler(this.ruleEditorTitle_TextChanged);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(392, 12);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(392, 42);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // matchLabel
            // 
            this.matchLabel.AutoSize = true;
            this.matchLabel.Location = new System.Drawing.Point(19, 48);
            this.matchLabel.Name = "matchLabel";
            this.matchLabel.Size = new System.Drawing.Size(148, 13);
            this.matchLabel.TabIndex = 2;
            this.matchLabel.Text = "&Match the following condition:";
            // 
            // typeList
            // 
            this.typeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeList.FormattingEnabled = true;
            this.typeList.Items.AddRange(new object[] {
            "Any",
            "All"});
            this.typeList.Location = new System.Drawing.Point(19, 69);
            this.typeList.MaxDropDownItems = 2;
            this.typeList.Name = "typeList";
            this.typeList.Size = new System.Drawing.Size(71, 21);
            this.typeList.TabIndex = 3;
            // 
            // conditionLabel
            // 
            this.conditionLabel.AutoSize = true;
            this.conditionLabel.Location = new System.Drawing.Point(97, 73);
            this.conditionLabel.Name = "conditionLabel";
            this.conditionLabel.Size = new System.Drawing.Size(120, 13);
            this.conditionLabel.TabIndex = 4;
            this.conditionLabel.Text = "of the following are true:";
            // 
            // actionLabel
            // 
            this.actionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.actionLabel.AutoSize = true;
            this.actionLabel.Location = new System.Drawing.Point(19, 250);
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Size = new System.Drawing.Size(251, 13);
            this.actionLabel.TabIndex = 7;
            this.actionLabel.Text = "For successful matches, apply the following actions:";
            // 
            // markMessageRead
            // 
            this.markMessageRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.markMessageRead.AutoSize = true;
            this.markMessageRead.Location = new System.Drawing.Point(19, 274);
            this.markMessageRead.Name = "markMessageRead";
            this.markMessageRead.Size = new System.Drawing.Size(124, 17);
            this.markMessageRead.TabIndex = 8;
            this.markMessageRead.Text = "Mark messages &read";
            this.markMessageRead.UseVisualStyleBackColor = true;
            // 
            // markMessagePriority
            // 
            this.markMessagePriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.markMessagePriority.AutoSize = true;
            this.markMessagePriority.Location = new System.Drawing.Point(19, 298);
            this.markMessagePriority.Name = "markMessagePriority";
            this.markMessagePriority.Size = new System.Drawing.Size(128, 17);
            this.markMessagePriority.TabIndex = 9;
            this.markMessagePriority.Text = "Mark message &priority";
            this.markMessagePriority.UseVisualStyleBackColor = true;
            // 
            // markMessageIgnored
            // 
            this.markMessageIgnored.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.markMessageIgnored.AutoSize = true;
            this.markMessageIgnored.Location = new System.Drawing.Point(19, 322);
            this.markMessageIgnored.Name = "markMessageIgnored";
            this.markMessageIgnored.Size = new System.Drawing.Size(133, 17);
            this.markMessageIgnored.TabIndex = 10;
            this.markMessageIgnored.Text = "Mark message &ignored";
            this.markMessageIgnored.UseVisualStyleBackColor = true;
            // 
            // markMessageFlag
            // 
            this.markMessageFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.markMessageFlag.AutoSize = true;
            this.markMessageFlag.Location = new System.Drawing.Point(19, 346);
            this.markMessageFlag.Name = "markMessageFlag";
            this.markMessageFlag.Size = new System.Drawing.Size(122, 17);
            this.markMessageFlag.TabIndex = 11;
            this.markMessageFlag.Text = "Set &flag on message";
            this.markMessageFlag.UseVisualStyleBackColor = true;
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(19, 108);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(448, 90);
            this.dataGrid.TabIndex = 5;
            this.dataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellValueChanged);
            this.dataGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGrid_CurrentCellDirtyStateChanged);
            this.dataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGrid_RowsAdded);
            // 
            // deleteRow
            // 
            this.deleteRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteRow.Location = new System.Drawing.Point(19, 204);
            this.deleteRow.Name = "deleteRow";
            this.deleteRow.Size = new System.Drawing.Size(101, 25);
            this.deleteRow.TabIndex = 6;
            this.deleteRow.Text = "&Delete Row";
            this.deleteRow.UseVisualStyleBackColor = true;
            this.deleteRow.Click += new System.EventHandler(this.deleteRow_Click);
            // 
            // RuleEditor
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(479, 378);
            this.Controls.Add(this.deleteRow);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.markMessageFlag);
            this.Controls.Add(this.markMessageIgnored);
            this.Controls.Add(this.markMessagePriority);
            this.Controls.Add(this.markMessageRead);
            this.Controls.Add(this.actionLabel);
            this.Controls.Add(this.conditionLabel);
            this.Controls.Add(this.typeList);
            this.Controls.Add(this.matchLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.titleField);
            this.Controls.Add(this.titleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(495, 417);
            this.Name = "RuleEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule Editor";
            this.Load += new System.EventHandler(this.RuleEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleField;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label matchLabel;
        private System.Windows.Forms.ComboBox typeList;
        private System.Windows.Forms.Label conditionLabel;
        private System.Windows.Forms.Label actionLabel;
        private System.Windows.Forms.CheckBox markMessageRead;
        private System.Windows.Forms.CheckBox markMessagePriority;
        private System.Windows.Forms.CheckBox markMessageIgnored;
        private System.Windows.Forms.CheckBox markMessageFlag;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button deleteRow;
    }
}