using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day5 : Day<long> {
        struct Path {
            public string next; // name of next path e.g. seed-to-soil with seed having "soil" as its next
            public long[] start, end; // source ranges, index corresponds to a given range
            public long[] change; // how much the source differs from the destination i.e. how much to add from source to get destination

            public Path(string next, int length) {
                this.next = next;

                // below have values added in parsing stage
                start = new long[length];
                end = new long[length];
                change = new long[length];
            }
        }
        private long[] seeds;
        private Dictionary<string, Path> paths;
        public Day5() {
            (seeds, paths) = ParseInput();
        }
        private (long[], Dictionary<string, Path>) ParseInput() {
            string[] input = File.ReadAllText("days/day_5/input.txt").Split("\n\r\n");
            
            string[] seedParse = input[0].Split(" ");
            long[] retSeeds = new long[seedParse.Length - 1];
            for (int i = 1; i < seedParse.Length; i++) {
                retSeeds[i - 1] = Convert.ToInt64(seedParse[i]);
            }

            Regex regex = new Regex(@"(\w+)-to-(\w+)");
            Dictionary<string, Path> retPaths = new Dictionary<string, Path>();
            for (int i = 1; i < input.Length; i++) {
                string[] section = input[i].Split("\n");
                GroupCollection group = regex.Match(section[0]).Groups;
                Path path = new Path(group[2].Value, section.Length - 1);
                for (int  j = 1; j < section.Length; j++) {
                    string[] splitNums = section[j].Split(" ");
                    long[] nums = new long[3];
                    for (int k = 0; k < splitNums.Length; k++) {
                        nums[k] = Convert.ToInt64(splitNums[k]);
                    }
                    path.start[j - 1] = nums[1];
                    path.end[j - 1] = nums[1] + nums[2] - 1;
                    path.change[j - 1] = nums[0] - nums[1];
                }
                retPaths.Add(group[1].Value, path);
            }

            return (retSeeds, retPaths);
        }
        // simple linear search
        private int Search(long find, long[] start, long[] end) {
            if (start.Length != end.Length) throw new ArgumentOutOfRangeException(nameof(start) + " " + nameof(end), "Expected lengths of start and end to match");
            for (int i = 0; i < start.Length; i++) {
                if (start[i] <= find && find <= end[i]) return i;
            }
            return -1;
        }
        public override long Part1() {
            long[] curr = new long[seeds.Length];
            for (int i = 0; i < seeds.Length; i++) {
                curr[i] = seeds[i]; // making a copy without referential logic applied
            }
            string nextPath = "seed";
            while (nextPath != "location") {
                Path currPath = paths[nextPath];
                for (int i = 0; i < curr.Length; i++) {
                    int index = Search(curr[i], currPath.start, currPath.end);
                    if (index == -1) continue;
                    curr[i] += currPath.change[index];
                }
                nextPath = currPath.next;
            }
            long min = Int32.MaxValue;
            for (int i = 0; i < curr.Length; i++) {
                if (curr[i] < min) min = curr[i];
            }
            return min;
        }
        public override long Part2() {
            // code here
            return Int64.MaxValue;
        }
    }
}