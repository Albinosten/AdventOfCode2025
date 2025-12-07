using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdventOfCode2025
{
    public class Day07
    {
        public int First(IList<string> input)
        {
            var teleporter = this.ParseInput(input);

            var beams = teleporter.Beam.ToHashSet();
            var beamSplits = 0;
            while(beams.Any())
            {
                var next = new HashSet<int>();
                foreach(var b in beams)
                {
                    var beamNextPos = GetNextPos(b, teleporter.xMax);
                    if(teleporter.BeamSplitters.Contains(beamNextPos))
                    {
                        beamSplits++;
                        foreach(var splitBeam in GetSplitBeam(b, teleporter.xMax))
                        {
                            next.Add(splitBeam);
                            teleporter.Beam.Add(splitBeam);
                        }
                    }
                    else if(beamNextPos / teleporter.xMax < teleporter.yMax)
                    {
                        next.Add(beamNextPos);
                        teleporter.Beam.Add(beamNextPos);
                    }

                }
                beams = next;

            }

            return beamSplits;
        }
        public long Second(IList<string> input)
        {
            var teleporter = this.ParseInput(input);

            var beams = teleporter.Beam.ToHashSet();
            var result = new Dictionary<int, long>
            {
                { beams.First(), 1 }
            };
            while(beams.Any())
            {
                var next = new HashSet<int>();
                foreach(var b in beams)
                {
                    var beamNextPos = GetNextPos(b, teleporter.xMax);
                
                    if(teleporter.BeamSplitters.Contains(beamNextPos))
                    {
                        foreach(var splitBeam in GetSplitBeam(b, teleporter.xMax))
                        {
                            UpdateResult(result, splitBeam, result[b]);
                            next.Add(splitBeam);
                        }
                    }
                    else if(beamNextPos < teleporter.yMax * teleporter.xMax)
                    {
                        UpdateResult(result, beamNextPos, result[b]);
                        next.Add(beamNextPos);
                    }

                }
                beams = next;
            }

            return result
                .Where(x => x.Key / teleporter.xMax == teleporter.yMax-1)
                .Select(x => x.Value)
                .Sum();
        }
        public static void UpdateResult(Dictionary<int, long> result, int key, long value)
        {
            result.TryAdd(key, 0);
            result[key] += value;
        }

        public int GetNextPos(int pos, int xMax)
        {
            return pos+xMax;
        }
        public IEnumerable<int> GetSplitBeam(int pos, int xMax)
        {
            if((pos % xMax) - 1 >= 0)
            {
                yield return GetNextPos(pos-1, xMax);
            }
            if((pos % xMax) + 1 <= xMax)
            {
                yield return GetNextPos(pos+1, xMax);
            }
        }
        public Teleporter ParseInput(IList<string> input)
        {
            var t = new Teleporter()
            {
                yMax = input.Count,
                xMax = input[0].Length,
            };
			for (int y = 0; y < input.Count; y++)
            {
                var row = input[y];
                for(int x = 0; x < row.Length; x++)
                {
                    if(row[x] == '^')
                    {
                        t.BeamSplitters.Add(GetPos(y,x,input[y].Length));
                    }

                    if(row[x] == 'S')
                    {
                        t.Beam.Add( GetNextPos(GetPos(y,x,input[y].Length), t.xMax));
                    }
                }
            } 
            return t;
        }
        private static int GetPos(int y, int x,  int xMax)
        {
            return y * xMax + x;
        }
        public class Teleporter
        {
            public Teleporter()
            {
                this.BeamSplitters = new HashSet<int>();
                this.Beam = new HashSet<int>();
            }
            public int xMax {get;set;}
            public int yMax {get;set;}
            public HashSet<int> BeamSplitters {get;set;}
            public HashSet<int> Beam {get;set;}

        }
    }
}