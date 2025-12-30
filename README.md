# AvocadoSmoothie.Barista.Tasting
A C# / .NET Framework 4.8 sample app built on **AvocadoSmoothie.Barista**, showcasing advanced running-median smoothing and export workflows.  
**AvocadoSmoothie.Barista.Tasting** is your tasting room : dial in the "recipe," iterate on parameters, and learn the library's behavior hands-on - so you can confidently brew your own production-ready data processing.


## AvocadoSmoothie.Barista & AvocadoSmoothie.Barista.Tasting
### Naming Philosophy

**AvocadoSmoothie.Barista** is a C# / .NET library for robust running median smoothing and export utilities, designed to help you "craft" your data with the care of a skilled barista. The name "Barista" reflects the library's purpose : to blend, refine, and serve data with precision and clarity.


**AvocadoSmoothie.Barista.Tasting** is the companion sample application - a "tasting" session for your data.

In specialty coffee, tasting is where craft is born : you dial-in the recipe, learn the variables, and train your palate before serving the final cup.  
This project is designed as that space for **AvocadoSmoothie.Barista** - a practical lab to test parameters, observe smoothing behavior, and understand boundary modes and export results with confidence.

Step into the tasting room, refine your technique, and ship your own full-scale application with a barista's certainty.


## What's New
### v1.0.0.0
#### December 30, 2025
> Initial release.

## How to Use AvocadoSmoothie.Barista (with Tasting Sample)

This section describes how to use the AvocadoSmoothie.Barista library, as demonstrated in the `AvocadoSmoothie.Barista.Tasting` sample application (`frmMain.cs` and `frmMain.Designer.cs`).

### 1. Add the Library

Reference the `AvocadoSmoothie.Barista` library in your .NET Framework 4.8 project.

### 2. Prepare Input Data

Input data should be a sequence of numbers (e.g., `List<double>`). The sample application allows you to paste or type values separated by spaces, commas, or line breaks into the "Initial Data" box.

### 3. Select Smoothing Methods

The UI provides checkboxes for :
- **All Median** : Applies a fixed-size window with boundary handling.
- **Middle Median** : Preserves edge values using a variable-length window.

You can select one or both methods to apply.

### 4. Configure Parameters

Adjust parameters using the UI controls :
- **Kernel Radius** : Size of the smoothing window (R).
- **Border Count** : For Middle Median, number of edge values to preserve.
- **Boundary Handling** : Symmetric, Adaptive, Replicate, or ZeroPad (for All Median).

### 5. Run Smoothing

Click **Start Smoothing** to process the input. The results for each selected method are displayed in the "Refined Data" box.

#### Example : Running Smoothing in Code
```csharp
// Compute AllMedian smoothing
var allMedian = SignatureMedian.ComputeMedians(
    input : data,
    useMiddle : false,
    kernelWidth : windowWidth,
    borderCount : borderCount,
    progress : progressReporter,
    boundaryMode : SignatureMedian.BoundaryMode.Symmetric
);

// Compute MiddleMedian smoothing
var middleMedian = SignatureMedian.ComputeMedians(
    input : data,
    useMiddle : true,
    kernelWidth : windowWidth,
    borderCount : borderCount,
    progress : progressReporter
);
```

### 6. Export Results

- **Export to CSV** : Save results in a CSV file (with headers describing all parameters).
- **Export to Excel** : Save results in an Excel file (with worksheet headers and a line chart).

#### Example : Exporting to CSV
```csharp
var ticket = new CsvOrderTicket
{
    InitialData = data,
    DatasetTitle = "My Dataset",
    KernelRadius = radius,
    BorderCount = borderCount,
    BasePath = @"C :\path\to\output.csv",
    Progress = progress,
    CancellationToken = cancellationToken,
    BoundaryMode = SignatureMedian.BoundaryMode.Symmetric
};

await CsvBrewService.ExportCsvAsync(ticket);
```

#### Example : Exporting to Excel
```csharp
var ticket = new ExcelOrderTicket
{
    InitialData = data,
    DatasetTitle = "My Dataset",
    KernelRadius = radius,
    BorderCount = borderCount,
    Progress = progress,
    CancellationToken = cancellationToken,
    BoundaryMode = SignatureMedian.BoundaryMode.Symmetric
};

await ExcelBrewService.ExcelCustomOrder(ticket);
```


> Both export features include the original and all selected smoothing results in the output file.  
> For parameter details, see the [docs/AvocadoSmoothie_API_Documentation.xml](doc/AvocadoSmoothie_API_Documentation.xml).

### 7. Advanced : Boundary Handling

- **Symmetric** : Reflects data at the edges (default, smoothest).
- **Adaptive** : Slides the window fully inside the valid range (no synthetic samples).
- **Replicate** : Clamps to the nearest edge value.
- **ZeroPad** : Pads with zeros outside the range.

Select the desired mode in the UI or via the `BoundaryMode` parameter.

## Installation

Add a reference to `AvocadoSmoothie.Barista` in your .NET Framework 4.8 project.

## Further Reading

- See the [docs/AvocadoSmoothie_API_Documentation.xml](doc/AvocadoSmoothie_API_Documentation.xml) for full API details.
- Explore `frmMain.cs` and `frmMain.Designer.cs` for practical usage patterns and UI integration.

---

**AvocadoSmoothie.Barista** : Brew clarity from noisy data.  
**AvocadoSmoothie.Barista.Tasting** : Taste, iterate, and dial in (fine-tune) your recipe.




