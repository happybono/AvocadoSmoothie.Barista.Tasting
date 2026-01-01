#include "pch.h"
#include "AvocadoSmoothie.Barista.Tasting.h"

using namespace System;
using namespace System::IO;
using namespace System::Text::RegularExpressions;
using namespace System::Collections::Generic;

using namespace AvocadoSmoothie::Barista;

ref class InputHelper
{
public:
    static List<double>^ ParseInputSeries(String^ input, [System::Runtime::InteropServices::Out] String^% warn)
    {
        auto tokens = Regex::Split(input == nullptr ? "" : input, "[\\s,]+");
        auto list = gcnew List<double>();
        auto bads = gcnew List<String^>();

        for each (String ^ t in tokens)
        {
            double v;
            if (Double::TryParse(
                t,
                System::Globalization::NumberStyles::Float | System::Globalization::NumberStyles::AllowThousands,
                System::Globalization::CultureInfo::InvariantCulture,
                v) ||
                Double::TryParse(
                    t,
                    System::Globalization::NumberStyles::Float | System::Globalization::NumberStyles::AllowThousands,
                    System::Globalization::CultureInfo::CurrentCulture,
                    v))
            {
                if (!Double::IsNaN(v))
                    list->Add(v);
            }
            else if (!String::IsNullOrWhiteSpace(t))
            {
                bads->Add(t);
            }
        }

        warn = bads->Count > 0 ? String::Format("Ignored tokens: {0}", String::Join(", ", bads)) : nullptr;
        return list;
    }
};

static void PrintSeries(String^ label, IList<double>^ series)
{
    Console::Write("{0}: ", label);
    for each (double v in series)
        Console::Write("{0} ", v);
    Console::WriteLine();
}

static void PrintIfChanged(String^ label, IList<double>^ result, IList<double>^ original)
{
    bool isSame = true;

    if (result == nullptr || original == nullptr || result->Count != original->Count)
    {
        isSame = false;
    }
    else
    {
        for (int i = 0; i < result->Count; ++i)
        {
            if (result[i] != original[i])
            {
                isSame = false;
                break;
            }
        }
    }

    if (isSame)
        Console::WriteLine("{0}: No effect (identical to original)", label);
    else
        PrintSeries(label, result);
}

static void ReportProgress(int p)
{
    Console::WriteLine("Progress: {0}%", p);
}

static SignatureMedian::BoundaryMode ReadBoundaryMode()
{
    Console::Write("Boundary mode (0: Symmetric, 1: Replicate, 2: ZeroPad, 3: Adaptive, default 0): ");
    String^ boundaryStr = Console::ReadLine();

    int boundaryInt = 0;
    int parsedBoundary = 0;
    if (!String::IsNullOrWhiteSpace(boundaryStr) && Int32::TryParse(boundaryStr, parsedBoundary))
        boundaryInt = parsedBoundary;

    return static_cast<SignatureMedian::BoundaryMode>(boundaryInt);
}

