using System.Text.RegularExpressions;
using AdventOfCode2023;
namespace AdventOfCode2023 {
    public class Day2 : Day {
        enum Limits {
            red = 12,
            green = 13,
            blue = 14
        }

        // array of dictionaries, holding the highest value for each colour in a game
        private Dictionary<string, int>[] ParseInput() {
            string[] input = File.ReadAllLines("days/day_2/input.txt");
            Dictionary<string, int>[] games = new Dictionary<string, int>[input.Length];
            Regex reg = new Regex(@"(\d+)\s+(red|green|blue)", RegexOptions.Compiled);
            for (int i = 0; i < input.Length; i++) {
                MatchCollection matches = reg.Matches(input[i]);
                Dictionary<string, int> maxColours = new Dictionary<string, int>{
                    {"red", 0},
                    {"green", 0},
                    {"blue", 0}
                };
                foreach (Match match in matches) {
                    GroupCollection group = match.Groups;
                    int curr = Convert.ToInt32(group[1].Value);
                    if (curr > maxColours[group[2].Value]) maxColours[group[2].Value] = curr;
                }
                games[i] = maxColours;
            }
            return games;
        }
        public override int Part1() {
            Dictionary<string, int>[] games = ParseInput();
            int sum = 0;
            for (int i = 0; i < games.Length; i++) {
                bool valid = true;
                if (games[i]["red"] > (int)Limits.red) valid = false;
                if (games[i]["green"] > (int)Limits.green) valid = false;
                if (games[i]["blue"] > (int)Limits.blue) valid = false;
                if (valid) sum += i + 1;
            }
            return sum;
        }
        public override int Part2() {
            Dictionary<string, int>[] games = ParseInput();
            int sum = 0;
            for (int i = 0; i < games.Length; i++) {
                sum += games[i]["red"] * games[i]["green"] * games[i]["blue"];
            }
            return sum;
        }
    }
}