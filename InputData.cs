using System.Collections.Generic;
// Stores all information for input file 
public class InputData
{
    public int Width { get; set; }

    public int Height { get; set; }

    public int Steps { get; set; }
// Stores a live cell as column and row 
    public List<(int Column, int Row)> LiveCells { get; set; }

    public InputData()
    {
        LiveCells = new List<(int Column, int Row)>();
    }
}