void Tasting::Run()
{
    // Enter common parameters
    Console::Write("Input data (space / comma separated): ");
    String^ inputText = Console::ReadLine();
    String^ warn;
    List<double>^ values = InputHelper::ParseInputSeries(inputText, warn);

    if (values->Count == 0)
    {
        Console::WriteLine("No input values.");
        return;
    }

    if (!String::IsNullOrEmpty(warn))
        Console::WriteLine(warn);

    // Input selection for Median type (useMiddle)
    Console::WriteLine("Select median methods to apply (Y / N, default N):");
    Console::Write("Middle Median? (Y / N): ");
    bool doMiddle = String::Equals(Console::ReadLine(), "Y", StringComparison::OrdinalIgnoreCase);
    Console::Write("All Median? (Y / N): ");
    bool doAll = String::Equals(Console::ReadLine(), "Y", StringComparison::OrdinalIgnoreCase);

    if (!doMiddle && !doAll)
    {
        Console::WriteLine("No method selected.");
        return;
    }

    Console::Write("Kernel radius (int, default 4): ");
    String^ radiusStr = Console::ReadLine();
    int radius = 4;
    int parsedRadius = 0;
    if (!String::IsNullOrWhiteSpace(radiusStr) && Int32::TryParse(radiusStr, parsedRadius))
        radius = parsedRadius;

    Console::Write("Border count (int, default 0): ");
    String^ borderStr = Console::ReadLine();
    int borderCount = 0;
    int parsedBorder = 0;
    if (!String::IsNullOrWhiteSpace(borderStr) && Int32::TryParse(borderStr, parsedBorder))
        borderCount = parsedBorder;

    SignatureMedian::BoundaryMode boundaryMode = ReadBoundaryMode();

    // Select export method
    Console::Write("Export to CSV? (Y / N, default Y): ");
    bool exportCsv = !String::Equals(Console::ReadLine(), "N", StringComparison::OrdinalIgnoreCase);

    Console::Write("Export to Excel? (Y / N, default N): ");
    bool exportExcel = String::Equals(Console::ReadLine(), "Y", StringComparison::OrdinalIgnoreCase);

    // Progress handler for displaying progress
    auto progress = gcnew Progress<int>(gcnew Action<int>(&ReportProgress));

    String^ exePath = System::Reflection::Assembly::GetEntryAssembly()->Location;
    String^ exeDir = Path::GetDirectoryName(exePath);

    Console::Write("Dataset title (default: AvocadoSmoothie Barista Tasting): ");
    String^ datasetTitle = Console::ReadLine();
    if (String::IsNullOrWhiteSpace(datasetTitle))
        datasetTitle = "AvocadoSmoothie Barista Tasting";

    // CSV Export (based on AvocadoSmoothie.Barista.CsvBrewService.ExportCsvAsync)
    if (exportCsv)
    {
        Console::Write("Base CSV file name (default: BaristaTasting.csv): ");
        String^ baseName = Console::ReadLine();
        if (String::IsNullOrWhiteSpace(baseName))
            baseName = "BaristaTasting.csv";

        String^ basePath = Path::Combine(exeDir, baseName);

        try
        {
            if (radius < 1)
                Console::WriteLine("Warning: radius should be >= 1 for meaningful smoothing.");

            if (borderCount < 0)
                Console::WriteLine("Warning: borderCount should be >= 0.");

            auto task = CsvBrewService::ExportCsvAsync(
                values,
                datasetTitle,
                radius,
                borderCount,
                basePath,
                progress,
                System::Threading::CancellationToken::None,
                boundaryMode);

            task->Wait();

            IList<String^>^ paths = task->Result;
            if (paths != nullptr && paths->Count > 0)
            {
                Console::WriteLine("CSV export complete.");
                for each (String ^ p in paths)
                    Console::WriteLine(" - {0}", p);
            }
            else
            {
                Console::WriteLine("CSV export completed, but no output paths were returned.");
            }
        }
        catch (AggregateException^ ex)
        {
            for each (Exception ^ e in ex->InnerExceptions)
            {
                if (dynamic_cast<ArgumentNullException^>(e) != nullptr)
                    Console::WriteLine("CSV Error: Null argument - {0}", e->Message);
                else if (dynamic_cast<ArgumentOutOfRangeException^>(e) != nullptr)
                    Console::WriteLine("CSV Error: Out of range - {0}", e->Message);
                else if (dynamic_cast<InvalidOperationException^>(e) != nullptr)
                    Console::WriteLine("CSV Error: Invalid operation - {0}", e->Message);
                else if (dynamic_cast<OperationCanceledException^>(e) != nullptr)
                    Console::WriteLine("CSV Export canceled.");
                else
                    Console::WriteLine("CSV Error: {0}", e->Message);
            }
        }
        catch (Exception^ e)
        {
            Console::WriteLine("CSV Error: {0}", e->Message);
        }
    }

    // Excel Export (requires ExcelBrewService signature confirmation)
    if (exportExcel)
    {
        Console::Write("Base Excel file name (default: BaristaTasting.xlsx): ");
        String^ excelName = Console::ReadLine();
        if (String::IsNullOrWhiteSpace(excelName))
            excelName = "BaristaTasting.xlsx";

        String^ excelPath = Path::Combine(exeDir, excelName);

        try
        {
#if 0
            // TODO: Enable after confirming ExcelBrewService API signature from XML / Object Browser.
            // Example placeholder (WILL NOT COMPILE until replaced with the real signature):
            // auto excelTask = ExcelBrewService::ExportExcelAsync(
            //     values,
            //     datasetTitle,
            //     radius,
            //     borderCount,
            //     excelPath,
            //     progress,
            //     System::Threading::CancellationToken::None,
            //     boundaryMode);
            // excelTask->Wait();
            // Console::WriteLine("Excel export complete: {0}", excelPath);
#else
            Console::WriteLine("Excel export requested, but ExcelBrewService API is not available in the provided AvocadoSmoothie.Barista.xml (4.0.0).");
            Console::WriteLine("Please provide the ExcelBrewService XML members or its method signature to enable this block.");
            Console::WriteLine("Intended save path: {0}", excelPath);
#endif
        }
        catch (AggregateException^ ex)
        {
            for each (Exception ^ e in ex->InnerExceptions)
            {
                if (dynamic_cast<ArgumentNullException^>(e) != nullptr)
                    Console::WriteLine("Excel Error: Null argument - {0}", e->Message);
                else if (dynamic_cast<ArgumentOutOfRangeException^>(e) != nullptr)
                    Console::WriteLine("Excel Error: Out of range - {0}", e->Message);
                else if (dynamic_cast<InvalidOperationException^>(e) != nullptr)
                    Console::WriteLine("Excel Error: Invalid operation - {0}", e->Message);
                else if (dynamic_cast<OperationCanceledException^>(e) != nullptr)
                    Console::WriteLine("Excel Export canceled.");
                else
                    Console::WriteLine("Excel Error: {0}", e->Message);
            }
        }
        catch (Exception^ e)
        {
            Console::WriteLine("Excel Error: {0}", e->Message);
        }
    }

    // Example of directly calling SignatureMedian to output results
    try
    {
        if (radius < 1)
            Console::WriteLine("Warning: radius should be >= 1 for meaningful smoothing.");

        auto original = values;

        if (doMiddle)
        {
            auto middle = SignatureMedian::ComputeMediansByRadius(
                values,
                true,
                radius,
                borderCount,
                progress,
                boundaryMode);

            PrintIfChanged("Middle Median", middle, original);
        }

        if (doAll)
        {
            auto all = SignatureMedian::ComputeMediansByRadius(
                values,
                false,
                radius,
                borderCount,
                progress,
                boundaryMode);

            PrintIfChanged("All Median", all, original);
        }
    }
    catch (ArgumentNullException^ e)
    {
        Console::WriteLine("Median Error: Null argument - {0}", e->Message);
    }
    catch (ArgumentOutOfRangeException^ e)
    {
        Console::WriteLine("Median Error: Out of range - {0}", e->Message);
    }
    catch (InvalidOperationException^ e)
    {
        Console::WriteLine("Median Error: Invalid operation - {0}", e->Message);
    }
    catch (Exception^ e)
    {
        Console::WriteLine("Median Error: {0}", e->Message);
    }
}