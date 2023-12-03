using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day1 : Day {
        private string input;
        public Day1() {
            input = ReadFile();
        }
        private string ReadFile() {
            return File.ReadAllText("days/day_1/input.txt");
        }
        public override int Part1() {
            // using regex for indexOf and lastIndexOf, since it seemed funny to do
            Regex reg = new Regex(@"^.*?(?=.*(\d))(\d)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(input);
            int sum = 0;
            foreach (Match match in matches) {
                GroupCollection group = match.Groups;
                sum += (group[2].Value[0] - '0') * 10 + (group[1].Value[0] - '0');
            }
            return sum;
        }
        public override int Part2() {
            Dictionary<string, int> dict = new Dictionary<string, int>{
                {"one",     1},
                {"two",     2},
                {"three",   3},
                {"four",    4},
                {"five",    5},
                {"six",     6},
                {"seven",   7},
                {"eight",   8},
                {"nine",    9}
            };
            // using regex again, cus I can
            // would've preferred to use a named capture group and reference its expression in the second one below, but I couldn't find a relevant option in .NET regex
            Regex reg = new Regex(@"^.*?(?=.*(\d|one|two|three|four|five|six|seven|eight|nine))(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(input);
            int sum = 0;
            foreach (Match match in matches) {
                GroupCollection group = match.Groups;
                int first = dict.ContainsKey(group[2].Value) ? dict[group[2].Value] : group[2].Value[0] - '0';
                int second = dict.ContainsKey(group[1].Value) ? dict[group[1].Value] : group[1].Value[0] - '0';
                sum += first * 10 + second;
            }
            return sum;
        }
    }
}