# AvocadoSmoothie.Barista

Robust running median based smoothing and export toolkit for .NET (VB.NET).
Implements efficient MiddleMedian and AllMedian filters for numeric sequences, with configurable window radius and edge preservation.
Supports high-performance CSV export with automatic partitioning for large datasets, and Excel export with integrated charting and document properties.
Parallelized smoothing for fast processing of large arrays.
Excel automation is available via strong-typed Interop on .NET Framework or late-binding COM on .NET Standard / Windows.

> Target : .NET Standard 2.0 and .NET Framework 4.8  
> Excel export requires Microsoft Excel installed (COM automation).  
All smoothing and export APIs are designed for batch and interactive scenarios, with progress and cancellation support for responsive UI.

---

## Contents
- [Overview](#overview)
- [Public API (exact signatures)](#public-api-exact-signatures)
- [Behavioral details](#behavioral-details)
- [Exceptions](#exceptions)
- [Notes and limitations](#notes-and-limitations)
- [License and third-party dependencies](#license-and-third-party-dependencies)

---

## Overview
- Median-based smoothing for numeric sequences (MiddleMedian and AllMedian).
- CSV export with automatic partitioning to respect Excel's row limits.
- Excel export (Interop on .NET Framework, late-binding COM on .NET Standard) with a chart and document properties.
- Configurable boundary handling for AllMedian (Symmetric, Replicate, ZeroPad, Adaptive), recorded in CSV and shown in Excel headers.

This Readme documents and exemplifies the exact behavior and public API present in :
- SignatureMedian.vb
- CsvOrderTicket.vb
- CsvCustomOrder.vb
- CsvBrewService.vb
- ExcelOrderTicket.vb
- ExcelCustomOrder.vb
- ExcelBrewService.vb

### Key concepts
- Radius vs Width :
  - Radius (R) is the user-specified smoothing parameter.
  - Window width (W) used by the median is W = (2 × R) + 1. Odd widths are recommended. Even widths are accepted and the median is the average of the two middle values.
  - `SignatureMedian.ComputeMedians` takes `kernelWidth` (W). To pass a true radius R, use `ComputeMediansByRadius` (converts R → W and validates inputs).

- MiddleMedian vs AllMedian :
  - MiddleMedian preserves the first and last `BorderCount` values unchanged. BoundaryMode is ignored in this path; a clamped, variable-length window is used near the edges.
  - AllMedian applies smoothing across the full series and honors `BoundaryMode` when synthesizing samples near the edges :
    - Symmetric (default) : periodic reflection at the edges.
    - Replicate : clamp to nearest edge value.
    - ZeroPad : treat out-of-range samples as zero.
    - Adaptive : keep the window centred on the current sample and shrink it symmetrically near edges (no synthetic samples; no phase shift).

-------------------------------------------------------------------------------

## Installation
NuGet (preferred) : dotnet add package AvocadoSmoothie.Barista  
Package Manager : Install-Package AvocadoSmoothie.Barista

---

## Quick Start
```vbnet
Imports AvocadoSmoothie.Barista

' Prepare your numeric data (as IList(Of Double))
Dim data As IList(Of Double) = {1.0, 2.0, 3.0, 10.0, 9.0, 5.0, 4.0, 3.0, 2.0}

Dim radius As Integer = 2        ' Smoothing radius (window width W = 2 × radius + 1 = 5)
Dim borderCount As Integer = 1   ' Number of edge values to preserve (MiddleMedian only)

' Compute MiddleMedian (preserves borderCount values at each end)
Dim middleMedian As List(Of Double) =
    SignatureMedian.ComputeMedians(
        input:=data.ToList(),
        useMiddle:=True,
        kernelWidth:=2 * radius + 1,   ' Window width (odd recommended)
        borderCount:=borderCount
    )

' Compute AllMedian (smooths the entire series)
Dim allMedian As List(Of Double) =
    SignatureMedian.ComputeMedians(
        input:=data.ToList(),
        useMiddle:=False,
        kernelWidth:=2 * radius + 1,   ' Window width (odd recommended)
        borderCount:=borderCount
    )
```

```vbnet
' Alternative: radius-based overload (avoids manual 2 * R + 1)
' This overload automatically converts kernelRadius to window width W = 2R + 1
' and performs input validation (null check, range check, overflow check).

Dim middleMedianAlt As List(Of Double) = SignatureMedian.ComputeMediansByRadius(
    input:=data.ToList(),
    useMiddle:=True,
    kernelRadius:=radius,
    borderCount:=borderCount
)

Dim allMedianAlt As List(Of Double) = SignatureMedian.ComputeMediansByRadius(
    input:=data.ToList(),
    useMiddle:=False,
    kernelRadius:=radius,
    borderCount:=borderCount
)
```

CSV Export Example :
```vbnet
Imports AvocadoSmoothie.Barista
Imports System.Threading

Dim ticket = New CsvOrderTicket With {
    .InitialData = data,
    .DatasetTitle = "Demo Dataset",
    .KernelRadius = radius,
    .BorderCount = borderCount,
    .BasePath = "C:\Temp\demo.csv"
}

' Export smoothed results to CSV (async)
Dim files = Await CsvBrewService.ExportCsvAsync(ticket)
```

Excel Export Example :
```vbnet
Imports AvocadoSmoothie.Barista
Imports System.Threading

Dim excelTicket = New ExcelOrderTicket With {
    .InitialData = data,
    .DatasetTitle = "Demo Dataset",
    .KernelRadius = radius,
    .BorderCount = borderCount
}

' Export to Excel (async, opens workbook in Excel)
Await ExcelBrewService.ExcelCustomOrder(excelTicket)
```
  
> Note : For SignatureMedian.ComputeMedians, kernelWidth is the window width W (≥ 1; odd recommended).  
> To pass a true radius R, use ComputeMediansByRadius which converts R to W = 2 × R + 1.  
> Boundary handling can be specified via the optional boundaryMode parameter (default = Symmetric) for AllMedian paths.

---

### Boundary Handling Example
```vbnet
' CSV ticket: add BoundaryMode
Dim ticket = New CsvOrderTicket With {
    .InitialData = data,
    .DatasetTitle = "My Dataset",
    .KernelRadius = 5,
    .BorderCount = 2,
    .BasePath = "C:\Temp\MyDataset.csv",
    .Progress = progress,
    .CancellationToken = cts.Token,
    .BoundaryMode = SignatureMedian.BoundaryMode.Adaptive
}
```

```vbnet
' Excel ticket: add BoundaryMode
Dim ticket = New ExcelOrderTicket With {
    .InitialData = data,
    .DatasetTitle = "Cosine Series",
    .KernelRadius = 9,
    .BorderCount = 4,
    .Progress = progress,
    .CancellationToken = cts.Token,
    .BoundaryMode = SignatureMedian.BoundaryMode.Symmetric
}
```

```vbnet
' Direct AllMedian with explicit boundary handling
Dim allMedianSym As List(Of Double) = SignatureMedian.ComputeMediansByRadius(
    input:=data.ToList(),
    useMiddle:=False,
    kernelRadius:=radius,
    borderCount:=borderCount,
    boundaryMode:=SignatureMedian.BoundaryMode.Symmetric
)
```
---

## Public API (exact signatures)
- Class SignatureMedian
  - Public Shared Function ComputeMediansByRadius(input As List(Of Double), useMiddle As Boolean, kernelRadius As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)
  - Public Shared Function ComputeMedians(input As List(Of Double), useMiddle As Boolean, kernelWidth As Integer, borderCount As Integer, Optional progress As IProgress(Of Integer) = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As List(Of Double)

- Enum SignatureMedian.BoundaryMode
  - Symmetric
  - Replicate
  - ZeroPad
  - Adaptive

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
  - Public Property CancellationToken As Threading.CancellationToken
  - Public Property Progress As IProgress(Of Integer)
  - Public Property BoundaryMode As SignatureMedian.BoundaryMode
  - Public Sub Validate()

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
  - Public Property BoundaryMode As SignatureMedian.BoundaryMode
  - Public Sub ValidateBasic()

- Class ExcelBrewService (NotInheritable)
  - Public Shared Function ExcelCustomOrder(request As ExcelOrderTicket) As Threading.Tasks.Task
  - Public Shared Async Function ExportExcelAsync(initialData As IList(Of Double), datasetTitle As String, kernelRadius As Integer, borderCount As Integer, progress As IProgress(Of Integer), Optional cancelToken As System.Threading.CancellationToken = Nothing, Optional boundaryMode As SignatureMedian.BoundaryMode = SignatureMedian.BoundaryMode.Symmetric) As Threading.Tasks.Task

-------------------------------------------------------------------------------

## Behavioral details
1) SignatureMedian
- ComputeMedians
  - input : List(Of Double). If empty, reports 0 to progress (when present) and returns an empty list.
  - kernelWidth: W ≥ 1. Odd is recommended; even W is supported (median is the average of the two central values).
  - Boundary handling :
    - useMiddle = True (MiddleMedian) : boundaryMode is ignored; a clamped, variable-length window is used near edges; first / last `BorderCount` samples are copied unchanged.
    - useMiddle = False (AllMedian) : boundaryMode is applied:
      - Symmetric : 64-bit periodic reflection mapping.
      - Replicate : edge clamping.
      - ZeroPad : zeros outside range.
      - Adaptive : keep the window centred on the current sample and shrink it symmetrically near edges (no synthetic samples; no phase shift).
  - kernelWidth : Window offsets are computed as:
    - offsetLow = (W - 1) \ 2
    - offsetHigh = (W - 1) - offsetLow
  - borderCount : Number of unchanged elements on both ends for MiddleMedian only.
  - progress : Receives integer counts of processed elements :
    - Reports 0 at start, periodic updates at roughly N / 200 intervals, and N at the end.
  - Implementation :
    - Parallel.For with a ThreadLocal window buffer Double().
    - Near edges: MiddleMedian clamps indices; AllMedian uses the configured boundaryMode (Adaptive shrinks the window symmetrically around the current index).

2) CSV export
- CsvOrderTicket.Validate
  - Ensures :
    - InitialData exists and non-empty.
    - DatasetTitle non-empty.
    - KernelRadius > 0.
    - BorderCount ≥ 0.
    - BasePath non-empty and its directory exists; otherwise throws DirectoryNotFoundException.
- CsvBrewService.ExportCsvAsync overloads
  - Overload with CsvOrderTicket :
    - Throws ArgumentNullException if request Is Nothing.
    - Calls request.Validate() and delegates.
  - Core overload (initialData, datasetTitle, kernelRadius, borderCount, basePath, progress, cancelToken, boundaryMode) :
    - Validations :
      - initialData non-empty; datasetTitle non-empty; basePath non-empty; kernelRadius > 0.
      - Smoothing parameters validated via IngredientRatioValidator.ThrowIfInvalidSmoothingParameters with windowSize W = 2 × R + 1 for both Middle and All passes (enforces W ≤ N; borderCount ≤ N; and for Middle: 2 × borderCount < W).
    - Smoothing :
      - Calls SignatureMedian.ComputeMediansByRadius for MiddleMedian then AllMedian on Task.Run, honoring cancelToken before invoking.
    - File partitioning :
      - Excel row limit is 1,048,576 (EXCEL_MAX_ROW).
      - HEADER_LINES = 12; The service computes maxDataRows = EXCEL_MAX_ROW - HEADER_LINES - 1.
      - Splits data into parts by maxDataRows.
    - File naming :
      - If a single part : basePath is used.
      - If multiple parts : {BaseName}_Part{index}{ext}. Default extension is ".csv" when missing.
    - File content (UTF-8) :
      - Header now includes boundary information and HEADER_LINES = 12 :
      1. Line 1 : DatasetTitle
      2. Line 2 : "Part {p} of {P}" (even for single part : "Part 1 of 1")
      3. Line 3 : empty
      4. "Smoothing Parameters"
      5. $"Kernel Radius : {kernelRadius}"            (R)
      6. $"Kernel Width : {kernelWidth}"              (W = 2 × R + 1)
      7. $"Border Count : {borderCount}"
      8. $"Boundary Method : {BoundaryMode}"
      9. empty
      10. $"Generated : {DateTime.Now.ToString("G", CurrentCulture)}"
      11. empty
      12. "Initial Data,MiddleMedian,AllMedian"
          - Splitting uses maxDataRows = EXCEL_MAX_ROW - HEADER_LINES - 1.
      13+. Data rows :
          - Values formatted with "G17" and InvariantCulture for each of Initial, Middle, All.
    - Progress :
      - During both median computations : progress receives forwarded counts from ComputeMedians (0 … N).
      - During file writing : progress receives percentage computed as CInt((i + 1) / CDbl(N) × 100) for each written row (monotonic across parts).
    - Cancellation :
      - cancelToken.ThrowIfCancellationRequested() is called before each compute phase and at the beginning of each part iteration.

3) Excel export
- ExcelOrderTicket.ValidateBasic
  - Ensures :
    - InitialData non-empty, DatasetTitle non-empty, KernelRadius > 0, BorderCount ≥ 0.
- ExcelBrewService.ExportExcelAsync
  - Validations :
    - initialData non-empty; datasetTitle non-empty; kernelRadius > 0; borderCount ≥ 0.
  - Smoothing :
    - n = initialData.Count
    - kernelWidth = 2 × kernelRadius + 1
    - ValidateSmoothingParameters(n, kernelWidth, kernelRadius, borderCount, useMiddle :=True)
    - MiddleMedian computed first, with kernelWidth.
    - ValidateSmoothingParameters(n, kernelWidth, kernelRadius, borderCount, useMiddle :=False)    ' uses kernelWidth for AllMedian as well
    - AllMedian computed second, with kernelWidth.
  - Progress :
    - Progress mapping (intended) : 0 – 29 (Middle), 30 – 59 (All), 60 – 100 (Excel writing).
    - MiddleMedian : progress receives Math.Min(29, Math.Max(0, v)).
    - AllMedian : maps ComputeMedians count to 30 … 59 using v / n.
    - Excel writing : reports 60, progresses through writing & charting, then 100, delays 200 ms, and reports 0.
    - Note: The writer may report intermediate steps (e.g., 30 / 60 / 80 / 100) and can reset to 0 at the end; do not assume strictly monotonic percentages.
  - Output workbook (platform dependent) :
    - NET48 : strongly typed Microsoft.Office.Interop.Excel.
    - Non-NET48 (net standard 2.0 path) : late-binding COM; requires Windows + Microsoft Excel installed; throws PlatformNotSupportedException on non-Windows and InvalidOperationException when Excel isn't registered. COM exceptions include a stage hint.
  - Sheet layout :
    - Worksheet name = datasetTitle.
    - Headers :
      - Cell(1,1) : datasetTitle
      - Cell(3,1) : "Smoothing Parameters"
      - Cell(4,1) : $"Kernel Radius : {kernelRadius}"
      - Cell(5,1) : $"Kernel Width : {kernelWidth}"
      - Cell(6,1) : $"Boundary Method : {boundaryMode}"
      - Cell(7,1) : $"Border Count : {borderCount}"
    - Data placement :
      - DATA_START_ROW = 4
      - Columns :
        - Initial Data from column 3 (with "Initial Data" label at row 3 in that column).
        - MiddleMedian begins two columns to the right of the last Initial column, labeled "MiddleMedian".
        - AllMedian begins two columns to the right of the last Middle column, labeled "AllMedian".
      - If a series exceeds 1,048,576 - DATA_START_ROW + 1 rows, the series continues in the next column (vertical blocks; one column per block).
    - Chart :
      - Line chart placed near the data, after the last used column.
      - Title : datasetTitle.
      - Value axis title : "Value".
      - Category axis title : "Sequence Number".
      - Series names :
        - "Initial Data"
        - "Middle Median"
        - "All Median"
    - Built-in document properties :
      - Title, Category, Subject, Author, Last Author, Keywords, Comments are populated. Comments vary with row count.
    - Application state :
      - Workbook Saved = False; Excel.Visible = True; DisplayAlerts = True.
- ExcelBrewService.ExcelCustomOrder
  - Validates with request.ValidateBasic(), then delegates to ExportExcelAsync with matching parameters.

-------------------------------------------------------------------------------

## Exceptions
- CsvOrderTicket.Validate
  - ArgumentException : InitialData empty, DatasetTitle missing, BasePath missing.
  - ArgumentOutOfRangeException : KernelRadius ≤ 0 or BorderCount < 0.
  - DirectoryNotFoundException : The directory part of BasePath does not exist.

- CsvBrewService.ExportCsvAsync (core overload)
  - ArgumentException : initialData empty, datasetTitle missing, basePath missing.
  - ArgumentOutOfRangeException : kernelRadius ≤ 0.
  - InvalidOperationException : invalid smoothing parameters (e.g., window too large for data, border too wide for MiddleMedian).
  - OperationCanceledException : when cancelToken is requested.
  - File I/O errors (e.g., UnauthorizedAccessException, IOException, PathTooLongException) can bubble from the FileStream / StreamWriter operations.

- ExcelOrderTicket.ValidateBasic
  - ArgumentException : InitialData empty, DatasetTitle missing.
  - ArgumentOutOfRangeException : KernelRadius ≤ 0 or BorderCount < 0.

- ExcelBrewService.ExportExcelAsync
  - ArgumentException : initialData empty, datasetTitle missing.
  - ArgumentOutOfRangeException : kernelRadius ≤ 0 or borderCount < 0.
  - InvalidOperationException : smoothing parameter violations (e.g., windowSize > dataCount; for MiddleMedian also 2 × borderCount ≥ windowSize).
  - OperationCanceledException : when cancelToken is requested.
  - .NET Framework (NET48) :
    - COMException during Excel automation is wrapped as ExcelInteropNotAvailableException.
  - .NET Standard (late-binding) :
    - PlatformNotSupportedException on non-Windows.
    - ExcelInteropNotAvailableException if Excel is not installed or not registered (ProgID "Excel.Application"), or if COM activation fails.
    - COMException with a stage hint on late-binding failures, wrapped as ExcelInteropNotAvailableException.

-------------------------------------------------------------------------------

## Usage examples (VB.NET)
- CSV export via ticket
```vbnet
Imports AvocadoSmoothie.Barista
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading

Module DemoCsv
    Async Function RunAsync() As Task
        ' Create a list of Double values from 1 to 10000
        Dim data As IList(Of Double) =
            Enumerable.Range(1, 10000) _
                      .Select(Function(i) CDbl(i)) _
                      .ToList()

        ' Progress reporter that writes progress percentage to the console
        Dim progress = New Progress(Of Integer)(
            Sub(v) Console.WriteLine($"Progress : {v}")
        )

        ' Cancellation token source for stopping the operation if needed
        Dim cts = New CancellationTokenSource()

        ' Create a CSV export ticket with configuration
        Dim ticket = New CsvOrderTicket With {
            .InitialData = data,
            .DatasetTitle = "My Dataset",
            .KernelRadius = 5,        ' R (window width W = 2 × R + 1 = 11)
            .BorderCount = 2,
            .BasePath = "C:\Temp\MyDataset.csv",
            .Progress = progress,
            .CancellationToken = cts.Token
        }

        ' Export CSV files asynchronously
        Dim files = Await CsvBrewService.ExportCsvAsync(ticket)

        ' Output the created file paths
        For Each f In files
            Console.WriteLine("Created : " & f)
        Next
    End Function

End Module
```

- CSV export via core overload
```vbnet
Imports AvocadoSmoothie.Barista
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading

Module DemoCsvCore

    Async Function RunAsync() As Task
        ' Generate a list of Double values representing a sine wave
        ' from 1 to 250,000 with a step of 1, scaled by 1 / 1000.0
        Dim data As IList(Of Double) =
            Enumerable.Range(1, 250000) _
                      .Select(Function(i) Math.Sin(i / 1000.0)) _
                      .ToList()

        ' Progress reporter that writes progress percentage to the console
        Dim progress = New Progress(Of Integer)(
            Sub(v) Console.WriteLine($"Progress : {v}")
        )

        ' Cancellation token source for stopping the operation if needed
        Dim cts = New CancellationTokenSource()

        ' Export CSV files asynchronously with the given parameters
        Dim files = Await CsvBrewService.ExportCsvAsync(
            initialData:=data,
            datasetTitle:="Sine Series",
            kernelRadius:=7,           ' R (window width W = 15)
            borderCount:=3,
            basePath:="C:\Temp\SineSeries.csv",
            progress:=progress,
            cancelToken:=cts.Token
        )

        ' Output the created file paths
        For Each f In files
            Console.WriteLine("Created : " & f)
        Next
    End Function

End Module
```

- Excel export via ticket
```vbnet
Imports AvocadoSmoothie.Barista
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading

Module DemoExcel

    Async Function RunAsync() As Task
        ' Generate a list of Double values representing a cosine wave
        ' from 1 to 10,000 with a step of 1, scaled by 1 / 123.0
        Dim data As IList(Of Double) =
            Enumerable.Range(1, 10000) _
                      .Select(Function(i) Math.Cos(i / 123.0)) _
                      .ToList()

        ' Progress reporter that writes progress percentage to the console
        Dim progress = New Progress(Of Integer)(
            Sub(v) Console.WriteLine($"Progress : {v}")
        )

        ' Cancellation token source for stopping the operation if needed
        Dim cts = New CancellationTokenSource()

        ' Create an Excel export ticket with configuration
        Dim ticket = New ExcelOrderTicket With {
            .InitialData = data,
            .DatasetTitle = "Cosine Series",
            .KernelRadius = 9,         ' R (window width W = 19)
            .BorderCount = 4,
            .Progress = progress,
            .CancellationToken = cts.Token
        }

        ' Generate the Excel workbook (visible, unsaved)
        Await ExcelBrewService.ExcelCustomOrder(ticket)
    End Function

End Module
```

- Excel export via core overload
```vbnet
Imports AvocadoSmoothie.Barista
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading

Module DemoExcelCore

    Async Function RunAsync() As Task
        ' Generate a list of Double values representing a logarithmic series
        ' from 1 to 50,000 with a step of 1
        Dim data As IList(Of Double) =
            Enumerable.Range(1, 50000) _
                      .Select(Function(i) Math.Log(i)) _
                      .ToList()

        ' Progress reporter that writes progress percentage to the console
        Dim progress = New Progress(Of Integer)(
            Sub(v) Console.WriteLine($"Progress : {v}")
        )

        ' Cancellation token source for stopping the operation if needed
        Dim cts = New CancellationTokenSource()

        ' Export Excel file asynchronously with the given parameters
        Await ExcelBrewService.ExportExcelAsync(
            initialData:=data,
            datasetTitle:="Log Series",
            kernelRadius:=6,           ' R (window width W = 13)
            borderCount:=2,
            progress:=progress,
            cancelToken:=cts.Token
        )
    End Function

End Module
```

- Excel export with robust exception handling (recommended pattern)
```vbnet
Try
    Await ExcelBrewService.ExcelCustomOrder(ticket)
Catch ex As AvocadoSmoothie.Barista.ExcelBrewService.ExcelInteropNotAvailableException
    Select Case ex.Reason
        Case AvocadoSmoothie.Barista.ExcelBrewService.ExcelInteropUnavailableReason.NotInstalled,
             AvocadoSmoothie.Barista.ExcelBrewService.ExcelInteropUnavailableReason.ClassNotRegistered
            Console.WriteLine("Excel is not installed or not registered. Details: " & ex.Message)
        Case AvocadoSmoothie.Barista.ExcelBrewService.ExcelInteropUnavailableReason.PlatformNotSupported
            Console.WriteLine("Excel export requires Windows. Details: " & ex.Message)
        Case AvocadoSmoothie.Barista.ExcelBrewService.ExcelInteropUnavailableReason.ActivationFailed
            Console.WriteLine("Failed to start Excel application. Details: " & ex.Message)
        Case Else
            Console.WriteLine("Excel export failed. Details: " & ex.Message)
    End Select
Catch ex As InvalidOperationException
    Console.WriteLine("Parameter error: " & ex.Message)
Catch ex As Exception
    Console.WriteLine("Unexpected error: " & ex.Message)
End Try
```

## Notes and limitations
- SignatureMedian.ComputeMedians does not take a CancellationToken; cancellations are managed by callers before invoking it.
- IngredientRatioValidator centralizes validation and message text for both CSV and Excel export paths.
- For Excel export on non-NET48 targets :
  - Windows OS is required.
  - Microsoft Excel must be installed and registered (ProgID "Excel.Application").
- Excel export applies built-in document properties and inserts a line chart with three series.

-------------------------------------------------------------------------------

## License and third-party dependencies
- MIT License. Include the full text in distribution (see root LICENSE if present).
- Uses Microsoft Office Interop (NET48) or late-binding COM (Windows-only) to automate Excel. Ensure Excel is installed for Excel export scenarios on non-NET48 targets.

---  

Blend your signals, export with ease - discover your data’s true flavor!