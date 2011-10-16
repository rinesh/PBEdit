namespace PBEdit
{
    partial class ArrayObjectEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrayObjectEditor));
            this.cmbTypes = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.Properties = new PropertyGridEx.PropertyGridEx();
            this.txtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbTypes
            // 
            this.cmbTypes.FormattingEnabled = true;
            this.cmbTypes.Location = new System.Drawing.Point(22, 12);
            this.cmbTypes.Name = "cmbTypes";
            this.cmbTypes.Size = new System.Drawing.Size(245, 21);
            this.cmbTypes.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(284, 32);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 22);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstItems
            // 
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(22, 72);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(351, 264);
            this.lstItems.TabIndex = 3;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // Properties
            // 
            // 
            // 
            // 
            this.Properties.DocCommentDescription.AccessibleName = "";
            this.Properties.DocCommentDescription.AutoEllipsis = true;
            this.Properties.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.Properties.DocCommentDescription.Name = "";
            this.Properties.DocCommentDescription.Size = new System.Drawing.Size(216, 37);
            this.Properties.DocCommentDescription.TabIndex = 1;
            this.Properties.DocCommentImage = null;
            // 
            // 
            // 
            this.Properties.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Properties.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.Properties.DocCommentTitle.Name = "";
            this.Properties.DocCommentTitle.Size = new System.Drawing.Size(216, 15);
            this.Properties.DocCommentTitle.TabIndex = 0;
            this.Properties.DocCommentTitle.UseMnemonic = false;
            this.Properties.Location = new System.Drawing.Point(410, 47);
            this.Properties.Name = "Properties";
            this.Properties.SelectedObject = ((object)(resources.GetObject("Properties.SelectedObject")));
            this.Properties.ShowCustomProperties = true;
            this.Properties.Size = new System.Drawing.Size(222, 288);
            this.Properties.TabIndex = 4;
            // 
            // 
            // 
            this.Properties.ToolStrip.AccessibleName = "ToolBar";
            this.Properties.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Properties.ToolStrip.AllowMerge = false;
            this.Properties.ToolStrip.AutoSize = false;
            this.Properties.ToolStrip.CanOverflow = false;
            this.Properties.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.Properties.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Properties.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.Properties.ToolStrip.Name = "";
            this.Properties.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.Properties.ToolStrip.Size = new System.Drawing.Size(222, 25);
            this.Properties.ToolStrip.TabIndex = 1;
            this.Properties.ToolStrip.TabStop = true;
            this.Properties.ToolStrip.Text = "PropertyGridToolBar";
            this.Properties.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.Properties_SelectedGridItemChanged);
            this.Properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Properties_PropertyValueChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(22, 39);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(245, 20);
            this.txtName.TabIndex = 5;
            // 
            // ArrayObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 393);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.lstItems);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbTypes);
            this.Name = "ArrayObjectEditor";
            this.Text = "Array Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ArrayObjectEditor_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTypes;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstItems;
        private PropertyGridEx.PropertyGridEx Properties;
        private System.Windows.Forms.TextBox txtName;
    }
}