using System;
using System.Collections.Generic;
using System.Linq;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var seats = System.IO.File.ReadAllLines("input.txt")
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(Seat.Parse)
                .OrderBy(s => s.Id)
                .ToList();

            var maxId = seats[seats.Count - 1].Id;
            Console.WriteLine($"Part 1: {maxId}");
            Console.WriteLine($"Part 2: {FindMissingIdWNeighbours(seats)}");
        }

        static int FindMissingIdWNeighbours(List<Seat> seats)
        {
            HashSet<int> existingIds = new HashSet<int>(seats.Select(s => s.Id));
            var firstSeat = new Seat { Row = 0, Column = 0 };
            var lastSeat = new Seat { Row = 127, Column = 7 };
            for(int i = firstSeat.Id; i <= lastSeat.Id; i++)
            {
                if(!existingIds.Contains(i) &&
                    existingIds.Contains(i - 1) &&
                    existingIds.Contains(i + 1))
                    return i;
            }

            return -1;
        }
    }

    class Seat
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Id => Row * 8 + Column;

        public override string ToString()
        {
            return $"R:{Row};C:{Column};I:{Id}";
        }

        public static Seat Parse(string input)
        {
            var result = new Seat();
            var rowMin = 0;
            var rowMax = 127;
            for (int i = 0; i < 6; i++)
            {
                var diff = rowMax - rowMin;
                if (input[i] == 'F')
                    rowMax -= (diff / 2) + 1;
                if (input[i] == 'B')
                    rowMin += (diff / 2) + 1;
            }

            result.Row = input[6] == 'F' ? rowMin : rowMax;

            var colMin = 0;
            var colMax = 7;
            for (int i = 7; i < 9; i++)
            {
                var diff = colMax - colMin;
                if (input[i] == 'L')
                    colMax -= (diff / 2) + 1;
                if (input[i] == 'R')
                    colMin += (diff / 2) + 1;
            }

            result.Column = input[9] == 'L' ? colMin : colMax;

            return result;
        }
    }
}
