namespace AdventOfCode2023 {
    public class Day {
        public void OutputParts() {
            Console.WriteLine("Part 1 is " + this.Part1());
            Console.WriteLine("Part 2 is " + this.Part2());
        }
        public virtual int Part1() {
            return Int32.MinValue;
        }
        public virtual int Part2() {
            return Int32.MinValue;
        }
    }
}