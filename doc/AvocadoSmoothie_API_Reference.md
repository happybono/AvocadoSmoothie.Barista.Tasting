# AvocadoSmoothie.Barista

Running median based smoothing for numeric sequences plus CSV / Excel export utilities :
- MiddleMedian and AllMedian smoothing
- CSV export with Excel row-limit aware partitioning
- Excel export with chart (Interop on .NET Framework, late-binding COM on Windows)

This document reflects the code exactly as implemented in :
- SignatureMedian.vb
- CsvOrderTicket.vb
- CsvCustomOrder.vb
- CsvBrewService.vb
- ExcelOrderTicket.vb
- ExcelCustomOrder.vb
- ExcelBrewService.vb

## Contents

- [Installation](#installation)
- [Features](#features)
- [Public API (exact signatures)](#public-api-exact-signatures)
- [Behavioral Details](#behavioral-details)
- [Exceptions](#exceptions)
- [Notes and Limitations](#notes-and-limitations)
- [Class : SignatureMedian](#class-signaturemedian)
- [Class : CsvOrderTicket](#class-csvorderticket)
- [Class : CsvBrewService](#class-csvbrewservice)
- [Class : ExcelOrderTicket](#class-excelorderticket)
- [Class : ExcelBrewService](#class-excelbrewservice)

## Installation

Install via NuGet (if the package is published as AvocadoSmoothie.Barista) :
- dotnet CLI :
  - dotnet add package AvocadoSmoothie.Barista
- Package Manager Console :
  - Install-Package AvocadoSmoothie.Barista
- PackageReference (csproj) :
  - <PackageReference Include="AvocadoSmoothie.Barista" Version="x.y.z" />

Alternatively :
- Add a project reference to the AvocadoSmoothie.Barista project, or
- Include the AvocadoSmoothie.Barista source files in your project.
- Build with Visual Studio 2022.

Excel export requirements :
- .NET Framework 4.8 (NET48) : uses Microsoft.Office.Interop.Excel (strong typing).
- Other targets (e.g., netstandard2.0) on Windows : uses late-binding COM and requires Microsoft Excel installed and registered (ProgID "Excel.Application").
- Non-Windows platforms are not supported for Excel export (late-binding path throws PlatformNotSupportedException).

## Features

- Median smoothing utilities
  - MiddleMedian: preserves BorderCount items at start / end, smoothing the middle region.
  - AllMedian: smooths across the full series.
  - Configurable boundary handling for AllMedian: Symmetric (default), Replicate, ZeroPad, Adaptive.
- CSV export
  - Writes dataset title and smoothing parameters.
  - Splits files to respect Excel’s 1,048,576-row limit (after deducting header lines).
  - Data formatted with G17 and invariant culture.
- Excel export
  - Writes headers, three data columns (Initial, MiddleMedian, AllMedian), and a line chart.
  - Populates built-in document properties (Title, Category, Subject, Author, Last Author, Keywords, Comments).
  - Opens Excel (Visible = True) with the generated workbook (Saved = False).

## Public API (exact signatures)

- Class SignatureMedian
  - Public Shared Function ComputeMediansByRadius(input As List(Of Double), useMiddle As Boolean, kernelRadius As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)
  - Public Shared Function ComputeMedians(input As List(Of Double), useMiddle As Boolean, kernelWidth As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)
  
- Enum SignatureMedian.BoundaryMode
  - Symmetric, Replicate, ZeroPad, Adaptive

- Class IngredientRatioValidator
  - Public Shared Sub ThrowIfInvalidBasicForCsv(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, basePath As String)
  - Public Shared Sub ThrowIfInvalidBasicForExcel(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer)
  - Public Shared Sub ThrowIfInvalidSmoothingParameters(dataCount As Integer, windowSize As Integer, radius As Integer, borderCount As Integer, useMiddle As Boolean)
  - Public Shared Function KernelWidthFromRadius(radius As Integer) As Integer
  - Public NotInheritable Class IngredientRatioValidator.MessageOnceGate
    - Public Shared Function TryEnter(message As String, Optional holdMillis As Integer = 2000) As Boolean

- Class CsvOrderTicket
  - Public Property InitialData As IList(Of Double)
  - Public Property DatasetTitle As String
  - Public Property KernelRadius As Integer
  - Public Property BorderCount As Integer
  - Public Property BasePath As String
  - Public Property CancellationToken As System.Threading.CancellationToken
  - Public Property Progress As IProgress(Of Integer)
  - Public Sub Validate()
  - Public Property BoundaryMode As SignatureMedian.BoundaryMode

- Class CsvBrewService (NotInheritable)
  - Public Shared Function ExportCsvAsync(request As CsvOrderTicket) As Threading.Tasks.Task(Of List(Of String))
  - Public Shared Async Function ExportCsvAsync(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, basePath As String, Optional progress As IProgress(Of Integer) = Nothing, Optional cancelToken As System.Threading.CancellationToken = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As Threading.Tasks.Task(Of List(Of String))

- Class ExcelOrderTicket
  - Public Property InitialData As IList(Of Double)
  - Public Property DatasetTitle As String
  - Public Property KernelRadius As Integer
  - Public Property BorderCount As Integer
  - Public Property Progress As IProgress(Of Integer)
  - Public Property CancellationToken As System.Threading.CancellationToken
  - Public Sub ValidateBasic()
  - Public Property BoundaryMode As SignatureMedian.BoundaryMode

- Class ExcelBrewService (NotInheritable)
  - Public Shared Function ExcelCustomOrder(request As ExcelOrderTicket) As Threading.Tasks.Task
  - Public Shared Async Function ExportExcelAsync(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, progress As IProgress(Of Integer), Optional cancelToken As System.Threading.CancellationToken = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As Threading.Tasks.Task

## Behavioral Details

1) SignatureMedian
- SignatureMedian.ComputeMediansByRadius
  - Accepts radius R (≥ 0), converts to odd window width W = (2 × R) + 1 with overflow check, then delegates to ComputeMedians.
  - Throws ArgumentNullException when input is Nothing; ArgumentOutOfRangeException when kernelRadius < 0 or too large.

- SignatureMedian.ComputeMedians
  - kernelWidth: W ≥ 1. Odd recommended; even W is supported (median = average of the two center values).
  - Boundary handling:
    - useMiddle = True : boundaryMode is ignored; clamped, variable-length window; first / last borderCount copied unchanged.
    - useMiddle = False : boundaryMode applied (Symmetric reflection, Replicate, ZeroPad, or Adaptive sliding full window).
  - input : if empty, reports 0 (when progress present) and returns an empty list.
  - useMiddle : True (MiddleMedian; first / last BorderCount copied unchanged) vs False (AllMedian).
  - kernelWidth : window width W (≥ 1). Offsets: (W - 1) \ 2 and (W - 1) - offsetLow.
  - Parallel.For with ThreadLocal window; per-index window clamps to [0, N - 1].
  - Median from a sorted slice; even length uses average of two middles.
  - Progress : reports 0 at start, periodic counts (~ N / 200), and final N.

2) CSV export (CsvBrewService.ExportCsvAsync)
- Validates : data present, title present, kernelRadius > 0, basePath present.
- Smoothing parameters validated via IngredientRatioValidator.ThrowIfInvalidSmoothingParameters using W = 2 × R + 1 for both Middle and All passes.
- Calls SignatureMedian.ComputeMediansByRadius with the requested radius for both MiddleMedian and AllMedian.
- Splits by Excel row limit (1,048,576) minus header lines (HEADER_LINES = 12).
- File naming : basePath or BaseName_Part{index}{ext}; defaults ext to .csv if missing.
- File format :
  - Title, Part X of Y, blank, "Smoothing Parameters"
  - Kernel Radius : R, Kernel Width : W, Border Count : B
  - Boundary Method : {boundaryMode}
  - blank, Generated : {Now (current culture "G")}, blank
  - HEADER_LINES = 12; data rows now start after the header row at line 12.
  - Header : Initial Data, MiddleMedian, AllMedian
  - Data rows in G17 (InvariantCulture)
- File content : see README sequence (Boundary Method inserted before "Generated : …").
- Progress : forwards counts during smoothing; percent (0 … 100) while writing.
- Cancellation : checked before each compute and at each part start.

3) Excel export (ExcelBrewService.ExportExcelAsync)
- Validates : data present, title present, kernelRadius > 0, borderCount ≥ 0.
- n = Count; kernelWidth = 2 × kernelRadius + 1.
- Headers include "Boundary Method : {boundaryMode}" (row 6) and "Border Count" is at row 7.
- ValidateSmoothingParameters :
  - Middle : windowSize:=kernelWidth; requires windowSize ≤ n, borderCount ≤ n, and 2 × borderCount < windowSize.
  - All : windowSize:=kernelWidth (W) is used as well.
- Progress mapping :
  - Progress mapping as designed : 0 – 29 Middle, 30 – 59 All, 60 – 100 writing.
  - Note : current WriteExcelAsync reports 30 / 60 / 80 / 100; align to ≥ 60 or document the variation.
- Worksheet :
  - Name = datasetTitle; headers at (1, 1) and (3, 1 … 7, 1).
  - DATA_START_ROW=4; Initial starts at col 3; Middle two cols after last Initial; All two cols after last Middle.
  - Each series continues in new columns if rows exceed Excel limit.
- Chart :
  - Line chart placed right of data; title = datasetTitle; axes titled "Value" and "Sequence Number".
  - Series : "Initial Data", "Middle Median", "All Median".
- Built-in properties set (Title / Category / Subject / Author / Last Author / Keywords / Comments).
- Excel is shown (Visible=True), workbook Saved=False, DisplayAlerts=True.
- Platform :
  - NET48 : strongly-typed Interop.
  - Else on Windows : late-binding COM (ChartObjects or Shapes.AddChart / AddChart2 fallback).

## Exceptions

- CsvOrderTicket.Validate
  - ArgumentException : InitialData empty, DatasetTitle missing, BasePath missing.
  - ArgumentOutOfRangeException : KernelRadius ≤ 0 or BorderCount < 0.
  - DirectoryNotFoundException : directory part of BasePath does not exist.

- CsvBrewService.ExportCsvAsync (core overload)
  - ArgumentException : initialData empty, datasetTitle missing, basePath missing.
  - ArgumentOutOfRangeException : kernelRadius ≤ 0.
  - InvalidOperationException : invalid smoothing parameters (window too large; border too wide).
  - OperationCanceledException : when canceled at checkpoints.
  - File I/O exceptions (UnauthorizedAccessException, IOException, PathTooLongException) may bubble.

- ExcelOrderTicket.ValidateBasic
  - ArgumentException : InitialData empty, DatasetTitle missing.
  - ArgumentOutOfRangeException : KernelRadius ≤ 0 or BorderCount < 0.

- ExcelBrewService.ExportExcelAsync
  - ArgumentException : initialData empty, datasetTitle missing.
  - ArgumentOutOfRangeException : kernelRadius ≤ 0 or borderCount < 0.
  - InvalidOperationException : invalid smoothing parameters (e.g., window too large, border too wide for Middle).
  - OperationCanceledException : when canceled.
  - .NET Framework (NET48) :
    - COMException during Excel automation is wrapped as ExcelInteropNotAvailableException.
  - .NET Standard (late-binding) :
    - PlatformNotSupportedException on non-Windows.
    - ExcelInteropNotAvailableException if Excel is not installed or not registered (ProgID "Excel.Application"), or if COM activation fails.
    - COMException with a stage hint on late-binding failures, wrapped as ExcelInteropNotAvailableException.

## Notes and Limitations

- In SignatureMedian.ComputeMedians, kernelWidth represents the window width W (≥ 1). Odd widths are recommended; even widths are accepted (median is the average of two middle values).
- SignatureMedian.ComputeMedians has no CancellationToken; callers check cancellation before invoking.
- Excel export requires Windows. On non-NET48 targets, Microsoft Excel must be installed and registered.
- Both Middle and All validation phases use windowSize = kernelWidth (W).
---

## Class : SignatureMedian

Static utilities for median-based smoothing.

### Methods

- Public Shared Function ComputeMediansByRadius(input As List(Of Double), useMiddle As Boolean, kernelRadius As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)
  - Accepts a true radius R and converts to W = (2 × R) + 1 (odd). Validates inputs and delegates to ComputeMedians.

- Public Shared Function ComputeMedians(input As List(Of Double), useMiddle As Boolean, kernelWidth As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)
  - Computes running medians over the input.
  - Parameters :
    - input : Input sequence (List(Of Double)). If empty, returns an empty list and reports 0 to progress (if present).
    - useMiddle : True for MiddleMedian (preserves edges); False for AllMedian.
    - kernelWidth : Median window size W (≥ 1; odd recommended). Even widths are accepted (median = average of two middle values).
    - borderCount : Number of elements to keep unchanged at both borders when useMiddle=True. Ignored for useMiddle=False.
    - boundaryMode : Boundary handling for AllMedian (Symmetric, Replicate, ZeroPad, Adaptive). Ignored for MiddleMedian.
    - progress : Optional reporter of processed element counts (0 … N). Reports initial 0, periodic counts, and final N.
  - Returns : List(Of Double) of smoothed values. For MiddleMedian, first and last borderCount values are copied from the input.
  - Implementation notes :
    - Parallel.For with a thread-local window buffer.
    - MiddleMedian clamps to the valid range near edges; AllMedian applies boundaryMode or slides a full window (Adaptive).

---

## Class : IngredientRatioValidator

Centralized validation and message helpers for CSV / Excel export and smoothing parameters.

### Constants (selected)

- MsgNoData, MsgDatasetTitleRequired, MsgKernelRadiusPositive, MsgBorderCountNonNegative, MsgBasePathRequired

### Methods

- Public Shared Sub ThrowIfInvalidBasicForCsv(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, basePath As String)
  - Throws ArgumentException (data / title / path), ArgumentOutOfRangeException (radius ≤ 0, border < 0).

- Public Shared Sub ThrowIfInvalidBasicForExcel(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer)
  - Throws ArgumentException (data / title), ArgumentOutOfRangeException (radius ≤ 0, border < 0).

- Public Shared Sub ThrowIfInvalidSmoothingParameters(dataCount As Integer, windowSize As Integer, radius As Integer, borderCount As Integer, useMiddle As Boolean)
  - Ensures windowSize ≤ dataCount; borderCount ≤ dataCount; and if useMiddle then 2 × borderCount < windowSize.
  - Throws InvalidOperationException on violations with detailed, user-friendly messages.

- Public Shared Function KernelWidthFromRadius(radius As Integer) As Integer
  - Computes W = 2 × radius + 1 with overflow guard; throws ArgumentOutOfRangeException when too large.

### Nested Class

- Public NotInheritable Class IngredientRatioValidator.MessageOnceGate
  - Public Shared Function TryEnter(message As String, Optional holdMillis As Integer = 2000) As Boolean
  - Utility to avoid duplicate UI messages within a time window.

---

## Class : CsvOrderTicket

Request DTO for CSV export.

### Properties

- Public Property InitialData As IList(Of Double)
  - Input numeric data to export.

- Public Property DatasetTitle As String
  - Dataset title (written to the CSV header).

- Public Property KernelRadius As Integer
  - Smoothing radius R. The actual window size W = (2 × R) + 1.

- Public Property BorderCount As Integer
  - Border width for MiddleMedian (elements preserved at each end).

- Public Property BasePath As String
  - Destination base file path for the CSV (e.g., a SaveFileDialog result). When split into multiple parts, an index suffix is appended.

- Public Property CancellationToken As Threading.CancellationToken
  - Cancellation token for the export operation.

- Public Property Progress As IProgress(Of Integer)
  - Optional progress reporter.

- Public Property BoundaryMode As SignatureMedian.BoundaryMode
  - Boundary handling for AllMedian; recorded in the CSV header. Ignored by MiddleMedian.

### Methods

- Public Sub Validate()
  - Ensures the request is consistent before export.
  - Throws :
    - ArgumentException when InitialData is empty, DatasetTitle is missing, or BasePath is missing.
    - ArgumentOutOfRangeException when KernelRadius ≤ 0 or BorderCount < 0.
    - DirectoryNotFoundException when the directory portion of BasePath does not exist.

---

## Class : CsvBrewService

CSV export service. Produces one or more CSV files with aligned columns: Initial Data, MiddleMedian, AllMedian.

Notes
- Class is NotInheritable with a private constructor; all members are Shared.
- Splits output by Excel’s row limit (1,048,576 rows) minus headers.
- Numeric formatting for data lines uses ToString("G17", InvariantCulture). A generated timestamp uses current culture.

### Methods

- Public Shared Function ExportCsvAsync(request As CsvOrderTicket) As Threading.Tasks.Task(Of List(Of String))
  - Validates the request and delegates to the core overload.
  - Passes request.BoundaryMode through to the core ExportCsvAsync overload.

- Public Shared Async Function ExportCsvAsync(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, basePath As String, Optional progress As IProgress(Of Integer) = Nothing, Optional cancelToken As System.Threading.CancellationToken = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As Threading.Tasks.Task(Of List(Of String))
  - Core CSV export. Computes MiddleMedian and AllMedian, writes CSV files, and reports progress.

---

## Class : ExcelOrderTicket

Request DTO for Excel export.

### Properties

- Public Property InitialData As IList(Of Double)
  - Input numeric data to export.

- Public Property DatasetTitle As String
  - Worksheet / chart title.

- Public Property KernelRadius As Integer
  - Smoothing radius R; window size W = (2 × R) + 1.

- Public Property BorderCount As Integer
  - Border width for MiddleMedian.

- Public Property Progress As IProgress(Of Integer)
  - Optional progress reporter.

- Public Property CancellationToken As Threading.CancellationToken
  - Cancellation token.

- Public Property BoundaryMode As SignatureMedian.BoundaryMode
  - Boundary handling for AllMedian; shown in the worksheet header. Ignored by MiddleMedian.

### Methods

- Public Sub ValidateBasic()
  - Validates core parameters for Excel export.
  - Throws :
    - ArgumentException when InitialData is empty or DatasetTitle is missing.
    - ArgumentOutOfRangeException when KernelRadius ≤ 0 or BorderCount < 0.

---

## Class : ExcelBrewService

Excel export service. Creates a workbook containing:
- Headers with smoothing parameters,
- Columns for Initial Data, MiddleMedian, and AllMedian,
- A line chart with the three series.

Notes
- NotInheritable with a private constructor; all members are Shared.
- Platform behavior :
  - NET48 : uses Microsoft.Office.Interop.Excel (strongly typed).
  - Non-NET48 : uses late-binding COM on Windows and requires Microsoft Excel to be installed.

### Methods

- Public Shared Function ExcelCustomOrder(request As ExcelOrderTicket) As Threading.Tasks.Task
  - Validates the ticket and delegates to ExportExcelAsync.
  - Passes request.BoundaryMode through to the core ExportExcelAsync overload.

- Public Shared Async Function ExportExcelAsync(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, progress As IProgress(Of Integer), Optional cancelToken As System.Threading.CancellationToken = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As Task
  - Throws:
    - ArgumentException, ArgumentOutOfRangeException, InvalidOperationException, OperationCanceledException
    - ExcelInteropNotAvailableException (late-binding or COM failure)
    - PlatformNotSupportedException (non-Windows, late-binding)
    - COMException (wrapped as ExcelInteropNotAvailableException)

---

Blend your signals, export with ease - discover your data’s true flavor!