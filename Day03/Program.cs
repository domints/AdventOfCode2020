using System;
using System.Collections.Generic;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            var rows = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();
            var slope1x1 = CountTrees(rows, 1, 1);
            var slope3x1 = CountTrees(rows, 3, 1);
            var slope5x1 = CountTrees(rows, 5, 1);
            var slope7x1 = CountTrees(rows, 7, 1);
            var slope1x2 = CountTrees(rows, 1, 2);
            
            Console.WriteLine($"Part 1: {slope3x1}");
            Console.WriteLine($"Part 2: {slope1x1 * slope3x1 * slope5x1 * slope7x1 * slope1x2}");
        }

        public static int CountTrees(List<string> rows, int xV, int yV)
        {
            var width = rows[0].Length;
            var height = rows.Count;
            var currentRow = 0;
            var currentCol = 0;
            var treeCount = 0;
            while (currentRow < height)
            {
                if(rows[currentRow][currentCol] == '#')
                    treeCount++;
                currentCol += xV;
                while (currentCol >= width)
                    currentCol -= width;
                currentRow += yV;
            }

            return treeCount;
        }
    }
}
