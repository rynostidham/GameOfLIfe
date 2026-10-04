using System;
using System.IO;
using System.Text;

public class GameOfLife
{
    private int width;
    private int height;

    // Double buffering:
    // currentGrid is read while nextGrid receives the results.
    private CellState[,] currentGrid;
    private CellState[,] nextGrid;

    public GameOfLife(int width, int height)
    {
        this.width = width;
        this.height = height;

        // C# 2D arrays are accessed using [row, column].
        currentGrid = new CellState[height, width];
        nextGrid = new CellState[height, width];
    }

    public void SetAlive(int column, int row)
    {
        if (row < 0 ||
            row >= height ||
            column < 0 ||
            column >= width)
        {
            throw new ArgumentOutOfRangeException(
                $"Cell ({column},{row}) is outside the grid."
            );
        }

        // Input coordinates use column,row.
        // The array uses [row,column].
        currentGrid[row, column] = CellState.Alive;
    }

    private int CountAliveNeighbors(int row, int column)
    {
        int aliveNeighbors = 0;

        // Use offsets from -1 through 1 to check
        // every surrounding position.
        for (int rowOffset = -1;
             rowOffset <= 1;
             rowOffset++)
        {
            for (int columnOffset = -1;
                 columnOffset <= 1;
                 columnOffset++)
            {
                // (0,0) represents the current cell,
                // so it should not count as its own neighbor.
                if (rowOffset == 0 &&
                    columnOffset == 0)
                {
                    continue;
                }

                int neighborRow =
                    row + rowOffset;

                int neighborColumn =
                    column + columnOffset;

                // Ignore coordinates outside the universe.
                // Out-of-bounds cells are treated as dead.
                if (neighborRow >= 0 &&
                    neighborRow < height &&
                    neighborColumn >= 0 &&
                    neighborColumn < width)
                {
                    if (currentGrid[
                            neighborRow,
                            neighborColumn
                        ] == CellState.Alive)
                    {
                        aliveNeighbors++;
                    }
                }
            }
        }

        return aliveNeighbors;
    }

    public void NextGeneration()
    {
        // The dense implementation must inspect
        // every position in the universe.
        for (int row = 0; row < height; row++)
        {
            for (int column = 0;
                 column < width;
                 column++)
            {
                int aliveNeighbors =
                    CountAliveNeighbors(row, column);

                if (currentGrid[row, column]
                    == CellState.Alive)
                {
                    // A living cell survives with
                    // exactly 2 or 3 neighbors.
                    if (aliveNeighbors == 2 ||
                        aliveNeighbors == 3)
                    {
                        nextGrid[row, column] =
                            CellState.Alive;
                    }
                    else
                    {
                        nextGrid[row, column] =
                            CellState.Dead;
                    }
                }
                else
                {
                    // A dead cell becomes alive
                    // with exactly 3 neighbors.
                    if (aliveNeighbors == 3)
                    {
                        nextGrid[row, column] =
                            CellState.Alive;
                    }
                    else
                    {
                        nextGrid[row, column] =
                            CellState.Dead;
                    }
                }
            }
        }

        // Swap the two buffers instead of copying
        // the entire next generation.
        CellState[,] temp = currentGrid;

        currentGrid = nextGrid;
        nextGrid = temp;
    }

    public void Run(int steps, bool graphics)
    {
        if (graphics)
        {
            Console.Clear();
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
                if (currentGrid[row, column]
                    == CellState.Alive)
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
        Console.Write(display.ToString());
    }

    // Used by the Part 4 comparison benchmark.
    public int GetLiveCellCount()
    {
        int count = 0;

        for (int row = 0;
             row < height;
             row++)
        {
            for (int column = 0;
                 column < width;
                 column++)
            {
                if (currentGrid[row, column]
                    == CellState.Alive)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void WriteOutput(
        string filePath,
        int steps)
    {
        int liveCellCount = 0;

        StringBuilder liveCells =
            new StringBuilder();

        // Find all living cells in the final generation.
        for (int row = 0;
             row < height;
             row++)
        {
            for (int column = 0;
                 column < width;
                 column++)
            {
                if (currentGrid[row, column]
                    == CellState.Alive)
                {
                    liveCellCount++;

                    liveCells.AppendLine(
                        $"{column},{row}"
                    );
                }
            }
        }

        StringBuilder output =
            new StringBuilder();

        output.AppendLine(
            $"{width},{height}"
        );

        output.AppendLine(
            steps.ToString()
        );

        output.AppendLine(
            liveCellCount.ToString()
        );

        output.Append(liveCells);

        string? directory =
            Path.GetDirectoryName(
                Path.GetFullPath(filePath)
            );

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(
            filePath,
            output.ToString()
        );
    }
}