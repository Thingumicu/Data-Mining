using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriCSV
{
    internal class afisare
    {
        static void afisareTabel(string filePath)
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
                        bool isDash = fields[i] == "-";
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

                        Console.Write(fields[i] + " ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
