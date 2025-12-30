namespace AvocadoSmoothie.Barista.Tasting
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.updRadius = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMiddleMedian = new System.Windows.Forms.CheckBox();
            this.chkAllMedian = new System.Windows.Forms.CheckBox();
            this.txtInit = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblBorderCnt = new System.Windows.Forms.Label();
            this.updBorderCnt = new System.Windows.Forms.NumericUpDown();
            this.btnCSVExport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblBoundaryMode = new System.Windows.Forms.Label();
            this.cbxBoundaryMode = new System.Windows.Forms.ComboBox();
            this.gbInitData = new System.Windows.Forms.GroupBox();
            this.gbRefData = new System.Windows.Forms.GroupBox();
            this.txtRefined = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.updRadius)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updBorderCnt)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbInitData.SuspendLayout();
            this.gbRefData.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.label2.Location = new System.Drawing.Point(36, 142);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 36);
            this.label2.TabIndex = 53;
            this.label2.Text = "Kernel Radius";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.btnExport.Location = new System.Drawing.Point(1382, 1096);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(354, 64);
            this.btnExport.TabIndex = 50;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // updRadius
            // 
            this.updRadius.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.updRadius.Location = new System.Drawing.Point(332, 138);
            this.updRadius.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.updRadius.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.updRadius.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.updRadius.Name = "updRadius";
            this.updRadius.Size = new System.Drawing.Size(240, 43);
            this.updRadius.TabIndex = 48;
            this.updRadius.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1198);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1798, 48);
            this.statusStrip1.TabIndex = 46;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slblStatus
            // 
            this.slblStatus.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F);
            this.slblStatus.ForeColor = System.Drawing.Color.White;
            this.slblStatus.Name = "slblStatus";
            this.slblStatus.Size = new System.Drawing.Size(84, 38);
            this.slblStatus.Text = "Ready";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMiddleMedian);
            this.groupBox1.Controls.Add(this.chkAllMedian);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(76, 122);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(608, 458);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Smoothing Methods";
            // 
            // chkMiddleMedian
            // 
            this.chkMiddleMedian.AutoSize = true;
            this.chkMiddleMedian.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.chkMiddleMedian.Location = new System.Drawing.Point(188, 234);
            this.chkMiddleMedian.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkMiddleMedian.Name = "chkMiddleMedian";
            this.chkMiddleMedian.Size = new System.Drawing.Size(223, 40);
            this.chkMiddleMedian.TabIndex = 1;
            this.chkMiddleMedian.Text = "Middle Median";
            this.chkMiddleMedian.UseVisualStyleBackColor = true;
            this.chkMiddleMedian.CheckedChanged += new System.EventHandler(this.chkMiddleMedian_CheckedChanged);
            // 
            // chkAllMedian
            // 
            this.chkAllMedian.AutoSize = true;
            this.chkAllMedian.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.chkAllMedian.Location = new System.Drawing.Point(188, 176);
            this.chkAllMedian.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkAllMedian.Name = "chkAllMedian";
            this.chkAllMedian.Size = new System.Drawing.Size(170, 40);
            this.chkAllMedian.TabIndex = 0;
            this.chkAllMedian.Text = "All Median";
            this.chkAllMedian.UseVisualStyleBackColor = true;
            this.chkAllMedian.CheckedChanged += new System.EventHandler(this.chkAllMedian_CheckedChanged);
            // 
            // txtInit
            // 
            this.txtInit.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInit.Location = new System.Drawing.Point(12, 56);
            this.txtInit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtInit.Multiline = true;
            this.txtInit.Name = "txtInit";
            this.txtInit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInit.Size = new System.Drawing.Size(976, 376);
            this.txtInit.TabIndex = 43;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.btnStart.Location = new System.Drawing.Point(76, 1096);
            this.btnStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(608, 64);
            this.btnStart.TabIndex = 42;
            this.btnStart.Text = "Start Smoothing";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblBorderCnt
            // 
            this.lblBorderCnt.AutoSize = true;
            this.lblBorderCnt.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.lblBorderCnt.Location = new System.Drawing.Point(36, 208);
            this.lblBorderCnt.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBorderCnt.Name = "lblBorderCnt";
            this.lblBorderCnt.Size = new System.Drawing.Size(177, 36);
            this.lblBorderCnt.TabIndex = 55;
            this.lblBorderCnt.Text = "Border Count";
            // 
            // updBorderCnt
            // 
            this.updBorderCnt.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.updBorderCnt.Location = new System.Drawing.Point(332, 204);
            this.updBorderCnt.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.updBorderCnt.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.updBorderCnt.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.updBorderCnt.Name = "updBorderCnt";
            this.updBorderCnt.Size = new System.Drawing.Size(240, 43);
            this.updBorderCnt.TabIndex = 54;
            this.updBorderCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnCSVExport
            // 
            this.btnCSVExport.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.btnCSVExport.Location = new System.Drawing.Point(1016, 1096);
            this.btnCSVExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCSVExport.Name = "btnCSVExport";
            this.btnCSVExport.Size = new System.Drawing.Size(354, 64);
            this.btnCSVExport.TabIndex = 56;
            this.btnCSVExport.Text = "Export to CSV";
            this.btnCSVExport.UseVisualStyleBackColor = true;
            this.btnCSVExport.Click += new System.EventHandler(this.btnCSVExport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblBoundaryMode);
            this.groupBox2.Controls.Add(this.cbxBoundaryMode);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.updRadius);
            this.groupBox2.Controls.Add(this.lblBorderCnt);
            this.groupBox2.Controls.Add(this.updBorderCnt);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F);
            this.groupBox2.Location = new System.Drawing.Point(76, 606);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox2.Size = new System.Drawing.Size(608, 458);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Smoothing Parameters";
            // 
            // lblBoundaryMode
            // 
            this.lblBoundaryMode.AutoSize = true;
            this.lblBoundaryMode.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.lblBoundaryMode.Location = new System.Drawing.Point(36, 274);
            this.lblBoundaryMode.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBoundaryMode.Name = "lblBoundaryMode";
            this.lblBoundaryMode.Size = new System.Drawing.Size(253, 36);
            this.lblBoundaryMode.TabIndex = 57;
            this.lblBoundaryMode.Text = "Boundary Handling :";
            // 
            // cbxBoundaryMode
            // 
            this.cbxBoundaryMode.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.cbxBoundaryMode.FormattingEnabled = true;
            this.cbxBoundaryMode.Items.AddRange(new object[] {
            "Symmetric",
            "Adaptive",
            "Replicate",
            "ZeroPad"});
            this.cbxBoundaryMode.Location = new System.Drawing.Point(332, 270);
            this.cbxBoundaryMode.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cbxBoundaryMode.Name = "cbxBoundaryMode";
            this.cbxBoundaryMode.Size = new System.Drawing.Size(236, 44);
            this.cbxBoundaryMode.TabIndex = 56;
            // 
            // gbInitData
            // 
            this.gbInitData.Controls.Add(this.txtInit);
            this.gbInitData.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInitData.Location = new System.Drawing.Point(732, 122);
            this.gbInitData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gbInitData.Name = "gbInitData";
            this.gbInitData.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gbInitData.Size = new System.Drawing.Size(1004, 458);
            this.gbInitData.TabIndex = 58;
            this.gbInitData.TabStop = false;
            this.gbInitData.Text = "Initial Data";
            // 
            // gbRefData
            // 
            this.gbRefData.Controls.Add(this.txtRefined);
            this.gbRefData.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRefData.Location = new System.Drawing.Point(732, 606);
            this.gbRefData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gbRefData.Name = "gbRefData";
            this.gbRefData.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gbRefData.Size = new System.Drawing.Size(1004, 458);
            this.gbRefData.TabIndex = 59;
            this.gbRefData.TabStop = false;
            this.gbRefData.Text = "Refined Data";
            // 
            // txtRefined
            // 
            this.txtRefined.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefined.Location = new System.Drawing.Point(12, 56);
            this.txtRefined.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtRefined.Multiline = true;
            this.txtRefined.Name = "txtRefined";
            this.txtRefined.ReadOnly = true;
            this.txtRefined.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRefined.Size = new System.Drawing.Size(976, 376);
            this.txtRefined.TabIndex = 45;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(62, 28);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(723, 62);
            this.lblTitle.TabIndex = 60;
            this.lblTitle.Text = "AvocadoSmoothie.Barista.Tasting";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1798, 1246);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.gbRefData);
            this.Controls.Add(this.gbInitData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCSVExport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AvocadoSmoothie.Barista.Tasting";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updRadius)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updBorderCnt)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbInitData.ResumeLayout(false);
            this.gbInitData.PerformLayout();
            this.gbRefData.ResumeLayout(false);
            this.gbRefData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.NumericUpDown updRadius;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slblStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkMiddleMedian;
        private System.Windows.Forms.CheckBox chkAllMedian;
        private System.Windows.Forms.TextBox txtInit;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblBorderCnt;
        private System.Windows.Forms.NumericUpDown updBorderCnt;
        private System.Windows.Forms.Button btnCSVExport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblBoundaryMode;
        private System.Windows.Forms.ComboBox cbxBoundaryMode;
        private System.Windows.Forms.GroupBox gbInitData;
        private System.Windows.Forms.GroupBox gbRefData;
        private System.Windows.Forms.TextBox txtRefined;
        private System.Windows.Forms.Label lblTitle;
    }
}

