using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day4 : Day<int> {
        HashSet<int>[] winning;
        int[][] nums;
        public Day4() {
            (winning, nums) = ParseInput();
        }
        private (HashSet<int>[], int[][]) ParseInput() {
            string[] input = File.ReadAllLines("days/day_4/input.txt");
            Regex regex = new Regex(@"\d+(?!\S)", RegexOptions.Compiled);
            HashSet<int>[] wins = new HashSet<int>[input.Length];
            int[][] cards = new int[input.Length][];
            for (int i = 0; i < input.Length; i++) {
                string[] sections = input[i].Split("|");
                wins[i] = new HashSet<int>();
                foreach (Match match in regex.Matches(sections[0])) {
                    wins[i].Add(Convert.ToInt32(match.Value));
                }
                int j = 0;
                MatchCollection matches = regex.Matches(sections[1]);
                cards[i] = new int[matches.Count];
                foreach (Match match in matches) {
                    cards[i][j++] = Convert.ToInt32(match.Value);
                }
            }
            return (wins, cards);
        }
        public override int Part1() {
            int sum = 0;
            for (int i = 0; i < nums.Length; i++) {
                int curr = 0;
                for (int j = 0; j < nums[i].Length; j++) {
                    if (winning[i].Contains(nums[i][j])) curr = curr == 0 ? 1 : curr * 2;
                }
                sum += curr;
            }
            return sum;
        }
        public override int Part2() {
            int[] copies = new int[nums.Length];
            for (int i = 0; i < copies.Length; i++) {
                copies[i] = 1;
            }
            int sum = 0;
            for (int i = 0; i < nums.Length; i++) {
                int curr = 0;
                for (int j = 0; j < nums[i].Length; j++) {
                    if (winning[i].Contains(nums[i][j])) curr++;
                }
                for (int j = i + 1, c = 0; j < copies.Length && c < curr; j++, c++) {
                    copies[j] += copies[i];
                }
                sum += copies[i];
            }
            return sum;
        }
    }
}