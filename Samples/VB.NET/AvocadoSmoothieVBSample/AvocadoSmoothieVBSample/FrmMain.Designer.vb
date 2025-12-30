<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.lblKernelRadius = New System.Windows.Forms.Label()
        Me.btnExcelExport = New System.Windows.Forms.Button()
        Me.updRadius = New System.Windows.Forms.NumericUpDown()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.slblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gbSmoothingMethod = New System.Windows.Forms.GroupBox()
        Me.chkMiddleMedian = New System.Windows.Forms.CheckBox()
        Me.chkAllMedian = New System.Windows.Forms.CheckBox()
        Me.btnCSVExport = New System.Windows.Forms.Button()
        Me.lblBorderCnt = New System.Windows.Forms.Label()
        Me.updBorderCnt = New System.Windows.Forms.NumericUpDown()
        Me.txtInit = New System.Windows.Forms.TextBox()
        Me.txtRefined = New System.Windows.Forms.TextBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.gbParameters = New System.Windows.Forms.GroupBox()
        Me.lblBoundaryMode = New System.Windows.Forms.Label()
        Me.cbxBoundaryMode = New System.Windows.Forms.ComboBox()
        Me.gbInitData = New System.Windows.Forms.GroupBox()
        Me.gbRefData = New System.Windows.Forms.GroupBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        CType(Me.updRadius, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.statusStrip1.SuspendLayout()
        Me.gbSmoothingMethod.SuspendLayout()
        CType(Me.updBorderCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbParameters.SuspendLayout()
        Me.gbInitData.SuspendLayout()
        Me.gbRefData.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblKernelRadius
        '
        Me.lblKernelRadius.AutoSize = True
        Me.lblKernelRadius.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.lblKernelRadius.Location = New System.Drawing.Point(18, 71)
        Me.lblKernelRadius.Name = "lblKernelRadius"
        Me.lblKernelRadius.Size = New System.Drawing.Size(90, 19)
        Me.lblKernelRadius.TabIndex = 64
        Me.lblKernelRadius.Text = "Kernel Radius"
        '
        'btnExcelExport
        '
        Me.btnExcelExport.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.btnExcelExport.Location = New System.Drawing.Point(691, 548)
        Me.btnExcelExport.Name = "btnExcelExport"
        Me.btnExcelExport.Size = New System.Drawing.Size(177, 32)
        Me.btnExcelExport.TabIndex = 63
        Me.btnExcelExport.Text = "Export to Excel"
        Me.btnExcelExport.UseVisualStyleBackColor = True
        '
        'updRadius
        '
        Me.updRadius.Font = New System.Drawing.Font("Segoe UI Variable Display", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadius.Location = New System.Drawing.Point(166, 69)
        Me.updRadius.Maximum = New Decimal(New Integer() {13, 0, 0, 0})
        Me.updRadius.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.updRadius.Name = "updRadius"
        Me.updRadius.Size = New System.Drawing.Size(120, 25)
        Me.updRadius.TabIndex = 62
        Me.updRadius.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'statusStrip1
        '
        Me.statusStrip1.AutoSize = False
        Me.statusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.statusStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.slblStatus})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 597)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(899, 24)
        Me.statusStrip1.TabIndex = 61
        Me.statusStrip1.Text = "statusStrip1"
        '
        'slblStatus
        '
        Me.slblStatus.Font = New System.Drawing.Font("Segoe UI Variable Display", 9.75!)
        Me.slblStatus.ForeColor = System.Drawing.Color.White
        Me.slblStatus.Name = "slblStatus"
        Me.slblStatus.Size = New System.Drawing.Size(44, 19)
        Me.slblStatus.Text = "Ready"
        '
        'gbSmoothingMethod
        '
        Me.gbSmoothingMethod.Controls.Add(Me.chkMiddleMedian)
        Me.gbSmoothingMethod.Controls.Add(Me.chkAllMedian)
        Me.gbSmoothingMethod.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 12.0!, System.Drawing.FontStyle.Bold)
        Me.gbSmoothingMethod.Location = New System.Drawing.Point(38, 61)
        Me.gbSmoothingMethod.Name = "gbSmoothingMethod"
        Me.gbSmoothingMethod.Size = New System.Drawing.Size(304, 229)
        Me.gbSmoothingMethod.TabIndex = 60
        Me.gbSmoothingMethod.TabStop = False
        Me.gbSmoothingMethod.Text = "Smoothing Method"
        '
        'chkMiddleMedian
        '
        Me.chkMiddleMedian.AutoSize = True
        Me.chkMiddleMedian.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.chkMiddleMedian.Location = New System.Drawing.Point(94, 117)
        Me.chkMiddleMedian.Name = "chkMiddleMedian"
        Me.chkMiddleMedian.Size = New System.Drawing.Size(117, 23)
        Me.chkMiddleMedian.TabIndex = 1
        Me.chkMiddleMedian.Text = "Middle Median"
        Me.chkMiddleMedian.UseVisualStyleBackColor = True
        '
        'chkAllMedian
        '
        Me.chkAllMedian.AutoSize = True
        Me.chkAllMedian.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.chkAllMedian.Location = New System.Drawing.Point(94, 88)
        Me.chkAllMedian.Name = "chkAllMedian"
        Me.chkAllMedian.Size = New System.Drawing.Size(91, 23)
        Me.chkAllMedian.TabIndex = 0
        Me.chkAllMedian.Text = "All Median"
        Me.chkAllMedian.UseVisualStyleBackColor = True
        '
        'btnCSVExport
        '
        Me.btnCSVExport.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.btnCSVExport.Location = New System.Drawing.Point(508, 548)
        Me.btnCSVExport.Name = "btnCSVExport"
        Me.btnCSVExport.Size = New System.Drawing.Size(177, 32)
        Me.btnCSVExport.TabIndex = 67
        Me.btnCSVExport.Text = "Export to CSV"
        Me.btnCSVExport.UseVisualStyleBackColor = True
        '
        'lblBorderCnt
        '
        Me.lblBorderCnt.AutoSize = True
        Me.lblBorderCnt.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.lblBorderCnt.Location = New System.Drawing.Point(18, 104)
        Me.lblBorderCnt.Name = "lblBorderCnt"
        Me.lblBorderCnt.Size = New System.Drawing.Size(92, 19)
        Me.lblBorderCnt.TabIndex = 66
        Me.lblBorderCnt.Text = "Border Count"
        '
        'updBorderCnt
        '
        Me.updBorderCnt.Font = New System.Drawing.Font("Segoe UI Variable Display", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updBorderCnt.Location = New System.Drawing.Point(166, 102)
        Me.updBorderCnt.Maximum = New Decimal(New Integer() {13, 0, 0, 0})
        Me.updBorderCnt.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.updBorderCnt.Name = "updBorderCnt"
        Me.updBorderCnt.Size = New System.Drawing.Size(120, 25)
        Me.updBorderCnt.TabIndex = 65
        Me.updBorderCnt.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtInit
        '
        Me.txtInit.Font = New System.Drawing.Font("Segoe UI Variable Display", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInit.Location = New System.Drawing.Point(6, 28)
        Me.txtInit.Multiline = True
        Me.txtInit.Name = "txtInit"
        Me.txtInit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInit.Size = New System.Drawing.Size(490, 190)
        Me.txtInit.TabIndex = 58
        '
        'txtRefined
        '
        Me.txtRefined.Font = New System.Drawing.Font("Segoe UI Variable Display", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRefined.Location = New System.Drawing.Point(6, 28)
        Me.txtRefined.Multiline = True
        Me.txtRefined.Name = "txtRefined"
        Me.txtRefined.ReadOnly = True
        Me.txtRefined.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRefined.Size = New System.Drawing.Size(490, 190)
        Me.txtRefined.TabIndex = 59
        '
        'btnStart
        '
        Me.btnStart.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.btnStart.Location = New System.Drawing.Point(38, 548)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(304, 32)
        Me.btnStart.TabIndex = 57
        Me.btnStart.Text = "Start Smoothing"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'gbParameters
        '
        Me.gbParameters.Controls.Add(Me.lblBoundaryMode)
        Me.gbParameters.Controls.Add(Me.lblKernelRadius)
        Me.gbParameters.Controls.Add(Me.cbxBoundaryMode)
        Me.gbParameters.Controls.Add(Me.updRadius)
        Me.gbParameters.Controls.Add(Me.updBorderCnt)
        Me.gbParameters.Controls.Add(Me.lblBorderCnt)
        Me.gbParameters.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 12.0!, System.Drawing.FontStyle.Bold)
        Me.gbParameters.Location = New System.Drawing.Point(38, 303)
        Me.gbParameters.Name = "gbParameters"
        Me.gbParameters.Size = New System.Drawing.Size(304, 229)
        Me.gbParameters.TabIndex = 68
        Me.gbParameters.TabStop = False
        Me.gbParameters.Text = "Smoothing Parameters"
        '
        'lblBoundaryMode
        '
        Me.lblBoundaryMode.AutoSize = True
        Me.lblBoundaryMode.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.lblBoundaryMode.Location = New System.Drawing.Point(18, 137)
        Me.lblBoundaryMode.Name = "lblBoundaryMode"
        Me.lblBoundaryMode.Size = New System.Drawing.Size(132, 19)
        Me.lblBoundaryMode.TabIndex = 59
        Me.lblBoundaryMode.Text = "Boundary Handling :"
        '
        'cbxBoundaryMode
        '
        Me.cbxBoundaryMode.Font = New System.Drawing.Font("Segoe UI Variable Display", 10.0!)
        Me.cbxBoundaryMode.FormattingEnabled = True
        Me.cbxBoundaryMode.Items.AddRange(New Object() {"Symmetric", "Adaptive", "Replicate", "ZeroPad"})
        Me.cbxBoundaryMode.Location = New System.Drawing.Point(166, 135)
        Me.cbxBoundaryMode.Name = "cbxBoundaryMode"
        Me.cbxBoundaryMode.Size = New System.Drawing.Size(120, 25)
        Me.cbxBoundaryMode.TabIndex = 58
        '
        'gbInitData
        '
        Me.gbInitData.Controls.Add(Me.txtInit)
        Me.gbInitData.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbInitData.Location = New System.Drawing.Point(366, 61)
        Me.gbInitData.Name = "gbInitData"
        Me.gbInitData.Size = New System.Drawing.Size(502, 229)
        Me.gbInitData.TabIndex = 69
        Me.gbInitData.TabStop = False
        Me.gbInitData.Text = "Initial Data"
        '
        'gbRefData
        '
        Me.gbRefData.Controls.Add(Me.txtRefined)
        Me.gbRefData.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbRefData.Location = New System.Drawing.Point(366, 303)
        Me.gbRefData.Name = "gbRefData"
        Me.gbRefData.Size = New System.Drawing.Size(502, 229)
        Me.gbRefData.TabIndex = 70
        Me.gbRefData.TabStop = False
        Me.gbRefData.Text = "Refined Data"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 17.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(31, 14)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(365, 31)
        Me.lblTitle.TabIndex = 71
        Me.lblTitle.Text = "AvocadoSmoothie.Barista.Tasting"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(899, 621)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.gbRefData)
        Me.Controls.Add(Me.gbInitData)
        Me.Controls.Add(Me.gbParameters)
        Me.Controls.Add(Me.btnExcelExport)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.gbSmoothingMethod)
        Me.Controls.Add(Me.btnCSVExport)
        Me.Controls.Add(Me.btnStart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AvocadoSmoothie.Barista.Tasting"
        CType(Me.updRadius, System.ComponentModel.ISupportInitialize).EndInit()
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        Me.gbSmoothingMethod.ResumeLayout(False)
        Me.gbSmoothingMethod.PerformLayout()
        CType(Me.updBorderCnt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbParameters.ResumeLayout(False)
        Me.gbParameters.PerformLayout()
        Me.gbInitData.ResumeLayout(False)
        Me.gbInitData.PerformLayout()
        Me.gbRefData.ResumeLayout(False)
        Me.gbRefData.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents lblKernelRadius As Label
    Private WithEvents btnExcelExport As Button
    Private WithEvents updRadius As NumericUpDown
    Private WithEvents statusStrip1 As StatusStrip
    Private WithEvents slblStatus As ToolStripStatusLabel
    Private WithEvents gbSmoothingMethod As GroupBox
    Private WithEvents chkMiddleMedian As CheckBox
    Private WithEvents chkAllMedian As CheckBox
    Private WithEvents btnCSVExport As Button
    Private WithEvents lblBorderCnt As Label
    Private WithEvents updBorderCnt As NumericUpDown
    Private WithEvents txtInit As TextBox
    Private WithEvents txtRefined As TextBox
    Private WithEvents btnStart As Button
    Friend WithEvents gbParameters As GroupBox
    Private WithEvents lblBoundaryMode As Label
    Private WithEvents cbxBoundaryMode As ComboBox
    Private WithEvents gbInitData As GroupBox
    Private WithEvents gbRefData As GroupBox
    Private WithEvents lblTitle As Label
End Class
