using System.Diagnostics;

namespace AdventOfCode2023 {
    public class Program {
        private static void Main() { // will clean up later
            int dayNum = WithinLimit(15); // skipping a few days for now, will get back to them later
            dynamic day;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            switch (dayNum) {
                case 1:
                    day = new Day1();
                    break;
                case 2:
                    day = new Day2();
                    break;
                case 3:
                    day = new Day3();
                    break;
                case 4:
                    day = new Day4();
                    break;
                case 5:
                    day = new Day5();
                    break;
                case 6:
                    day = new Day6();
                    break;
                case 7:
                    day = new Day7();
                    break;
                case 8:
                    day = new Day8();
                    break;
                case 9:
                    day = new Day9();
                    break;
                case 10:
                    day = new Day10();
                    break;
                case 11:
                    day = new Day11();
                    break;
                case 15:
                    day = new Day15();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayNum), "Encountered a day not in the switch statement");
            }
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            Console.WriteLine(String.Format("Parsing the input.txt file took: {0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
            day.OutputParts();
        }

        private static int WithinLimit(int end) {
            Console.WriteLine("Please enter a day (1 to {0})", end);
            int input = Convert.ToInt32(Console.ReadLine());
            if (input < 1 || input > end) throw new ArgumentOutOfRangeException(nameof(input), "Paramater cannot be less than 1 or greater than " + end);
            return input;
        }
    }
}