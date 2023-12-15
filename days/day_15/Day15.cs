namespace AdventOfCode2023 {
    public class Day15 : Day<int> {
        enum InstructionType {
            Remove,
            Insert
        }
        struct Data {
            public string label;
            public int num;
            public Data(string label, int num = -1) {
                this.label = label;
                this.num = num;
            }
        }
        struct Instruction {
            public InstructionType type;
            public Data data;
            public Instruction(InstructionType type, Data data) {
                this.type = type;
                this.data = data;
            }
        }
        string[] input;
        Instruction[] instructions;
        public Day15() {
            (input, instructions) = ParseInput();
        }
        private (string[], Instruction[]) ParseInput() {
            string[] input = File.ReadAllText("days/day_15/input.txt").Split(",");
            Instruction[] instructions = new Instruction[input.Length];
            for (int i = 0; i < input.Length; i++) {
                if (input[i][input[i].Length - 1] == '-') {
                    instructions[i] = new Instruction(InstructionType.Remove, new Data(input[i].Substring(0, input[i].Length - 1)));
                } else {
                    string[] args = input[i].Split("=");
                    instructions[i] = new Instruction(InstructionType.Insert, new Data(args[0], Convert.ToInt32(args[1])));
                }
            }
            return (input, instructions);
        }
        private int HashAlgorithm(string str) {
            int res = 0;
            for (int i = 0; i < str.Length; i++) {
                res = (res + str[i]) * 17 % 256;
            }
            return res;
        }
        public override int Part1() {
            int sum = 0;
            for (int i = 0; i < input.Length; i++) {
                sum += HashAlgorithm(input[i]);
            }
            return sum;
        }
        public override int Part2() {
            List<Data>[] boxes = new List<Data>[256];
            for (int i = 0; i < boxes.Length; i++) {
                boxes[i] = new List<Data>();
            }
            for (int i = 0; i < instructions.Length; i++) {
                int hash = HashAlgorithm(instructions[i].data.label);
                if (instructions[i].type == InstructionType.Insert) {
                    bool found = false;
                    for (int j = 0; j < boxes[hash].Count; j++) {
                        if (boxes[hash][j].label == instructions[i].data.label) {
                            boxes[hash][j] = instructions[i].data;
                            found = true;
                            break;
                        }
                    }
                    if (!found) boxes[hash].Add(instructions[i].data);
                } else {
                    for (int j = 0; j < boxes[hash].Count; j++) {
                        if (boxes[hash][j].label == instructions[i].data.label) {
                            boxes[hash].RemoveAt(j);
                            break;
                        }
                    }
                }
            }
            int sum = 0;
            for (int i = 0; i < boxes.Length; i++) {
                for (int j = 0; j < boxes[i].Count; j++) {
                    sum += (i + 1) * (j + 1) * boxes[i][j].num;
                }
            }
            return sum;
        }
    }
}