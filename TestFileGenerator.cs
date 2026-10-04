using System;
using System.Collections.Generic;
using System.IO;

public static class TestFileGenerator
{
    public static void GenerateTests()
    {
        string testDirectory =
            "part4_tests";

        Directory.CreateDirectory(
            testDirectory
        );

        GenerateDenseSoup(
            Path.Combine(
                testDirectory,
                "dense_soup.txt"
            )
        );

        GenerateSparseVoid(
            Path.Combine(
                testDirectory,
                "sparse_void.txt"
            )
        );

        Console.WriteLine(
            "Part 4 test files generated:"
        );

        Console.WriteLine(
            Path.Combine(
                testDirectory,
                "dense_soup.txt"
            )
        );

        Console.WriteLine(
            Path.Combine(
                testDirectory,
                "sparse_void.txt"
            )
        );
    }

    private static void GenerateDenseSoup(
        string filePath)
    {
        int width = 1000;
        int height = 1000;
        int generations = 200;

        // A fixed seed makes the random test
        // repeatable every time.
        Random random =
            new Random(2240);

        List<(int Column, int Row)>
            liveCells =
                new List<
                    (int Column, int Row)
                >();

        for (int row = 0;
             row < height;
             row++)
        {
            for (int column = 0;
                 column < width;
                 column++)
            {
                // Approximately 50% of cells
                // begin alive.
                if (random.NextDouble() < 0.50)
                {
                    liveCells.Add(
                        (column, row)
                    );
                }
            }
        }

        using StreamWriter writer =
            new StreamWriter(filePath);

        writer.WriteLine(
            $"{width},{height}"
        );

        writer.WriteLine(
            generations
        );

        writer.WriteLine(
            liveCells.Count
        );

        foreach (
            (int column, int row)
            in liveCells)
        {
            writer.WriteLine(
                $"{column},{row}"
            );
        }
    }

    private static void GenerateSparseVoid(
        string filePath)
    {
        int width = 10000;
        int height = 10000;
        int generations = 200;

        // Only a few live cells exist in this
        // extremely large universe.
        //
        // The first three form a blinker.
        // The remaining five form a glider.
        (int Column, int Row)[] liveCells =
        {
            (5000, 5000),
            (5000, 5001),
            (5000, 5002),

            (100, 100),
            (101, 101),
            (99, 102),
            (100, 102),
            (101, 102)
        };

        using StreamWriter writer =
            new StreamWriter(filePath);

        writer.WriteLine(
            $"{width},{height}"
        );

        writer.WriteLine(
            generations
        );

        writer.WriteLine(
            liveCells.Length
        );

        foreach (
            (int column, int row)
            in liveCells)
        {
            writer.WriteLine(
                $"{column},{row}"
            );
        }
    }
}