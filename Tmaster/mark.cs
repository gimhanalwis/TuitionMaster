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

    public partial class mark : Form
    {



        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=tm;port=3307");
        MySqlCommand command;
        MySqlDataReader reader;
        public string currentYear;
        public string currentMonth;
        public string currentDate;
        public string currentTime;
        public string Name;
        public string LName;
        public string ID;
        public string Parent;

        public mark()
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

        void Fillcombo2()
          {
            comboBox3.Items.Clear();
            connection.Open();
            string Query = "select Type from teacher where TName='"+comboBox1.Text+"'";
            
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

        private void mark_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }

        private void label2_Click(object sender, EventArgs e)
        {
            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            string currentTime = DateTime.Now.ToString("h:mm tt");




            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();

            if (textBox1.Text == "Student ID" || textBox1.Text == null)

            {
                errorProvider1.SetError(textBox1, "Student Id shouldn't be null");

            }
           else if (comboBox3.Text == "Type")

            {
                errorProvider4.SetError(comboBox3, "Please Select Class Type");

            }
           else if (comboBox1.Text == "Teachers")
            {
                errorProvider1.SetError(comboBox1, "Teachers Should be selected");
            }
           else if (comboBox2.Text == "Status")
            {
                errorProvider3.SetError(comboBox2, "Status Should be selected");
            }
            else
            {
                connection.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select * from student where SID='" + textBox1.Text + "' ", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string GetInfo = "select * from student where SID='" + textBox1.Text + "'";
                    MySqlCommand command = new MySqlCommand(GetInfo, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    string Name = reader.GetString("Name");
                    string Lname = reader.GetString("LName");
                    string ID = reader.GetString("ID");
                    string Parent = reader.GetString("Parent");
                    byte[] img = (byte[])(reader["img"]);
                    if (img == null)
                    {
                        pictureBox13.Image = null;
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox13.Image = System.Drawing.Image.FromStream(ms);
                    }
                    label3.Text = Name;
                    label4.Text = Lname;
                    label5.Text = ID;


                }
                else
                {
                    errorProvider1.SetError(textBox1, "Invalid Student Id");
                }
                connection.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            pictureBox13.Image = null;
            textBox1.SelectAll();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            connection.Open();

            string GetInfo = "select * from student where SID='" + textBox1.Text + "'";
            MySqlCommand command = new MySqlCommand(GetInfo, connection);
            reader = command.ExecuteReader();
            reader.Read();
            string Name = reader.GetString("Name");
            string Lname = reader.GetString("LName");
            string ID = reader.GetString("ID");
            string Parent = reader.GetString("Parent");
            reader.Close();

            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            string currentTime = DateTime.Now.ToString("h:mm tt");





            MySqlDataAdapter sdap = new MySqlDataAdapter("select * from payment where SID='" + textBox1.Text + "' and PMonth='" + currentMonth + "' and Year='" + currentYear + "'and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "' ", connection);
            DataTable dtp = new DataTable();
            sdap.Fill(dtp);


            if (dtp.Rows.Count > 0)
            {


                if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Entrance" && checkBox2.Checked == false)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','In')";
                    command = new MySqlCommand(query, connection);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        try
                        {
                            string tel = "+94";
                            string tel1 = Parent;

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
                            sp.WriteLine(Name + " is arrived to the class at " + currentTime + " on " + currentDate + Environment.NewLine + "TUITION MASTER" + Environment.NewLine);
                            sp.Write(new byte[] { 26 }, 0, 1);
                            Thread.Sleep(1000);
                            var response = sp.ReadExisting();
                            sp.Close();

                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Usb Modem Issue");
                        }
                        

                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }

                else if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Interval" && checkBox2.Checked == false)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','In')";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                else if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Other" && checkBox2.Checked == false)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','In')";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                else if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Entrance" && checkBox2.Checked)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','Out')";
                    command = new MySqlCommand(query, connection);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        try
                        {
                            string tel = "+94";
                            string tel1 = Parent;

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
                            sp.WriteLine(Name + " left at " + currentTime + " on " + currentDate + " from the class" + Environment.NewLine + "TUITON MASTER" + Environment.NewLine);
                            sp.Write(new byte[] { 26 }, 0, 1);
                            Thread.Sleep(1000);
                            var response = sp.ReadExisting();
                            sp.Close();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Usb Modem Issue");
                        }
                        

                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }

                }

                else if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Interval" && checkBox2.Checked)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','Out')";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                else if (textBox1.Text != null && comboBox1.Text != "Teachers" && comboBox2.Text == "Other" && checkBox2.Checked)
                {

                    string query = "INSERT INTO attendance( SID, TName, CType, Date, Time, Status, Action) VALUES('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + currentDate + "','" + currentTime + "','" + comboBox2.Text + "','Out')";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }


            }
            else
            {
                MessageBox.Show("Unpaid");
            }
            connection.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        
        private void label10_Click(object sender, EventArgs e)
        {
            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            string currentTime = DateTime.Now.ToString("h:mm tt");

            connection.Close();
            connection.Open();
            
            MySqlDataAdapter sda = new MySqlDataAdapter("select SID from payment where  PMonth= '" + currentMonth + "' and Year='" + currentYear + "' and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "'", connection);
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

            for (i = 0; i < column0Array.Length-1; i++)
            {
                MySqlDataAdapter sda2 = new MySqlDataAdapter("select * from attendance where SID='"+column0Array[i]+"' and Date= '" + currentDate + "' and Status='Entrance' and Action='In'and TName='" + comboBox1.Text + "' and CType='" + comboBox3.Text + "'", connection);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    MySqlDataAdapter sda3 = new MySqlDataAdapter("select * from attendance where SID='" + column0Array[i] + "' and Date= '" + currentDate + "'  and Status='Entrance' and Action='Out'", connection);
                    DataTable dt3 = new DataTable();
                    sda3.Fill(dt3);
                    if (dt3.Rows.Count > 0)
                    {
                        
                        
                        //supiri

                    }
                    else
                    {
                        MySqlDataAdapter sda4 = new MySqlDataAdapter("SELECT * FROM attendance WHERE SID='" + column0Array[i] + "' AND TIME=(SELECT MAX(TIME) FROM attendance WHERE sid='" + column0Array[i] + "' and Date='" + currentDate + "') and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "' LIMIT 1", connection);
                        DataTable dt4 = new DataTable();
                        sda4.Fill(dt4);
                        
                        if (dt4.Rows.Count > 0)
                        {

                            //padiri
                            try
                            {
                                string Query = "select * from student where SID='" + column0Array[i] + "'";
                                MySqlCommand command = new MySqlCommand(Query, connection);
                                reader = command.ExecuteReader();
                                reader.Read();
                                string tel1 = reader.GetString("Parent");
                                string name = reader.GetString("Name");
                                reader.Close();


                                string Query2 = "SELECT * FROM attendance WHERE SID='" + column0Array[i] + "' AND TIME=(SELECT MAX(TIME) FROM attendance WHERE sid='" + column0Array[i] + "' and Date='" + currentDate + "') and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "' LIMIT 1";
                                MySqlCommand command2 = new MySqlCommand(Query2, connection);
                                reader = command2.ExecuteReader();
                                reader.Read();
                                string time = reader.GetString("Time");
                                string tname = reader.GetString("TName");
                                string date = reader.GetString("Date");
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
                                sp.WriteLine("You Child " + name + " was left from the class at: " + time + Environment.NewLine + "on: " + currentDate + "from MR/MRs/Ms" + tname + "'s class." + Environment.NewLine + "TUITION MASTER" + Environment.NewLine);
                                sp.Write(new byte[] { 26 }, 0, 1);
                                Thread.Sleep(1000);
                                var response = sp.ReadExisting();
                                sp.Close();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show("Something Went Wrong!");
                            }
                            


                        }

                        
                    }

                }
                else
                {
                    //wala
                    try
                    {
                        string Query = "select * from student where SID='" + column0Array[i] + "'";
                        MySqlCommand command = new MySqlCommand(Query, connection);
                        reader = command.ExecuteReader();
                        reader.Read();
                        string tel1 = reader.GetString("Parent");
                        string name = reader.GetString("Name");
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
                        sp.WriteLine("Sorry.Your child, " + name + " not attended to MR/MRs " + comboBox1.Text + "'s class on today(" + currentDate + ")" + Environment.NewLine + "TUITION MASTER" + Environment.NewLine);
                        sp.Write(new byte[] { 26 }, 0, 1);
                        Thread.Sleep(1000);
                        var response = sp.ReadExisting();
                        sp.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Something Went Wrong!");
                    }
                    

                }
                
                


            }


            connection.Close();
           
        }


        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            Fillcombo2();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            //start class
            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            string currentTime = DateTime.Now.ToString("h:mm tt");

            try
            {
                connection.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select SID from payment where  PMonth= '" + currentMonth + "' and Year='" + currentYear + "' and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "'", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView2.DataSource = dt;

                string[] start = new string[dataGridView2.Rows.Count];


                int i = 0;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    start[i] = row.Cells[0].Value != null ? row.Cells[0].Value.ToString() : string.Empty;

                    i++;
                }

                for (i = 0; i < start.Length - 1; i++)

                {

                    string Query = "select * from student where SID='" + start[i] + "'";
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
                    sp.WriteLine("Mr/Mrs " + comboBox1.Text + "'s class now started." + Environment.NewLine + "TUITION MASTER" + Environment.NewLine);
                    sp.Write(new byte[] { 26 }, 0, 1);
                    Thread.Sleep(1000);
                    var response = sp.ReadExisting();
                    sp.Close();




                }

                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something Went Wrong!");
            }
            
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string currentMonth = DateTime.Now.ToString("MMMM");
            string currentYear = DateTime.Now.Year.ToString();
            string currentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            string currentTime = DateTime.Now.ToString("h:mm tt");
            //end class
            try
            {
                connection.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select SID from payment where  PMonth= '" + currentMonth + "' and Year='" + currentYear + "' and TName='" + comboBox1.Text + "'and CType='" + comboBox3.Text + "'", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView3.DataSource = dt;

                string[] end = new string[dataGridView3.Rows.Count];


                int i = 0;
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    end[i] = row.Cells[0].Value != null ? row.Cells[0].Value.ToString() : string.Empty;

                    i++;
                }

                for (i = 0; i < end.Length - 1; i++)
                {
                    string Query = "select * from student where SID='" + end[i] + "'";
                    MySqlCommand command = new MySqlCommand(Query, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    string tel1 = reader.GetString("Parent");
                    string name = reader.GetString("Name");
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
                    sp.WriteLine("Mr/Mrs " + comboBox1.Text + "'s class now ended." + Environment.NewLine + "TUITION MASTER" + Environment.NewLine);
                    sp.Write(new byte[] { 26 }, 0, 1);
                    Thread.Sleep(1000);
                    var response = sp.ReadExisting();
                    sp.Close();
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something Went Wrong!");
            }
            
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
