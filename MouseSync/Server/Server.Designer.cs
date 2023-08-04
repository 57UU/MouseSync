namespace MouseSync.Server
{
    partial class Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            textBox1 = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            optionToolStripMenuItem = new ToolStripMenuItem();
            changeMouseMovingScanRateToolStripMenuItem = new ToolStripMenuItem();
            betaToolStripMenuItem = new ToolStripMenuItem();
            testHookToolStripMenuItem = new ToolStripMenuItem();
            errorProvider1 = new ErrorProvider(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            splitContainer1 = new SplitContainer();
            label4 = new Label();
            trackBar1 = new TrackBar();
            label3 = new Label();
            button1 = new Button();
            label2 = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(136, 24);
            label1.TabIndex = 0;
            label1.Text = "Boardcast Port";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 41);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(165, 30);
            textBox1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionToolStripMenuItem, betaToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1545, 32);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(56, 28);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(149, 34);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeMouseMovingScanRateToolStripMenuItem });
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new Size(87, 28);
            optionToolStripMenuItem.Text = "Option";
            // 
            // changeMouseMovingScanRateToolStripMenuItem
            // 
            changeMouseMovingScanRateToolStripMenuItem.Name = "changeMouseMovingScanRateToolStripMenuItem";
            changeMouseMovingScanRateToolStripMenuItem.Size = new Size(399, 34);
            changeMouseMovingScanRateToolStripMenuItem.Text = "Change Mouse Moving Scan Rate";
            changeMouseMovingScanRateToolStripMenuItem.Click += changeMouseMovingScanRateToolStripMenuItem_Click;
            // 
            // betaToolStripMenuItem
            // 
            betaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { testHookToolStripMenuItem });
            betaToolStripMenuItem.Name = "betaToolStripMenuItem";
            betaToolStripMenuItem.Size = new Size(64, 28);
            betaToolStripMenuItem.Text = "Beta";
            // 
            // testHookToolStripMenuItem
            // 
            testHookToolStripMenuItem.Name = "testHookToolStripMenuItem";
            testHookToolStripMenuItem.Size = new Size(196, 34);
            testHookToolStripMenuItem.Text = "Test Hook";
            testHookToolStripMenuItem.Click += testHookToolStripMenuItem_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.Location = new Point(13, 27);
            listView1.Name = "listView1";
            listView1.Size = new Size(1300, 523);
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Number";
            columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Name";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Resolution";
            columnHeader3.Width = 110;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 32);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(trackBar1);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(label1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(listView1);
            splitContainer1.Size = new Size(1545, 550);
            splitContainer1.SplitterDistance = 216;
            splitContainer1.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 158);
            label4.Name = "label4";
            label4.Size = new Size(15, 24);
            label4.TabIndex = 5;
            label4.Text = " ";
            // 
            // trackBar1
            // 
            trackBar1.LargeChange = 1;
            trackBar1.Location = new Point(0, 113);
            trackBar1.Maximum = 100;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(213, 69);
            trackBar1.TabIndex = 4;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 86);
            label3.Name = "label3";
            label3.Size = new Size(183, 24);
            label3.TabIndex = 3;
            label3.Text = "Mouse Moving Rate";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Bottom;
            button1.Location = new Point(0, 516);
            button1.Name = "button1";
            button1.Size = new Size(216, 34);
            button1.TabIndex = 2;
            button1.Text = "Apply";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(165, 24);
            label2.TabIndex = 4;
            label2.Text = "Device Connected";
            // 
            // Server
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1545, 582);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Server";
            Text = "Server";
            Load += Server_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ErrorProvider errorProvider1;
        private SplitContainer splitContainer1;
        private Label label2;
        private ListView listView1;
        private Button button1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ToolStripMenuItem betaToolStripMenuItem;
        private ToolStripMenuItem testHookToolStripMenuItem;
        private ToolStripMenuItem optionToolStripMenuItem;
        private ToolStripMenuItem changeMouseMovingScanRateToolStripMenuItem;
        private TrackBar trackBar1;
        private Label label3;
        private Label label4;
    }
}