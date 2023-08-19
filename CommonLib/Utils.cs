﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib;

public static class Utils
{

    public static string readFile(string path)
    {
/*        try
        {*/
            return File.ReadAllText(path);
/*        }
        catch(Exception e) {
            return null;
        }*/
        
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
    public static string format(params object[] array)
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < array.Length - 1;i++)
        {
            sb.Append(array[i]);
            sb.Append(DataExchange.SPLIT);
        }
        sb.Append(array[array.Length - 1]);
        return sb.ToString();
    }
    public static string[] split(string msg)
    {
        return msg.Split(DataExchange.SPLIT);
    }
    public static bool isContain(string[] args,object parameter)
    {
        if (args.Contains(parameter.ToString()))
        {
            return true;
        }
        return false;
    }

}
