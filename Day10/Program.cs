using System;
using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => long.Parse(l.Trim())).ToList();

            var ordered = numbers.OrderBy(n => n).ToList();
            var t3Count = 1 + (ordered[0] == 3 ? 1 : 0);
            var t1Count = 0 + (ordered[0] == 1 ? 1 : 0);
            for(int i = 1; i < ordered.Count; i++)
            {
                if(ordered[i] - ordered[i-1] == 3)
                    t3Count++;
                if(ordered[i] - ordered[i-1] == 1)
                    t1Count++;
            }
            Console.WriteLine($"Part 1: {t1Count} * {t3Count} =  {t1Count * t3Count}");

            var adapters = ordered.ToDictionary(a => a, a => new Adapter { Joltage = a });
            var deviceValue = ordered[^1] + 3;
            adapters.Add(deviceValue, new Adapter { Joltage = deviceValue, IsDevice = true });
            var socket = new Adapter { Joltage = 0 };
            AddConnectables(socket, adapters);
            foreach(var adapter in adapters.Values)
            {
                AddConnectables(adapter, adapters);
            }

            
            adapters.Add(socket.Joltage, socket);

            /*
            // VERY terrible solution, essentially checking every possible path. 
            // Good if you have a lot of time :D
            long pathCount = 0;
            FindPathsToDevice(socket, ref pathCount);
            */

            // Waaay better solution
            var adapterList = adapters.Values.OrderBy(a => a.Joltage).ToList();
            Dictionary<long, long> counts = new Dictionary<long, long>
            {
                [deviceValue] = 1
            };

            for (int i = adapterList.Count - 2; i >= 0; i--)
            {
                long connections = 0;
                foreach(var connectedAdapter in adapterList[i].Connectable)
                {
                    connections += counts[connectedAdapter.Joltage];
                }

                counts[adapterList[i].Joltage] = connections;
            }


            long pathCount = counts[0];
            Console.WriteLine($"Part 2: {pathCount}");
            
        }

        static void FindPathsToDevice(Adapter currentAdapter, ref long pathCount)
        {
            foreach(var adapter in currentAdapter.Connectable)
            {
                if(adapter.IsDevice)
                {
                    pathCount++;
                }
                else
                {
                    FindPathsToDevice(adapter, ref pathCount);
                }
            }
        }

        static void AddConnectables(Adapter adapter, Dictionary<long, Adapter> adapters)
        {
            AddIfExists(adapter.Joltage + 1, adapter.Connectable, adapters);
            AddIfExists(adapter.Joltage + 2, adapter.Connectable, adapters);
            AddIfExists(adapter.Joltage + 3, adapter.Connectable, adapters);
        }

        static void AddIfExists(long key, List<Adapter> target, Dictionary<long, Adapter> adapters)
        {
            if(adapters.ContainsKey(key))
                target.Add(adapters[key]);
        }
    }

    class Adapter
    {
        public Adapter()
        {
            Connectable = new List<Adapter>();
        }
        public long Joltage { get; set; }
        public bool IsDevice { get; set; }
        public List<Adapter> Connectable { get; set; }

        public override string ToString()
        {
            return $"A:{Joltage}, D:{IsDevice}, {Connectable.Count} children";
        }
    }
}
