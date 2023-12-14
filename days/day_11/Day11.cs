namespace AdventOfCode2023 {
    public class Day11 : Day<long> {
        struct Coordinate {
            public int x, y, galaxy_behind_x, galaxy_behind_y;
            public Coordinate(int x, int y, int galaxy_behind_x, int galaxy_behind_y) {
                this.x = x;
                this.y = y;
                this.galaxy_behind_x = galaxy_behind_x;
                this.galaxy_behind_y = galaxy_behind_y;
            }
        }
        List<Coordinate> galaxies;
        public Day11() {
            galaxies = ParseInput();
        }
        private List<Coordinate> ParseInput() {
            string[] input = File.ReadAllLines("days/day_11/input.txt");
            Dictionary<int, Dictionary<int, int>> galaxies = new Dictionary<int, Dictionary<int, int>>(); // galaxies corresponding to x values, y: {x: n, ...}
            List<int>[] coords = new List<int>[input.Length]; // galaxies corresponding to y values, y: [x0, x1, ...]
            for (int i = 0; i < coords.Length; i++) {
                coords[i] = new List<int>();
            }
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    if (input[y][x] == '#') {
                        if (galaxies.ContainsKey(x)) {
                            galaxies[x].Add(y, 0);
                        } else {
                            galaxies.Add(x, new Dictionary<int, int>{{y, 0}});
                        }
                        coords[y].Add(x);
                    }
                }
            }
            for (int y = 0, c = 0; y < coords.Length; y++) {
                if (coords[y].Count > 0) {
                    for (int i = 0; i < coords[y].Count; i++) {
                        galaxies[coords[y][i]][y] = c;
                        
                    }
                    c++;
                }
            }
            List<Coordinate> res = new List<Coordinate>();
            int galaxy_behind_x = 0;
            foreach (KeyValuePair<int, Dictionary<int, int>> galaxy in galaxies) {
                foreach (KeyValuePair<int, int> galaxy_coord in galaxy.Value) {
                    res.Add(new Coordinate(galaxy.Key, galaxy_coord.Key, galaxy_behind_x, galaxy_coord.Value));
                }
                galaxy_behind_x++;
            }
            return res;
        }
        private long CalculateSum(int n) {
            long sum = 0;
            for (int i = 0; i < galaxies.Count - 1; i++) {
                for (int j = i + 1; j < galaxies.Count; j++) {
                    int diffX = Math.Abs(galaxies[j].x - galaxies[i].x);
                    int diffY = Math.Abs(galaxies[j].y - galaxies[i].y);
                    int diffGalaxyCountX = Math.Abs(galaxies[j].galaxy_behind_x - galaxies[i].galaxy_behind_x);
                    int diffGalaxyCountY = Math.Abs(galaxies[j].galaxy_behind_y - galaxies[i].galaxy_behind_y);
                    sum += diffX + diffY + (n - 1) * (diffX - diffGalaxyCountX + diffY - diffGalaxyCountY);
                }
            }
            return sum;
        }
        public override long Part1() {
            return CalculateSum(2);
        }
        public override long Part2() {
            return CalculateSum(1000000);
        }
    }
}