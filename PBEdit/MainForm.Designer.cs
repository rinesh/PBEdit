namespace PBEdit
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.btnNewEntity = new System.Windows.Forms.Button();
            this.cmbEntityList = new System.Windows.Forms.ComboBox();
            this.cmbComponentList = new System.Windows.Forms.ComboBox();
            this.btnNewComponent = new System.Windows.Forms.Button();
            this.cmbComponents = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Properties = new PropertyGridEx.PropertyGridEx();
            ((System.ComponentModel.ISupportInitialize)(this.MainFlash)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFlash
            // 
            this.MainFlash.Enabled = true;
            this.MainFlash.Location = new System.Drawing.Point(12, 34);
            this.MainFlash.Name = "MainFlash";
            this.MainFlash.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MainFlash.OcxState")));
            this.MainFlash.Size = new System.Drawing.Size(945, 668);
            this.MainFlash.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(247, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 26);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Location = new System.Drawing.Point(397, 2);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(115, 25);
            this.btnPlayPause.TabIndex = 5;
            this.btnPlayPause.Text = "Play/Pause";
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(12, 623);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtXML.Size = new System.Drawing.Size(945, 111);
            this.txtXML.TabIndex = 4;
            this.txtXML.WordWrap = false;
            // 
            // btnNewEntity
            // 
            this.btnNewEntity.Location = new System.Drawing.Point(6, 19);
            this.btnNewEntity.Name = "btnNewEntity";
            this.btnNewEntity.Size = new System.Drawing.Size(98, 24);
            this.btnNewEntity.TabIndex = 6;
            this.btnNewEntity.Text = "New Entity";
            this.btnNewEntity.UseVisualStyleBackColor = true;
            this.btnNewEntity.Click += new System.EventHandler(this.btnNewEntity_Click);
            // 
            // cmbEntityList
            // 
            this.cmbEntityList.FormattingEnabled = true;
            this.cmbEntityList.Location = new System.Drawing.Point(6, 49);
            this.cmbEntityList.Name = "cmbEntityList";
            this.cmbEntityList.Size = new System.Drawing.Size(351, 21);
            this.cmbEntityList.TabIndex = 7;
            this.cmbEntityList.SelectedIndexChanged += new System.EventHandler(this.cmbEntityList_TextChanged);
            // 
            // cmbComponentList
            // 
            this.cmbComponentList.FormattingEnabled = true;
            this.cmbComponentList.Location = new System.Drawing.Point(6, 106);
            this.cmbComponentList.Name = "cmbComponentList";
            this.cmbComponentList.Size = new System.Drawing.Size(351, 21);
            this.cmbComponentList.Sorted = true;
            this.cmbComponentList.TabIndex = 9;
            // 
            // btnNewComponent
            // 
            this.btnNewComponent.Location = new System.Drawing.Point(6, 76);
            this.btnNewComponent.Name = "btnNewComponent";
            this.btnNewComponent.Size = new System.Drawing.Size(98, 24);
            this.btnNewComponent.TabIndex = 8;
            this.btnNewComponent.Text = "New Component";
            this.btnNewComponent.UseVisualStyleBackColor = true;
            this.btnNewComponent.Click += new System.EventHandler(this.btnNewComponent_Click);
            // 
            // cmbComponents
            // 
            this.cmbComponents.FormattingEnabled = true;
            this.cmbComponents.Location = new System.Drawing.Point(6, 19);
            this.cmbComponents.Name = "cmbComponents";
            this.cmbComponents.Size = new System.Drawing.Size(351, 21);
            this.cmbComponents.Sorted = true;
            this.cmbComponents.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbComponentList);
            this.groupBox1.Controls.Add(this.btnNewComponent);
            this.groupBox1.Controls.Add(this.cmbEntityList);
            this.groupBox1.Controls.Add(this.btnNewEntity);
            this.groupBox1.Location = new System.Drawing.Point(976, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 133);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entity New/Edit";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Properties);
            this.groupBox2.Controls.Add(this.cmbComponents);
            this.groupBox2.Location = new System.Drawing.Point(976, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 561);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
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
            this.Properties.DocCommentDescription.Size = new System.Drawing.Size(345, 37);
            this.Properties.DocCommentDescription.TabIndex = 1;
            this.Properties.DocCommentImage = null;
            // 
            // 
            // 
            this.Properties.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Properties.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.Properties.DocCommentTitle.Name = "";
            this.Properties.DocCommentTitle.Size = new System.Drawing.Size(345, 15);
            this.Properties.DocCommentTitle.TabIndex = 0;
            this.Properties.DocCommentTitle.UseMnemonic = false;
            this.Properties.Location = new System.Drawing.Point(6, 46);
            this.Properties.Name = "Properties";
            this.Properties.SelectedObject = ((object)(resources.GetObject("Properties.SelectedObject")));
            this.Properties.ShowCustomProperties = true;
            this.Properties.Size = new System.Drawing.Size(351, 509);
            this.Properties.TabIndex = 11;
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
            this.Properties.ToolStrip.Size = new System.Drawing.Size(351, 25);
            this.Properties.ToolStrip.TabIndex = 1;
            this.Properties.ToolStrip.TabStop = true;
            this.Properties.ToolStrip.Text = "PropertyGridToolBar";
            this.Properties.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.Properties_SelectedGridItemChanged);
            this.Properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Properties_PropertyValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 746);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnPlayPause);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.MainFlash);
            this.Name = "MainForm";
            this.Text = "PBEdit";
            ((System.ComponentModel.ISupportInitialize)(this.MainFlash)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxShockwaveFlashObjects.AxShockwaveFlash MainFlash;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Button btnNewEntity;
        private System.Windows.Forms.ComboBox cmbEntityList;
        private System.Windows.Forms.ComboBox cmbComponentList;
        private System.Windows.Forms.Button btnNewComponent;
        private System.Windows.Forms.ComboBox cmbComponents;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private PropertyGridEx.PropertyGridEx Properties;


    }
}

