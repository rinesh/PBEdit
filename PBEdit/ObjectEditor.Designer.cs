namespace PBEdit
{
    partial class ObjectEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectEditor));
            this.Properties = new PropertyGridEx.PropertyGridEx();
            this.cmbTypes = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.Properties.DocCommentDescription.Size = new System.Drawing.Size(300, 37);
            this.Properties.DocCommentDescription.TabIndex = 1;
            this.Properties.DocCommentImage = null;
            // 
            // 
            // 
            this.Properties.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Properties.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.Properties.DocCommentTitle.Name = "";
            this.Properties.DocCommentTitle.Size = new System.Drawing.Size(300, 15);
            this.Properties.DocCommentTitle.TabIndex = 0;
            this.Properties.DocCommentTitle.UseMnemonic = false;
            this.Properties.Location = new System.Drawing.Point(12, 77);
            this.Properties.Name = "Properties";
            this.Properties.SelectedObject = ((object)(resources.GetObject("Properties.SelectedObject")));
            this.Properties.ShowCustomProperties = true;
            this.Properties.Size = new System.Drawing.Size(306, 261);
            this.Properties.TabIndex = 6;
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
            this.Properties.ToolStrip.Size = new System.Drawing.Size(306, 25);
            this.Properties.ToolStrip.TabIndex = 1;
            this.Properties.ToolStrip.TabStop = true;
            this.Properties.ToolStrip.Text = "PropertyGridToolBar";
            // 
            // cmbTypes
            // 
            this.cmbTypes.FormattingEnabled = true;
            this.cmbTypes.Location = new System.Drawing.Point(12, 14);
            this.cmbTypes.Name = "cmbTypes";
            this.cmbTypes.Size = new System.Drawing.Size(306, 21);
            this.cmbTypes.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(242, 41);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 22);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // ObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 350);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.cmbTypes);
            this.Name = "ObjectEditor";
            this.Text = "ObjectEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridEx.PropertyGridEx Properties;
        private System.Windows.Forms.ComboBox cmbTypes;
        private System.Windows.Forms.Button btnAdd;
    }
}