using AvocadoSmoothie.Barista;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AvocadoSmoothie.Barista.SignatureMedian;

namespace AvocadoSmoothie.Barista.Tasting
{
    public partial class FrmMain : Form
    {
        private static readonly string[] BoundaryModes = { "Symmetric", "Adaptive", "Replicate", "ZeroPad" };

        public FrmMain()
        {
            InitializeComponent();

            cbxBoundaryMode.SelectedIndex = 0; // Default selection
            UpdateParameterControls();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Ensure one method is selected by default
            if (!chkMiddleMedian.Checked && !chkAllMedian.Checked)
                chkMiddleMedian.Checked = true;

            // Remove 32K limit so both sections can be displayed for large outputs
            txtInit.MaxLength = 0;            // 0 = no limit
            txtInit.Multiline = true;         // ensure multiline (just in case)
            txtInit.WordWrap = true;

            txtRefined.MaxLength = 0;         // 0 = no limit
            txtRefined.Multiline = true;      // ensure multiline (just in case)
            txtRefined.WordWrap = true;

            UpdateStatus("Ready");
        }

        private void SmoothingMethod_CheckedChanged(object sender, EventArgs e)
        {
            UpdateParameterControls();
        }

        private void UpdateParameterControls()
        {
            // BoundaryMode : Enable AllMedian only
            cbxBoundaryMode.Enabled = chkAllMedian.Checked;
            lblBoundaryMode.Enabled = chkAllMedian.Checked;

            // BorderCount : Enable MiddleMedian only
            updBorderCnt.Enabled = chkMiddleMedian.Checked;
            lblBorderCnt.Enabled = chkMiddleMedian.Checked;
        }

        private SignatureMedian.BoundaryMode GetSelectedBoundaryMode()
        {
            switch (cbxBoundaryMode.SelectedItem?.ToString())
            {
                case "Symmetric": return SignatureMedian.BoundaryMode.Symmetric;
                case "Adaptive": return SignatureMedian.BoundaryMode.Adaptive;
                case "Replicate": return SignatureMedian.BoundaryMode.Replicate;
                case "ZeroPad": return SignatureMedian.BoundaryMode.ZeroPad;
                default: return SignatureMedian.BoundaryMode.Symmetric;
            }
        }

        private void chkMiddleMedian_CheckedChanged(object sender, EventArgs e)
        {
            SmoothingMethod_CheckedChanged(sender, e);
        }

        private void chkAllMedian_CheckedChanged(object sender, EventArgs e)
        {
            SmoothingMethod_CheckedChanged(sender, e);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                SetBusy(true);

                var data = ParseInputData(txtInit.Text);
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show(this, "Please provide at least one numeric value in the Initial Data box.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No data to process");
                    return;
                }

