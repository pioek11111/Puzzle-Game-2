using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Lab03_a
{
    public partial class Form1 : Form
    {
        int[,] arr;        
        int[,] gamerClicks;
        int lifes;
        int currLifes;
        int gameTime;
        int score;
        int marked;
        int toMarked;
        bool winBefore = false;
        bool game;
        LinkedList<Timer> queueOfTimers;
        LinkedList<Button> queueOfButtons;
        //Timer t;
        //Button red;
        public Form1()
        {
            InitializeComponent();
            //t = new Timer();
            queueOfTimers = new LinkedList<Timer>();
            queueOfButtons = new LinkedList<Button>();
            lifes = 3;
            gameTime = 10000;
            arr = new int[5, 5];
            gamerClicks = new int[5, 5];
            game = true;
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    Button button = c as Button;
                    button.Enabled = false;
                    button.BackColor = Color.RoyalBlue;
                    button.Text = "?";
                    button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    button.Font = new Font("", 16, FontStyle.Regular);
                    button.MouseDown += mouseClicked;
                    button.MouseEnter += mouseEnter;
                    button.MouseLeave += mouseLeave;
                }
                if (c.GetType() == typeof(Label))
                {
                    Label l = c as Label;
                    l.Text = "";
                }
            }
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.BackColor != Color.Black && button.BackColor != Color.Red && button.BackColor != Color.White)
            {
                button.BackColor = Color.RoyalBlue;
                button.Text = "?";
            }
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "";
            if (button.BackColor == Color.RoyalBlue) button.BackColor = Color.Yellow;
        }     
        private void mouseClicked(object sender, MouseEventArgs me)
        {
            if (gameMenuItem.Checked && game)  // game mode
            {
                timer1.Start();
                Button button = sender as Button;
                if (me.Button == MouseButtons.Left)
                {
                    var c = tableLayoutPanel1.GetPositionFromControl(button);
                    if (arr[c.Row, c.Column] == 1) // mozna pokolorowac
                    {
                        button.BackColor = Color.Black;
                        gamerClicks[c.Row, 0]++;
                        gamerClicks[0, c.Column]++;
                        gamerClicks[c.Row, c.Column] = 1;
                        score += 50;
                        pointsLebel.Text = "Score: " + score.ToString();
                        marked++;
                        if (marked == toMarked)
                        {
                            score += 500;
                            winBefore = true;
                            newGame();
                        }
                    }
                    else
                    {
                        if (arr[c.Row, c.Column] == 0)
                        {
                            currLifes--;
                            lifesLebel.Text = "Lifes: " + currLifes.ToString();
                            if (currLifes == 0)
                            {
                                timer1.Stop();
                                MessageBox.Show("Your score: " + score.ToString(), "Congratulation");
                                game = false;
                                return;
                            }
                            lifesLebel.Text = "Lifes: " + currLifes.ToString();
                            button.BackColor = Color.Red;
                            button.Text = "";
                            //
                            Timer tq = new Timer();
                            tq.Interval = 500;
                            queueOfTimers.AddLast(tq);
                            queueOfButtons.AddLast(button);
                            tq.Start();
                            tq.Tick += waitHalfSec;
                        }
                    }
                }
                if (me.Button == MouseButtons.Right && button.BackColor != Color.Black && game)
                {
                    var c = tableLayoutPanel1.GetPositionFromControl(button);
                    button.BackColor = Color.White;
                    button.Text = "";
                    gamerClicks[c.Row, 0]--;
                    gamerClicks[0, c.Column]--;
                }
            }
            else // edit mode
            {
                if (game)
                {
                    Button button = sender as Button;
                    var c = tableLayoutPanel1.GetPositionFromControl(button);
                    if (me.Button == MouseButtons.Left && button.BackColor!= Color.Black)
                    {
                        button.BackColor = Color.Black;
                        arr[c.Row, c.Column] = 1;
                        arr[0, c.Column]++;
                        arr[c.Row, 0]++;
                        toMarked++;
                    }
                    if(me.Button == MouseButtons.Right)
                    {
                        button.BackColor = Color.White;
                        if (arr[c.Row, c.Column] == 1)
                        {
                            arr[c.Row, c.Column] = 0;
                            arr[0, c.Column]--;
                            arr[c.Row, 0]--;
                            toMarked--;
                        }
                    }
                }
            }
        }
        private void waitHalfSec(object sender, EventArgs e)
        {
            if (queueOfButtons.Count != 0)
            {
                Button b = queueOfButtons.First();
                Timer t = queueOfTimers.First();
                t.Stop();
                queueOfButtons.RemoveFirst();
                queueOfTimers.RemoveFirst();
                b.BackColor = Color.RoyalBlue;
                b.Text = "?";
            }
        }
        
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            const string message = "Are you sure?";
            const string caption = "Confirmation";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer t = sender as Timer;
            if (progressBar1.Value - t.Interval >= 0) progressBar1.Value -= t.Interval;
            else
            {
                t.Stop();
                game = false;
                //disableButtons();
                MessageBox.Show( "Your score: " + score.ToString(), "Congratulation");
            }
        }
        private void disableButtons()
        {
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    Button button = c as Button;
                    button.Enabled = false;
                }
            }
        }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }
        private void newGame(bool randGame = true, bool showLabels = true)
        {
            game = true;
            if (!winBefore) score = 0;
            pointsLebel.Text = "Score: " + score.ToString();
            lifesLebel.Text = "Lifes: " + lifes.ToString();
            currLifes = lifes;
            if(randGame) arr = new int[5, 5];
            gamerClicks = new int[5, 5];
            winBefore = false;
            marked = 0;
            if (randGame) toMarked = 0;
            progressBar1.Maximum = gameTime;
            progressBar1.Value = gameTime;
            Random rand = new Random();
            if (randGame)
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        int r = rand.Next(0, 2);
                        arr[i, j] = r;
                        if (r == 1)
                        {
                            arr[0, j]++;
                            arr[i, 0]++;
                            toMarked++;
                        }
                    }
                }
            }
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(Label) && showLabels)
                {
                    Label l = c as Label;
                    var idx = tableLayoutPanel1.GetPositionFromControl(l);
                    c.Text = arr[idx.Row, idx.Column].ToString();
                    //label[idx.Row, idx.Column] = r;                
                }
                if (c.GetType() == typeof(Button))
                {
                    Button button = c as Button;
                    button.BackColor = Color.RoyalBlue;
                    button.Text = "?";
                    button.Enabled = true;
                    //button.BackColor = this.BackColor;
                }
            }
        }
        //
        // setings
        //
        private void setingsToolStripMenuItem_Click(object sender, EventArgs e) 
        {            
            Form2 f = new Form2();
            timer1.Stop();
            f.KeyDown += keyDown;
            f.KeyDown += keyDown;
            f.NumGameTime = gameTime / 1000;
            f.NumLifes = lifes;
            f.ShowDialog();
            game = false;
            if (f.DialogResult == DialogResult.OK)
            {
                lifes = (int)f.NumLifes;
                gameTime = (int)f.NumGameTime*1000; 
                
                //newGame(false,false);
            }
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            Form2 f = sender as Form2;
            if (e.KeyCode == Keys.Enter)
            {
                lifes = (int)f.NumLifes;
                gameTime = (int)f.NumGameTime * 1000;
                f.Close();
                newGame(false,false);
            }
            if(e.KeyCode == Keys.Escape)
            {
                f.Close();
            }
        }

        private void gameMenuItem_Click(object sender, EventArgs e)
        {
            newGame(false);
            menuStrip1.BackColor = Color.Yellow;
            editMenuItem.Checked = false;
            menuStrip1.BackColor = SystemColors.Control;
            newGameToolStripMenuItem.Enabled = true;
            setingsToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = false;
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            game = true;
            arr = new int[5, 5];
            toMarked = 0;
            newGameToolStripMenuItem.Enabled = false;
            setingsToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = true;
            gameMenuItem.Checked = false;
            menuStrip1.BackColor = Color.Yellow;
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(Label))
                {
                    Label l = c as Label;
                    var idx = tableLayoutPanel1.GetPositionFromControl(l);
                    c.Text = "";               
                }
                if (c.GetType() == typeof(Button))
                {
                    Button button = c as Button;
                    button.BackColor = Color.White;
                    button.Text = "";
                    button.Enabled = true;
                }
            }
        }
        private void save()
        {
            string s = null;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    s += arr[i, j].ToString();
                }
            }
            
            DialogResult result = saveFileDialog1.ShowDialog();
            string file = saveFileDialog1.FileName;
            if (result == DialogResult.OK) // Test result.
            {                
                try
                {
                    File.WriteAllText(file, s);
                }
                catch (IOException)
                {
                }
            }
            
        }
        private void open()
        {
            DialogResult result = openFileDialog1.ShowDialog();
            string line = null;
            if (result == DialogResult.OK)
            {
                try
                {
                    string file = openFileDialog1.FileName;
                    using (StreamReader sr = new StreamReader(file))
                    {
                        line = sr.ReadToEnd();
                        Console.WriteLine(line);
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
                int length = line.Length;
                length--;
                toMarked = 0;
                for (int i = 4; i >= 0; i--)
                {
                    for (int j = 4; j >= 0; j--)
                    {
                        //char c = line[length--];
                        arr[i, j] = (int)char.GetNumericValue(line[length--]);
                        if (i != 0 && j != 0 && arr[i, j] == 1) toMarked++;
                        //Int32.TryParse(c.ToString(), out arr[i, j]);
                    }
                }
                newGame(false);
            }            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
