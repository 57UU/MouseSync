using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync;

public class Info
{
    static Info()
    {
        load();
    }
    private const string CLIENT_SETTING = "Setting.json";
    private Info() { instance = this; }


    public static Info instance { get; private set; }

    //eg: 127.0.0.1:4567
    public string Server_IP_Port { 
        get 
        { 
            return $"{Server_IP}:{Server_Port}";
        }
        set 
        { 
            var s = value.Split(":");
            Server_IP = s[0];
            Server_Port =int.Parse(s[1]); 
        }
    }
    public int Server_Port { get; set; } = 4757;
    public string Server_IP { get; set; }
    public int Boardcast_Port { get; set; } = 4756;
    public bool isServerUnset { get { return Server_IP == null; } }


    public static Exception? save()
    {
        try
        {
            string text = JsonConvert.SerializeObject(instance);
            Utils.writeFile(CLIENT_SETTING, text);
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }

    }
    public static Exception? load()
    {
        try
        {
            string text = Utils.readFile(CLIENT_SETTING);
            Info obj = JsonConvert.DeserializeObject<Info>(text);
            instance = obj;
            return null;
        }
        catch (Exception ex)
        {
            new Info();
            return ex;
        }

    }

}
