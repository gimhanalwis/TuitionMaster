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
    
    public partial class payment : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        
        public payment()
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        void Fillcombo2()
        {
            string Query = "select Type from teacher where TName='" + comboBox1.Text + "'";
            connection.Open();
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


        private void payment_Load(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(id);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            Fillcombo2();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string currentYear = DateTime.Now.Year.ToString();
            label3.Text = currentYear;
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();
            errorProvider5.Clear();
            if (comboBox1.Text=="Teachers")
            {
                errorProvider3.SetError(comboBox1, "Select Teacher");
            }
            else if (comboBox3.Text == "Type")
            {
                errorProvider5.SetError(comboBox3, "Select Class Type");
            }
            else if (comboBox2.Text == "Month")
            {
                errorProvider4.SetError(comboBox2, "Select Month");
            }
            else if (textBox2.Text == "Amount")
            {
                errorProvider2.SetError(textBox2, "Enter Amount");
            }
            else
            {
                
                connection.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select * from student where SID='" + textBox1.Text + "' ", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MySqlDataAdapter sda2 = new MySqlDataAdapter("select * from payment where SID='" + textBox1.Text + "' and TName='" + comboBox1.Text + "'and PMonth='" + comboBox2.Text + "'and CType='" + comboBox3.Text + "'and Year='" + label3.Text+"' ", connection);
                    
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        payed frm = new payed();
                        frm.Show();
                    }
                    else
                    {
                        String insertQuery = "INSERT INTO payment(SID,TName,CType,Payment,Date,PMonth,Year) VALUES(@SID, @TName, @CType, @Payment, @Date, @PMonth, @Year)";
                        DateTime thisDay = DateTime.Today;
                        


                        command = new MySqlCommand(insertQuery, connection);

                        command.Parameters.Add("@SID", MySqlDbType.Int16, 5);
                        command.Parameters.Add("@TName", MySqlDbType.VarChar, 30);
                        command.Parameters.Add("@CType", MySqlDbType.VarChar, 20);
                        command.Parameters.Add("@Payment", MySqlDbType.VarChar, 5);
                        command.Parameters.Add("@Date", MySqlDbType.Date);
                        command.Parameters.Add("@PMonth", MySqlDbType.VarChar, 10);
                        command.Parameters.Add("@Year", MySqlDbType.VarChar, 5);


                        command.Parameters["@SID"].Value = textBox1.Text;
                        command.Parameters["@TName"].Value = comboBox1.Text;
                        command.Parameters["@CType"].Value = comboBox3.Text;
                        command.Parameters["@Payment"].Value = textBox2.Text;
                        command.Parameters["@Date"].Value = thisDay.Date;
                        command.Parameters["@PMonth"].Value = comboBox2.Text;
                        command.Parameters["@Year"].Value = label3.Text;


                        if (command.ExecuteNonQuery() == 1)
                        {
                            paysuccess msg = new paysuccess();
                            msg.Show();

                            string Query = "select * from student where SID='"+textBox1.Text+"'";
                            MySqlCommand command = new MySqlCommand(Query, connection);
                            reader = command.ExecuteReader();
                            reader.Read();
                            string tel1= reader.GetString("Mobile");
                            string parent = reader.GetString("Parent");
                            reader.Close();



                            string tel = "+94";
                            

                            SerialPort sp = new SerialPort();
                            sp.PortName = "COM4";
                            sp.Open();
                            sp.WriteLine("AT" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp.WriteLine("AT+CMGS=\"" + tel + tel1 + "\"" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp.WriteLine("Payment Successfully. " + Environment.NewLine + "Student Id: " + textBox1.Text + Environment.NewLine + "Payment Amount: " + textBox2.Text + Environment.NewLine + "Teacher Name: " + comboBox1.Text + Environment.NewLine + "Month: " + comboBox2.Text + Environment.NewLine);
                            sp.Write(new byte[] { 26 }, 0, 1);
                            Thread.Sleep(100);
                            var response = sp.ReadExisting();
                            sp.Close();


                            SerialPort sp1 = new SerialPort();
                            sp1.PortName = "COM4";
                            sp1.Open();
                            sp1.WriteLine("AT" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp1.WriteLine("AT+CMGF=1" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp1.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp1.WriteLine("AT+CMGS=\"" + tel + parent + "\"" + Environment.NewLine);
                            Thread.Sleep(100);
                            sp1.WriteLine("Payment Successfully. " + Environment.NewLine + "Student Id: " + textBox1.Text + Environment.NewLine + "Payment Amount: " + textBox2.Text + Environment.NewLine + "Teacher Name: " + comboBox1.Text + Environment.NewLine + "Month: " + comboBox2.Text + Environment.NewLine);
                            sp1.Write(new byte[] { 26 }, 0, 1);
                            Thread.Sleep(100);
                            var response1 = sp1.ReadExisting();
                            sp1.Close();


                        }
                    }
                }
                else
                {
                    errorProvider1.SetError(textBox1, "Invalid Student Id");
                }
                connection.Close();
                textBox1.SelectAll();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(id);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                try
                {

                    DataTable dt = new DataTable();

                    MySqlDataAdapter da = new MySqlDataAdapter("select* from payment where SID='"+textBox1.Text+"' ", connection);
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

            if (textBox1.Text != null && comboBox1.Text!="Teachers")
            {
                try
                {

                    DataTable dt = new DataTable();

                    MySqlDataAdapter da = new MySqlDataAdapter("select* from payment where SID='" + textBox1.Text + "' and TName='"+comboBox1.Text+"' ", connection);
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

            if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox1.Text!="Month")
            {
                try
                {

                    DataTable dt = new DataTable();

                    MySqlDataAdapter da = new MySqlDataAdapter("select* from payment where SID='" + textBox1.Text + "' and TName='" + comboBox1.Text + "' and PMonth='"+comboBox2.Text+"' ", connection);
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

