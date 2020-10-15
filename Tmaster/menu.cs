using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tmaster
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            teachers frm = new teachers();
            frm.Show();
            this.Hide();
        }
        

        private void menu_Load(object sender, EventArgs e)
        {

        }

        private void label12_DoubleClick(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            payment frm = new payment();
            frm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            attendance frm = new attendance();
            frm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            student frm = new student();
            frm.Show();
            this.Hide();
        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            info frm = new info();
            frm.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            mark frm = new mark();
            frm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            report frm = new report();
            frm.Show();
            this.Hide();
        }
    }
}
