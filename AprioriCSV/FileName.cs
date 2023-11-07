using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriCSV
{
    internal class FileName
    {
        static Dictionary<string, int> itemseturiNoiSortate(Dictionary<string, int> p, string filePath)
        {
            Dictionary<string, int> itemsets = new Dictionary<string, int>();

            List<string> productKeys = new List<string>(p.Keys);

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                parser.ReadLine(); // Skip the header row

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    List<string> usedProducts = new List<string>();

                    for (int i = 1; i < fields.Length; i++)
                    {
                        bool isDash = fields[i] == "-";
                        if (fields[i] == "?")
                        {
                            fields[i] = "-";
                        }

                        if (!usedProducts.Contains(fields[i]) && !isDash)
                        {
                            usedProducts.Add(fields[i]);
                        }
                    }

                    for (int i = 0; i < usedProducts.Count - 1; i++)
                    {
                        for (int j = i + 1; j < usedProducts.Count; j++)
                        {
                            string itemset = $"{usedProducts[i]},{usedProducts[j]}";

                            if (!itemset.Contains("-"))
                            {
                                if (!itemsets.ContainsKey(itemset))
                                {
                                    itemsets[itemset] = 0;
                                }

                                itemsets[itemset]++;
                            }
                        }
                    }
                }
            }
            itemsets = itemsets.Where(pair => !pair.Key.Contains("-")).ToDictionary(pair => pair.Key, pair => pair.Value);

            var sortedItemsets = itemsets.OrderBy(pair =>
            {
                string key = pair.Key;
                string[] keys = key.Split(',');

                if (keys.All(k => k[0] == 'P' && int.TryParse(k.Substring(1), out int number)))
                {
                    int firstNumber = int.Parse(keys[0].Substring(1));
                    int secondNumber = int.Parse(keys[1].Substring(1));
                    return firstNumber != secondNumber ? firstNumber.CompareTo(secondNumber) : keys[0].CompareTo(keys[1]);
                }
                return int.MaxValue;
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedItemsets;
        }

        static void Mains()
        {

            //Dictionary<string, int> itemsets = itemseturiNoiSortate(dictionar_sortat,filePath);

            //foreach (var key in itemsets.Keys)
            //{
            //    Console.WriteLine($"{key} {itemsets[key]}");
            //}

            //Dictionary<string, int> twoItemsets = Generate2Itemsets(dictionar_sortat,filePath);

            //foreach (var key in twoItemsets.Keys)
            //{
            //    Console.WriteLine($"{key} {twoItemsets[key]}");
            //}

        }
    }
}