                int radius = DecimalToIntSafe(updRadius.Value); // R
                int borderCount = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value));
                if (radius <= 0)
                {
                    MessageBox.Show(this, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateStatus("Invalid radius");
                    return;
                }

                int windowWidth = (2 * radius) + 1; // odd window width W
                bool runAll = chkAllMedian.Checked;
                bool runMiddle = chkMiddleMedian.Checked;

                if (!runAll && !runMiddle)
                {
                    MessageBox.Show(this, "Please select at least one method: All Median and/or Middle Median.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No method selected");
                    return;
                }

                // Clamp Middle Median border count so 2B < (2R + 1) => B <= R - 1
                int middleBorder = borderCount;
                int maxValidB = Math.Max(0, radius - 1);
                if (runMiddle && middleBorder > maxValidB)
                {
                    middleBorder = maxValidB;
                    UpdateStatus($"Middle Median: Border Count clamped to {middleBorder} (R={radius})");
                }

                UpdateStatus($"Smoothing started (R={radius}, W={windowWidth})...");
                txtRefined.Clear();

                var n = data.Count;
                var sb = new StringBuilder();

                // Run All Median if requested
                if (runAll)
                {
                    var progressAll = new Progress<int>(v =>
                    {
                        int percent = n > 0 ? Math.Min(100, Math.Max(0, (int)Math.Round((v / (double)n) * 100))) : 0;
                        UpdateStatus($"AllMedian progress: {percent}%");
                    });

                    var boundaryMode = GetSelectedBoundaryMode();

                    var allResult = await Task.Run(() =>
                        SignatureMedian.ComputeMedians(
                            input: data.ToList(),
                            useMiddle: false,
                            kernelWidth: windowWidth,  // API expects W (odd window width)
                            borderCount: borderCount,
                            progress: progressAll,
                            boundaryMode: boundaryMode));

                    sb.AppendLine("[All Median]");
                    foreach (var val in allResult)
                        sb.AppendLine(val.ToString("G17", CultureInfo.InvariantCulture));

                    if (runMiddle)
                    {
                        sb.AppendLine();
                    }
                }

                // Run Middle Median if requested
                if (runMiddle)
                {
                    var progressMiddle = new Progress<int>(v =>
                    {
                        int percent = n > 0 ? Math.Min(100, Math.Max(0, (int)Math.Round((v / (double)n) * 100))) : 0;
                        UpdateStatus($"MiddleMedian progress: {percent}%");
                    });

                    var middleResult = await Task.Run(() =>
                        SignatureMedian.ComputeMedians(
                            input: data.ToList(),
                            useMiddle: true,
                            kernelWidth: windowWidth,    // API expects W (odd window width)
                            borderCount: middleBorder,   // clamped if needed
                            progress: progressMiddle));

                    sb.AppendLine("[Middle Median]");
                    foreach (var val in middleResult)
                        sb.AppendLine(val.ToString("G17", CultureInfo.InvariantCulture));
                }

                txtRefined.Text = sb.ToString().TrimEnd();
                UpdateStatus("Smoothing completed");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(this, ex.Message, "Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Out of range");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Argument error");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Operation error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error");
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnExcelExport_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cts = null;
            try
            {
                SetBusy(true);

                var data = ParseInputData(txtInit.Text);
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show(this, "Please provide at least one numeric value in the Initial Data box before exporting.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No data to export");
                    return;
                }

                int radius = DecimalToIntSafe(updRadius.Value);   // R
                int borderCount = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value));
                if (radius <= 0)
                {
                    MessageBox.Show(this, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateStatus("Invalid radius");
                    return;
                }

                int windowWidth = (2 * radius) + 1; // W = 2R + 1
                if (chkMiddleMedian.Checked && (2 * borderCount) >= windowWidth)
                {
                    var resp = MessageBox.Show(this,
                        "For Middle Median, 2 × Border Count must be strictly less than Kernel Width (W = 2R + 1).\n\nContinue anyway?",
                        "Parameter Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (resp == DialogResult.No)
                    {
                        UpdateStatus("Export canceled by user");
                        return;
                    }
                }

                var progress = new Progress<int>(v =>
                {
                    UpdateStatus($"Excel export: {v}%");
                });

                cts = new CancellationTokenSource();

                // Build and sanitize dataset title to satisfy Excel worksheet name constraints
                var rawTitle = BuildDatasetTitle();
                var safeTitle = SanitizeWorksheetName(rawTitle);
                if (!string.Equals(rawTitle, safeTitle, StringComparison.Ordinal))
                {
                    UpdateStatus($"Worksheet name adjusted to: {safeTitle}");
                }

                var ticket = new ExcelOrderTicket
                {
                    InitialData = data,
                    DatasetTitle = safeTitle,   // Excel worksheet name; sanitized
                    KernelRadius = radius,      // ticket expects R (service computes W internally)
                    BorderCount = borderCount,
                    Progress = progress,
                    CancellationToken = cts.Token,
                    BoundaryMode = GetSelectedBoundaryMode()
                };

                UpdateStatus($"Launching Excel export (R={radius}, W={windowWidth})...");
                await ExcelBrewService.ExcelCustomOrder(ticket);

                UpdateStatus("Excel export completed");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Argument error");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Operation error");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show(this, ex.Message, "Excel Interop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Interop error");
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("Export cancelled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error");
            }
            finally
            {
                cts?.Dispose();
                SetBusy(false);
            }
        }

        private static int DecimalToIntSafe(decimal value)
        {
            if (value > int.MaxValue) return int.MaxValue;
            if (value < int.MinValue) return int.MinValue;
            return (int)value;
        }

        private IList<double> ParseInputData(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<double>();

            var tokens = text.Split(new[] { '\r', '\n', ',', ';', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<double>(tokens.Length);
            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value) ||
                    double.TryParse(token, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value))
                {
                    if (!double.IsNaN(value) && !double.IsInfinity(value))
                        list.Add(value);
                }
                else
                {
                    MessageBox.Show(this, $"Invalid numeric value: \"{token}\"", "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            return list;
        }

        private string BuildDatasetTitle()
        {
            return $"Avocado Smoothie - Barista";
        }

        // Ensures the title is a valid Excel worksheet name
        private static string SanitizeWorksheetName(string title)
        {
            string name = string.IsNullOrWhiteSpace(title) ? "Dataset" : title;
            // Remove invalid characters: : \ / ? * [ ]
            char[] invalid = { ':', '\\', '/', '?', '*', '[', ']' };
            foreach (var ch in invalid)
                name = name.Replace(ch.ToString(), string.Empty);

            name = name.Trim();

            // Limit to 31 characters
            if (name.Length > 31)
                name = name.Substring(0, 31);

            if (string.IsNullOrWhiteSpace(name))
                name = "Dataset";

            return name;
        }

        private void UpdateStatus(string text)
        {
            if (IsDisposed) return;
            if (statusStrip1.InvokeRequired)
            {
                try { statusStrip1.Invoke(new Action(() => slblStatus.Text = text)); } catch { /* ignore if disposing */ }
            }
            else
            {
                slblStatus.Text = text;
            }
        }

        private void SetBusy(bool busy)
        {
            this.UseWaitCursor = busy;
            btnStart.Enabled = !busy;
            btnExcelExport.Enabled = !busy;
            updRadius.Enabled = !busy;
            updBorderCnt.Enabled = !busy;
            chkMiddleMedian.Enabled = !busy;
            chkAllMedian.Enabled = !busy;
            btnCSVExport.Enabled = !busy;
        }

        private async void btnCSVExport_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cts = null;
            try
            {
                SetBusy(true);

                var data = ParseInputData(txtInit.Text);
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show(this, "Please provide at least one numeric value in the Initial Data box before exporting.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No data to export");
                    return;
                }

                int radius = DecimalToIntSafe(updRadius.Value);   // R
                int borderCount = Math.Max(0, DecimalToIntSafe(updBorderCnt.Value));
                if (radius <= 0)
                {
                    MessageBox.Show(this, "Kernel Radius must be greater than 0.", "Invalid Radius", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateStatus("Invalid radius");
                    return;
                }

                int windowWidth = (2 * radius) + 1; // W
                if (chkMiddleMedian.Checked && (2 * borderCount) >= windowWidth)
                {
                    var resp = MessageBox.Show(this,
                        "For Middle Median, 2 × Border Count must be strictly less than Kernel Width (W = 2R + 1).\n\nContinue anyway?",
                        "Parameter Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (resp == DialogResult.No)
                    {
                        UpdateStatus("CSV export canceled by user");
                        return;
                    }
                }

                // Ask user where to save CSV
                var title = BuildDatasetTitle();
                var sfd = new SaveFileDialog
                {
                    Title = "Save CSV",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = SanitizeFileName(title) + ".csv",
                    AddExtension = true,
                    OverwritePrompt = true,
                    SupportMultiDottedExtensions = true
                };

                if (sfd.ShowDialog(this) != DialogResult.OK)
                {
                    UpdateStatus("CSV export canceled");
                    return;
                }

                var progress = new Progress<int>(v =>
                {
                    UpdateStatus($"CSV export: {v}%");
                });

                cts = new CancellationTokenSource();

                var ticket = new CsvOrderTicket
                {
                    InitialData = data,
                    DatasetTitle = title,
                    KernelRadius = radius,   // ticket expects R; service computes W internally
                    BorderCount = borderCount,
                    BasePath = sfd.FileName,
                    Progress = progress,
                    CancellationToken = cts.Token,
                    BoundaryMode = GetSelectedBoundaryMode()
                };

                UpdateStatus($"Launching CSV export (R={radius}, W={windowWidth})...");
                await CsvBrewService.ExportCsvAsync(ticket);

                UpdateStatus("CSV export completed");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Argument error");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(this, ex.Message, "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Directory error");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Operation error");
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("CSV export canceled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error");
            }
            finally
            {
                cts?.Dispose();
                SetBusy(false);
            }
        }

        private static string SanitizeFileName(string name)
        {
            var fallback = "dataset";
            if (string.IsNullOrWhiteSpace(name)) return fallback;

            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name.Trim())
            {
                if (Array.IndexOf(invalid, ch) >= 0) continue;
                sb.Append(ch);
            }

            var cleaned = sb.ToString();
            if (string.IsNullOrWhiteSpace(cleaned)) cleaned = fallback;

            if (cleaned.Length > 64) cleaned = cleaned.Substring(0, 64);
            return cleaned;
        }
    }
}