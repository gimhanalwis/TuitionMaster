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
    public partial class info : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public info()
        {
            InitializeComponent();
        }

        private void info_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            connection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("select * from student where SID='" + textBox1.Text + "' ", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                
                string Query = "select * from student where SID='" + textBox1.Text + "'";
                MySqlCommand command = new MySqlCommand(Query, connection);
                reader = command.ExecuteReader();
                reader.Read();
                string Name = reader.GetString("Name");
                string LName = reader.GetString("LName");
                string ID = reader.GetString("ID");
                string DOB = Convert.ToDateTime(reader["DOB"]).ToString("yyyy/MM/dd");
                string Address = reader.GetString("Address");
                string Mobile = reader.GetString("Mobile");
                string Parent = reader.GetString("Parent");

                byte[] img = (byte[])(reader["img"]);
                if (img == null)
                {
                    pictureBox10.Image = null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox10.Image = System.Drawing.Image.FromStream(ms);
                }

                label1.Text = LName;
                label2.Text = ID;
                label3.Text = Address;
                label4.Text = Name;
                label5.Text = DOB;
                label6.Text = Mobile;
                label7.Text = Parent;


                
            }
            else
            {
                errorProvider1.SetError(textBox1, "Invalid Student Id");
            }

            connection.Close();
            textBox1.SelectAll();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            
            String id = textBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(id);

            connection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("select * from student where SID='" + textBox1.Text + "' ", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                string Query = "select * from student where SID='" + textBox1.Text + "'";
                MySqlCommand command = new MySqlCommand(Query, connection);
                reader = command.ExecuteReader();
                reader.Read();
                string Name = reader.GetString("Name");
                string LName = reader.GetString("LName");
                string ID = reader.GetString("ID");
                string DOB = Convert.ToDateTime(reader["DOB"]).ToString("yyyy/MM/dd");
                string Address = reader.GetString("Address");
                string Mobile = reader.GetString("Mobile");
                string Parent = reader.GetString("Parent");

                byte[] img = (byte[])(reader["img"]);
                if (img == null)
                {
                    pictureBox10.Image = null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox10.Image = System.Drawing.Image.FromStream(ms);
                }

                label1.Text = LName;
                label2.Text = ID;
                label3.Text = Address;
                label4.Text = Name;
                label5.Text = DOB;
                label6.Text = Mobile;
                label7.Text = Parent;



            }
            else
            {
                errorProvider1.SetError(textBox1, "Invalid Student Id");
            }

            connection.Close();
            


        }

        private void label9_Click(object sender, EventArgs e)
        {
            label1.Text = "Last Name";
            label2.Text = "NIC";
            label3.Text = "Address";
            label4.Text = "Name";
            label5.Text = "DOB";
            label6.Text = "Mobile";
            label7.Text = "Parent Number";
            pictureBox10.Image = null;
            textBox1.Text = "SID";
            textBox1.SelectAll();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
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
