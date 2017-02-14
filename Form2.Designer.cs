namespace IT_Master_Ap
{
    partial class addTableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(addTableForm));
            this.addColumns = new System.Windows.Forms.DataGridView();
            this.saveTable = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.tableName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.allowNulls = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.addColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // addColumns
            // 
            this.addColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.addColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.dataType,
            this.allowNulls});
            this.addColumns.Location = new System.Drawing.Point(74, 64);
            this.addColumns.Name = "addColumns";
            this.addColumns.Size = new System.Drawing.Size(344, 203);
            this.addColumns.TabIndex = 0;
            this.addColumns.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.addColumns_RowEnter);
            // 
            // saveTable
            // 
            this.saveTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(136)))), ((int)(((byte)(229)))));
            this.saveTable.FlatAppearance.BorderSize = 0;
            this.saveTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveTable.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTable.ForeColor = System.Drawing.Color.White;
            this.saveTable.Location = new System.Drawing.Point(256, 273);
            this.saveTable.Name = "saveTable";
            this.saveTable.Size = new System.Drawing.Size(75, 23);
            this.saveTable.TabIndex = 1;
            this.saveTable.Text = "Save";
            this.saveTable.UseVisualStyleBackColor = false;
            this.saveTable.Click += new System.EventHandler(this.saveTable_Click);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(136)))), ((int)(((byte)(229)))));
            this.Cancel.FlatAppearance.BorderSize = 0;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.White;
            this.Cancel.Location = new System.Drawing.Point(337, 273);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // tableName
            // 
            this.tableName.Location = new System.Drawing.Point(203, 12);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(199, 20);
            this.tableName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(71, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter table name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(125, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter column name and data type";
            // 
            // columnName
            // 
            this.columnName.HeaderText = "Column Name";
            this.columnName.Name = "columnName";
            // 
            // dataType
            // 
            this.dataType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataType.HeaderText = "Data Type";
            this.dataType.Items.AddRange(new object[] {
            "nvarchar(max)",
            "int",
            "bigint",
            "smallint",
            "tinyint",
            "bit",
            "decimal",
            "numeric",
            "float",
            "float",
            "datetime",
            "smalldatetime",
            "date",
            "date",
            "char",
            "varchar",
            "varchar(max)",
            "text",
            "nchar",
            "nvarchar",
            "binary",
            "varbinary"});
            this.dataType.Name = "dataType";
            this.dataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // allowNulls
            // 
            this.allowNulls.FalseValue = "NOT NULL";
            this.allowNulls.HeaderText = "Allow Nulls";
            this.allowNulls.IndeterminateValue = "NOT NULL";
            this.allowNulls.Name = "allowNulls";
            this.allowNulls.TrueValue = "NULL";
            // 
            // addTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(45)))), ((int)(((byte)(89)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(470, 308);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableName);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.saveTable);
            this.Controls.Add(this.addColumns);
            this.DoubleBuffered = true;
            this.Name = "addTableForm";
            this.Text = "Add Table Form";
            ((System.ComponentModel.ISupportInitialize)(this.addColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void addColumns_RowEnter(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            addColumns.Rows[e.RowIndex].Cells[1].Value = "nvarchar(max)";
            addColumns.Rows[e.RowIndex].Cells[2].Value = "NULL";
        }

        #endregion

        private System.Windows.Forms.DataGridView addColumns;
        private System.Windows.Forms.Button saveTable;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowNulls;

    }
}