namespace Minesweeper
{
    public class Game
    {
        private int flagCount;
        private int playerPositionX;
        private int playerPositionY;
        private Table table;
        public Game()
        {
            int difficultySelection = 0;
            while (difficultySelection == 0)
            {
                Console.Clear();
                Console.WriteLine("Select a difficulty");
                Console.WriteLine("1.Easy");
                Console.WriteLine("2.Medium");
                Console.WriteLine("3.Hard");
                Console.WriteLine();
                Console.WriteLine("4.Exit the game");
                try
                {
                    difficultySelection = int.Parse(Console.ReadLine());
                    if (difficultySelection < 0 || difficultySelection > 4)
                        difficultySelection = 0;
                }
                catch
                {
                    Console.WriteLine("Wrong input!");
                }
            }
            int sizeX = 0;
            int sizeY = 0;
            int numOfMines = 0;
            switch (difficultySelection)
            {
                case 1:
                    numOfMines = 10;
                    sizeX = 8;
                    sizeY = 8;
                    break;
                case 2:
                    numOfMines = 40;
                    sizeX = 15;
                    sizeY = 13;
                    break;
                case 3:
                    numOfMines = 99;
                    sizeX = 30;
                    sizeY = 16;
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            table = new Table(sizeX, sizeY, numOfMines);
            playerPositionX = 0;
            playerPositionY = 0;
            flagCount = numOfMines;
        }
        public void Play()
        {
            bool isFirstMove = true;
            bool isVictory = false;
            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                table.DrawTable(playerPositionX, playerPositionY);
                Console.WriteLine("FLAG COUNT: " + flagCount);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        Move(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        Move(1, 0);
                        break;
                    case ConsoleKey.UpArrow:
                        Move(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        Move(0, 1);
                        break;
                    case ConsoleKey.Spacebar:
                        if (!table.IsCellFlagged(playerPositionX, playerPositionY))
                        {
                            gameOver = table.CheckCell(playerPositionX, playerPositionY, isFirstMove);
                            isFirstMove = false;
                        }
                        break;
                    case ConsoleKey.F:
                        if ((flagCount > 0 || table.IsCellFlagged(playerPositionX, playerPositionY)) && !isFirstMove)
                            flagCount += table.FlagCell(playerPositionX, playerPositionY);
                        break;
                    default:
                        break;
                }
                if (CheckForWin())
                {
                    isVictory = true;
                    break;
                }
            }
            Console.Clear();
            table.DrawTable(playerPositionX, playerPositionY, true);
            if (isVictory)
                Console.WriteLine("WIN!");
            else
                Console.WriteLine("LOSS!");
            Console.ReadKey();
        }
        private void Move(int moveX, int moveY)
        {
            if (moveX == -1 && playerPositionX == 0) return;
            if (moveY == -1 && playerPositionY == 0) return;
            if (moveX == 1 && playerPositionX == table.SizeX - 1) return;
            if (moveY == 1 && playerPositionY == table.SizeY - 1) return;
            playerPositionX += moveX;
            playerPositionY += moveY;
        }
        private bool CheckForWin()
        {
            if (table.CountRevealedCells() == table.SizeY * table.SizeX - table.NumOfMines)
                return true;
            return false;
        }
    }
}
