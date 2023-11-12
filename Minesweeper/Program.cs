namespace Minesweeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game game = new Game();
                game.Play();
            }
        }
    }
}
