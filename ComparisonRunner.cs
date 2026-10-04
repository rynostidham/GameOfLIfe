using System;
using System.Diagnostics;
using System.IO;

public static class ComparisonRunner
{
    public static void RunComparisons()
    {
        string csvFile =
            "part4_comparison.csv";

        using StreamWriter writer =
            new StreamWriter(csvFile);

        writer.WriteLine(
            "Test Case,Engine,Initial Live," +
            "Avg ms/Generation,Peak Memory MB,Final Live"
        );
                    writer.AutoFlush = true;

        Console.WriteLine(
            "Starting Part 4 comparison..."
        );

        Console.WriteLine();

        RunTest(
            "Dense Soup",
            Path.Combine(
                "part4_tests",
                "dense_soup.txt"
            ),
            writer
        );

        RunTest(
            "Sparse Void",
            Path.Combine(
                "part4_tests",
                "sparse_void.txt"
            ),
            writer
        );

        Console.WriteLine();

        Console.WriteLine(
            "Comparison complete."
        );

        Console.WriteLine(
            $"Results saved to {csvFile}"
        );
    }

    private static void RunTest(
        string testName,
        string filePath,
        StreamWriter writer)
    {
        InputData data =
            InputFileReader.Read(filePath);

        Console.WriteLine(
            $"Testing {testName}..."
        );

        RunDense(
            testName,
            data,
            writer
        );

        // Attempt to clean up between
        // implementation tests.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        RunSparse(
            testName,
            data,
            writer
        );
    }

    private static void RunDense(
        string testName,
        InputData data,
        StreamWriter writer)
    {
        GameOfLife game =
            new GameOfLife(
                data.Width,
                data.Height
            );

        foreach (
            (int column, int row)
            in data.LiveCells)
        {
            game.SetAlive(
                column,
                row
            );
        }

        int initialLive =
            data.LiveCells.Count;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Process process =
            Process.GetCurrentProcess();

        Stopwatch stopwatch =
            Stopwatch.StartNew();

        for (int generation = 0;
             generation < data.Steps;
             generation++)
        {
            game.NextGeneration();
        }

        stopwatch.Stop();

        process.Refresh();

        double averageMilliseconds =
            stopwatch
                .Elapsed
                .TotalMilliseconds
            / data.Steps;

        double peakMemoryMB =
            process.PeakWorkingSet64 /
            (1024.0 * 1024.0);

        int finalLive =
            game.GetLiveCellCount();

        Console.WriteLine(
            $"  Dense: " +
            $"{averageMilliseconds:F4} ms/gen, " +
            $"{peakMemoryMB:F2} MB, " +
            $"Final Live: {finalLive}"
        );

        writer.WriteLine(
            $"{testName}," +
            $"Dense," +
            $"{initialLive}," +
            $"{averageMilliseconds:F4}," +
            $"{peakMemoryMB:F2}," +
            $"{finalLive}"
        );
    }

    private static void RunSparse(
        string testName,
        InputData data,
        StreamWriter writer)
    {
        SparseGameOfLife game =
            new SparseGameOfLife(
                data.Width,
                data.Height
            );

        foreach (
            (int column, int row)
            in data.LiveCells)
        {
            game.SetAlive(
                column,
                row
            );
        }

        int initialLive =
            data.LiveCells.Count;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Process process =
            Process.GetCurrentProcess();

        Stopwatch stopwatch =
            Stopwatch.StartNew();

        for (int generation = 0;
             generation < data.Steps;
             generation++)
        {
            game.NextGeneration();
        }

        stopwatch.Stop();

        process.Refresh();

        double averageMilliseconds =
            stopwatch
                .Elapsed
                .TotalMilliseconds
            / data.Steps;

        double peakMemoryMB =
            process.PeakWorkingSet64 /
            (1024.0 * 1024.0);

        int finalLive =
            game.GetLiveCellCount();

        Console.WriteLine(
            $"  Sparse: " +
            $"{averageMilliseconds:F4} ms/gen, " +
            $"{peakMemoryMB:F2} MB, " +
            $"Final Live: {finalLive}"
        );

        writer.WriteLine(
            $"{testName}," +
            $"Sparse," +
            $"{initialLive}," +
            $"{averageMilliseconds:F4}," +
            $"{peakMemoryMB:F2}," +
            $"{finalLive}"
        );
    }
}