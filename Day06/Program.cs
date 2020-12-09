using System;
using System.Collections.Generic;
using System.Linq;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var rows = System.IO.File.ReadAllLines("input.txt");
            var groups = CountYesAnswersInGroups(rows);
            Console.WriteLine($"Part 1: {groups.Sum()}");
            var commonGroups = CountCommonYesAnswersInGroups(rows);
            Console.WriteLine($"Part 2: {commonGroups.Sum()}");
        }

        static List<int> CountCommonYesAnswersInGroups(IEnumerable<string> rows)
        {
            List<int> result = new List<int>();

            HashSet<char> currentGroup = new HashSet<char>();
            var firstInGroup = true;
            foreach (var r in rows)
            {
                if (string.IsNullOrWhiteSpace(r))
                {
                    result.Add(currentGroup.Count);
                    currentGroup = new HashSet<char>();
                    firstInGroup = true;
                    continue;
                }

                if (firstInGroup)
                {
                    foreach (var ch in r)
                    {
                        if (!currentGroup.Contains(ch))
                            currentGroup.Add(ch);
                    }
                }
                else 
                {
                    foreach(var ch in currentGroup)
                    {
                        if(!r.Contains(ch))
                            currentGroup.Remove(ch);
                    }
                }
                
                firstInGroup = false;
            }

            return result;
        }

        static List<int> CountYesAnswersInGroups(IEnumerable<string> rows)
        {
            List<int> result = new List<int>();

            HashSet<char> currentGroup = new HashSet<char>();
            foreach (var r in rows)
            {
                if (string.IsNullOrWhiteSpace(r))
                {
                    result.Add(currentGroup.Count);
                    currentGroup = new HashSet<char>();
                    continue;
                }

                foreach (var ch in r)
                {
                    if (!currentGroup.Contains(ch))
                        currentGroup.Add(ch);
                }
            }

            return result;
        }
    }
}
