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

namespace Tmaster
{
    public partial class student : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public student()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox13.Image = Image.FromFile(opf.FileName);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string Name = textBox1.Text;
            string LName = textBox2.Text;
            string NIC = textBox3.Text;
            string Address = textBox4.Text;
            string Mobile = textBox5.Text;
            string Parent = textBox6.Text;
            

            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();
            errorProvider5.Clear();
            errorProvider6.Clear();
            errorProvider7.Clear();

            if ((Name==""||Name=="Name")|| (LName == "" || LName == "Last Name")|| (NIC == "" || NIC == "NIC")|| (Mobile == "" || Mobile == "Mobile")|| (Parent == "" || Parent == "Parent Name")|| (Address == "" || Address == "Address") )
            {
                

                if ((Name == "" || Name == "Name"))
                {
                    errorProvider1.SetError(textBox1, "Firstname Should Not Be Null ");
                }

                if ((LName == "" || LName == "Last Name"))
                {
                    errorProvider2.SetError(textBox2, "Lastname Should Not Be Null ");
                }

                if ((NIC == "" || NIC == "NIC"))
                {
                    errorProvider3.SetError(textBox3, "NIC Should Not Be Null ");
                }

                if ((Address == "" || Address == "Address"))
                {
                    errorProvider4.SetError(textBox4, "Address Should Not Be Null ");
                }

                if ((Mobile == "" || Mobile == "Mobile"))
                {
                    errorProvider5.SetError(textBox5, "Mobile Number Should Not Be Null ");
                }

                if ((Parent == "" || Parent == "Parent Number"))
                {
                    errorProvider6.SetError(textBox6, "Parent Number Should Not Be Null ");
                }

                
            }

            else if (Name.All(Char.IsLetter) == false|| LName.All(Char.IsLetter) == false)
            {
                Nameerror frm = new Nameerror();
                frm.Show();
            }

            else if (NIC.All(Char.IsLetter) == true)
            {              

               errorProvider3.SetError(textBox3, "Numeric Values Only ");
            }

            
            else if(pictureBox13.Image == null)
            {
                errorProvider8.SetError(pictureBox13, "Insert a image");
            }

            else
            {
                connection.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select * from student where ID='" + textBox3.Text + "' ", connection);
                

                DataTable dt = new DataTable();
                
                sda.Fill(dt);
                


                if (dt.Rows.Count > 0)
                {
                    already frm = new already();
                    frm.Show();
                    

                }
                
                else
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox13.Image.Save(ms, pictureBox13.Image.RawFormat);
                    byte[] img = ms.ToArray();

                    String insertQuery = "INSERT INTO student( Name, LName, ID, DOB, Address, Mobile, Parent, img) VALUES(@Name, @LName, @ID, @DOB, @Address, @Mobile, @Parent, @img)";

                    

                    command = new MySqlCommand(insertQuery, connection);

                    
                    command.Parameters.Add("@Name", MySqlDbType.VarChar, 20);
                    command.Parameters.Add("@LName", MySqlDbType.VarChar, 20);
                    command.Parameters.Add("@ID", MySqlDbType.VarChar, 11);
                    command.Parameters.Add("@DOB", MySqlDbType.Date);
                    command.Parameters.Add("@Address", MySqlDbType.VarChar, 200);
                    command.Parameters.Add("@Mobile", MySqlDbType.Int64, 12);
                    command.Parameters.Add("@Parent", MySqlDbType.Int64, 12);
                    command.Parameters.Add("@img", MySqlDbType.LongBlob);

                    
                    command.Parameters["@Name"].Value = textBox1.Text;
                    command.Parameters["@LName"].Value = textBox2.Text;
                    command.Parameters["@ID"].Value = textBox3.Text;
                    command.Parameters["@DOB"].Value = dateTimePicker1.Value.Date;
                    command.Parameters["@Address"].Value = textBox4.Text;
                    command.Parameters["@Mobile"].Value = textBox5.Text;
                    command.Parameters["@Parent"].Value = textBox6.Text;
                    command.Parameters["@img"].Value = img;

                    if (command.ExecuteNonQuery() == 1)
                    {
                        success f = new success();
                        f.Show();
                        string Query = "select SID from student where ID='" + textBox3.Text + "'";
                        MySqlCommand command = new MySqlCommand(Query, connection);
                        reader = command.ExecuteReader();
                        reader.Read();
                        string sid = reader.GetString("SID");

                        string tel = "+94";
                        string tel1 = textBox5.Text;

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
                        sp.WriteLine("Your Registration has been confirmed. " + Environment.NewLine + "Student Id: " + sid + Environment.NewLine + "Student Name: " + textBox1.Text + Environment.NewLine + "NIC: " + textBox3.Text + Environment.NewLine);
                        sp.Write(new byte[] { 26 }, 0, 1);
                        Thread.Sleep(100);
                        var response = sp.ReadExisting();
                        sp.Close();

                    }

                    

                }
                connection.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            textBox6.Text = "Parent Number";
            textBox5.Text = "Mobile";
            textBox4.Text = "Address";
            textBox3.Text = "NIC";
            textBox2.Text = "Last Name";
            textBox1.Text = "Name";
            pictureBox13.Image = null;



        }

        private void label4_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter("select* from student", connection);
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

        private void student_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_Click_1(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
