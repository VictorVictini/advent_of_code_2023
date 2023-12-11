using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace AdventOfCode2023 {
    public class Day10 : Day<int> {
        // a struct for indicating the start and end directions, if they change
        struct DirectionChange {
            public Direction start, end;
            public DirectionChange(Direction start, Direction end) {
                this.start = start;
                this.end = end;
            }
        }
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
        // indicates what axis to change corresponding to the current direction
        Dictionary<Direction, Move> moveBy = new Dictionary<Direction, Move>{
            {Direction.North,   new Move(0, -1)},
            {Direction.East,    new Move(1,  0)},
            {Direction.South,   new Move(0,  1)},
            {Direction.West,    new Move(-1, 0)}
        };
        // indicates what the direction changes are, correlating with the given symbols
        Dictionary<char, DirectionChange> moves = new Dictionary<char, DirectionChange>{
            {'|', new DirectionChange(Direction.South,    Direction.South)},
            {'-', new DirectionChange(Direction.East,     Direction.East)},
            {'L', new DirectionChange(Direction.South,    Direction.East)},
            {'J', new DirectionChange(Direction.South,    Direction.West)},
            {'7', new DirectionChange(Direction.East,     Direction.South)},
            {'F', new DirectionChange(Direction.West,     Direction.South)},
        };
        int furthestSteps, area;
        public Day10() {
            (furthestSteps, area) = ParseInput();
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
            }
            return Direction.North;
        }

        // used to find if two coordinates continue from each other, if so then add it to the relevant path
        // should realistically only account for S, but just in case it supports other cases
        private void FindPathExists(string[] map, List<Coordinate> paths, List<Direction> directions, Coordinate prev, Coordinate coord, Direction expectedDirection) {
            if (coord.y < 0 || coord.y >= map.Length) return; // possibly replace with out of range exceptions
            if (coord.x < 0 || coord.x >= map[0].Length) return;

            // ensures there is only one 'move' between them, similar to XNOR
            if (prev.x != coord.x && prev.y != coord.y) return;
            if (prev.x == coord.x && prev.y == coord.y) return;
            if (Math.Abs(prev.x - coord.x) != 1 && Math.Abs(prev.y - coord.y) != 1) return;

            // if the given symbol does not have a path
            if (!moves.ContainsKey(map[coord.y][coord.x])) return;

            // if the expected direction is not in the given context
            DirectionChange relevantDirections = moves[map[coord.y][coord.x]];
            if (relevantDirections.start != expectedDirection && ReverseDirection(relevantDirections.end) != expectedDirection) return;

            // add to paths and provide the next direction to take
            paths.Add(coord);
            directions.Add(relevantDirections.start == expectedDirection ? relevantDirections.end : ReverseDirection(relevantDirections.start));
        }
        // used for changing to the next coordinate
        // assumes all reversed/normal work fine
        // assumes no out of range errors
        private (Coordinate, Direction) MoveNext(string[] map, Coordinate coords, Direction direction) {
            // finds the next amount to change the coordinates by
            Move moveAmount = moveBy[direction];

            // applies those changes
            Coordinate newCoords = new Coordinate(coords.x + moveAmount.x, coords.y + moveAmount.y);

            // finds the directions correlating to the next coords
            DirectionChange relevantDirections = moves[map[newCoords.y][newCoords.x]];

            // sets up next direction, if they need to be reversed they are
            Direction newDirection = relevantDirections.start == direction ? relevantDirections.end : ReverseDirection(relevantDirections.start);
            return (newCoords, newDirection);
        }
        private void AddToDictionary(string[] map, Dictionary<int, List<int>> coords, Coordinate coord) {
            char chr = map[coord.y][coord.x];
            if (!moves.ContainsKey(chr) || (moves[chr].start == Direction.East || coords.ContainsKey(coord.y)) && moves[chr].end == Direction.East) return;
            if (!coords.ContainsKey(coord.y)) {
                coords.Add(coord.y, new List<int>{coord.x});
                return;
            }
            coords[coord.y].Add(coord.x);
        }

        // finds the furthest path, and all x-y coordinates in the path
        private (int, int) ParseInput() {
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
            FindPathExists(map, paths, directions, start, new Coordinate(start.x, start.y - 1), Direction.North);
            FindPathExists(map, paths, directions, start, new Coordinate(start.x + 1, start.y), Direction.East);
            FindPathExists(map, paths, directions, start, new Coordinate(start.x, start.y + 1), Direction.South);
            FindPathExists(map, paths, directions, start, new Coordinate(start.x - 1, start.y), Direction.West);
            if (paths.Count != 2) throw new ArgumentOutOfRangeException(nameof(paths), "Expected 2 paths");
            if (paths.Count != directions.Count) throw new ArgumentOutOfRangeException(nameof(paths), "Expected paths length to equal directions length");

            // initialising pathCoords
            Dictionary<int, List<int>> pathCoords = new Dictionary<int, List<int>>();

            // adds x and y coordinates to the dictionary as necessary
            AddToDictionary(map, pathCoords, start);
            AddToDictionary(map, pathCoords, paths[0]);
            AddToDictionary(map, pathCoords, paths[1]);

            // counts steps/iterations, starts at 1 since the paths are one coordinate after start
            int steps = 1;

            // initialising the area
            int area = 0;

            // continues around until the two ends 'meet' in the middle
            while (true) {
                steps++;

                // moves the two paths as necessary
                (paths[0], directions[0]) = MoveNext(map, paths[0], directions[0]);
                (paths[1], directions[1]) = MoveNext(map, paths[1], directions[1]);

                // add to pathCoords as necessary
                AddToDictionary(map, pathCoords, paths[0]);
                if (paths[0].x == paths[1].x && paths[0].y == paths[1].y) break; // gets rid of duplicates and ends loop
                AddToDictionary(map, pathCoords, paths[1]);

                if (pathCoords.ContainsKey(paths[0].y)) Console.WriteLine("{2} && {0}: [{1}]", paths[0].y, String.Join(", ", pathCoords[paths[0].y]), steps);
                if (pathCoords.ContainsKey(paths[1].y)) Console.WriteLine("{2} && {0}: [{1}]", paths[1].y, String.Join(", ", pathCoords[paths[1].y]), steps);
            }
            return (steps, area);
        }
        public override int Part1() {
            return furthestSteps;
        }
        // dual-pivoted quicksort algorithm
        private void QuickSort(List<int> arr, int start, int end) {
            if (start >= end) return;
            (int leftPivot, int rightPivot) = Partition(arr, start, end);
            QuickSort(arr, start, leftPivot - 1);
            QuickSort(arr, leftPivot + 1, rightPivot - 1);
            QuickSort(arr, rightPivot + 1, end);
        }
        private (int, int) Partition(List<int> arr, int start, int end) {
            if (arr[start] > arr[end]) swap(arr, start, end);
            int leftPivot = start + 1, rightPivot = end - 1;
            for (int i = start + 1; i <= rightPivot;) {
                if (arr[i] < arr[start]) {
                    swap(arr, leftPivot++, i++);
                } else if (arr[i] > arr[end]) {
                    swap(arr, rightPivot--, i);
                } else {
                    i++;
                }
            }
            swap(arr, start, --leftPivot);
            swap(arr, end, ++rightPivot);
            return (leftPivot, rightPivot);
        }
        private void swap(List<int> arr, int a, int b) {
            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }
        public override int Part2() {
            return area;
        }
    }
}