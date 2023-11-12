using System.Drawing;

namespace Minesweeper
{
    public class Table
    {
        private int sizeX;
        private int sizeY;
        private int numOfMines;
        private Cell[,] cells;
        public int SizeX { get => sizeX; }
        public int SizeY { get => sizeY; }
        public int NumOfMines { get => numOfMines; }

        public Table(int sizeX, int sizeY, int numOfMines)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.numOfMines = numOfMines;
            cells = new Cell[sizeY, sizeX];
            InitializeCells();
            GenerateMines();
            CalculateAllAdjacentMines();
        }
        private void InitializeCells()
        {
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    cells[i, j] = new Cell();
                }
            }
        }
        private void GenerateMines()
        {
            Random rand = new Random();
            int minesPlaced = 0;
            if (sizeX * sizeY < numOfMines)
            {
                double mineRatio = 0.15;
                numOfMines = (int)((sizeX * sizeY) * mineRatio);
            }
            while (minesPlaced < numOfMines)
            {
                int row = rand.Next(0, sizeY);
                int col = rand.Next(0, SizeX);

                if (!cells[row, col].IsMine)
                {
                    cells[row, col].IsMine = true;
                    minesPlaced++;
                }
            }
        }
        public void DrawTable(int currentX, int currentY, bool revealAllMines = false)
        {
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    Console.BackgroundColor = (i == currentY && j == currentX) ? ConsoleColor.DarkYellow : cells[i, j].IsRevealed ? ConsoleColor.White : ConsoleColor.Gray;
                    SetColor(i, j);
                    if (cells[i, j].IsFlagged)
                        Console.Write("F ");
                    else if (cells[i, j].IsRevealed && !cells[i, j].IsMine)
                        Console.Write(cells[i, j].AdjacentMines == 0 ? "  " : cells[i, j].AdjacentMines.ToString() + " ");
                    else
                        Console.Write(cells[i, j].IsMine && revealAllMines == true ? "X " : "  ");
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }
        private void SetColor(int coordinateY, int coordinateX)
        {
            if (cells[coordinateY, coordinateX].IsMine || cells[coordinateY, coordinateX].IsFlagged)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                return;
            }
            switch (cells[coordinateY, coordinateX].AdjacentMines)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }
        private void CalculateAllAdjacentMines()
        {
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    cells[i, j].AdjacentMines = cells[i, j].CountAdjacentMines(cells, sizeY, SizeX, i, j);
                }
            }
        }
        public bool CheckCell(int coordinateX, int coordinateY, bool isFirstMove)
        {
            if (isFirstMove)
            {
                while (cells[coordinateY, coordinateX].IsMine || cells[coordinateY, coordinateX].AdjacentMines != 0)
                {
                    cells = new Cell[sizeY, sizeX];
                    InitializeCells();
                    GenerateMines();
                    CalculateAllAdjacentMines();
                }
            }
            if (cells[coordinateY, coordinateX].IsMine)
            {
                RevealAllCells();
                return true;
            }
            cells[coordinateY, coordinateX].IsRevealed = true;
            if (cells[coordinateY, coordinateX].AdjacentMines == 0)
            {
                bool[,] visited = new bool[sizeY, SizeX];
                FloodFill(coordinateX, coordinateY, visited);
            }
            return false;
        }
        private void FloodFill(int x, int y, bool[,] visited)
        {
            if (x < 0 || y < 0 || x >= sizeX || y >= sizeY || visited[y, x])
                return;
            if (cells[y, x].AdjacentMines != 0)
            {
                cells[y, x].IsRevealed = true;
                return;
            }
            visited[y, x] = true;
            cells[y, x].IsRevealed = true;
            FloodFill(x - 1, y, visited);
            FloodFill(x + 1, y, visited);
            FloodFill(x, y - 1, visited);
            FloodFill(x, y + 1, visited);
            FloodFill(x - 1, y - 1, visited);
            FloodFill(x + 1, y + 1, visited);
            FloodFill(x + 1, y - 1, visited);
            FloodFill(x - 1, y + 1, visited);
        }
        private void RevealAllCells()
        {
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    cells[i, j].IsRevealed = true;
                }
            }
        }
        public int CountRevealedCells()
        {
            int revealedCellsCount = 0;
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    if (cells[i, j].IsRevealed)
                        revealedCellsCount++;
                }
            }
            return revealedCellsCount;
        }
        public int FlagCell(int coordinateX, int coordinateY)
        {
            if (cells[coordinateY, coordinateX].IsRevealed && !cells[coordinateY, coordinateX].IsFlagged)
                return 0;
            if (cells[coordinateY, coordinateX].IsFlagged)
            {
                cells[coordinateY, coordinateX].IsFlagged = false;
                return 1;
            }
            cells[coordinateY, coordinateX].IsFlagged = true;
            return -1;
        }
        public bool IsCellFlagged(int coordinateX, int coordinateY) => cells[coordinateY, coordinateX].IsFlagged;
    }
}
