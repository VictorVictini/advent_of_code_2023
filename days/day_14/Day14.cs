namespace AdventOfCode2023 {
    public class Day14 : Day<int> {
        enum RockType {
            Round,
            Cube
        }
        struct yCoordinate {
            public int y;
            public RockType type;
            public yCoordinate(int y, RockType type) {
                this.y = y;
                this.type = type;
            }
        }
        List<yCoordinate>[] coords; // x: [{y: ..., type: ...}, {...}, ...]
        public Day14() {
            coords = ParseInput();
        }
        private List<yCoordinate>[] ParseInput() {
            string[] input = File.ReadAllLines("days/day_14/input.txt");
            List<yCoordinate>[] coords = new List<yCoordinate>[input[0].Length];
            for (int x = 0; x < input[0].Length; x++) {
                coords[x] = new List<yCoordinate>();
                for (int y = 0; y < input.Length; y++) {
                    if (input[y][x] == '#') {
                        coords[x].Add(new yCoordinate(y, RockType.Cube));
                    } else if (input[y][x] == 'O') {
                        coords[x].Add(new yCoordinate(y, RockType.Round));
                    }
                }
            }
            return coords;
        }
        public override int Part1() {
            int sum = 0;
            for (int x = 0; x < coords.Length; x++) {
                int level = 0;
                for (int i = 0; i < coords[x].Count; i++) {
                    if (coords[x][i].type == RockType.Cube) {
                        level = coords[x][i].y + 1;
                    } else {
                        sum += coords.Length - level;
                        level++;
                    }
                }
            }
            return sum;
        }
        public override int Part2() {
            // code here
            return Int32.MaxValue;
        }
    }
}