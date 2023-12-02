using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode2023;
namespace AdventOfCode2023 {
    public class Day2 : Day {
        enum Limits {
            red = 12,
            green = 13,
            blue = 14
        }

        // 2D array such that:
        // first dimension is for the amount of games
        // second dimension is for the number of 'turns' in a game
        // further values are given via a map, relating the colours to the values
        private Dictionary<string, int>[][] ParseInput() { // simplify after initial solution
            string[] input = File.ReadAllLines("days/day_2/input.txt");
            Dictionary<string, int>[][] games = new Dictionary<string, int>[input.Length][];
            Regex reg = new Regex(@"(\d+)\s+(red|green|blue)", RegexOptions.Compiled);
            for (int i = 0; i < input.Length; i++) {
                string[] currTurn = input[i].Split(";");
                Dictionary<string, int>[] turns = new Dictionary<string, int>[currTurn.Length];
                for (int j = 0; j < currTurn.Length; j++) {
                    MatchCollection matches = reg.Matches(currTurn[j]);
                    Dictionary<string, int> colours = new Dictionary<string, int>();
                    foreach (Match match in matches) {
                        GroupCollection group = match.Groups;
                        colours.Add(group[2].Value, Convert.ToInt32(group[1].Value));
                    }
                    turns[j] = colours;
                }
                games[i] = turns;
            }
            return games;
        }
        public override int Part1() {
            Dictionary<string, int>[][] games = ParseInput();
            int sum = 0;
            for (int i = 0; i < games.Length; i++) {
                bool valid = true;
                for (int j = 0; j < games[i].Length & valid; j++) {
                    if (games[i][j].ContainsKey("red") && games[i][j]["red"] > (int)Limits.red) valid = false;
                    if (games[i][j].ContainsKey("green") && games[i][j]["green"] > (int)Limits.green) valid = false;
                    if (games[i][j].ContainsKey("blue") && games[i][j]["blue"] > (int)Limits.blue) valid = false;
                }
                if (valid) sum += i + 1;
            }
            return sum;
        }
        public override int Part2() {
            Dictionary<string, int>[][] games = ParseInput();
            int sum = 0;
            for (int i = 0; i < games.Length; i++) {
                Dictionary<string, int> maxColours = new Dictionary<string, int>{
                    {"red", 0},
                    {"green", 0},
                    {"blue", 0}
                };
                for (int j = 0; j < games[i].Length; j++) {
                    if (games[i][j].ContainsKey("red") && games[i][j]["red"] > maxColours["red"]) maxColours["red"] = games[i][j]["red"];
                    if (games[i][j].ContainsKey("green") && games[i][j]["green"] > maxColours["green"]) maxColours["green"] = games[i][j]["green"];
                    if (games[i][j].ContainsKey("blue") && games[i][j]["blue"] > maxColours["blue"]) maxColours["blue"] = games[i][j]["blue"];
                }
                sum += maxColours["red"] * maxColours["green"] * maxColours["blue"];
            }
            return sum;
        }
    }
}