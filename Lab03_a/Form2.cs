using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_a
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        

        public Button Button1Prop
        {
            get { return button1; }
        }
        public Button Button2Prop
        {
            get { return button2; }
        }

        public decimal NumLifes
        {
            get
            {
                return numericUpDown1.Value;
            }
            set
            {
                numericUpDown1.Value = value;
            }
        }
        public decimal NumGameTime
        {
            get
            {
                return numericUpDown2.Value;
            }
            set
            {
                numericUpDown2.Value = value;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
