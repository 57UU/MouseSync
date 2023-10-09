using MouseSync.Client;

namespace MouseSync;

public partial class Form1 : Form
{
    string _serverIP = null;
    string ServerIP
    {
        get { return _serverIP; }
        set
        {
            if (!comboBox1.Items.Contains(value))
            {
                comboBox1.Items.Add(value);
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            }

            _serverIP = value;
        }
    }
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        string server = Info.instance.Server_IP_Port;
        if (!string.IsNullOrEmpty(server))
        {
            ServerIP = server;
        }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ServerIP = (string)comboBox1.Items[comboBox1.SelectedIndex];
        
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new AboutBox1().ShowDialog(this);
    }
 
    private void saveServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //Info.instance.Server_IP_Port = ServerIP;
        Info.save();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        new InputBox(t =>
        {
            if (!string.IsNullOrEmpty(t))
            {
                ServerIP = t;
                //comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            }
        })
        { Hint = "eg:127.0.0.1:4567" }.ShowDialog(this);
    }
}