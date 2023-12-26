namespace AdventOfCode2023 {
    public class Day24 : Day<int> {
        struct Value {
            public double x, y, z;
            public Value(double x, double y, double z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        struct Path {
            public Value coordinate, velocity;
            public Path(Value coordinate, Value velocity) {
                this.coordinate = coordinate;
                this.velocity = velocity;
            }
        }
        Path[] paths;
        public Day24() {
            paths = ParseInput();
        }
        private Path[] ParseInput() {
            string[] input = File.ReadAllLines("days/day_24/input.txt");
            Path[] paths = new Path[input.Length];
            for (int i = 0; i < input.Length; i++) {
                string[] section = input[i].Split(" @ ");
                string[] coordinate = section[0].Split(", ");
                string[] velocity = section[1].Split(", ");
                paths[i] = new Path(
                    new Value(Convert.ToDouble(coordinate[0]), Convert.ToDouble(coordinate[1]), Convert.ToDouble(coordinate[2])),
                    new Value(Convert.ToDouble(velocity[0]), Convert.ToDouble(velocity[1]), Convert.ToDouble(velocity[2]))
                );
            }
            return paths;
        }
        private (double, double, double, double) Calculate2DIntersect(double x1, double y1, double vx1, double vy1, double x2, double y2, double vx2, double vy2) {
            // calculate first coordinate's details
            double m1 = vy1 / vx1;
            double c1 = m1 * (-x1) + y1;

            // calculate second coordinate's details
            double m2 = vy2 / vx2;
            double c2 = m2 * (-x2) + y2;

            // if the coordinates do not intersect / are parallel
            if (m1 == m2 && (x1 != x2 || y1 != y2)) return (Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue);

            // calculate point of intersection
            double x = (c2 - c1) / (m1 - m2);
            double y = m1 * x + c1;

            // calculating nanoseconds taken for each path to reach POI
            double t1 = (x - x1) / vx1;
            double t2 = (x - x2) / vx2;

            return (x, y, t1, t2);
        }
        public override int Part1() {
            int count = 0;
            double low = 200000000000000, high = 400000000000000;
            for (int i = 0; i < paths.Length - 1; i++) { // treat as first
                for (int j = i + 1; j < paths.Length; j++) { // treat as second
                    double x, y, t1, t2;
                    (x, y, t1, t2) = Calculate2DIntersect(
                        paths[i].coordinate.x, paths[i].coordinate.y, paths[i].velocity.x, paths[i].velocity.y,
                        paths[j].coordinate.x, paths[j].coordinate.y, paths[j].velocity.x, paths[j].velocity.y
                    );
                    if (t1 < 0) continue;
                    if (t2 < 0) continue;
                    if (x < low || x > high) continue;
                    if (y < low || y > high) continue;
                    count++;
                }
            }
            return count;
        }
        public override int Part2() {
            // code here
            return Int32.MaxValue;
        }
    }
}