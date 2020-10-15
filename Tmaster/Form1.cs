using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Tmaster
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            connection.Close();
            connection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("select * from user where username='" + textBox1.Text + "'and password='" + textBox2.Text + "' ",connection);

            DataTable dt = new DataTable();
            sda.Fill(dt);

           
            if(dt.Rows.Count > 0)
            {
                menu frm = new menu();
                frm.Show();
                this.Hide();

            }
            else
            {
                errorProvider1.Clear();
                string username, password;
                username = textBox1.Text;
                password = textBox2.Text;


                if ("admin".ToString() == username && "nibm1234".ToString() == password)
                {
                    admin frm = new admin();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    errorProvider1.SetError(textBox1, "Invalid Username or Password");
                }
            }

            connection.Close();

        }

        private void textBox2_Click_1(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            textBox2.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("dd");

            connection.Open();

            if (currentDate == "28")
            {

                
                MySqlDataAdapter sda2 = new MySqlDataAdapter("select * from paymentrem where Year='"+currentYear+"' and Month='"+currentMonth+"'", connection);

                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);

                if (dt2.Rows.Count <= 0)
                {
                    
                    MySqlDataAdapter sda = new MySqlDataAdapter("select SID from student", connection);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;

                    string[] column0Array = new string[dataGridView1.Rows.Count];


                    int i = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        column0Array[i] = row.Cells[0].Value != null ? row.Cells[0].Value.ToString() : string.Empty;

                        i++;
                    }

                    for (i = 0; i < column0Array.Length - 1; i++)
                    {
                        string Query = "select * from student where SID='" + column0Array[i] + "'";
                        MySqlCommand command = new MySqlCommand(Query, connection);
                        reader = command.ExecuteReader();
                        reader.Read();
                        string tel1 = reader.GetString("Parent");                        
                        reader.Close();


                        string tel = "+94";


                        SerialPort sp = new SerialPort();
                        sp.PortName = "COM4";
                        sp.Open();
                        sp.WriteLine("AT" + Environment.NewLine);
                        Thread.Sleep(1000);
                        sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                        Thread.Sleep(1000);
                        sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);
                        Thread.Sleep(1000);
                        sp.WriteLine("AT+CMGS=\"" + tel + tel1 + "\"" + Environment.NewLine);
                        Thread.Sleep(1000);
                        sp.WriteLine("Payment Reminder..Please make your class fees for next month.Thank you.."+Environment.NewLine);
                        sp.Write(new byte[] { 26 }, 0, 1);
                        Thread.Sleep(1000);
                        var response = sp.ReadExisting();
                        sp.Close();

                        


                    }
                    string query2 = "INSERT INTO paymentrem(Year,Month) VALUES('" + currentYear + "','" + currentMonth + "')";
                    command = new MySqlCommand(query2, connection);
                    command.ExecuteNonQuery();

                }


                connection.Close();
            } 
        }

        private void label12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
