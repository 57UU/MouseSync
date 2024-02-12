//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib;

public class Info
{
    static Info()
    {
        //Console.WriteLine("Configuration loading");
        //load();
    }
    public static string CLIENT_SETTING { set; get; } = "Setting.config";
    private Info() { instance = this; }


    public static Info instance { get; private set; }

    //eg: 127.0.0.1:4567
    public string Server_IP_Port { 
        get 
        { 
            return $"[{Server_IP}]:{Server_Port}";
        }

    }
    public int Server_Port = 34757;
    public string Server_IP  = null;
    public int Boardcast_Port = 34756;
    public int MouseMovingRate = 5;
    public bool IsHideOnStart=true;
    public bool IsEnableBoardcast = true;
    public bool IsRetryInstantly = false;
    public bool IsEnableHotKey = true;


    public bool isServerUnset { get { return string.IsNullOrEmpty(Server_IP); } }

    const string ip = "Server_IP";
    const string port = "Server_Port";
    const string boardcastPort = "Boardcast_Port";
    const string mouseMovingRate = "Mouse_moving_rate";
    const string isHideOnStart = "Hide_on_start";
    const string isEnableBoardcast = "Enable_Boardcast";
    const string isRetryInstantly = "Is_retry_instantly";
    const string isEnableHotKey = "Is_Enable_Hot-Key";


    private static bool isNeedSave=false;
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
                {boardcastPort,instance.Boardcast_Port},
                {mouseMovingRate,instance.MouseMovingRate},
                {isHideOnStart,instance.IsHideOnStart},
                {isEnableBoardcast,instance.IsEnableBoardcast},
                {isRetryInstantly,instance.IsRetryInstantly},
                { isEnableHotKey,instance.IsEnableHotKey},
            });

            Utils.writeFile(CLIENT_SETTING, text);
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }

    }
    private static Dictionary<string, string> temp;
    static void TrySet(ref int verible,string key)
    {
        if(temp != null)
        {
            if (temp.ContainsKey(key))
            {
                int r;
                bool flag = int.TryParse(temp[key],out r);
                if (flag)
                {
                    verible = r;
                }
                else
                {
                    //Error
                    Console.WriteLine($"Key {key} prase error,use default value instead");
                }
            }
            else
            {
                isNeedSave = true;
            }
            
        }
    }
    static void TrySet(ref string verible,string key)
    {
        if (temp != null)
        {
            if (temp.ContainsKey(key))
            {
                verible = temp[key];
            }
            else
            {
                isNeedSave = true;
            }
        }

    }
    
    static void TrySet(ref bool verible,string key)
    {
        if (temp != null)
        {
            if (temp.ContainsKey(key))
            {
                string value = temp[key];
                if (value.ToLower() == "true")
                {
                    verible = true;
                }else if (value.ToLower() == "false")
                {
                    verible = false;
                }
                else
                {
                    Console.WriteLine($"Unknow value <{value}>");
                }
            }
            else
            {
                isNeedSave = true;
            }

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

            temp = dict;

            //instance.Server_IP = dict[ip];
            TrySet(ref instance.Server_IP,ip);
            //instance.Server_Port = int.Parse(dict[port]);
            TrySet(ref instance.Server_Port,port);
            //instance.Boardcast_Port = int.Parse(dict[boardcastPort]);
            TrySet(ref instance.Boardcast_Port,boardcastPort);
            //instance.MouseMovingRate = int.Parse(dict[mouseMovingRate]);
            TrySet(ref instance.MouseMovingRate,mouseMovingRate);
            TrySet(ref instance.IsHideOnStart,isHideOnStart);
            TrySet(ref instance.IsEnableBoardcast,isEnableBoardcast);
            TrySet(ref instance.IsRetryInstantly,isRetryInstantly);
            TrySet(ref instance.IsEnableHotKey,isEnableHotKey);
            if (isNeedSave)
            {
                save();
                isNeedSave=false;
            }

            temp = null;

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
