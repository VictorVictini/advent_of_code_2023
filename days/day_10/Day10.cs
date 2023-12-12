namespace AdventOfCode2023 {
    public class Day10 : Day<int> {
        // struct indicating how much to change x and y coords by
        struct Move {
            public int x, y;
            public Move(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }

        // struct identical to the above, but for coordinates, separated purely for readability
        struct Coordinate {
            public int x, y;
            public Coordinate(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }

        // simple constants for readability
        enum Direction {
            North,
            East,
            South,
            West
        }
        // indicates amounts to change coordinates by correlating to the given directions
        Dictionary<Direction, Move> moveBy = new Dictionary<Direction, Move>{
            {Direction.North,   new Move(0, -1)},
            {Direction.East,    new Move(1,  0)},
            {Direction.South,   new Move(0,  1)},
            {Direction.West,    new Move(-1, 0)}
        };

        // indicates what the direction changes are, correlating with the given symbols
        Dictionary<char, Direction[]> connections = new Dictionary<char, Direction[]>{
            {'|', new Direction[]{Direction.North, Direction.South}},
            {'-', new Direction[]{Direction.East,  Direction.West}},
            {'L', new Direction[]{Direction.North, Direction.East}},
            {'J', new Direction[]{Direction.North, Direction.West}},
            {'7', new Direction[]{Direction.South, Direction.West}},
            {'F', new Direction[]{Direction.East,  Direction.South}},
            {'S', new Direction[]{Direction.North, Direction.East, Direction.South, Direction.West}}
        };
        string[] map;
        int furthestSteps, minY, maxY;
        Dictionary<int, List<int>> peaks;
        public Day10() {
            (map, furthestSteps, peaks, minY, maxY) = ParseInput();
        }
        // reverses a direction e.g. interpreting North as 'forwards' and South as 'backwards', it reverses forwards to move backwards
        private Direction ReverseDirection(Direction dir) {
            switch (dir) {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    throw new Exception("ReverseDirection() provided a direction that is not accounted for");
            }
        }
        // determines if the given coordinate can go to the given direction
        private bool isValidDirection(string[] map, Coordinate coord, Direction expectedDirection) {
            // simple range check for the provided coordinate
            if (coord.y < 0 || coord.y >= map.Length) return false;
            if (coord.x < 0 || coord.x >= map[0].Length) return false;

            // finds the character corresponding to the coordinate, and determines if it is invalid
            char chr = map[coord.y][coord.x];
            if (!connections.ContainsKey(chr)) return false;
            Direction[] directions = connections[chr];

            // if the coordinate cannot go in the intended direction
            if (!directions.Contains(expectedDirection)) return false;
            
            // calculates next character and provides range checks
            Move changeBy = moveBy[expectedDirection];
            int nextX = coord.x + changeBy.x, nextY = coord.y + changeBy.y;
            if (nextY < 0 || nextY >= map.Length) return false;
            if (nextX < 0 || nextX >= map[0].Length) return false;
            char nextChr = map[nextY][nextX];

            // if the next coordinate is invalid
            if (!connections.ContainsKey(nextChr)) return false;

            // if the character contains the opposite direction e.g. East-West (-><-)
            return connections[nextChr].Contains(ReverseDirection(expectedDirection));
        }

        // used to find create paths correlating with start location S
        private void FindPathExists(string[] map, List<Coordinate> paths, List<Direction> directions, Coordinate coord, Direction expectedDirection) {
            // checks that the previous coordinate can move in the given direction
            if (!isValidDirection(map, coord, expectedDirection)) return;

            // add to paths and provide the next direction to take
            paths.Add(coord);
            directions.Add(expectedDirection);
        }
        // used to find the first direction that is not the provided direction
        private Direction NotContains(Direction[] directions, Direction notFind) {
            for (int i = 0; i < directions.Length; i++) {
                if (directions[i] != notFind) return directions[i];
            }
            throw new Exception("Expected a direction to not equal the provided notFind direction");
        }
        // used for changing to the next coordinate
        // assume the next coordinate is a correct continuation
        private (Coordinate, Direction) MoveNext(string[] map, Coordinate coords, Direction direction) {
            // finds the next amount to change the coordinates by
            Move moveAmount = moveBy[direction];

            // applies those changes
            Coordinate newCoords = new Coordinate(coords.x + moveAmount.x, coords.y + moveAmount.y);

            // finds the direction correlating to the next coords
            char chr = map[newCoords.y][newCoords.x];
            Direction newDirection = NotContains(connections[chr], ReverseDirection(direction));

            return (newCoords, newDirection);
        }
        // finds the leftmost coordinate and the rightmost coordinate of a coordinate, without changing its y-value during traversing (effectively travels through east/west until it can't anymore)
        private (Coordinate, Coordinate) ExpandCoord(string[] map, Coordinate coord) {
            // traversing towards West until it can't anymore
            Coordinate left = coord;
            Direction direction = Direction.West;
            while (isValidDirection(map, left, Direction.West)) {
                (left, direction) = MoveNext(map, left, direction);
            }

            // traversing towards East until it can't anymore
            Coordinate right = coord;
            direction = Direction.East;
            while (isValidDirection(map, right, Direction.East)) {
                (right, direction) = MoveNext(map, right, direction);
            }

            return (left, right);
        }

        // figures out if a 'peak' occurs at the relevant coordinate, and adds it to the list if it does and doesn't already exist
        // we defined peak as a point such as F---7 or, to reword it, anything in the path that would match the regex `F-*7`
        private void AddPeak(string[] map, Dictionary<int, List<int>> peaks, Coordinate coord) {
            // expands the coordinates to get the start of the peak and the end of the peak
            (Coordinate left, Coordinate right) = ExpandCoord(map, coord);

            // if the peak is already in the dictionary
            if (peaks.ContainsKey(left.y) && peaks[left.y].Contains(left.x)) return;

            // if it is not a valid peak
            char leftChr = map[left.y][left.x], rightChr = map[right.y][right.x];
            if (!connections.ContainsKey(leftChr) || !connections.ContainsKey(rightChr)) return;
            if (!isValidDirection(map, left, Direction.East) || !isValidDirection(map, left, Direction.South)) return;
            if (!isValidDirection(map, right, Direction.West) || !isValidDirection(map, right, Direction.South)) return;

            // adds leftmost coordinate to the relevant peak
            if (peaks.ContainsKey(left.y)) {
                peaks[left.y].Add(left.x);
            } else {
                peaks.Add(left.y, new List<int>{left.x});
            }
        }

        // finds the furthest path, and all x-y coordinates in the path
        private (string[], int, Dictionary<int, List<int>>, int, int) ParseInput() {
            string[] map = File.ReadAllLines("days/day_10/input.txt");

            // finds the start location
            bool found = false;
            Coordinate start = new Coordinate(-1, -1);
            for (int y = 0; !found && y < map.Length; y++) {
                for (int x = 0; !found && x < map[y].Length; x++) {
                    if (map[y][x] == 'S') {
                        found = true;
                        start = new Coordinate(x, y);
                    }
                }
            }
            if (!found) throw new Exception("Expected file to contain 'S'");

            // assuming 2 paths from S, finds them
            List<Coordinate> paths = new List<Coordinate>();
            List<Direction> directions = new List<Direction>();
            bool[] isNorth = new bool[2];
            FindPathExists(map, paths, directions, start, Direction.North);
            FindPathExists(map, paths, directions, start, Direction.East);
            FindPathExists(map, paths, directions, start, Direction.South);
            FindPathExists(map, paths, directions, start, Direction.West);
            if (paths.Count != 2) throw new ArgumentOutOfRangeException(nameof(paths), "Expected 2 paths");
            if (paths.Count != directions.Count) throw new ArgumentOutOfRangeException(nameof(paths), "Expected paths length to equal directions length");

            // creating a dictionary to record all 'peaks' for a given y-axis (i.e. y:[x0, x1, x2, ...])
            Dictionary<int, List<int>> peaks = new Dictionary<int, List<int>>();

            // counts steps/iterations, starts at 1 since the paths are one coordinate after start
            int steps = 0;

            // initialising min and max y-values
            int minY = start.y, maxY = start.y;

            // continues around until the two ends 'meet' in the middle
            do {
                steps++;

                // adds to peaks as necessary
                AddPeak(map, peaks, paths[0]);
                AddPeak(map, peaks, paths[1]);

                // stores if the direction was North before 
                isNorth[0] = directions[0] == Direction.North;
                isNorth[1] = directions[1] == Direction.North;

                // moves the two paths as necessary
                (paths[0], directions[0]) = MoveNext(map, paths[0], directions[0]);
                (paths[1], directions[1]) = MoveNext(map, paths[1], directions[1]);

                if (paths[0].y < minY) minY = paths[0].y;
                if (paths[0].y > maxY) maxY = paths[0].y;
                if (paths[1].y < minY) minY = paths[1].y;
                if (paths[1].y > maxY) maxY = paths[1].y;
            } while (paths[0].x != paths[1].x || paths[0].y != paths[1].y);
            return (map, steps, peaks, minY, maxY);
        }
        public override int Part1() {
            return furthestSteps;
        }
        // bubble sort
        private void BubbleSort(List<Coordinate> paths, List<Direction> directions) {
            bool swap = true;
            for (int i = 0; swap && i < paths.Count; i++) {
                swap = false;
                for (int j = 1; j < paths.Count - i; j++) {
                    if (paths[j].x < paths[j - 1].x) {
                        Coordinate temp = paths[j];
                        paths[j] = paths[j - 1];
                        paths[j - 1] = temp;

                        Direction tempDir = directions[j];
                        directions[j] = directions[j - 1];
                        directions[j - 1] = tempDir;

                        swap = true;
                    }
                }
            }
        }
        public override int Part2() {
            // getting initial values for paths
            List<Coordinate> paths = new List<Coordinate>();
            List<Direction> directions = new List<Direction>();
            for (int i = 0; i < peaks[minY].Count; i++) {
                (Coordinate left, Coordinate right) = ExpandCoord(map, new Coordinate(peaks[minY][i], minY));
                paths.Add(left);
                paths.Add(right);
                directions.Add(Direction.South);
                directions.Add(Direction.South);
            }
            BubbleSort(paths, directions);

            // initialising area
            int area = 0;

            // traversing from top (minY + 1) to bottom (when the paths are gone, it is done traversing)
            for (int y = minY + 1; paths.Count > 0; y++) {
                // adding new paths at this y-level
                if (peaks.ContainsKey(y)) {
                    for (int i = 0; i < peaks[y].Count; i++) {
                        (Coordinate left, Coordinate right) = ExpandCoord(map, new Coordinate(peaks[y][i], y));
                        paths.Add(left);
                        paths.Add(right);
                        directions.Add(Direction.South);
                        directions.Add(Direction.South);
                    }
                    BubbleSort(paths, directions);
                }

                // moving all paths to the current y value and creating an array of what to delete
                List<int> remove = new List<int>();
                for (int i = 0; i < paths.Count; i++) {
                    // traversing until y is correct level, escaping if it starts heading north
                    while ((paths[i].y != y || directions[i] != Direction.South) && directions[i] != Direction.North) {
                        (paths[i], directions[i]) = MoveNext(map, paths[i], directions[i]);
                    }
                    if (paths[i].y != y || directions[i] != Direction.South) {
                        remove.Add(i);
                    }
                }

                // calculating area
                for (int i = 1; i < paths.Count; i += 2) {
                    (Coordinate _, Coordinate right) = ExpandCoord(map, paths[i - 1]);
                    (Coordinate left, _) = ExpandCoord(map, paths[i]);
                    int add = left.x - right.x - 1;
                    if (left.x - right.x - 1 > 0) area += add;
                }

                // deleting where needed
                for (int i = remove.Count - 1; i >= 0; i--) {
                    paths.RemoveAt(remove[i]);
                    directions.RemoveAt(remove[i]);
                }
            }
            return area;
        }
    }
}