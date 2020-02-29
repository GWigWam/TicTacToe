using System;
using System.Linq;

namespace TicTacToe
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Go first? (Y/N): ");
            var goFirst = "y" == $"{Console.ReadKey().KeyChar}".ToLower();
            Console.Clear();

            var game = new TicTacToe(PlayerTurn);
            game.Run(goFirst);

            Console.Clear();
            Console.WriteLine($"\n\t---Game over--- Winner: {game.Board.GetWinner()}");
            PrintBoard(game.Board);

            Console.ReadKey();
        }

        private static int PlayerTurn(Board board)
        {
            Console.Clear();
            PrintBoard(board);
            while (true)
            {
                Console.WriteLine("Move: ");
                int move = -1;
                var inp = Console.ReadLine();

                if (inp.Length == 1)
                {
                    int.TryParse(inp, out move);
                }
                else if (inp.Length == 2)
                {
                    if (int.TryParse($"{inp[0]}", out var c0) && int.TryParse($"{inp[1]}", out var c1))
                    {
                        if (c0 > 0 && c0 <= 3 && c1 > 0 && c1 <= 3)
                        {
                            move = (c0 - 1) + ((c1 - 1) * 3);
                        }
                    }
                }

                if (move >= 0 && move <= 8 && board[move] == State.Empty)
                {
                    return move;
                }

                Console.WriteLine("INVALID");
            }
        }

        private static void PrintBoard(Board board)
        {
            string StateStr(State s) => s == State.Empty ? "-" : s == State.O ? "O" : "X";

            Console.WriteLine();
            for (int y = 0; y < 3; y++)
            {
                Console.Write("\t");
                for (int x = 0; x < 3; x++)
                {
                    var c = board[x + (y * 3)];
                    Console.Write($"{StateStr(c)} ");
                }
                Console.WriteLine();
            }
        }
    }
}
