namespace TicTacToe
{
    public enum State : byte
    {
        Empty, O, X
    }

    public static class StateExtensions
    {
        public static State Inverse(this State inp) => inp == State.Empty ? State.Empty : inp == State.X ? State.O : State.X;
    }
}
