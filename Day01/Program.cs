using System;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .ToList();

            for (int i = 0; i < lines.Count - 1; i++)
            {
                for (int y = i + 1; y < lines.Count; y++)
                {
                    if (lines[i] + lines[y] == 2020)
                    {
                        Console.WriteLine($"Part 1: {lines[i] * lines[y]}");
                    }
                    if (i < lines.Count - 2 && y < lines.Count - 3)
                    {
                        for (int z = y + 1; z < lines.Count; z++)
                        {
                            if (lines[i] + lines[y] + lines[z] == 2020)
                            {
                                Console.WriteLine($"Part 2: {lines[i] * lines[y] * lines[z]}");
                            }
                        }
                    }
                }
            }
        }
    }
}
