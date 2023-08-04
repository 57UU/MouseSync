using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseSync.Server;

public partial class Server : Form
{

    public static Server instance;

    public Server()
    {
        InitializeComponent();
        instance = this;
    }
    List<ClientPC> clients = new List<ClientPC>();
    public readonly object globalLock = new();
    public void addClient(ClientPC pc)
    {
        lock (globalLock)
        {
            clients.Add(pc);
        }
        refresh();
    }
    void refresh()
    {
        lock (globalLock)
        {
            listView1.Items.Clear();
            foreach (ClientPC pc in clients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = pc.Name;
                item.SubItems.Add(pc.Resolution);
                item.SubItems.Add(pc.IP);
                listView1.Items.Add(item);
            }
        }

    }
    private void Server_Load(object sender, EventArgs e)
    {
        textBox1.Text = Info.instance.Boardcast_Port.ToString();
        trackBar1.Value = MouseHook.instance.maxCount;
        updateRateLable();
        MouseHook.addCallback(mouseHandler);

    }
    private void mouseHandler(object sender, MSLLHOOKSTRUCT e)
    {

        foreach (ClientPC pc in clients)
        {
            pc.sendLocation(e);
        }
    }


    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Info.save();
    }

    private void testHookToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new TestHook().Show();

    }

    private void changeMouseMovingScanRateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //new ChangeScanRate().Show();
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        MouseHook.instance.maxCount = trackBar1.Value;
        updateRateLable();
    }
    void updateRateLable()
    {
        label4.Text = $"Only 1/{MouseHook.instance.maxCount} of move\n event will be sent";
    }
}
public class ClientPC
{
    public Connection Connection { get; private set; }
    public ClientPC(string name, string resolution, string iP)
    {
        //only for test
        Name = name;
        Resolution = resolution;
        IP = iP;
        Server.instance.addClient(this);
    }
    public ClientPC(TcpClient tcp)
    {
        this.tcp = tcp;
        this.IP = ((IPEndPoint)(tcp.Client.RemoteEndPoint)).Address.ToString();
        Server.instance.addClient(this);
        Connection = new(tcp);
        Connection.messageHander = received;
    }


    //this is a call back
    public void received(string msg)
    {
        var splited = msg.Split(':');
        if (splited.Length > 0)
        {
            if (splited[0] == DataExchange.RESOLUTION)
            {
                Resolution = splited[1];
            }
            else if (splited[0] == DataExchange.NAME)
            {
                Name = splited[1];
            }
        }
    }
    //send data format: (x:y:mousedata:flags)
    public void sendLocation(MSLLHOOKSTRUCT e)
    {
        Connection.send($"{e.pt.X}:{e.pt.Y}:{e.mouseData}:{e.flags}");
    }

    public string Name { get; set; }
    public string Resolution { get; set; }
    public string IP { get; set; }
    public TcpClient tcp { get; set; }

}