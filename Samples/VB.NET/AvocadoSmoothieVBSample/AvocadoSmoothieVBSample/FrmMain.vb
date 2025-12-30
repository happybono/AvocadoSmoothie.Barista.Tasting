Imports AvocadoSmoothie.Barista
Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class FrmMain

    Private Shared ReadOnly BoundaryModes As String() = {"Symmetric", "Adaptive", "Replicate", "ZeroPad"}

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Ensure one method is selected by default
        If Not chkMiddleMedian.Checked AndAlso Not chkAllMedian.Checked Then
            chkMiddleMedian.Checked = True
        End If

        ' Remove 32K limit so large outputs can be displayed
        txtInit.MaxLength = 0
        txtInit.Multiline = True
        txtInit.WordWrap = True

        txtRefined.MaxLength = 0
        txtRefined.Multiline = True
        txtRefined.WordWrap = True

        cbxBoundaryMode.Items.Clear()
        cbxBoundaryMode.Items.AddRange(BoundaryModes)
        If cbxBoundaryMode.Items.Count > 0 Then
            cbxBoundaryMode.SelectedIndex = 0
        End If

        UpdateParameterControls()
        UpdateStatus("Ready")
    End Sub

    ' Enable/disable controls based on method selection
    Private Sub UpdateParameterControls()
        ' BoundaryMode : Enable only if AllMedian is checked
        cbxBoundaryMode.Enabled = chkAllMedian.Checked
        lblBoundaryMode.Enabled = chkAllMedian.Checked

        ' BorderCount : Enable only if MiddleMedian is checked
        updBorderCnt.Enabled = chkMiddleMedian.Checked
        lblBorderCnt.Enabled = chkMiddleMedian.Checked
    End Sub

    Private Sub chkAllMedian_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllMedian.CheckedChanged
        UpdateParameterControls()
    End Sub

    Private Sub chkMiddleMedian_CheckedChanged(sender As Object, e As EventArgs) Handles chkMiddleMedian.CheckedChanged
        UpdateParameterControls()
    End Sub

    ' Helper to get the selected boundary mode (for smoothing / export)
    Private Function GetSelectedBoundaryMode() As SignatureMedian.BoundaryMode
        Select Case cbxBoundaryMode.SelectedItem?.ToString()
            Case "Symmetric" : Return SignatureMedian.BoundaryMode.Symmetric
            Case "Adaptive" : Return SignatureMedian.BoundaryMode.Adaptive
            Case "Replicate" : Return SignatureMedian.BoundaryMode.Replicate
            Case "ZeroPad" : Return SignatureMedian.BoundaryMode.ZeroPad
            Case Else : Return SignatureMedian.BoundaryMode.Symmetric
        End Select
    End Function

    Private Async Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Try
            SetBusy(True)

            Dim data = ParseInputData(txtInit.Text)
            If data Is Nothing OrElse data.Count = 0 Then
                MessageBox.Show(Me, "Please provide at least one numeric value in the Initial Data box.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UpdateStatus("No data to process")
                Return
            End If

            Dim radius As Integer = DecimalToIntSafe(updRadius.Value) ' R
            Dim borderCount As Integer = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value))
            If radius <= 0 Then
                MessageBox.Show(Me, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                UpdateStatus("Invalid radius")
                Return
            End If

            Dim windowWidth As Integer = (2 * radius) + 1 ' odd window width W
            Dim runAll As Boolean = chkAllMedian.Checked
            Dim runMiddle As Boolean = chkMiddleMedian.Checked

            If Not runAll AndAlso Not runMiddle Then
                MessageBox.Show(Me, "Please select at least one method: All Median and / or Middle Median.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UpdateStatus("No method selected")
                Return
            End If

            ' Validation (same rule used in exports): 2 × B must be strictly less than W
            If runMiddle AndAlso (2 * borderCount) >= windowWidth Then
                Dim resp = MessageBox.Show(Me,
                "For Middle Median, 2 × Border Count must be strictly less than Kernel Width (W = 2R + 1)." & vbCrLf & vbCrLf & "Continue anyway?",
                "Parameter Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)
                If resp = DialogResult.No Then
                    UpdateStatus("Smoothing canceled by user")
                    Return
                End If
            End If

            ' Clamp Middle Median border count so 2B < (2R + 1) => B <= R - 1
            Dim middleBorder As Integer = borderCount
            Dim maxValidB As Integer = Math.Max(0, radius - 1)
            If runMiddle AndAlso middleBorder > maxValidB Then
                middleBorder = maxValidB
                UpdateStatus(String.Format("Middle Median: Border Count clamped to {0} (R={1})", middleBorder, radius))
            End If

            UpdateStatus(String.Format("Smoothing started (R={0}, W={1})...", radius, windowWidth))
            txtRefined.Clear()

            Dim n = data.Count
            Dim sb As New StringBuilder()

            ' Run All Median if requested
            If runAll Then
                Dim progressAll As IProgress(Of Integer) = New Progress(Of Integer)(
                Sub(vp)
                    Dim percent As Integer = If(n > 0, Math.Min(100, Math.Max(0, CInt(Math.Round((vp / CDbl(n)) * 100)))), 0)
                    UpdateStatus(String.Format("AllMedian progress: {0}%", percent))
                End Sub)

                Dim boundaryMode = GetSelectedBoundaryMode()

                Dim allResult = Await Task.Run(Function()
                                                   Return SignatureMedian.ComputeMedians(
                                                   input:=data.ToList(),
                                                   useMiddle:=False,
                                                   kernelWidth:=windowWidth,
                                                   borderCount:=borderCount,
                                                   progress:=progressAll,
                                                   boundaryMode:=boundaryMode)
                                               End Function)

                sb.AppendLine("[All Median]")
                For Each v In allResult
                    sb.AppendLine(v.ToString("G17", CultureInfo.InvariantCulture))
                Next

                If runMiddle Then
                    sb.AppendLine()
                End If
            End If

            ' Run Middle Median if requested
            If runMiddle Then
                Dim progressMiddle As IProgress(Of Integer) = New Progress(Of Integer)(
                Sub(vp)
                    Dim percent As Integer = If(n > 0, Math.Min(100, Math.Max(0, CInt(Math.Round((vp / CDbl(n)) * 100)))), 0)
                    UpdateStatus(String.Format("MiddleMedian progress: {0}%", percent))
                End Sub)

                Dim middleResult = Await Task.Run(Function()
                                                      Return SignatureMedian.ComputeMedians(
                                                      input:=data.ToList(),
                                                      useMiddle:=True,
                                                      kernelWidth:=windowWidth,    ' API expects W (odd window width)
                                                      borderCount:=middleBorder,   ' clamped if needed
                                                      progress:=progressMiddle)
                                                  End Function)

                sb.AppendLine("[Middle Median]")
                For Each v In middleResult
                    sb.AppendLine(v.ToString("G17", CultureInfo.InvariantCulture))
                Next
            End If

            txtRefined.Text = sb.ToString().TrimEnd(ControlChars.Cr, ControlChars.Lf)
            UpdateStatus("Smoothing completed")
        Catch ex As ArgumentOutOfRangeException
            MessageBox.Show(Me, ex.Message, "Out of Range", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Out of range")
        Catch ex As ArgumentException
            MessageBox.Show(Me, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Argument error")
        Catch ex As InvalidOperationException
            MessageBox.Show(Me, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Operation error")
        Catch ex As AggregateException
            Dim flat = ex.Flatten()
            Dim inner = flat.InnerExceptions.FirstOrDefault()
            If TypeOf inner Is InvalidOperationException Then
                MessageBox.Show(Me, inner.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                UpdateStatus("Operation error")
            Else
                MessageBox.Show(Me, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                UpdateStatus("Operation error")
            End If
        Catch ex As Exception
            ' Align unexpected errors with "Operation Error" for btnStart
            MessageBox.Show(Me, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Operation error")
        Finally
            SetBusy(False)
        End Try
    End Sub

    ' Hook this to btnExport.Click in the designer
    Private Async Sub btnExcelExport_Click(sender As Object, e As EventArgs) Handles btnExcelExport.Click
        Dim cts As CancellationTokenSource = Nothing
        Try
            SetBusy(True)

            Dim data = ParseInputData(txtInit.Text)
            If data Is Nothing OrElse data.Count = 0 Then
                MessageBox.Show(Me, "Please provide at least one numeric value in the Initial Data box before exporting.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UpdateStatus("No data to export")
                Return
            End If

            Dim radius As Integer = DecimalToIntSafe(updRadius.Value)   ' R
            Dim borderCount As Integer = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value))
            If radius <= 0 Then
                MessageBox.Show(Me, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                UpdateStatus("Invalid radius")
                Return
            End If

            Dim windowWidth As Integer = (2 * radius) + 1 ' W = 2R + 1
            If chkMiddleMedian.Checked AndAlso (2 * borderCount) >= windowWidth Then
                Dim resp = MessageBox.Show(Me,
                                           "For Middle Median, 2 × Border Count must be strictly less than Kernel Width (W = 2R + 1)." & vbCrLf & vbCrLf & "Continue anyway?",
                                           "Parameter Warning",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning)
                If resp = DialogResult.No Then
                    UpdateStatus("Export canceled by user")
                    Return
                End If
            End If

            Dim progress As IProgress(Of Integer) = New Progress(Of Integer)(
                Sub(v)
                    UpdateStatus(String.Format("Excel export: {0}%", v))
                End Sub)

            cts = New CancellationTokenSource()

            ' Build and sanitize dataset title to satisfy Excel worksheet name constraints
            Dim rawTitle = BuildDatasetTitle()
            Dim safeTitle = SanitizeWorksheetName(rawTitle)
            If Not String.Equals(rawTitle, safeTitle, StringComparison.Ordinal) Then
                UpdateStatus(String.Format("Worksheet name adjusted to: {0}", safeTitle))
            End If

            Dim ticket As New ExcelOrderTicket() With {
                .InitialData = data,
                .DatasetTitle = safeTitle,   ' Excel worksheet name; sanitized
                .KernelRadius = radius,      ' ticket expects R (service computes W internally)
                .BorderCount = borderCount,
                .Progress = progress,
                .BoundaryMode = GetSelectedBoundaryMode(),
                .CancellationToken = cts.Token
            }

            UpdateStatus(String.Format("Launching Excel export (R={0}, W={1})...", radius, windowWidth))
            Await ExcelBrewService.ExcelCustomOrder(ticket)

            UpdateStatus("Excel export completed")
        Catch ex As ArgumentException
            MessageBox.Show(Me, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Argument error")
        Catch ex As InvalidOperationException
            MessageBox.Show(Me, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Operation error")
        Catch ex As Runtime.InteropServices.COMException
            MessageBox.Show(Me, ex.Message, "Excel Interop Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Interop error")
        Catch ex As OperationCanceledException
            UpdateStatus("Export canceled")
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Error")
        Finally
            If cts IsNot Nothing Then cts.Dispose()
            SetBusy(False)
        End Try
    End Sub

    Private Async Sub btnCSVExport_Click(sender As Object, e As EventArgs) Handles btnCSVExport.Click
        Dim cts As CancellationTokenSource = Nothing
        Try
            SetBusy(True)

            Dim data = ParseInputData(txtInit.Text)
            If data Is Nothing OrElse data.Count = 0 Then
                MessageBox.Show(Me, "Please provide at least one numeric value in the Initial Data box before exporting.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UpdateStatus("No data to export")
                Return
            End If

            Dim radius As Integer = DecimalToIntSafe(updRadius.Value) ' R
            Dim borderCount As Integer = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value))
            If radius <= 0 Then
                MessageBox.Show(Me, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                UpdateStatus("Invalid radius")
                Return
            End If

            Dim windowWidth As Integer = (2 * radius) + 1 ' W
            If chkMiddleMedian.Checked AndAlso (2 * borderCount) >= windowWidth Then
                Dim resp = MessageBox.Show(Me,
                                           "For Middle Median, 2 × Border Count must be strictly less than Kernel Width (W = 2R + 1)." & vbCrLf & vbCrLf & "Continue anyway?",
                                           "Parameter Warning",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning)
                If resp = DialogResult.No Then
                    UpdateStatus("CSV export canceled by user")
                    Return
                End If
            End If

            ' Ask user where to save CSV
            Dim title = BuildDatasetTitle()
            Dim sfd As New SaveFileDialog() With {
                .Title = "Save CSV",
                .Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                .FileName = SanitizeFileName(title) & ".csv",
                .AddExtension = True,
                .OverwritePrompt = True,
                .SupportMultiDottedExtensions = True
            }

            If sfd.ShowDialog(Me) <> DialogResult.OK Then
                UpdateStatus("CSV export canceled")
                Return
            End If

            Dim progress As IProgress(Of Integer) = New Progress(Of Integer)(
                Sub(v)
                    UpdateStatus(String.Format("CSV export: {0}%", v))
                End Sub)

            cts = New CancellationTokenSource()

            Dim ticket As New CsvOrderTicket() With {
                .InitialData = data,
                .DatasetTitle = title,
                .KernelRadius = radius,   ' ticket expects R; service computes W internally
                .BorderCount = borderCount,
                .BasePath = sfd.FileName,
                .Progress = progress,
                .BoundaryMode = GetSelectedBoundaryMode(),
                .CancellationToken = cts.Token
            }

            UpdateStatus(String.Format("Launching CSV export (R={0}, W={1})...", radius, windowWidth))
            Await CsvBrewService.ExportCsvAsync(ticket)

            UpdateStatus("CSV export completed")
        Catch ex As ArgumentException
            MessageBox.Show(Me, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Argument error")
        Catch ex As DirectoryNotFoundException
            MessageBox.Show(Me, ex.Message, "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Directory error")
        Catch ex As InvalidOperationException
            MessageBox.Show(Me, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Operation error")
        Catch ex As OperationCanceledException
            UpdateStatus("CSV export canceled")
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            UpdateStatus("Error")
        Finally
            If cts IsNot Nothing Then cts.Dispose()
            SetBusy(False)
        End Try
    End Sub

    Private Shared Function DecimalToIntSafe(value As Decimal) As Integer
        If value > Integer.MaxValue Then Return Integer.MaxValue
        If value < Integer.MinValue Then Return Integer.MinValue
        Return CInt(value)
    End Function

    Private Function ParseInputData(text As String) As IList(Of Double)
        If String.IsNullOrWhiteSpace(text) Then
            Return New List(Of Double)()
        End If

        Dim tokens = text.Split(New Char() {ControlChars.Cr, ControlChars.Lf, ","c, ";"c, ControlChars.Tab, " "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim list As New List(Of Double)(tokens.Length)
        For Each token In tokens
            Dim value As Double
            If Double.TryParse(token, NumberStyles.Float Or NumberStyles.AllowThousands, CultureInfo.InvariantCulture, value) OrElse
               Double.TryParse(token, NumberStyles.Float Or NumberStyles.AllowThousands, CultureInfo.CurrentCulture, value) Then

                If Not Double.IsNaN(value) AndAlso Not Double.IsInfinity(value) Then
                    list.Add(value)
                End If
            Else
                MessageBox.Show(Me, String.Format("Invalid numeric value: ""{0}""", token), "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End If
        Next
        Return list
    End Function

    Private Function BuildDatasetTitle() As String
        Return "Avocado Smoothie - Barista"
    End Function

    ' Ensures the title is a valid Excel worksheet name
    Private Shared Function SanitizeWorksheetName(title As String) As String
        Dim name = If(String.IsNullOrWhiteSpace(title), "Dataset", title)

        ' Remove invalid characters: : \ / ? * [ ]
        Dim invalid As Char() = {":"c, "\"c, "/"c, "?"c, "*"c, "["c, "]"c}
        For Each ch In invalid
            name = name.Replace(ch.ToString(), String.Empty)
        Next

        name = name.Trim()

        ' Limit to 31 characters
        If name.Length > 31 Then
            name = name.Substring(0, 31)
        End If

        If String.IsNullOrWhiteSpace(name) Then
            name = "Dataset"
        End If

        Return name
    End Function

    Private Sub UpdateStatus(text As String)
        If Me.IsDisposed Then Return

        If statusStrip1 Is Nothing OrElse slblStatus Is Nothing Then Return

        If statusStrip1.InvokeRequired Then
            Try
                statusStrip1.Invoke(New Action(Sub() slblStatus.Text = text))
            Catch
                ' ignore if disposing
            End Try
        Else
            slblStatus.Text = text
        End If
    End Sub

    Private Sub SetBusy(busy As Boolean)
        Me.UseWaitCursor = busy

        If btnStart IsNot Nothing Then btnStart.Enabled = Not busy
        If btnExcelExport IsNot Nothing Then btnExcelExport.Enabled = Not busy
        If updRadius IsNot Nothing Then updRadius.Enabled = Not busy
        If updBorderCnt IsNot Nothing Then updBorderCnt.Enabled = Not busy
        If chkMiddleMedian IsNot Nothing Then chkMiddleMedian.Enabled = Not busy
        If chkAllMedian IsNot Nothing Then chkAllMedian.Enabled = Not busy
        If btnCSVExport IsNot Nothing Then btnCSVExport.Enabled = Not busy
    End Sub

    Private Shared Function SanitizeFileName(name As String) As String
        Dim fallback = "dataset"
        If String.IsNullOrWhiteSpace(name) Then Return fallback

        Dim invalid = Path.GetInvalidFileNameChars()
        Dim sb As New StringBuilder(name.Length)
        For Each ch In name.Trim()
            If Array.IndexOf(invalid, ch) >= 0 Then Continue For
            sb.Append(ch)
        Next

        Dim cleaned = sb.ToString()
        If String.IsNullOrWhiteSpace(cleaned) Then cleaned = fallback

        If cleaned.Length > 64 Then cleaned = cleaned.Substring(0, 64)
        Return cleaned
    End Function
End Class