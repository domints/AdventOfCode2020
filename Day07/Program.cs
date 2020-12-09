using System;
using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var rows = System.IO.File.ReadAllLines("input.txt");
            var bags = GetBags(rows);
            MakeConnections(bags);
            var myBag = bags["shiny gold"];
            var possibleOutermostBags = new HashSet<string>();
            FindPossibleOutermostBags(myBag, possibleOutermostBags);
            Console.WriteLine($"Part 1: {possibleOutermostBags.Count}");

            var innerBagsCount = CountInnerBags(myBag);
            Console.WriteLine($"Part 2: {innerBagsCount}");
        }

        static Dictionary<string, Bag> GetBags(IEnumerable<string> rows)
        {
            Dictionary<string, Bag> result = new Dictionary<string, Bag>();

            foreach (var row in rows)
            {
                var bag = new Bag();
                var definition = row.Split("contain").Select(d => d.Trim()).ToArray();
                bag.Name = string.Join(' ', definition[0].Split(' ').Take(2));
                bag.ContainementDefinitions = definition[1].Split(',').Select(d => d.Trim().Trim('.')).ToList();
                result.Add(bag.Name, bag);
            }

            return result;
        }

        static void MakeConnections(Dictionary<string, Bag> bags)
        {
            foreach (var bag in bags.Values)
            {
                foreach (var def in bag.ContainementDefinitions)
                {
                    try
                    {
                        var splitDef = def.Split(' ');
                        if (splitDef[0] == "no")
                            continue;

                        var name = $"{splitDef[1]} {splitDef[2]}";
                        var count = int.Parse(splitDef[0]);
                        var containedBag = bags[name];
                        containedBag.ContainedIn.Add(bag);
                        bag.Contains.Add((count, containedBag));
                    }
                    catch
                    {
                        Console.WriteLine($"EXC! {def}");
                        throw;
                    }
                }
            }
        }

        static void FindPossibleOutermostBags(Bag currentBag, HashSet<string> possibilities)
        {
            foreach (var bag in currentBag.ContainedIn)
            {
                if (!possibilities.Contains(bag.Name))
                {
                    possibilities.Add(bag.Name);
                    FindPossibleOutermostBags(bag, possibilities);
                }
            }
        }

        static int CountInnerBags(Bag currentBag)
        {
            var count = 0;
            foreach (var bag in currentBag.Contains)
            {
                count += bag.count + (bag.count * CountInnerBags(bag.bag));
            }

            return count;
        }
    }

    class Bag
    {
        public Bag()
        {
            ContainedIn = new List<Bag>();
            ContainementDefinitions = new List<string>();
            Contains = new List<(int count, Bag bag)>();
        }

        public string Name { get; set; }
        public List<Bag> ContainedIn { get; set; }
        public List<string> ContainementDefinitions { get; set; }
        public List<(int count, Bag bag)> Contains { get; set; }
    }
}
