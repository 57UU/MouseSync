//using Newtonsoft.Json;
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
        //Console.WriteLine("Configuration loading");
        load();
    }
    private const string CLIENT_SETTING = "Setting.config";
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
    public string Server_IP { get; set; } = null;
    public int Boardcast_Port { get; set; } = 4756;
    public bool isServerUnset { get { return Server_IP == null; } }

    const string ip = "Server_IP";
    const string port = "Server_Port";
    const string boardcastPort = "Boardcast_Port";
    public static Exception? save()
    {
        try
        {
            string text=null;

            //text = JsonConvert.SerializeObject(instance);
            text = Configuration.Config.Serialize(new Dictionary<string, object>
            {
                {ip,instance.Server_IP },
                {port,instance.Server_Port },
                {boardcastPort,instance.Boardcast_Port}
            });

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

            /*            Info obj = JsonConvert.DeserializeObject<Info>(text);
                        instance = obj;*/
            new Info();
            var dict=Configuration.Config.Deserialize(text);
            instance.Server_IP = dict[ip];
            instance.Server_Port = int.Parse(dict[port]);
            instance.Boardcast_Port = int.Parse(dict[boardcastPort]);

            return null;
        }catch(IOException ex)
        {
            new Info();
            save();
            return ex;
        }
        catch (Exception ex)
        {
            new Info();
            //save();
            return ex;
        }

    }

}
