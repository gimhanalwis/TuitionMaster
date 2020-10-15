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
    public partial class teachers : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public teachers()
        {
            InitializeComponent();
            Fillcombo();
        }

        void Fillcombo()
        {
            string Query = "select * from tm.teacher";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(Query, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string sName = reader.GetString("TName");
                    comboBox1.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }


        private void label2_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();

            if (comboBox1.Text == "Teachers" || comboBox1.Text == "")
            {
                errorProvider1.SetError(comboBox1, "Enter Teachers Name");
            }
            if (textBox2.Text == "Subject"|| textBox2.Text == "")
            {
                errorProvider2.SetError(textBox2, "Enter Subject");
            }
            if (textBox3.Text == "Mobile"|| textBox3.Text == "")
            {
                errorProvider3.SetError(textBox3, "Enter Mobile Number");
            }
            else if (textBox4.Text == "Class Type" || textBox4.Text == "")
            {
                errorProvider4.SetError(textBox4, "Enter Class Type");
            }

            else if (comboBox1.Text.All(Char.IsLetter) == false)
            {
                errorProvider1.SetError(comboBox1, "Enter Valid Name");
            }
            else if (textBox2.Text.All(Char.IsLetter) == false)
            {
                errorProvider2.SetError(textBox2, "Enter Valid Subject");
            }
            
            else
            {
                connection.Open();
                String insertQuery = "INSERT INTO teacher(TName,Subject,Type,Mobile) VALUES('"+comboBox1.Text+"','"+textBox2.Text+"','" + textBox4.Text + "','" +textBox3.Text+"')";
                command = new MySqlCommand(insertQuery, connection);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("error");
                }
                connection.Close();
            }

        }

       

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != null && (textBox2.Text=="Subject" || textBox2.Text == null))
            {
                try
                {

                    DataTable dt = new DataTable();

                    MySqlDataAdapter da = new MySqlDataAdapter("select* from teacher where TName like '%" + comboBox1.Text + "%' ", connection);
                    da.Fill(dt);
                    connection.Open();
                    dataGridView1.DataSource = dt;
                    connection.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error" + ex);
                }
                comboBox1.SelectAll();
            }
        }

        private void teachers_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
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

        private void label1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Subject";
            textBox3.Text = "Mobile";
            textBox4.Text = "Class Type";
            comboBox1.Text = "Teachers";
        }
    }
}
