using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SparseGameOfLife
{
    private int width;
    private int height;

    // Only living cells are stored.
    private HashSet<(int Column, int Row)> liveCells;

    public SparseGameOfLife(
        int width,
        int height)
    {
        this.width = width;
        this.height = height;

        liveCells =
            new HashSet<(int Column, int Row)>();
    }

    public void SetAlive(
        int column,
        int row)
    {
        if (column < 0 ||
            column >= width ||
            row < 0 ||
            row >= height)
        {
            throw new ArgumentOutOfRangeException(
                $"Cell ({column},{row}) is outside the grid."
            );
        }

        liveCells.Add((column, row));
    }

    public void NextGeneration()
    {
        // Stores the number of living neighbors
        // surrounding each relevant coordinate.
        Dictionary<(int Column, int Row), int>
            neighborCounts =
                new Dictionary<
                    (int Column, int Row),
                    int
                >();

        // Instead of scanning the entire universe,
        // start with only the cells that are alive.
        foreach ((int column, int row) in liveCells)
        {
            for (int rowOffset = -1;
                 rowOffset <= 1;
                 rowOffset++)
            {
                for (int columnOffset = -1;
                     columnOffset <= 1;
                     columnOffset++)
                {
                    // Do not count the cell itself.
                    if (rowOffset == 0 &&
                        columnOffset == 0)
                    {
                        continue;
                    }

                    int neighborColumn =
                        column + columnOffset;

                    int neighborRow =
                        row + rowOffset;

                    // Preserve the finite boundaries used
                    // by the dense implementation.
                    if (neighborColumn < 0 ||
                        neighborColumn >= width ||
                        neighborRow < 0 ||
                        neighborRow >= height)
                    {
                        continue;
                    }

                    var neighbor =
                        (
                            Column: neighborColumn,
                            Row: neighborRow
                        );

                    // Increase the number of live neighbors
                    // recorded for this coordinate.
                    if (neighborCounts.ContainsKey(neighbor))
                    {
                        neighborCounts[neighbor]++;
                    }
                    else
                    {
                        neighborCounts[neighbor] = 1;
                    }
                }
            }
        }

        HashSet<(int Column, int Row)>
            nextLiveCells =
                new HashSet<
                    (int Column, int Row)
                >();

        // Only evaluate coordinates that actually
        // have at least one living neighbor.
        foreach (var entry in neighborCounts)
        {
            var cell = entry.Key;
            int neighbors = entry.Value;

            bool currentlyAlive =
                liveCells.Contains(cell);

            // Living cell survives with 2 or 3 neighbors.
            if (currentlyAlive &&
                (neighbors == 2 ||
                 neighbors == 3))
            {
                nextLiveCells.Add(cell);
            }

            // Dead cell becomes alive with exactly 3.
            else if (!currentlyAlive &&
                     neighbors == 3)
            {
                nextLiveCells.Add(cell);
            }
        }

        // Replace the old live-cell set with
        // the newly calculated generation.
        liveCells = nextLiveCells;
    }

    public void Run(
        int steps,
        bool graphics)
    {
        if (graphics)
        {
            Display(0);
        }

        for (int generation = 1;
             generation <= steps;
             generation++)
        {
            NextGeneration();

            if (graphics)
            {
                Display(generation);
            }
        }
    }

    public void Display(int generation)
    {
        long area =
            (long)width * height;

        // Printing an enormous sparse universe would
        // eliminate much of the sparse design's benefit.
        if (area > 1_000_000)
        {
            Console.WriteLine(
                "Graphics disabled for grids larger than 1,000,000 cells."
            );

            return;
        }

        StringBuilder display =
            new StringBuilder();

        display.AppendLine(
            $"Generation: {generation}"
        );

        for (int row = 0;
             row < height;
             row++)
        {
            for (int column = 0;
                 column < width;
                 column++)
            {
                if (liveCells.Contains(
                    (column, row)))
                {
                    display.Append('#');
                }
                else
                {
                    display.Append('.');
                }
            }

            display.AppendLine();
        }

        Console.SetCursorPosition(0, 0);

        Console.Write(
            display.ToString()
        );
    }

    public int GetLiveCellCount()
    {
        return liveCells.Count;
    }

    public void WriteOutput(
        string filePath,
        int steps)
    {
        StringBuilder output =
            new StringBuilder();

        output.AppendLine(
            $"{width},{height}"
        );

        output.AppendLine(
            steps.ToString()
        );

        output.AppendLine(
            liveCells.Count.ToString()
        );

        // HashSets do not guarantee iteration order.
        // Sorting makes output files predictable.
        List<(int Column, int Row)>
            sortedCells =
                new List<
                    (int Column, int Row)
                >(liveCells);

        sortedCells.Sort(
            (a, b) =>
            {
                int rowComparison =
                    a.Row.CompareTo(b.Row);

                if (rowComparison != 0)
                {
                    return rowComparison;
                }

                return a.Column.CompareTo(
                    b.Column
                );
            }
        );

        foreach (
            (int column, int row)
            in sortedCells)
        {
            output.AppendLine(
                $"{column},{row}"
            );
        }

        string? directory =
            Path.GetDirectoryName(
                Path.GetFullPath(filePath)
            );

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(
                directory
            );
        }

        File.WriteAllText(
            filePath,
            output.ToString()
        );
    }
}