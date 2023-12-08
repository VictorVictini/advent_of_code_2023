namespace AdventOfCode2023 {
    public class Day8 : Day<long> {
        string steps;
        Dictionary<string, string[]> paths;
        List<string> starts;
        public Day8() {
            (steps, paths, starts) = ParseInput();
        }
        private (string, Dictionary<string, string[]>, List<string>) ParseInput() {
            string[] input = File.ReadAllLines("days/day_8/input.txt");
            Dictionary<string, string[]> retPaths = new Dictionary<string, string[]>();
            List<string> retStarts = new List<string>();
            for (int i = 2; i < input.Length; i++) {
                string currPath = input[i].Substring(0, 3);
                retPaths.Add(currPath, new string[]{input[i].Substring(7, 3), input[i].Substring(12, 3)});
                if (currPath[2] == 'A') retStarts.Add(currPath);
            }
            return (input[0], retPaths, retStarts);
        }
        private long CalculatePathLen(string start) {
            long count = 0;
            string next = start;
            for (int i = 0; next[2] != 'Z'; i++, count++) {
                if (i >= steps.Length) i = 0;
                next = steps[i] == 'L' ? paths[next][0] : paths[next][1];
            }
            return count;
        }
        public override long Part1() {
            return CalculatePathLen("AAA");
        }
        private long CalculateLCM(long a, long b) {
            return a / CalculateGCD(a, b) * b; // (a * b) / gcd(a, b)
        }
        private long CalculateGCD(long a, long b) {
            while (a % b > 0)  {
                long mod = a % b;
                a = b;
                b = mod;
            }
            return b;
        }
        public override long Part2() {
            long lcm = CalculatePathLen(starts[0]);
            for (int i = 1; i < starts.Count; i++) {
                lcm = CalculateLCM(lcm, CalculatePathLen(starts[i]));
            }
            return lcm;
        }
    }
}