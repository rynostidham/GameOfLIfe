using System;
using System.Diagnostics;
using System.IO;

public static class BenchmarkRunner
{
    public static void RunBenchmarks()
    {
        // Required grid sizes.
        int[] sizes =
        {
            100,
            200,
            500,
            1000,
            10000
        };

        // Use the same number of generations
        // for every benchmark.
        int generations = 200;

        string csvFile =
            "benchmark_results.csv";

        using StreamWriter writer =
            new StreamWriter(csvFile);

        writer.AutoFlush = true;

        writer.WriteLine(
            "Grid Size,Grid Area," +
            "Avg ms/Generation,Peak Memory MB"
        );

        Console.WriteLine(
            "Starting Game of Life benchmark..."
        );

        Console.WriteLine();

        Console.WriteLine(
            "Grid Size | Grid Area | " +
            "Avg ms/Generation | Peak Memory MB"
        );

        Console.WriteLine(
            "-------------------------------------------------------------"
        );

        foreach (int size in sizes)
        {
            GameOfLife game =
                new GameOfLife(size, size);

            int center = size / 2;

            // Same small starting pattern
            // for every grid size.
            game.SetAlive(
                center,
                center - 1
            );

            game.SetAlive(
                center,
                center
            );

            game.SetAlive(
                center,
                center + 1
            );

            // Try to reduce garbage collection interference
            // before beginning the timed section.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Process process =
                Process.GetCurrentProcess();

            Stopwatch stopwatch =
                Stopwatch.StartNew();

            for (int generation = 0;
                 generation < generations;
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
                / generations;

            // PeakWorkingSet64 reports bytes.
            double peakMemoryMB =
                process.PeakWorkingSet64 /
                (1024.0 * 1024.0);

            long gridArea =
                (long)size * size;

            Console.WriteLine(
                $"{size}x{size} | " +
                $"{gridArea} | " +
                $"{averageMilliseconds:F4} ms | " +
                $"{peakMemoryMB:F2} MB"
            );

            // Also save the benchmark result
            // in CSV format for the report.
            writer.WriteLine(
                $"{size}x{size}," +
                $"{gridArea}," +
                $"{averageMilliseconds:F4}," +
                $"{peakMemoryMB:F2}"
            );
        }

        Console.WriteLine();

        Console.WriteLine(
            "Benchmark complete."
        );

        Console.WriteLine(
            $"Results saved to {csvFile}"
        );
    }
}