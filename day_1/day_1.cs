using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    class Day1 {
        public Day1() {
            string input = File.ReadAllText("day_1/input.txt");
            Console.WriteLine("Part 1 is " + Part1(input));
            Console.WriteLine("Part 2 is " + Part2(input));
        }
        private int Part1(string input) {
            // using regex for indexOf and lastIndexOf, since it seemed funny to do
            Regex reg = new Regex(@"^.*?(?=.*(\d))(\d)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(input);
            int sum = 0;
            foreach (Match match in matches) {
                GroupCollection group = match.Groups;
                sum += Convert.ToInt32(group[2].Value + group[1].Value);
            }
            return sum;
        }
        private int Part2(string input) {
            Dictionary<string, string> dict = new Dictionary<string, string>{{"one", "1"}, {"two", "2"}, {"three", "3"}, {"four", "4"}, {"five", "5"}, {"six", "6"}, {"seven", "7"}, {"eight", "8"}, {"nine", "9"}};
            // using regex again, cus I can
            // would've preferred to use a named capture group and reference its expression in the second one below, but I couldn't find a relevant option in .NET regex
            Regex reg = new Regex(@"^.*?(?=.*(\d|one|two|three|four|five|six|seven|eight|nine))(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(input);
            int sum = 0;
            foreach (Match match in matches) {
                GroupCollection group = match.Groups;
                string first = dict.ContainsKey(group[2].Value) ? dict[group[2].Value] : group[2].Value;
                string second = dict.ContainsKey(group[1].Value) ? dict[group[1].Value] : group[1].Value;
                sum += Convert.ToInt32(first + second);
            }
            return sum;
        }
    }
}