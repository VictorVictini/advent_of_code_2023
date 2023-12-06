using System.Diagnostics;

namespace AdventOfCode2023 {
    public abstract class Day<T> {
        public void OutputParts() {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            T res1 = this.Part1();
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string time1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            watch.Start();
            T res2 = this.Part2();
            watch.Stop();
            ts = watch.Elapsed;
            string time2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);


            Console.WriteLine("Part 1 took {0}ms and returned: {1}", time1, res1);
            Console.WriteLine("Part 2 took {0}ms and returned: {1}", time2, res2);
        }
        public abstract T Part1();
        public abstract T Part2();
    }
}