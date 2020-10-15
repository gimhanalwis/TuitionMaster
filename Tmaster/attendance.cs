using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using MySql.Data.MySqlClient;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace Tmaster
{
    public partial class attendance : Form
    {
        
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public attendance()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void attendance_Load(object sender, EventArgs e)
        {
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

            
           if (textBox1.Text != null && checkBox1.Checked)
            {
                try
                {

                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter("select* from attendance where SID='" + textBox1.Text + "' and Date='"+this.dateTimePicker1.Text+"' ", connection);

                   


                    da.Fill(dt);
                    connection.Open();
                    dataGridView1.DataSource = dt;
                    connection.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error" + ex);
                }
            }
           else if(textBox1.Text!=null && comboBox1.Text != "Month")
            {
                try
                {

                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter("select * from attendance where month(Date)=('"+comboBox1.Text+ "') and SID='"+textBox1.Text+"'", connection);




                    da.Fill(dt);
                    connection.Open();
                    dataGridView1.DataSource = dt;
                    connection.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error" + ex);
                }
            }
            textBox1.SelectAll();


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(id);
            textBox1.SelectAll();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            menu frm = new menu();
            frm.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }
    }
}
