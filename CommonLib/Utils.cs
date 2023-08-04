using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync;

public static class Utils
{

    public static string readFile(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch {
            return null;
        }
        
    }
    public static bool writeFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            return true;
        }catch
        {
            return false;
        }
    }

}
