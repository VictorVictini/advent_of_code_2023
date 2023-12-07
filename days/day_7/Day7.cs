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
        struct CardType {
            public string card;
            public int bid, type;
            public CardType(CardBid cardBid, int type) {
                card = cardBid.card;
                bid = cardBid.bid;
                this.type = type;
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
        private void QuickSort(CardType[] arr, int start, int end, bool isPart2) {
            if (end <= start) return;
            int pivot = Partition(arr, start, end, isPart2);
            QuickSort(arr, start, pivot - 1, isPart2);
            QuickSort(arr, pivot + 1, end, isPart2);
        }
        private int Partition(CardType[] arr, int start, int end, bool isPart2) {
            CardType pivotVal = arr[end], temp;
            int pivot = start;
            for (int i = start; i < end; i++) {
                if (LessThan(arr[i], pivotVal, isPart2)) {
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
        private bool LessThan(CardType a, CardType b, bool isPart2) {
            if (a.type == b.type) {
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
                for (int i = 0; i < a.card.Length; i++) { // assume a.card.Length == b.card.Length
                    if (a.card[i] != b.card[i]) return strengths[a.card[i]] < strengths[b.card[i]];
                }
            }
            return a.type < b.type;
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
        private int CalculateSum(CardType[] cardTypes) {
            int sum = 0;
            for (int i = 0; i < cardTypes.Length; i++) {
                sum += (i + 1) * cardTypes[i].bid;
            }
            return sum;
        }
        private CardType[] CreateCardTypes(CardBid[] cardBids, bool isPart2) {
            CardType[] cardTypes = new CardType[cardBids.Length];
            for (int i = 0; i < cardBids.Length; i++) {
                cardTypes[i] = new CardType(cardBids[i], CalculateType(cardBids[i].card, isPart2));
            }
            return cardTypes;
        }
        public override int Part1() {
            CardType[] cardTypes = CreateCardTypes(cards, false);
            QuickSort(cardTypes, 0, cardTypes.Length - 1, false);
            return CalculateSum(cardTypes);
        }
        public override int Part2() {
            CardType[] cardTypes = CreateCardTypes(cards, true);
            QuickSort(cardTypes, 0, cardTypes.Length - 1, true);
            return CalculateSum(cardTypes);
        }
    }
}