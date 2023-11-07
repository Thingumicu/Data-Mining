public class Apriori
{
    public List<HashSet<int>> generareC1(List<HashSet<int>> tranzactii)
    {
        List<HashSet<int>> C1 = new List<HashSet<int>>();

        foreach (var t in tranzactii)
        {
            foreach (var item in t)
            {
                HashSet<int> itemSet = new HashSet<int> { item };
                C1.Add(itemSet);
            }
        }

        return C1.Distinct().ToList();
    }

    public List<HashSet<int>> itemSeturiFrecvente(List<HashSet<int>> C1, List<HashSet<int>> tranzactii, int suport_minim)
    {
        Dictionary<HashSet<int>, int> aparitii = new Dictionary<HashSet<int>, int>();

        foreach (var t in tranzactii)
        {
            foreach (var candidat in C1)
            {
                if (t.IsSupersetOf(candidat))
                {
                    if (!aparitii.ContainsKey(candidat))
                    {
                        aparitii[candidat] = 1;
                    }
                    else
                    {
                        aparitii[candidat]++;
                    }

                }

            }
        }

        return aparitii.Where(kvp => kvp.Value >= suport_minim).Select(kvp => kvp.Key).ToList();
    }

    public void afisare(List<HashSet<int>> seturi)
    {
        foreach (var set in seturi)
        {
            Console.WriteLine(string.Join(", ", set));
        }
    }
}

class Program
{
    static void Main()
    {
        List<HashSet<int>> tranzactii = new List<HashSet<int>>
        {
            new HashSet<int> { 1,2,5 },
            new HashSet<int> { 2,4 },
            new HashSet<int> { 2,3 },
            new HashSet<int> { 1,2,4 },
            new HashSet<int> { 1,3 },
            new HashSet<int> { 2,3 },
            new HashSet<int> { 1,3 },
            new HashSet<int> { 1,2,3,5 },
            new HashSet<int> { 1,2,3}
        };

        Apriori apriori = new Apriori();
        List<HashSet<int>> C1 = apriori.generareC1(tranzactii);
        List<HashSet<int>> L1 = apriori.itemSeturiFrecvente(C1,tranzactii,2);

        Console.WriteLine("C1:");
        apriori.afisare(C1);

        Console.WriteLine("\nL1:");
        apriori.afisare(L1);
    }
}