using System.Collections.Generic;

public class InputData
{
    public int Width { get; set; }

    public int Height { get; set; }

    public int Steps { get; set; }

    public List<(int Column, int Row)> LiveCells { get; set; }

    public InputData()
    {
        LiveCells = new List<(int Column, int Row)>();
    }
}