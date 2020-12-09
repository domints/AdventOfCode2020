using System;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(Password.Parse)
                .ToList();

            Console.Write("Part 1: ");
            Console.WriteLine(
                lines.Count(p => 
                    p.Pass.Count(c => c == p.RequiredChar) >= p.FirstNumber
                &&  p.Pass.Count(c => c == p.RequiredChar) <= p.SecondNumber));

            Console.Write("Part 2: ");
            Console.WriteLine(lines.Count(Part2Check));
        }

        private static bool Part2Check(Password p)
        {
            var fIx = p.FirstNumber - 1;
            var sIx = p.SecondNumber - 1;
            char? fCh = fIx < p.Pass.Length ? p.Pass[fIx] : null;
            char? sCh = sIx < p.Pass.Length ? p.Pass[sIx] : null;
            bool fC = fCh == p.RequiredChar;
            bool sC = sCh == p.RequiredChar;
            return fC ^ sC;
        }
    }

    class Password
    {
        public char RequiredChar { get; set; }
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public string Pass { get; set; }

        public static Password Parse(string input)
        {
            var parts = input.Trim().Split(' ');
            var counts = parts[0].Split('-');
            var result = new Password
            {
                FirstNumber = int.Parse(counts[0]),
                SecondNumber = int.Parse(counts[1]),
                RequiredChar = parts[1][0],
                Pass = parts[2]
            };
            return result;
        }
    }
}
