namespace Minesweeper
{
    public class Cell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsFlagged { get; set; }
        public Cell()
        {
            IsMine = false;
            IsRevealed = false;
            AdjacentMines = 0;
        }
        public int CountAdjacentMines(Cell[,] cells, int sizeY, int sizeX, int coordinateX, int coordinateY)
        {
            int count = 0;
            for (int i = Math.Max(0, coordinateX - 1); i < Math.Min(coordinateX + 2, sizeY); i++)
            {
                for (int j = Math.Max(0, coordinateY - 1); j < Math.Min(coordinateY + 2, sizeX); j++)
                {
                    if ((i, j) != (coordinateX, coordinateY) && cells[i, j].IsMine)
                        count++;
                }
            }
            return count;
        }
    }
}
