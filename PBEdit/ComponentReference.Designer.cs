namespace PBEdit
{
    partial class ComponentReference
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
            this.cmbEntityName = new System.Windows.Forms.ComboBox();
            this.cmbComponentName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbEntityName
            // 
            this.cmbEntityName.FormattingEnabled = true;
            this.cmbEntityName.Location = new System.Drawing.Point(43, 31);
            this.cmbEntityName.Name = "cmbEntityName";
            this.cmbEntityName.Size = new System.Drawing.Size(221, 21);
            this.cmbEntityName.TabIndex = 0;
            // 
            // cmbComponentName
            // 
            this.cmbComponentName.FormattingEnabled = true;
            this.cmbComponentName.Location = new System.Drawing.Point(43, 78);
            this.cmbComponentName.Name = "cmbComponentName";
            this.cmbComponentName.Size = new System.Drawing.Size(221, 21);
            this.cmbComponentName.TabIndex = 1;
            // 
            // ComponentReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 129);
            this.Controls.Add(this.cmbComponentName);
            this.Controls.Add(this.cmbEntityName);
            this.Name = "ComponentReference";
            this.Text = "ComponentReference";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEntityName;
        private System.Windows.Forms.ComboBox cmbComponentName;
    }
}