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
    public partial class report : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public report()
        {
            InitializeComponent();
            Fillcombo();
        }

        void Fillcombo()
        {
            string Query = "select * from tm.teacher";
            connection.Open();
            try
            {
                
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

        void Fillcombo2()
        {
            comboBox3.Items.Clear();
            connection.Close();
            connection.Open();
            string Query = "select Type from teacher where TName='" + comboBox1.Text + "'";

            try
            {

                MySqlCommand command = new MySqlCommand(Query, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string Type = reader.GetString("Type");
                    comboBox3.Items.Add(Type);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();

        }

        private void report_Load(object sender, EventArgs e)
        {
            
            string currentYear = DateTime.Now.Year.ToString();
            string currentMonth = DateTime.Now.ToString("MMMM");
            try
            {
                connection.Open();
                string amount = "select sum(payment) from payment where PMonth='" + currentMonth + "'and  Year='" + currentYear + "'";
                MySqlCommand command = new MySqlCommand(amount, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                    label10.Text = reader.GetString(0);
                }
                connection.Close();

            }
            catch (Exception ex)
            {
                label10.Text = Convert.ToString(0);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            if (checkBox1.Checked)
            {
                if (comboBox1.Text == "Teachers")
                {
                    errorProvider1.SetError(comboBox1, "Select Teacher");
                }
                else if (comboBox3.Text == "Type")
                {
                    errorProvider2.SetError(comboBox3, "Select Class Type");
                }
                else if (comboBox2.Text == "Month")
                {
                    errorProvider3.SetError(comboBox2, "Select Month");
                }
                else
                {
                    try
                    {
                        string currentYear = DateTime.Now.Year.ToString();

                        connection.Open();
                        string amount = "select sum(payment) from payment where TName='" + comboBox1.Text + "' and PMonth='" + comboBox2.Text + "'and CType='" + comboBox3.Text + "' and Year='" + currentYear + "'";
                        MySqlCommand command = new MySqlCommand(amount, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {

                            label2.Text = reader.GetString(0);
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        label2.Text = Convert.ToString(0);
                    }



                }
            }
            else
            {
                if (comboBox1.Text == "Teachers")
                {
                    errorProvider1.SetError(comboBox1, "Select Teacher");
                }
               
                else if (comboBox2.Text == "Month")
                {
                    errorProvider3.SetError(comboBox2, "Select Month");
                }
                else
                {
                    try
                    {
                        string currentYear = DateTime.Now.Year.ToString();

                        connection.Open();
                        string amount = "select sum(payment) from payment where TName='" + comboBox1.Text + "' and PMonth='" + comboBox2.Text + "' and Year='" + currentYear + "'";
                        MySqlCommand command = new MySqlCommand(amount, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            label2.Text = reader.GetString(0);
                        }
                        connection.Close();

                    }
                    
                    catch (Exception ex)
                    {
                        label2.Text = Convert.ToString(0);
                    }

                }

            }



           
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            
            double z;
            bool cal = Double.TryParse(label2.Text, out z);
            label5.Text = (z*0.2).ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_TextChanged(object sender, EventArgs e)
        {
            double a,b;
            bool cal = Double.TryParse(label5.Text, out a);
            bool cal2 = Double.TryParse(label2.Text, out b);
            label7.Text = (b-a).ToString();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            Fillcombo2();
        }

        private void label10_TextChanged(object sender, EventArgs e)
        {
            double a;
            bool cal = Double.TryParse(label10.Text, out a);
            label11.Text = (a * 0.2).ToString();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            menu frm = new menu();
            frm.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Sunray Higher Educational Centre  ", new Font("Arial", 25, FontStyle.Bold), Brushes.Black, new Point(25, 100));
            e.Graphics.DrawString("No :119 A,Ambagamuwa Road, ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 150));
            e.Graphics.DrawString("Nawalapitiya, ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 175));
            e.Graphics.DrawString("TP : 081-8949346 / 081-8949347 ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 200));
            e.Graphics.DrawString(label25.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 300));
            e.Graphics.DrawString("Date :" + DateTime.Now, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 325));
            e.Graphics.DrawString("Teachers Name :"+comboBox1.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 350));
            e.Graphics.DrawString("Month :" + comboBox2.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 375));
            e.Graphics.DrawString("Net Amount :" + label2.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 400));
            e.Graphics.DrawString("Institute Charges(20%) :" + label5.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 425));
            e.Graphics.DrawString("Teachers Income :" + label7.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 450));
            
            e.Graphics.DrawString("Tuition Master", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(25, 500));
        }

        private void label24_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
    }
}
