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
            this.lblKernelRadius = new System.Windows.Forms.Label();
            this.btnExcelExport = new System.Windows.Forms.Button();
            this.updRadius = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbSmoothingMethods = new System.Windows.Forms.GroupBox();
            this.chkMiddleMedian = new System.Windows.Forms.CheckBox();
            this.chkAllMedian = new System.Windows.Forms.CheckBox();
            this.txtInit = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblBorderCnt = new System.Windows.Forms.Label();
            this.updBorderCnt = new System.Windows.Forms.NumericUpDown();
            this.btnCSVExport = new System.Windows.Forms.Button();
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.lblBoundaryMode = new System.Windows.Forms.Label();
            this.cbxBoundaryMode = new System.Windows.Forms.ComboBox();
            this.gbInitData = new System.Windows.Forms.GroupBox();
            this.gbRefData = new System.Windows.Forms.GroupBox();
            this.txtRefined = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.updRadius)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.gbSmoothingMethods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updBorderCnt)).BeginInit();
            this.gbParameters.SuspendLayout();
            this.gbInitData.SuspendLayout();
            this.gbRefData.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblKernelRadius
            // 
            this.lblKernelRadius.AutoSize = true;
            this.lblKernelRadius.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.lblKernelRadius.Location = new System.Drawing.Point(18, 71);
            this.lblKernelRadius.Name = "lblKernelRadius";
            this.lblKernelRadius.Size = new System.Drawing.Size(90, 19);
            this.lblKernelRadius.TabIndex = 53;
            this.lblKernelRadius.Text = "Kernel Radius";
            // 
            // btnExcelExport
            // 
            this.btnExcelExport.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.btnExcelExport.Location = new System.Drawing.Point(691, 548);
            this.btnExcelExport.Name = "btnExcelExport";
            this.btnExcelExport.Size = new System.Drawing.Size(177, 32);
            this.btnExcelExport.TabIndex = 50;
            this.btnExcelExport.Text = "Export to Excel";
            this.btnExcelExport.UseVisualStyleBackColor = true;
            this.btnExcelExport.Click += new System.EventHandler(this.btnExcelExport_Click);
            // 
            // updRadius
            // 
            this.updRadius.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.updRadius.Location = new System.Drawing.Point(166, 69);
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
            this.updRadius.Size = new System.Drawing.Size(120, 25);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 597);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(899, 24);
            this.statusStrip1.TabIndex = 46;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slblStatus
            // 
            this.slblStatus.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F);
            this.slblStatus.ForeColor = System.Drawing.Color.White;
            this.slblStatus.Name = "slblStatus";
            this.slblStatus.Size = new System.Drawing.Size(44, 19);
            this.slblStatus.Text = "Ready";
            // 
            // gbSmoothingMethods
            // 
            this.gbSmoothingMethods.Controls.Add(this.chkMiddleMedian);
            this.gbSmoothingMethods.Controls.Add(this.chkAllMedian);
            this.gbSmoothingMethods.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold);
            this.gbSmoothingMethods.Location = new System.Drawing.Point(38, 61);
            this.gbSmoothingMethods.Name = "gbSmoothingMethods";
            this.gbSmoothingMethods.Size = new System.Drawing.Size(304, 229);
            this.gbSmoothingMethods.TabIndex = 45;
            this.gbSmoothingMethods.TabStop = false;
            this.gbSmoothingMethods.Text = "Smoothing Methods";
            // 
            // chkMiddleMedian
            // 
            this.chkMiddleMedian.AutoSize = true;
            this.chkMiddleMedian.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.chkMiddleMedian.Location = new System.Drawing.Point(94, 117);
            this.chkMiddleMedian.Name = "chkMiddleMedian";
            this.chkMiddleMedian.Size = new System.Drawing.Size(117, 23);
            this.chkMiddleMedian.TabIndex = 1;
            this.chkMiddleMedian.Text = "Middle Median";
            this.chkMiddleMedian.UseVisualStyleBackColor = true;
            this.chkMiddleMedian.CheckedChanged += new System.EventHandler(this.chkMiddleMedian_CheckedChanged);
            // 
            // chkAllMedian
            // 
            this.chkAllMedian.AutoSize = true;
            this.chkAllMedian.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.chkAllMedian.Location = new System.Drawing.Point(94, 88);
            this.chkAllMedian.Name = "chkAllMedian";
            this.chkAllMedian.Size = new System.Drawing.Size(91, 23);
            this.chkAllMedian.TabIndex = 0;
            this.chkAllMedian.Text = "All Median";
            this.chkAllMedian.UseVisualStyleBackColor = true;
            this.chkAllMedian.CheckedChanged += new System.EventHandler(this.chkAllMedian_CheckedChanged);
            // 
            // txtInit
            // 
            this.txtInit.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInit.Location = new System.Drawing.Point(6, 28);
            this.txtInit.Multiline = true;
            this.txtInit.Name = "txtInit";
            this.txtInit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInit.Size = new System.Drawing.Size(490, 190);
            this.txtInit.TabIndex = 43;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.btnStart.Location = new System.Drawing.Point(38, 548);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(304, 32);
            this.btnStart.TabIndex = 42;
            this.btnStart.Text = "Start Smoothing";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblBorderCnt
            // 
            this.lblBorderCnt.AutoSize = true;
            this.lblBorderCnt.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.lblBorderCnt.Location = new System.Drawing.Point(18, 104);
            this.lblBorderCnt.Name = "lblBorderCnt";
            this.lblBorderCnt.Size = new System.Drawing.Size(92, 19);
            this.lblBorderCnt.TabIndex = 55;
            this.lblBorderCnt.Text = "Border Count";
            // 
            // updBorderCnt
            // 
            this.updBorderCnt.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.updBorderCnt.Location = new System.Drawing.Point(166, 102);
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
            this.updBorderCnt.Size = new System.Drawing.Size(120, 25);
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
            this.btnCSVExport.Location = new System.Drawing.Point(508, 548);
            this.btnCSVExport.Name = "btnCSVExport";
            this.btnCSVExport.Size = new System.Drawing.Size(177, 32);
            this.btnCSVExport.TabIndex = 56;
            this.btnCSVExport.Text = "Export to CSV";
            this.btnCSVExport.UseVisualStyleBackColor = true;
            this.btnCSVExport.Click += new System.EventHandler(this.btnCSVExport_Click);
            // 
            // gbParameters
            // 
            this.gbParameters.Controls.Add(this.lblBoundaryMode);
            this.gbParameters.Controls.Add(this.cbxBoundaryMode);
            this.gbParameters.Controls.Add(this.lblKernelRadius);
            this.gbParameters.Controls.Add(this.updRadius);
            this.gbParameters.Controls.Add(this.lblBorderCnt);
            this.gbParameters.Controls.Add(this.updBorderCnt);
            this.gbParameters.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F);
            this.gbParameters.Location = new System.Drawing.Point(38, 303);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(304, 229);
            this.gbParameters.TabIndex = 57;
            this.gbParameters.TabStop = false;
            this.gbParameters.Text = "Smoothing Parameters";
            // 
            // lblBoundaryMode
            // 
            this.lblBoundaryMode.AutoSize = true;
            this.lblBoundaryMode.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F);
            this.lblBoundaryMode.Location = new System.Drawing.Point(18, 137);
            this.lblBoundaryMode.Name = "lblBoundaryMode";
            this.lblBoundaryMode.Size = new System.Drawing.Size(132, 19);
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
            this.cbxBoundaryMode.Location = new System.Drawing.Point(166, 135);
            this.cbxBoundaryMode.Name = "cbxBoundaryMode";
            this.cbxBoundaryMode.Size = new System.Drawing.Size(120, 25);
            this.cbxBoundaryMode.TabIndex = 56;
            // 
            // gbInitData
            // 
            this.gbInitData.Controls.Add(this.txtInit);
            this.gbInitData.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInitData.Location = new System.Drawing.Point(366, 61);
            this.gbInitData.Name = "gbInitData";
            this.gbInitData.Size = new System.Drawing.Size(502, 229);
            this.gbInitData.TabIndex = 58;
            this.gbInitData.TabStop = false;
            this.gbInitData.Text = "Initial Data";
            // 
            // gbRefData
            // 
            this.gbRefData.Controls.Add(this.txtRefined);
            this.gbRefData.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRefData.Location = new System.Drawing.Point(366, 303);
            this.gbRefData.Name = "gbRefData";
            this.gbRefData.Size = new System.Drawing.Size(502, 229);
            this.gbRefData.TabIndex = 59;
            this.gbRefData.TabStop = false;
            this.gbRefData.Text = "Refined Data";
            // 
            // txtRefined
            // 
            this.txtRefined.Font = new System.Drawing.Font("Segoe UI Variable Display", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefined.Location = new System.Drawing.Point(6, 28);
            this.txtRefined.Multiline = true;
            this.txtRefined.Name = "txtRefined";
            this.txtRefined.ReadOnly = true;
            this.txtRefined.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRefined.Size = new System.Drawing.Size(490, 190);
            this.txtRefined.TabIndex = 45;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(31, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(365, 31);
            this.lblTitle.TabIndex = 60;
            this.lblTitle.Text = "AvocadoSmoothie.Barista.Tasting";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(899, 621);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.gbRefData);
            this.Controls.Add(this.gbInitData);
            this.Controls.Add(this.gbParameters);
            this.Controls.Add(this.btnCSVExport);
            this.Controls.Add(this.btnExcelExport);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gbSmoothingMethods);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AvocadoSmoothie.Barista.Tasting";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updRadius)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gbSmoothingMethods.ResumeLayout(false);
            this.gbSmoothingMethods.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updBorderCnt)).EndInit();
            this.gbParameters.ResumeLayout(false);
            this.gbParameters.PerformLayout();
            this.gbInitData.ResumeLayout(false);
            this.gbInitData.PerformLayout();
            this.gbRefData.ResumeLayout(false);
            this.gbRefData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblKernelRadius;
        private System.Windows.Forms.Button btnExcelExport;
        private System.Windows.Forms.NumericUpDown updRadius;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slblStatus;
        private System.Windows.Forms.GroupBox gbSmoothingMethods;
        private System.Windows.Forms.CheckBox chkMiddleMedian;
        private System.Windows.Forms.CheckBox chkAllMedian;
        private System.Windows.Forms.TextBox txtInit;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblBorderCnt;
        private System.Windows.Forms.NumericUpDown updBorderCnt;
        private System.Windows.Forms.Button btnCSVExport;
        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.Label lblBoundaryMode;
        private System.Windows.Forms.ComboBox cbxBoundaryMode;
        private System.Windows.Forms.GroupBox gbInitData;
        private System.Windows.Forms.GroupBox gbRefData;
        private System.Windows.Forms.TextBox txtRefined;
        private System.Windows.Forms.Label lblTitle;
    }
}

