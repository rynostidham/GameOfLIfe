using System;
using System.IO;

public static class InputFileReader
{
    public static InputData Read(string filePath)
    {
        // Read every line of the input file.
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 3)
        {
            throw new FormatException(
                "Input file does not contain enough information."
            );
        }

        // Line 1 contains width,height.
        string[] dimensions = lines[0].Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries
        );

        if (dimensions.Length != 2)
        {
            throw new FormatException(
                "Line 1 must contain width and height separated by a comma."
            );
        }

        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());

        if (width <= 0 || height <= 0)
        {
            throw new FormatException(
                "Width and height must be positive."
            );
        }

        // Line 2 contains the number of generations.
        int steps = int.Parse(lines[1].Trim());

        if (steps < 0)
        {
            throw new FormatException(
                "Simulation steps cannot be negative."
            );
        }

        // Line 3 contains the number of initially living cells.
        int liveCellCount = int.Parse(lines[2].Trim());

        if (liveCellCount < 0)
        {
            throw new FormatException(
                "Live cell count cannot be negative."
            );
        }

        // Make sure enough coordinate lines exist.
        if (lines.Length < 3 + liveCellCount)
        {
            throw new FormatException(
                "The input file contains fewer live cells than specified."
            );
        }

        InputData data = new InputData();

        data.Width = width;
        data.Height = height;
        data.Steps = steps;

        // Coordinates begin at index 3 because indexes
        // 0, 1, and 2 contain the file header information.
        for (int i = 0; i < liveCellCount; i++)
        {
            string[] coordinates =
                lines[i + 3].Split(',');

            if (coordinates.Length != 2)
            {
                throw new FormatException(
                    $"Invalid coordinate: {lines[i + 3]}"
                );
            }

            int column =
                int.Parse(coordinates[0].Trim());

            int row =
                int.Parse(coordinates[1].Trim());

            // Make sure the coordinate is inside the universe.
            if (column < 0 ||
                column >= width ||
                row < 0 ||
                row >= height)
            {
                throw new FormatException(
                    $"Cell ({column},{row}) is outside the grid."
                );
            }

            data.LiveCells.Add((column, row));
        }

        return data;
    }
}