namespace AdventOfCode2023 {
    public class Day16 : Day<int> {
        enum Direction {
            Up,
            Down,
            Left,
            Right
        }
        Dictionary<char, Direction[]> mirror = new Dictionary<char, Direction[]>{
            {'/', new Direction[]{Direction.Left, Direction.Up}},
            {'\\', new Direction[]{Direction.Right, Direction.Up}}
        };
        Dictionary<char, Direction[]> split = new Dictionary<char, Direction[]>{
            {'|', new Direction[]{Direction.Up, Direction.Down}},
            {'-', new Direction[]{Direction.Left, Direction.Right}}
        };
        struct Coordinate {
            public int x, y;
            public Coordinate(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }
        Dictionary<Direction, Coordinate> moveBy = new Dictionary<Direction, Coordinate>{
            {Direction.Up, new Coordinate(0, -1)},
            {Direction.Down, new Coordinate(0, 1)},
            {Direction.Left, new Coordinate(-1, 0)},
            {Direction.Right, new Coordinate(1, 0)}
        };
        string[] map;
        Dictionary<Coordinate, List<Direction>> paths;
        public Day16() {
            map = ParseInput();
            paths = new Dictionary<Coordinate, List<Direction>>();
        }
        private string[] ParseInput() {
            return File.ReadAllLines("days/day_16/input.txt");
        }

        private void CreateBeam(Coordinate coord, Direction direction) {
            // traverses until the path is outside the limits and is not already existing
            while (
                coord.x >= 0 && coord.x < map[0].Length &&
                coord.y >= 0 && coord.y < map.Length &&
                (!paths.ContainsKey(coord) || !paths[coord].Contains(direction))) {

                // adding the current path to the dictionary as needed
                if (paths.ContainsKey(coord)) {
                    paths[coord].Add(direction);
                } else {
                    paths.Add(coord, new List<Direction>{direction});
                }

                // replacing with coordinates & direction of next step
                (coord, direction) = InterpretMove(coord, direction);
            }
        }
        private Direction ReverseDirection(Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new Exception("Unsupported value provided for direction");
            }
        }
        private (Coordinate, Direction) InterpretMove(Coordinate coord, Direction direction) {
            char mapChar = map[coord.y][coord.x];
            if (mirror.ContainsKey(mapChar)) {
                if (mirror[mapChar].Contains(ReverseDirection(direction))) {
                    direction = mirror[mapChar][0] == ReverseDirection(direction) ? mirror[mapChar][1] : mirror[mapChar][0];
                } else {
                    direction = mirror[mapChar][0] == direction ? ReverseDirection(mirror[mapChar][1]) : ReverseDirection(mirror[mapChar][0]);
                }
            } else if (split.ContainsKey(mapChar)) {
                if (!split[mapChar].Contains(direction)) {
                    for (int i = 0; i < split[mapChar].Length; i++) {
                        Coordinate changeBranch = moveBy[split[mapChar][i]];
                        CreateBeam(new Coordinate(coord.x + changeBranch.x, coord.y + changeBranch.y), split[mapChar][i]);
                    }
                    return (new Coordinate(-1, -1), direction); // ends the current beam by giving an invalid coordinate to proceed with
                }
            }
            Coordinate change = moveBy[direction];
            coord = new Coordinate(coord.x + change.x, coord.y + change.y);
            return (coord, direction);
        }
        public override int Part1() {
            CreateBeam(new Coordinate(0, 0), Direction.Right);
            return paths.Count;
        }
        public override int Part2() {
            int max = Int32.MinValue;
            for (int y = 0; y < map.Length; y++) {
                paths.Clear();
                CreateBeam(new Coordinate(0, y), Direction.Right);
                if (max < paths.Count) max = paths.Count;
                paths.Clear();
                CreateBeam(new Coordinate(map[y].Length - 1, y), Direction.Left);
                if (max < paths.Count) max = paths.Count;
            }
            for (int x = 0; x < map[0].Length; x++) {
                paths.Clear();
                CreateBeam(new Coordinate(x, 0), Direction.Down);
                if (max < paths.Count) max = paths.Count;
                paths.Clear();
                CreateBeam(new Coordinate(x, map.Length - 1), Direction.Up);
                if (max < paths.Count) max = paths.Count;
            }
            return max;
        }
    }
}