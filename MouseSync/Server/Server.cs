using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsHID;

namespace MouseSync.Server;

public partial class Server : Form
{

    public static Server instance;
    private NetworkServer server = new(Info.instance.Server_Port);
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
        refreshFromOtherThread();
    }
    public void removeClient(ClientPC pc)
    {
        lock (globalLock)
        {
            clients.Remove(pc);
        }
        refreshFromOtherThread();
    }
    public void refreshFromOtherThread()
    {
        this.Invoke(() =>
        {
            refresh();
        });
    }
    void refresh()
    {
        lock (globalLock)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (ClientPC pc in clients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = pc.Name;
                item.SubItems.Add(pc.Resolution);
                item.SubItems.Add(pc.IP);
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();
            label2.Text = $"Device Connected : {clients.Count} Devices";
        }

    }
    private void Server_Load(object sender, EventArgs e)
    {
        textBox1.Text = Info.instance.Boardcast_Port.ToString();
        trackBar1.Value = MouseHook.instance.maxCount;
        updateRateLable();
        MouseHook.addMouseCallback(mouseHandler);
        server.Start();

    }
    private void mouseHandler(object sender, MouseAllData e)
    {

        foreach (ClientPC pc in clients)
        {
            pc.sendMouse(e);
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

    private void Server_FormClosed(object sender, FormClosedEventArgs e)
    {
        DisposeResource();
    }

    private void addToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new ClientPC("SB", "1270*720", "127.0.0.1");
    }
    private void DisposeResource()
    {
        server.connectionServer.close();
#if DEBUG
        ConsoleHelper.FreeConsole();
#endif
    }

    private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
    {

    }
}
