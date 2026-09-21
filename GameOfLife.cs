public enum CellState
{
    Dead,
    Alive
}
/// Creates our 2D array of cells and contains the logic for the game of life.
public class GameOfLife
{
    private int width;
    private int height;

    private CellState[,] currentGrid;
    private CellState[,] nextGrid;

    public GameOfLife(int width, int height)
    {
        this.width = width;
        this.height = height;

        currentGrid = new CellState[height, width];
        nextGrid = new CellState[height, width];
    }
/// Sets all cells to alive 
    public void SetAlive(int column, int row)
    {
        currentGrid[row, column] = CellState.Alive;
    }
/// Loop for looking at all neighbors and counting in all directions how many are alive 
    private int CountAliveNeighbors(int row, int column)
    {
        int aliveNeighbors = 0;

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                /// Important to skip the cell counting itself as a neighbor
                if (rowOffset == 0 && columnOffset == 0)
                {
                    continue;
                }

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;
                /// Ensures that we do not go out of bounds of the grid when checking neighbors
                if (neighborRow >= 0 &&
                    neighborRow < height &&
                    neighborColumn >= 0 &&
                    neighborColumn < width)
                {
                    if (currentGrid[neighborRow, neighborColumn] == CellState.Alive)
                    {
                        aliveNeighbors++;
                    }
                }
            }
        }

        return aliveNeighbors;
    }
}