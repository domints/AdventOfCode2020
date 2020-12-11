using System;
using System.Collections.Generic;
using System.Linq;

namespace Day11
{
    class Program
    {
        static readonly (int x, int y)[] _occupancyCheck = new [] {
            (-1, -1),
            (0, -1),
            (1, -1),
            (-1, 0),
            (1, 0),
            (-1, 1),
            (0, 1),
            (1, 1)
        };

        static Dictionary<(int x, int y), Seat> _seats = new Dictionary<(int x, int y), Seat>();
        static void Main(string[] args)
        {
            var seatLines = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l));
            ParseSeats(seatLines);
            if(_seats.Values.Any(s => s.State == State.Undefined))
                throw new ArgumentException("Something is wrong with file, I couldn't parse some seats.");

            ComputeSeats(CountOccupied, 4);

            Console.WriteLine($"Part 1: {GetSeats().Count(s => s.State == State.Taken)}");

            ParseSeats(seatLines);
            if(_seats.Values.Any(s => s.State == State.Undefined))
                throw new ArgumentException("Something is wrong with file, I couldn't parse some seats.");

            ComputeSeats(CountOccupiedV2, 5);

            Console.WriteLine($"Part 2: {GetSeats().Count(s => s.State == State.Taken)}");
        }

        static void ComputeSeats(Func<(int x, int y), int> checkFunc, int emptyThreshold)
        {
            var isUnstable = true;
            while(isUnstable)
            {
                foreach(var seat in GetSeats())
                {
                    var _aroundOccupancy = checkFunc(seat.Coords);
                    if(seat.State == State.Free && _aroundOccupancy == 0)
                    {
                        seat.NextState = State.Taken;
                    }
                    else if(seat.State == State.Taken && _aroundOccupancy >= emptyThreshold)
                    {
                        seat.NextState = State.Free;
                    }
                    else
                    {
                        seat.NextState = seat.State;
                    }
                }

                isUnstable = CommitState();
            }
        }

        /// <summary>
        /// Gets only seat (non-floor) blocks
        /// </summary>
        static IEnumerable<Seat> GetSeats()
        {
            return _seats.Values.Where(s => s.State != State.Floor);
        }

        static int CountOccupied((int x, int y) coords)
        {
            return _occupancyCheck
                .Select(c => (c.x + coords.x, c.y + coords.y))
                .Count(c => SeatTaken(c));
        }

        static int CountOccupiedV2((int x, int y) coords)
        {
            int occupied = 0;

            foreach(var oc in _occupancyCheck)
            {
                (int x, int y) currentCheck = (oc.x + coords.x, oc.y + coords.y);
                var foundState = State.Floor;
                while(foundState == State.Floor)
                {
                    if(_seats.TryGetValue(currentCheck, out Seat s))
                    {
                        foundState = s.State;
                    }
                    else
                    {
                        foundState = State.Undefined;
                    }

                    currentCheck = (currentCheck.x + oc.x, currentCheck.y + oc.y);
                }

                if(foundState == State.Taken) occupied++;
            }

            return occupied;
        }

        static bool SeatTaken((int x, int y) coords)
        {
            return _seats.TryGetValue((coords.x, coords.y), out Seat s)
                && s.State == State.Taken;
        }

        /// <summary>
        /// Commits the state of seats.
        /// </summary>
        /// <returns>True if any seat has changed this round.</returns>
        static bool CommitState()
        {
            bool isUnstable = false;
            foreach(var s in GetSeats())
            {
                if(s.State != s.NextState)
                    isUnstable = true;

                s.State = s.NextState;
                s.NextState = State.Undefined;
            }

            return isUnstable;
        }

        static void ParseSeats(IEnumerable<string> lines)
        {
            _seats.Clear();
            int y = 0;
            foreach(var l in lines)
            {
                int x = 0;
                foreach(var ch in l)
                {
                    var seat = new Seat{
                        Coords = (x, y),
                        NextState = State.Undefined
                    };

                    switch (ch)
                    {
                        case '.':
                            seat.State = State.Floor;
                            break;
                        case 'L':
                            seat.State = State.Free;
                            break;
                        case '#':
                            seat.State = State.Taken;
                            break;
                        default:
                            seat.State = State.Undefined;
                            break;
                    }

                    _seats.Add(seat.Coords, seat);

                    x++;
                }

                y++;
            }
        }
    }
}
