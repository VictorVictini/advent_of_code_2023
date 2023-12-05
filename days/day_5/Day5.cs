using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day5 : Day<long> {
        struct Path {
            public string next; // name of next path e.g. seed-to-soil with seed having "soil" as its next
            public List<long> start, end; // source ranges, index corresponds to a given range
            public List<long> change; // how much the source differs from the destination i.e. how much to add from source to get destination

            public Path(string next) {
                this.next = next;

                // below have values added in parsing stage
                start = new List<long>();
                end = new List<long>();
                change = new List<long>();
            }
        }
        private long[] seeds;
        private Dictionary<string, Path> paths;
        public Day5() {
            (seeds, paths) = ParseInput();
        }
        private int FindInsert(long find, List<long> list) {
            int first = 0, last = list.Count - 1;
            while (first <= last) {
                int mid = (first + last) / 2;
                if (find >= list[mid] && (mid + 1 >= list.Count || find <= list[mid + 1])) {
                    return mid + 1;
                } else if (list[mid] > find) {
                    last = mid - 1;
                } else {
                    first = mid + 1;
                }
            }
            return 0;
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
                Path path = new Path(group[2].Value);
                for (int  j = 1; j < section.Length; j++) {
                    string[] splitNums = section[j].Split(" ");
                    long[] nums = new long[3];
                    for (int k = 0; k < splitNums.Length; k++) {
                        nums[k] = Convert.ToInt64(splitNums[k]);
                    }
                    int index = FindInsert(nums[1], path.start);
                    path.start.Insert(index, nums[1]);
                    path.end.Insert(index, nums[1] + nums[2] - 1);
                    path.change.Insert(index, nums[0] - nums[1]);
                }
                retPaths.Add(group[1].Value, path);
            }

            return (retSeeds, retPaths);
        }
        // simple binary search
        private int BinarySearch(long find, List<long> start, List<long> end) {
            if (start.Count != end.Count) throw new ArgumentOutOfRangeException(nameof(start), "Expected lengths of start and end to match");
            int first = 0, last = start.Count - 1;
            while (first <= last) {
                int mid = (first + last) / 2;
                if (start[mid] <= find && find <= end[mid]) {
                    return mid;
                } else if (start[mid] > find) {
                    last = mid - 1;
                } else {
                    first = mid + 1;
                }
            }
            return -1;
        }
        private int RangeBinarySearch(long find, List<long> start, List<long> end) {
            if (start.Count != end.Count) throw new ArgumentOutOfRangeException(nameof(start), "Expected lengths of start and end to match");
            int first = 0, last = start.Count - 1;
            while (first <= last) {
                int mid = (first + last) / 2;
                if (start[mid] <= find && find <= end[mid]) {
                    return mid;
                } else if (start[mid] > find) {
                    last = mid - 1;
                } else {
                    first = mid + 1;
                }
            }
            first = 0;
            last = start.Count - 1;
            while (first <= last) {
                int mid = (first + last) / 2;
                if ((mid - 1 < 0 || end[mid - 1] <= find) && find <= start[mid]) {
                    return mid;
                } else if (start[mid] > find) {
                    last = mid - 1;
                } else {
                    first = mid + 1;
                }
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
                    int index = BinarySearch(curr[i], currPath.start, currPath.end);
                    if (index == -1) continue;
                    curr[i] += currPath.change[index];
                }
                nextPath = currPath.next;
            }
            long min = Int64.MaxValue;
            for (int i = 0; i < curr.Length; i++) {
                if (curr[i] < min) min = curr[i];
            }
            return min;
        }
        // assumes start and end are sorted in non-descending order
        private (List<long>, List<long>) MergeRanges(List<long> start, List<long> end) {
            if (start.Count != end.Count) throw new ArgumentOutOfRangeException(nameof(start), "Expected lengths of start and end to match");
            List<long> retStart = new List<long>{start[0]}, retEnd = new List<long>{end[0]};
            for (int i = 1; i < start.Count; i++) {
                if (start[i] <= end[i - 1]) {
                    if (end[i] > retEnd[retEnd.Count - 1]) retEnd[retEnd.Count - 1] = end[i];
                } else {
                    retStart.Add(start[i]);
                    retEnd.Add(end[i]);
                }
            }
            return (retStart, retEnd);
        }
        public override long Part2() {
            // format the seeds input into a bunch of ranges, sorted in non-descending order of start
            List<long> start = new List<long>(), end = new List<long>();
            for (int i = 0; i < seeds.Length; i += 2) {
                int index = FindInsert(seeds[i], start);
                start.Insert(index, seeds[i]);
                end.Insert(index, seeds[i] + seeds[i + 1] - 1);
            }

            string nextPath = "seed";
            while (nextPath != "location") {
                Path currPath = paths[nextPath];

                // rewrites all ranges into new ranges with changes applied when needed
                List<long> nextStart = new List<long>(), nextEnd = new List<long>();
                for (int i = 0; i < start.Count; i++) {
                    int index = RangeBinarySearch(start[i], currPath.start, currPath.end);
                    
                    // if the start value is outwith all the ranges for having a change applied
                    if (index == -1) {
                        int j = FindInsert(start[i], nextStart);
                        nextStart.Insert(j, start[i]);
                        
                        // if the whole range start[i] and end[i] is outwith all source ranges, insert it and proceed to the next iteration
                        if (end[i] < currPath.start[0] || start[i] > currPath.end[currPath.end.Count - 1]) {
                            nextEnd.Insert(j, end[i]);
                            continue;
                        }

                        // if the end is contained in one of the source ranges, add everything up to (but not including) the smallest range
                        nextEnd.Insert(j, currPath.start[0] - 1);
                        start[i] = currPath.start[0];
                        index = 0;
                    }
                    
                    // if the start is within a 'gap' e.g. 13-15 in [1, 5, 16] [4, 12, 20]
                    if (start[i] < currPath.start[index]) {
                        int j = FindInsert(start[i], nextStart);
                        nextStart.Insert(j, start[i]);
                        if (end[i] < currPath.start[index]) {
                            nextEnd.Insert(j, end[i]);
                            continue;
                        }
                        nextEnd.Insert(j, currPath.start[index] - 1);
                        start[i] = currPath.start[index];
                    }

                    // if the end is greater than the largest source end, cut and replace as necessary
                    if (end[i] > currPath.end[currPath.end.Count - 1]) {
                        long startVal = currPath.end[currPath.end.Count - 1] + 1;
                        int j = FindInsert(startVal, nextStart);
                        nextStart.Insert(j, startVal);
                        nextEnd.Insert(j, end[i]);
                        end[i] = startVal - 1;
                    }

                    // now we should be provided the index to start with, and the start[i] and end[i] values should all be within the expected ranges
                    for (int j = index; j < currPath.end.Count; j++) {
                        long startVal = start[i] + currPath.change[j];
                        int k = FindInsert(startVal, nextStart);
                        nextStart.Insert(k, startVal);
                        if (end[i] <= currPath.end[j]) {
                            nextEnd.Insert(k, end[i] + currPath.change[j]);
                            break;
                        }
                        nextEnd.Insert(k, currPath.end[j] + currPath.change[j]);
                        start[i] = currPath.end[j] + 1;
                        
                        // check for stuff in-between the 
                        if (currPath.end[j] + 1 < currPath.start[j + 1]) {
                            k = FindInsert(start[i], nextStart);
                            nextStart.Insert(k, start[i]);
                            if (end[i] < currPath.start[j + 1]) {
                                nextEnd.Insert(k, end[i]);
                                break;
                            } else {
                                nextEnd.Insert(k, currPath.start[j + 1] - 1);
                                start[i] = currPath.start[j + 1];
                            }
                        }
                    }
                }
                
                // setting up next iteration, merging a few ranges in the list as necessary
                nextPath = currPath.next;
                (start, end) = MergeRanges(nextStart, nextEnd);
            }
            long min = Int64.MaxValue;
            foreach (long value in start) {
                if (value < min) min = value;
            }
            return min;
        }
    }
}