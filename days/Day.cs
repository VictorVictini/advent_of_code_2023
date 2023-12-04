namespace AdventOfCode2023 {
    public abstract class Day<T> {
        public void OutputParts() {
            Console.WriteLine("Part 1 is " + this.Part1());
            Console.WriteLine("Part 2 is " + this.Part2());
        }
        public abstract T Part1();
        public abstract T Part2();
    }
}