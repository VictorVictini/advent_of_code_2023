namespace AdventOfCode2023 {
    public class Day<T> where T : new() {
        public void OutputParts() {
            Console.WriteLine("Part 1 is " + this.Part1());
            Console.WriteLine("Part 2 is " + this.Part2());
        }
        public virtual T Part1() {
            T data = new();
            return data;
        }
        public virtual T Part2() {
            T data = new();
            return data;
        }
    }
}