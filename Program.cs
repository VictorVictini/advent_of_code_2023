namespace AdventOfCode2023 {
    public class Program {
        private static void Main() {
            int dayNum = WithinLimit(2);
            switch (dayNum) {
                case 1:
                    new Day1().OutputParts();
                    break;
                case 2:
                    new Day2().OutputParts();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayNum), "Encountered a day not in the switch statement");
            }
        }

        private static int WithinLimit(int end) {
            Console.WriteLine("Please enter a day (1 to {0})", end);
            int input = Convert.ToInt32(Console.ReadLine());
            if (input < 1 || input > end) throw new ArgumentOutOfRangeException(nameof(input), "Paramater cannot be less than 1 or greater than " + end);
            return input;
        }
    }
}