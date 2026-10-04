using System.Collections.Generic;

// Stores all information read from an input file.
public class InputData
{
    public int Width { get; set; }

    public int Height { get; set; }

    public int Steps { get; set; }

    // Stores the coordinates of all initially living cells.
    // Coordinates are stored as (column, row).
    public List<(int Column, int Row)> LiveCells { get; set; }

    public InputData()
    {
        LiveCells = new List<(int Column, int Row)>();
    }
}