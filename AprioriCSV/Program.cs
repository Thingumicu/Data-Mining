using Microsoft.VisualBasic.FileIO;

public class Algoritm
{
    public static string defPath = @"C:\\Users\\Radu\\source\\repos\\Data Mining\\AprioriCSV\\test_59_2.csv";
    public static int defPrag = 30;
    static void Main()
    {
        Console.WriteLine("Introduceti calea fisierului de procesat:");
        
        Console.WriteLine("Numarul de aparitii trebuie sa fie  >= " + defPrag);

        Console.WriteLine("Cale default ----> " + verificaIntrare(defPath));

        string filePath = Console.ReadLine();

        filePath = verificaIntrare(filePath);

        DateFisier();

        if (File.Exists(filePath))
        {
            Dictionary<string, int> dictionar = new Dictionary<string, int>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    for (int i = 1; i < fields.Length; i++)
                    {
                        if (fields[i] == "?")
                        {
                            fields[i] = "-";
                        }

                        if (!dictionar.ContainsKey(fields[i]))
                        {
                            if (fields[i] != "-")
                            {
                                dictionar[fields[i]] = 1;
                            }
                        }
                        else
                        {
                            if (fields[i] != "-")
                            {
                                dictionar[fields[i]]++;
                            }

                        }

                        Console.Write(fields[i].PadLeft(5));
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nApasa orice pentru C1");
            Console.ReadKey();
            deleteLine();

            var dictionar_sortat = dictionar.OrderBy(pair =>
            {
                string key = pair.Key;
                if (key[0] == 'P' && int.TryParse(key.Substring(1), out int number))
                {
                    return number;
                }
                return int.MaxValue;
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            Console.WriteLine("\nC1\n");

            foreach (var key in dictionar_sortat.Keys)
            {
                Console.WriteLine($"{key}: {dictionar_sortat[key]}");
            }

            Console.WriteLine("\nApasa orice pentru L1");
            Console.ReadKey();
            deleteLine();

            Console.WriteLine("\nL1\n");

            var dictionar_filtrat = dictionar_sortat.Where(pair => pair.Value >= defPrag).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var key in dictionar_filtrat.Keys)
            {
                Console.WriteLine($"{key}: {dictionar_filtrat[key]}");
            }

            Console.WriteLine("\nApasa orice pentru C2");
            Console.ReadKey();
            deleteLine();

            Console.WriteLine("\nC2 and L2 not fully developed yet\n");

            List<string> productKeys_filtered = new List<string>(dictionar_filtrat.Keys);
            Dictionary<string, int> combinationsCount_filtered = new Dictionary<string, int>();

            for (int i = 0; i < productKeys_filtered.Count - 1; i++)
            {
                for (int j = i + 1; j < productKeys_filtered.Count; j++)
                {
                    string combination = $"{productKeys_filtered[i]},{productKeys_filtered[j]}";

                    if (!combinationsCount_filtered.ContainsKey(combination))
                    {
                        combinationsCount_filtered[combination] = 1;
                    }
                    else
                    {
                        combinationsCount_filtered[combination]++;
                    }
                }
            }

            foreach (var key in combinationsCount_filtered.Keys)
            {
                    Console.WriteLine($"{key}: {combinationsCount_filtered[key]}");
            }

            Console.WriteLine("something else");

            List<string> filteredProducts = dictionar_filtrat.Keys.ToList();
            List<string> pairs = new List<string>();

            for (int i = 0; i < filteredProducts.Count - 1; i++)
            {
                for (int j = i + 1; j < filteredProducts.Count; j++)
                {
                    pairs.Add($"{filteredProducts[i]},{filteredProducts[j]}");
                }
            }

            foreach (var pair in pairs)
            {
                Console.WriteLine(pair);
            }



            closeProgram();
        }
        else
        {
            Console.WriteLine("Invalid file path. Please make sure the file exists.");
            closeProgram();
        }
    }

    private static string verificaIntrare(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = defPath;
        }

        return filePath;
    }

    static void closeProgram()
    {
        Console.WriteLine("Press any key to close this window . . .");
        Console.ReadKey();
    }

    static void deleteLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.WindowWidth));
    }
    static void DateFisier()
    {
        string dateFisier =
        "  _____            _              __   _         _               \n" +
        " |  __ \\          | |            / _| (_)       (_)              \n" +
        " | |  | |   __ _  | |_    ___   | |_   _   ___   _    ___   _ __  \n" +
        " | |  | |  / _` | | __|  / _ \\  |  _| | | / __| | |  / _ \\ | '__| \n" +
        " | |__| | | (_| | | |_  |  __/  | |   | | \\__ \\ | | |  __/ | |    \n" +
        " |_____/   \\__,_|  \\__|  \\___|  |_|   |_| |___/ |_|  \\___| |_|    \n" +
        "                                                                  \n";

        Console.WriteLine(dateFisier);
    }

}