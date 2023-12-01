using System;

namespace AdventOfCode2023 {
    public class Program {
        private static void Main() {
            Day1 view = new Day1();
        }

        // lazy & recursive try-catch
        private static int WithinLimit(int small, int large)
        {
            Console.WriteLine("Please enter a number within the range {0} to {1}", small, large);
            int input = small - 1;
            do
            {
                input = Convert.ToInt32(Console.ReadLine());
                if (input < small || input > large)
                {
                    Console.WriteLine("Input is invalid. Please keep it within the range {0} to {1}", small, large);
                }
            }
            while (input < small || input > large);
            return input;
        }
    }

}