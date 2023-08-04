using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseSync
{
    public partial class InputBox : Form
    {
        public delegate void InputHandler(string text);
        InputHandler clicked;
        public string Hint { set { label1.Text = value; } get { return label1.Text; } }
        public InputBox(InputHandler clicked)
        {
            this.clicked = clicked;
            InitializeComponent();
        
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.FormClosing -= InputBox_FormClosing;
            clicked(textBox1.Text);
            this.Dispose();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {

        }

        private void InputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            button1_Click(null, null);
        }
    }
}
