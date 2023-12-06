using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day6 : Day<long> {
        private List<int> time, distance; // part 1 data structures
        long longTime, longDistance; // part 2 data
        public Day6() {
            (time, distance, longTime, longDistance) = ParseInput();
        }
        private (List<int>, List<int>, long, long) ParseInput() {
            string[] input = File.ReadAllLines("days/day_6/input.txt");
            if (input.Length != 2) throw new FormatException("Expected input to be two lines");
            List<int> retTime = new List<int>(), retDistance = new List<int>();
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);
            StringBuilder lTime = new StringBuilder(), lDistance = new StringBuilder();
            foreach (Match match in regex.Matches(input[0])) {
                retTime.Add(Convert.ToInt32(match.Value));
                lTime.Append(match.Value);
            }
            foreach (Match match in regex.Matches(input[1])) {
                retDistance.Add(Convert.ToInt32(match.Value));
                lDistance.Append(match.Value);
            }
            if (retTime.Count != retDistance.Count) throw new ArgumentOutOfRangeException(nameof(retTime), "Expected lengths of retTime and retDistance to match");
            return (retTime, retDistance, Convert.ToInt64(lTime.ToString()), Convert.ToInt64(lDistance.ToString()));
        }
        public override long Part1() {
            int product = 1;
            for (int i = 0; i < time.Count; i++) {
                int count = 1 + time[i] % 2; // assuming always at least 1 is correst
                for (int j = time[i] / 2 - 1; j >= 0 && (time[i] - j) * j > distance[i]; j--) {
                    count += 2;
                }
                product *= count;
            }
            return product;
        }
        public override long Part2() {
            long count = 1 + longTime % 2; // assuming always at least 1 is correst
            for (long j = longTime / 2 - 1; j >= 0 && (longTime - j) * j > longDistance; j--) {
                count += 2;
            }
            return count;
        }
    }
}