using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = System.IO.File.ReadAllLines("input.txt")
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(Instruction.Parse)
                .ToList();

            var lastResult = RunInstructions(instructions);
            Console.WriteLine($"Part 1: {lastResult.acc}");
            var nextChange = 0;
            while(!lastResult.success && nextChange < instructions.Count)
            {
                if(instructions[nextChange].Name == "acc")
                {
                    nextChange++;
                    continue;
                }

                var oldName = instructions[nextChange].Name;
                if(oldName == "jmp")
                {
                    instructions[nextChange].Name = "nop";
                }
                else if(oldName == "nop" && instructions[nextChange].Argument != 0)
                {
                    instructions[nextChange].Name = "jmp";
                }

                lastResult = RunInstructions(instructions);

                instructions[nextChange].Name = oldName;
                nextChange++;
            }

            Console.WriteLine($"Part 2: success: {lastResult.success}, acc: {lastResult.acc}");
        }

        static (int acc, bool success) RunInstructions(List<Instruction> instructions)
        {
            var accumulator = 0;
            var pointer = 0;
            HashSet<int> ranInstructions = new HashSet<int>();
            while(!ranInstructions.Contains(pointer))
            {
                ranInstructions.Add(pointer);
                var ci = instructions[pointer];
                switch(ci.Name)
                {
                    case "nop":
                        pointer++;
                        break;
                    case "acc":
                        accumulator += ci.Argument;
                        pointer++;
                        break;
                    case "jmp":
                        pointer += ci.Argument;
                        break;
                }

                if(pointer == instructions.Count)
                {
                    return (accumulator, true);
                }
            }

            return (accumulator, false);
        }
    }

    internal class Instruction
    {
        public string Name { get; set; }
        public int Argument { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Argument}";
        }

        public static Instruction Parse(string row)
        {
            var result = new Instruction();
            var split = row.Split(' ');
            result.Name = split[0];
            result.Argument = int.Parse(split[1].Substring(1));
            if(split[1].StartsWith('-'))
                result.Argument *= -1;

            return result;
        }
    }
}
