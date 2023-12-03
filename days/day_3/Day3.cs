using System.Text.RegularExpressions;

namespace AdventOfCode2023 {
    public class Day3 : Day {
        private struct Symbol {
            public int x, y;
            public char symbol;
            public Symbol(int x, int y, char symbol) {
                this.x = x;
                this.y = y;
                this.symbol = symbol;
            }
        }
        // finds the number in input at position pos (looking for the longest possible number including the relevant position)
        private int FindNumber(string input, int pos) {
            if (pos >= input.Length) throw new ArgumentOutOfRangeException(nameof(pos), "Position provided is too large");
            while(pos >= 0 && Char.IsNumber(input[pos])) pos--;
            int num = 0;
            for (pos++; pos < input.Length && Char.IsNumber(input[pos]); pos++) {
                num = num * 10 + (input[pos] - '0');
            }
            return num;
        }

        // finds all numbers correlating to a given symbol
        // assumes that there is at most one symbol correlating to a number
        private Dictionary<Symbol, List<int>> ParseInput() {
            string[] input = File.ReadAllLines("days/day_3/input.txt");
            Dictionary<Symbol, List<int>> symbols = new Dictionary<Symbol, List<int>>();
            for (int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    if (Char.IsNumber(input[y][x]) || input[y][x] == '.') continue;
                    Symbol symbol = new Symbol(x, y, input[y][x]);
                    for (int currY = y - 1; currY <= y + 1 && currY < input.Length; currY++) {
                        if (currY < 0) continue;
                        for (int currX = x - 1; currX <= x + 1 && currX < input[currY].Length; currX++) {
                            if (currX < 0) continue;
                            if (!Char.IsNumber(input[currY][currX])) continue;
                            int num = FindNumber(input[currY], currX);
                            if (symbols.ContainsKey(symbol)) {
                                symbols[symbol].Add(num);
                            } else {
                                symbols.Add(symbol, new List<int>{num});
                            }
                            if (currX + 1 < input[currY].Length && Char.IsNumber(input[currY][currX + 1])) break; // if the next character is a number, whole row (of 3) is irrelevant
                        }
                    }
                }
            }
            return symbols;
        }
        public override int Part1() {
            Dictionary<Symbol, List<int>> symbols = ParseInput();
            int sum = 0;
            foreach (KeyValuePair<Symbol, List<int>> pair in symbols) {
                for (int i = 0; i < pair.Value.Count; i++) sum += pair.Value[i];
            }
            return sum;
        }
        public override int Part2() {
            Dictionary<Symbol, List<int>> symbols = ParseInput();
            int sum = 0;
            foreach (KeyValuePair<Symbol, List<int>> pair in symbols) {
                if (pair.Key.symbol != '*') continue;
                if (pair.Value.Count != 2) continue;
                sum += pair.Value[0] * pair.Value[1];
            }
            return sum;
        }
    }
}