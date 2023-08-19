using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsHID;

namespace MouseSync.Server
{
    public partial class TestHook : Form
    {
        public TestHook()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            this.Disposed += (s, e) => { MouseHook.MouseAction -= callback;KeyboardHook.KeyboardAction -= callbackKeyboard; };
        }
        

        private void TestHook_Load(object sender, EventArgs e)
        {
            MouseHook.addCallback(callback);
            KeyboardHook.addCallback(callbackKeyboard);
        }
        private void callbackKeyboard(object s,KeyboardInputData data)
        {
            richTextBox1.Text += Utils.format(
                DataExchange.KEY,
                data.code
                ) + "\n";
        }
        private void callback(object sender,MouseInputData e)
        {
            richTextBox1.Text += Utils.format(
            DataExchange.MOUSE,
            ((int)e.mouseMessage),
            e.hookStruct.pt.X,
            e.hookStruct.pt.Y,
            e.hookStruct.mouseData
            )+"\n";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }
    }
}
