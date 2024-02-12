using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileMouse;

public static class Utils
{
    public static string getIPAddress()
    {
        List<IPAddress> result = new();
        try
        {
            var upAndNotLoopbackNetworkInterfaces = 
                NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            foreach (var networkInterface in upAndNotLoopbackNetworkInterfaces)
            {
                var iPInterfaceProperties = networkInterface.GetIPProperties();

                var unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork);
                if (unicastIpAddressInformation == null) continue;

                result.Add(unicastIpAddressInformation.Address);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to find IP: {ex.Message}");
        }

        foreach (var item in result)
        {
            var r = item.ToString();
            if (r.Contains("192.168"))
            {
                return r;
            }
            
        }
        return result[0].ToString();


    }
}
