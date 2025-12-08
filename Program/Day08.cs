namespace AdventOfCode2025
{
    public class Day08
    {
       /*
        Yes, my datastructures look like monsters..
       */
        public int First(IList<string> input, int maxNumberOfConnections)
        {
            var distances = this.GetDistances(this.ParseInput(input));
            var circuitHolder = new Dictionary<(int x, int y, int z), int>();
            for(int i = 0; i<maxNumberOfConnections;i++)
            {
                var distance = distances.Dequeue();
                this.Updatecircuits(circuitHolder,distance);
            }
            var numbers = circuitHolder
                .GroupBy(x=>x.Value)
                .Select(x=>x.Count())
                .OrderByDescending(x => x)
                .Take(3)
                .ToList();
            return numbers
                .Aggregate((x,y) => x*y);
        }

        public long Second(IList<string> input, int maxNumberOfConnections)
        {
            var distances = this.GetDistances(this.ParseInput(input));
            var circuitHolder = new Dictionary<(int x, int y, int z), int>();
            for(int i = 0; i < distances.Count;i++)
            {
                var distance = distances.Dequeue();
                this.Updatecircuits(circuitHolder, distance);
                if(circuitHolder.Count == maxNumberOfConnections)
                {
                    return (long)distance.pos.pos1.x * distance.pos.pos2.x;
                }
            }
            throw new Exception("No result");
        }
        
        public long GetDistance((int x, int y, int z)pos1, (int x, int y, int z)pos2)
        {
            long dX = pos1.x-pos2.x;
            long dY = pos1.y-pos2.y;
            long dZ = pos1.z-pos2.z;
            return dX*dX+dY*dY+dZ*dZ;//No Math.Sqrt needed since distance relations stay the same
        }


        public void Updatecircuits(Dictionary<(int x, int y, int z), int>circuitHolder
        , (long distance, ((int x, int y, int z)pos1, (int x, int y, int z)pos2))distance
        )
        {
            if(circuitHolder.TryGetValue(distance.Item2.pos1, out int cn1)
             && circuitHolder.TryGetValue(distance.Item2.pos2, out int cn2))//both are connected
            {
                if(cn1 != cn2)
                {
                    var circuitsToJoin = circuitHolder
                        .Where(x => x.Value == cn2)
                        .Select(x => x.Key)
                        .ToList()
                        ;
                    foreach(var circuitKey in circuitsToJoin)
                    {
                        circuitHolder[circuitKey] = cn1;
                    }   
                }
            }
            else if(!circuitHolder.ContainsKey(distance.Item2.pos1)
                && !circuitHolder.ContainsKey(distance.Item2.pos2)
            )//none connected to circuit
            {
                circuitHolder.Add(distance.Item2.pos1, (int)distance.distance);
                circuitHolder.Add(distance.Item2.pos2, (int)distance.distance);
            }
            else if(circuitHolder.ContainsKey(distance.Item2.pos1)
                && !circuitHolder.ContainsKey(distance.Item2.pos2)
            )//first connected to circuit second is not
            {
                var cn = circuitHolder[distance.Item2.pos1];
                circuitHolder.Add(distance.Item2.pos2, cn);
            }
            else if(!circuitHolder.ContainsKey(distance.Item2.pos1)
                && circuitHolder.ContainsKey(distance.Item2.pos2)
            )//second connected to circuit first is not
            {
                var cn = circuitHolder[distance.Item2.pos2];
                circuitHolder.Add(distance.Item2.pos1, cn);
            }
        }
        public PriorityQueue<(long distance, ((int x, int y, int z)pos1, (int x, int y, int z)pos2)pos),long> GetDistances(IList<(int x, int y, int z)> junctions)
        {
            var priorityQueue = new PriorityQueue<(long distance, ((int x, int y, int z)pos1, (int x, int y, int z)pos2)pos),long>();
            for(int i = 0; i<junctions.Count;i++)
            {
                for(int j = i+1; j < junctions.Count;j++)
                {
                    var first = junctions[i];
                    var second = junctions[j];
                    var distance = GetDistance(first,second);
                    priorityQueue.Enqueue((distance, (first,second)), distance);
                } 
            }
            
            return priorityQueue;
        }
        public IList<(int x, int y, int z)> ParseInput(IList<string> input)
        {
			var values = new List<(int x, int y, int z)>();

			foreach (var line in input)
            {
                var pos = line
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
                values.Add((pos[0],pos[1],pos[2]));
            }
            return values;
        }
    }
}