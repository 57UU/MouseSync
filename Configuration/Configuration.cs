using System.Text;

namespace Configuration;
public static class Config
{
    public static string SplitSymbol { get; set; } = ":";
    public static Dictionary<string, string> Deserialize(string text)
    {
        Dictionary<string,string> dict = new Dictionary<string,string>();
        var lines = text.Split("\n");
        foreach(string i in lines)
        {
            if(string.IsNullOrEmpty(i)) continue;
            string[] splited = i.Split(SplitSymbol);
            dict[splited[0]] = splited[1];
        }
        return dict;
    }
    public static string Serialize(Dictionary<string, object> dict)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(var i in dict)
        {
            object value = i.Value;
            if(value == null)
            {
                value = "";
            }
            stringBuilder.Append($"{i.Key}{SplitSymbol}{value.ToString()}\n");
        }
        return stringBuilder.ToString();
    }
    public static void Test()
    {
        //Test 
        Dictionary<string, object> dict = new()
        {
            { "Name", null },
            { "age", "123" }
        };
        string text=Serialize(dict);
        Console.WriteLine(text);
        var s=Deserialize(text);
        foreach (var kvp in dict)
        {
            Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
    }
}