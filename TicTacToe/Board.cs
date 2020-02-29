using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Board : IEnumerable<State>
    {
        private static readonly int[,] _wins = new int[,] {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
            { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        public State[] States { get; }

        public State this[int i] {
            get => States[i];
            set => States[i] = value;
        }

        public State this[int x, int y] {
            get => States[x + (y * 3)];
            set => States[x + (y * 3)] = value;
        }

        public Board()
        {
            States = new State[3 * 3];
        }

        public bool GameOver() => GetWinner().HasValue;

        public State? GetWinner()
        {
            for (int g = 0; g < 8; g++)
            {
                var ct = States[_wins[g, 0]];
                if (ct != State.Empty)
                {
                    if (States[_wins[g, 1]] == ct && States[_wins[g, 2]] == ct)
                    {
                        return ct;
                    }
                }
            }
            var isFull = !States.Any(s => s == State.Empty);
            return isFull ? (State?)State.Empty : null;
        }

        public Board Copy()
        {
            var res = new Board();
            States.CopyTo(res.States, 0);
            return res;
        }

        public IEnumerator<State> GetEnumerator() => States.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => States.GetEnumerator();
    }
}
