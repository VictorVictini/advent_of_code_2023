namespace AdventOfCode2023 {
    public class Day9 : Day<int> {
        int[][] nums;
        public Day9() {
            nums = ParseInput();
        }
        private int[][] ParseInput() {
            string[] input = File.ReadAllLines("days/day_9/input.txt");
            int[][] res = new int[input.Length][];
            for (int i = 0; i < input.Length; i++) {
                string[] numbers = input[i].Split(' ');
                res[i] = new int[numbers.Length];
                for (int j = 0; j < numbers.Length; j++) {
                    res[i][j] = Convert.ToInt32(numbers[j]);
                }
            }
            return res;
        }
        private bool isEqual(int[] arr) {
            for (int i = 1; i < arr.Length; i++) {
                if (arr[i] != arr[i - 1]) return false;
            }
            return true;
        }
        private int CalculateNext(int[] arr) {
            int sum = arr[arr.Length - 1];
            while (!isEqual(arr)) {
                int[] temp = new int[arr.Length - 1];
                for (int i = 0; i + 1 < arr.Length; i++) {
                    temp[i] = arr[i + 1] - arr[i];
                }
                sum += temp[temp.Length - 1];
                arr = temp;
            }
            return sum;
        }
        public override int Part1() {
            int sum = 0;
            for (int i = 0; i < nums.Length; i++) {
                sum += CalculateNext(nums[i]);
            }
            return sum;
        }
        private int CalculatePrev(int[] arr) {
            List<int> left = new List<int>{arr[0]};
            while (!isEqual(arr)) {
                int[] temp = new int[arr.Length - 1];
                for (int i = 0; i + 1 < arr.Length; i++) {
                    temp[i] = arr[i + 1] - arr[i];
                }
                left.Add(temp[0]);
                arr = temp;
            }
            int prev = 0;
            for (int i = left.Count - 1; i >= 0; i--) {
                prev = left[i] - prev;
            }
            return prev;
        }
        public override int Part2() {
            int sum = 0;
            for (int i = 0; i < nums.Length; i++) {
                sum += CalculatePrev(nums[i]);
            }
            return sum;
        }
    }
}