namespace Day11
{
    public class Seat
    {
        public (int x, int y) Coords { get; set; }
        public State State { get; set; }
        public State NextState { get; set; }
    }

    public enum State
    {
        Undefined,
        Floor,
        Free,
        Taken
    }
}