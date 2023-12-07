namespace AdventOfCode2023 {
    public class Day7 : Day<int> {
        struct CardBid {
            public string card;
            public int bid;
            public CardBid(string card, int bid) {
                this.card = card;
                this.bid = bid;
            }
        }
        CardBid[] cards;
        Dictionary<char, int> strengths = new Dictionary<char, int>{
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'J', 11},
            {'Q', 12},
            {'K', 13},
            {'A', 14}
        };
        public Day7() {
            cards = ParseInput();
        }
        private CardBid[] ParseInput() {
            string[] input = File.ReadAllLines("days/day_7/input.txt");
            CardBid[] cardbids = new CardBid[input.Length];
            for (int i = 0; i < input.Length; i++) {
                string[] sections = input[i].Split(" ");
                if (sections.Length != 2) throw new ArgumentOutOfRangeException(nameof(sections), "Expected length to be 2");
                if (sections[0].Length != 5) throw new ArgumentOutOfRangeException(nameof(sections), "Expected first argument's length to be 5");
                cardbids[i] = new CardBid(sections[0], Convert.ToInt32(sections[1])); 
            }
            return cardbids;
        }
        private void QuickSort(List<CardBid> arr, int start, int end, bool isPart2) {
            if (end <= start) return;
            int pivot = Partition(arr, start, end, isPart2);
            QuickSort(arr, start, pivot - 1, isPart2);
            QuickSort(arr, pivot + 1, end, isPart2);
        }
        private int Partition(List<CardBid> arr, int start, int end, bool isPart2) {
            CardBid pivotVal = arr[end], temp;
            int pivot = start;
            for (int i = start; i < end; i++) {
                if (LessThan(arr[i].card, pivotVal.card, isPart2)) {
                    temp = arr[pivot];
                    arr[pivot] = arr[i];
                    arr[i] = temp;
                    pivot++;
                }
            }
            temp = arr[pivot];
            arr[pivot] = pivotVal;
            arr[end] = temp;
            return pivot;
        }
        private bool LessThan(string a, string b, bool isPart2) {
            strengths['J'] = isPart2 ? 1 : 11;
            for (int i = 0; i < a.Length; i++) { // assume a.Length == b.Length
                if (a[i] != b[i]) return strengths[a[i]] < strengths[b[i]];
            }
            return false;
        }
        // 1 = High card, 2 = One pair, ..., 6 = Four of a kind, 7 = Five of a kind
        private int CalculateType(string card, bool isPart2) {
            Dictionary<char, int> quantity = new Dictionary<char, int>();
            int largest = Int32.MinValue;
            for (int i = 0; i < card.Length; i++) {
                if (quantity.ContainsKey(card[i])) {
                    quantity[card[i]]++;
                } else {
                    quantity.Add(card[i], 1);
                }
                if (quantity[card[i]] > largest && (!isPart2 || card[i] != 'J')) largest = quantity[card[i]];
            }
            int count = quantity.Count;
            if (isPart2 && quantity.ContainsKey('J')) {
                if (count == 1) {
                    largest = quantity['J'];
                } else {
                    largest += quantity['J'];
                    count--;
                }
            }
            switch (count) {
                case 5:
                    return 1;
                case 4:
                    return 2;
                case 3:
                    return largest == 2 ? 3 : 4;
                case 2:
                    return largest == 4 ? 6 : 5;
                case 1:
                    return 7;
            }
            return -1;
        }
        private int CalculateSum(Dictionary<int, List<CardBid>> types, bool isPart2) {
            int sum = 0;
            for (int rank = 1, type = 1; type <= 7; type++) {
                QuickSort(types[type], 0, types[type].Count - 1, isPart2);
                for (int i = 0; i < types[type].Count; i++, rank++) {
                    sum += rank * types[type][i].bid;
                }
            }
            return sum;
        }
        private Dictionary<int, List<CardBid>> CreateCardTypes(CardBid[] cardBids, bool isPart2) {
            Dictionary<int, List<CardBid>> types = new Dictionary<int, List<CardBid>>{
                {1, new List<CardBid>()},
                {2, new List<CardBid>()},
                {3, new List<CardBid>()},
                {4, new List<CardBid>()},
                {5, new List<CardBid>()},
                {6, new List<CardBid>()},
                {7, new List<CardBid>()}
            };
            for (int i = 0; i < cardBids.Length; i++) {
                types[CalculateType(cardBids[i].card, isPart2)].Add(cardBids[i]);
            }
            return types;
        }
        public override int Part1() {
            Dictionary<int, List<CardBid>> types = CreateCardTypes(cards, false);
            return CalculateSum(types, false);
        }
        public override int Part2() {
            Dictionary<int, List<CardBid>> types = CreateCardTypes(cards, true);
            return CalculateSum(types, true);
        }
    }
}