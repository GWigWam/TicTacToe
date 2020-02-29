using System;
using System.Linq;
using System.Collections.Generic;

namespace TicTacToe
{
    public class TicTacToe
    {
        public delegate int PlayTurn(Board board);

        public const State PlayersMove = State.X;
        public const State PcsMove = State.O;

        public Board Board { get; private set; }

        private readonly PlayTurn _player;

        public TicTacToe(PlayTurn player)
        {
            Board = new Board();
            _player = player;
        }

        public void Run(bool playerStart)
        {
            var turn = playerStart ? Turn.Player : Turn.Pc;

            while (!Board.GameOver())
            {
                if (turn == Turn.Player)
                {
                    DoPlayerTurn();
                }
                else
                {
                    DoPcTurn();
                }
                turn = turn == Turn.Pc ? Turn.Player : Turn.Pc;
            }
        }

        private void DoPlayerTurn()
        {
            var move = _player(Board);
            Board[move] = PlayersMove;
        }

        private Random _rnd = new Random();
        private void DoPcTurn()
        {
            var move = Ai.GetMove(this, PcsMove);
            Board[move] = PcsMove;
        }

        private enum Turn
        {
            Player, Pc
        }

        private static class Ai
        {
            public static int GetMove(TicTacToe game, State player)
            {
                var scores = ScoreAllMoves(game.Board, player)
                    //.AsParallel()
                    .ToArray();

                var ordered = scores
                    .Where(t => t.score.HasValue)
                    .OrderByDescending(t => t.score.Value)
                    .ToArray();

                return ordered.Length > 0 ? ordered.First().move : throw new Exception("No valid moves found!");
            }

            private static IEnumerable<(int move, double? score)> ScoreAllMoves(Board board, State player) {
                for (int m = 0; m < 9; m++) {
                    yield return (m, ScoreMove(board, player, m));
                }
            }

            private static double? ScoreMove(Board board, State player, int move)
            {
                double? score = null;
                if (board[move] == State.Empty)
                {
                    var cBoard = board.Copy();
                    cBoard[move] = player;
                    var res = Simulate(cBoard, player, player.Inverse()).ToArray();

                    var byDepth = res.GroupBy(t => t.depth).OrderBy(g => g.Key);
                    var depthScores = byDepth.Select(g => g.Average(r => (double)r.res) / g.Key);
                    score = depthScores.Sum();
                }
                return score;
            }

            private static IEnumerable<(Result res, int depth)> Simulate(Board board, State player, State turn, int depth = 1)
            {
                if (board.GetWinner() is State w)
                {
                    yield return (w == player ? Result.Win : w == State.Empty ? Result.Draw : Result.Lose, depth);
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (board[i] == State.Empty)
                        {
                            var boardCopy = board.Copy();
                            boardCopy[i] = turn;

                            foreach (var res in Simulate(boardCopy, player, turn.Inverse(), depth + 1)) yield return res;
                        }
                    }
                }
            }

            enum Result : int { Lose = -1, Draw = 0, Win = 1 }
        }
    }
}
