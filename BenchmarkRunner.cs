using System;
using System.Diagnostics;

// Had to look up and research this one I have never actually ran a stopwatch for my code to evalutate runtime. 
public static class BenchmarkRunner
{
    public static void RunBenchmarks()
    {
        int[] sizes = { 100, 200, 500, 1000, 10000 };

        int generations = 200;

        Console.WriteLine("Starting Game of Life benchmark...");
        Console.WriteLine();

        Console.WriteLine(
            "Grid Size | Grid Area | Avg ms/Generation | Peak Memory MB"
        );

        Console.WriteLine(
            "-------------------------------------------------------------"
        );

        foreach (int size in sizes)
        {
            GameOfLife game = new GameOfLife(size, size);

            int center = size / 2;

            game.SetAlive(center, center - 1);
            game.SetAlive(center, center);
            game.SetAlive(center, center + 1);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Process process = Process.GetCurrentProcess();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int generation = 0;
                 generation < generations;
                 generation++)
            {
                game.NextGeneration();
            }

            stopwatch.Stop();

            process.Refresh();

            double averageMilliseconds =
                stopwatch.Elapsed.TotalMilliseconds / generations;

            double peakMemoryMB =
                process.PeakWorkingSet64 / (1024.0 * 1024.0);

            long gridArea = (long)size * size;

            Console.WriteLine(
                $"{size}x{size} | " +
                $"{gridArea} | " +
                $"{averageMilliseconds:F4} ms | " +
                $"{peakMemoryMB:F2} MB"
            );
        }

        Console.WriteLine();
        Console.WriteLine("Benchmark complete.");
    }
}