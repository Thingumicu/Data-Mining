using System.Text.RegularExpressions;
using System.Xml;
using WordFrequency;

public class XML
{
    public static void Main()
    {
        var training = @"C:\ReutersDataSet\Reuters\Reuters_34\Training";
        var testing = @"C:\ReutersDataSet\Reuters\Reuters_34\Testing";

        string[] trainingAll = Directory.GetFiles(training, "*NEWS.xml");
        string[] testingAll = Directory.GetFiles(testing, "*NEWS.xml");

        string[] all = trainingAll.Concat(testingAll).ToArray();

        //foreach (string filePath in testingAll)
        //{
        //    processFile(filePath);
        //}

        //foreach (string filePath in trainingAll)
        //{
        //    processFile(filePath);
        //}

        foreach (string filePath in all)
        {
            processFile(filePath);
        }
    }

    private static void processFile(string filePath)
    {
        XmlDocument xml = new XmlDocument();
        PorterStemmer stemmer = new PorterStemmer();
        xml.Load(filePath);

        XmlNodeList titles = xml.GetElementsByTagName("title");
        var title = titles[0].InnerText;
        title = stemmer.StemWord(removeNonLetter(title)).ToLower();

        XmlNodeList texts = xml.GetElementsByTagName("text");
        var text = texts[0].InnerText;
        text = stemmer.StemWord(removeNonLetter(text)).ToLower();

        XmlNodeList metadatas = xml.GetElementsByTagName("metadata");
        var metadata = metadatas[0];
        XmlNodeList codeNodes = metadata.SelectNodes("//codes/code");

        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine(text);
        Console.WriteLine();
        foreach (XmlNode codeNode in codeNodes)
        {
            string code = codeNode.Attributes["code"].Value;
            code = stemmer.StemWord(removeNonLetter(code)).ToLower();

            string action = codeNode.SelectSingleNode("editdetail").Attributes["action"].Value;
            action = stemmer.StemWord(removeNonLetter(action)).ToLower();

            string date = codeNode.SelectSingleNode("editdetail").Attributes["date"].Value;
            date = stemmer.StemWord(removeNonLetter(date)).ToLower();

            Console.WriteLine(code);
            Console.WriteLine(action);
            Console.WriteLine(date);
        }
    }

    static string removeNonLetter(string input)
    {
        return Regex.Replace(input, "[^a-zA-Z]+", " ");
    }
}