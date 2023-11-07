public class DisplayParameters
{
    public List<List<string>> FrequentItemsets { get; set; }
    public List<List<string>> Transactions { get; set; }
    public Dictionary<string, int> ItemMap { get; set; }
}

class AprioriAlgorithm
{
    public List<List<string>> GenerateFrequentItemsets(List<List<string>> transactions, double minSupport)
    {
        Dictionary<string, int> candidateSet = GetInitialCandidateSet(transactions);
        List<List<string>> frequentItemsets = new List<List<string>>();

        int transactionCount = transactions.Count;
        int minSupportCount = (int)(minSupport * transactionCount);

        while (candidateSet.Count > 0)
        {
            candidateSet = PruneCandidateSet(candidateSet, minSupportCount);

            foreach (var transaction in transactions)
            {
                foreach (var candidate in candidateSet.Keys.ToList())
                {
                    var items = candidate.Split(',');
                    if (items.All(transaction.Contains))
                    {
                        candidateSet[candidate]++;
                    }
                }
            }

            frequentItemsets.AddRange(candidateSet.Keys.Select(item => item.Split(',').ToList()));

            candidateSet = GenerateNextCandidateSet(candidateSet);
        }

        return frequentItemsets;
    }

    private Dictionary<string, int> GetInitialCandidateSet(List<List<string>> transactions)
    {
        Dictionary<string, int> candidateSet = new Dictionary<string, int>();

        foreach (var transaction in transactions)
        {
            foreach (var item in transaction)
            {
                if (!candidateSet.ContainsKey(item))
                {
                    candidateSet[item] = 1;
                }
                else
                {
                    candidateSet[item]++;
                }
            }
        }

        return candidateSet;
    }

    private Dictionary<string, int> PruneCandidateSet(Dictionary<string, int> candidateSet, int minSupportCount)
    {
        return candidateSet.Where(pair => pair.Value >= minSupportCount).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private Dictionary<string, int> GenerateNextCandidateSet(Dictionary<string, int> prevCandidateSet)
    {
        Dictionary<string, int> nextCandidateSet = new Dictionary<string, int>();

        foreach (var item1 in prevCandidateSet.Keys)
        {
            foreach (var item2 in prevCandidateSet.Keys)
            {
                string[] items1 = item1.Split(',');
                string[] items2 = item2.Split(',');

                if (items1.Take(items1.Length - 1).SequenceEqual(items2.Take(items2.Length - 1)) && String.Compare(items1.Last(), items2.Last()) < 0)
                {
                    string newItemset = string.Join(",", items1.Concat(new[] { items2.Last() }));
                    nextCandidateSet[newItemset] = 0;
                }
            }
        }

        return nextCandidateSet;
    }
}
class Program
{
    static void Main()
    {
        List<List<string>> transactions = new List<List<string>>
        {
            new List<string> { "l1", "l2", "l4", "l5" },
            new List<string> { "l2", "l4" },
            new List<string> { "l2", "l3"},
            new List<string> { "l1", "l2", "l4" },
            new List<string> { "l1", "l3"},
            new List<string> { "l2", "l3"},
            new List<string> { "l1", "l3" },
            new List<string> { "l1", "l2", "l3", "l5" },
            new List<string> { "l1", "l2", "l3" }
        };

        List<string> uniqueItems = transactions.SelectMany(t => t).Distinct().OrderBy(item => item).ToList();
        Dictionary<string, int> itemMap = uniqueItems.Select((item, index) => new { item, index }).ToDictionary(pair => pair.item, pair => pair.index);

        List<List<string>> transactionsWithNumbers = transactions.Select(t => t).ToList();

        AprioriAlgorithm apriori = new AprioriAlgorithm();
        List<List<string>> frequentItemsets = apriori.GenerateFrequentItemsets(transactionsWithNumbers, 0.4);

        DisplayParameters displayParams = new DisplayParameters
        {
            FrequentItemsets = frequentItemsets,
            Transactions = transactions,
            ItemMap = itemMap
        };

        DisplayFrequentItemsets(displayParams);
    }

    static void DisplayFrequentItemsets(DisplayParameters parameters)
    {
        Console.WriteLine("Frequent Itemsets:");

        int currentLength = 1;
        foreach (var itemset in parameters.FrequentItemsets)
        {
            List<int> itemNumbers = itemset.Select(item => int.Parse(item.Substring(1))).ToList();
            itemNumbers.Sort();
            List<string> sortedItemset = itemNumbers.Select(num => "l" + num).ToList();

            if (sortedItemset.Count != currentLength)
            {
                currentLength = sortedItemset.Count;
                Console.WriteLine($"C{currentLength}");
            }

            int count = CountItemsetOccurrences(parameters.Transactions, sortedItemset);

            if (count == 0)
            {
                Console.WriteLine($"{string.Join(" - ", sortedItemset)} {count} taiem");
            }
            else
            {
                Console.WriteLine($"{string.Join(" - ", sortedItemset)} {count}");
            }
        }
    }


    static int CountItemsetOccurrences(List<List<string>> transactions, List<string> itemset)
    {
        int count = 0;

        foreach (var transaction in transactions)
        {
            if (itemset.All(item => transaction.Contains(item)))
            {
                count++;
            }
        }

        return count;
    }
}

