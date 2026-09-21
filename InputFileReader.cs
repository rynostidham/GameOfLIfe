using System;
using System.IO;

public static class InputFileReader
{
    public static InputData Read(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 3)
        {
            throw new FormatException(
                "Input file does not contain enough information."
            );
        }

        string[] dimensions = lines[0].Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        if (dimensions.Length != 2)
        {
            throw new FormatException(
                "Line 1 must contain width and height."
            );
        }

        int width = int.Parse(dimensions[0]);
        int height = int.Parse(dimensions[1]);

        if (width <= 0 || height <= 0)
        {
            throw new FormatException(
                "Width and height must be positive."
            );
        }

        int steps = int.Parse(lines[1]);

        if (steps < 0)
        {
            throw new FormatException(
                "Simulation steps cannot be negative."
            );
        }

        int liveCellCount = int.Parse(lines[2]);

        if (liveCellCount < 0)
        {
            throw new FormatException(
                "Live cell count cannot be negative."
            );
        }

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

            data.LiveCells.Add((column, row));
        }

        return data;
    }
}