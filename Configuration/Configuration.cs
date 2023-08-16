using System.Text;

namespace Configuration;
public class Config
{
    public static string SplitSymbol { get; set; } = ":";
    public Dictionary<string, string> Serialize(string text)
    {
        Dictionary<string,string> dict = new Dictionary<string,string>();
        foreach(string i in text.Split("\n"))
        {
            if(string.IsNullOrEmpty(i)) continue;
            string[] splited = i.Split(SplitSymbol);
            dict[splited[0]] = splited[1];
        }
        return dict;
    }
    public string Deserialize(Dictionary<string, string> dict)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(var i in dict)
        {
            stringBuilder.Append($"{i.Key}{SplitSymbol}{i.Value}\n");
        }
        return stringBuilder.ToString();
    }
    public static void Main(string[] args)
    {

    }
}