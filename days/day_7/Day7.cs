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
        private void QuickSort(CardBid[] arr, int start, int end, bool isPart2) {
            if (end <= start) return;
            int pivot = Partition(arr, start, end, isPart2);
            QuickSort(arr, start, pivot - 1, isPart2);
            QuickSort(arr, pivot + 1, end, isPart2);
        }
        private int Partition(CardBid[] arr, int start, int end, bool isPart2) {
            CardBid pivotVal = arr[end], temp;
            int i = start;
            for (int j = start; j < end; j++) {
                if (LessThan(arr[j].card, pivotVal.card, isPart2)) {
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    i++;
                }
            }
            temp = arr[i];
            arr[i] = pivotVal;
            arr[end] = temp;
            return i;
        }
        private bool LessThan(string a, string b, bool isPart2) {
            int typeA = CalculateType(a, isPart2), typeB = CalculateType(b, isPart2);
            if (typeA == typeB) {
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
                if (isPart2) strengths['J'] = 1;
                for (int i = 0; i < a.Length; i++) { // assume a.Length == b.Length
                    if (a[i] != b[i]) return strengths[a[i]] < strengths[b[i]];
                }
            }
            return typeA < typeB;
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
        private int CalculateSum(CardBid[] cardBids) {
            int sum = 0;
            for (int i = 0; i < cardBids.Length; i++) {
                sum += (i + 1) * cardBids[i].bid;
            }
            return sum;
        }
        public override int Part1() {
            // we make a copy so that the original data is unsorted, to prevent Part2 from being slower, as quicksort is not very good with nearly sorted data
            CardBid[] cardBids = new CardBid[cards.Length];
            for (int i = 0; i < cards.Length; i++) {
                cardBids[i] = new CardBid(cards[i].card, cards[i].bid);
            }
            
            QuickSort(cardBids, 0, cardBids.Length - 1, false);
            return CalculateSum(cardBids);
        }
        public override int Part2() {
            QuickSort(cards, 0, cards.Length - 1, true);
            return CalculateSum(cards);
        }
    }
}