using System;
using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<long> preamble = new Queue<long>();

            var numbers = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => long.Parse(l.Trim())).ToList();

            for(int i = 0; i < 25; i++)
            {
                preamble.Enqueue(numbers[i]);
            }

            for(int i = 25; i < numbers.Count; i++)
            {
                if(!IsSumOfPrevious(numbers[i], preamble.ToList()))
                {
                    Console.WriteLine($"Part 1: {numbers[i]}");
                    var block = FindContinousBlockLeadingTo(numbers[i], i, numbers);
                    if(block.start == -1 || block.start == block.end)
                        continue;
                    
                    Console.WriteLine($"Part 2: {SumOfMinMax(block, numbers)}");

                    return;
                }

                preamble.Dequeue();
                preamble.Enqueue(numbers[i]);
            }
        }

        static long SumOfMinMax((int start, int end) block, List<long> numbers)
        {
            long min = long.MaxValue;
            long max = 0;
            for (int i = block.start; i <= block.end; i++)
            {
                if(numbers[i] < min)
                    min = numbers[i];
                if(numbers[i] > max)
                    max = numbers[i];
            }

            return min + max;
        }

        static (int start, int end) FindContinousBlockLeadingTo(long number, int numberIx, List<long> numbers)
        {
            for(int i = 0; i < numberIx - 1; i++)
            {
                long currentSum = numbers[i];
                for(int x = i + 1; x < numberIx; x++)
                {
                    currentSum += numbers[x];
                    if(currentSum == number)
                        return (i, x);

                    if(currentSum > number)
                        break;
                }
            }

            return (-1, -1);
        }

        static bool IsSumOfPrevious(long number, List<long> previous)
        {
            for(int x = 0; x < previous.Count - 1; x++)
            {
                for (int y = 1; y < previous.Count; y++)
                {
                    if(previous[x] + previous[y] == number)
                        return true;
                }
            }

            return false;
        }
    }
}
