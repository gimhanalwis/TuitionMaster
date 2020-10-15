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



namespace Tmaster
{
    public partial class admin : Form
    {
        MySqlConnection connection = new MySqlConnection("server=sql7.freesqldatabase.com;user id=sql7233778;password=cF2IXhWfDh;database=sql7233778");
        
        
        



        public admin()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void admin_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            try
            {
                
                DataTable dt = new DataTable();
                
                MySqlDataAdapter da = new MySqlDataAdapter("select* from user", connection);
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter("select* from user", connection);
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

        private void label4_Click(object sender, EventArgs e)
        {
            
            try
            {

                connection.Open();
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = "INSERT into user (username,password) values('" + textBox1.Text + "','" + textBox2.Text + "') ";
                comm.ExecuteNonQuery();
                connection.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex);
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter("select* from user", connection);
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

        private void label5_Click(object sender, EventArgs e)
        {
            try
            {

                connection.Open();
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = "DELETE FROM user where (username)='" + textBox1.Text + "'";
                comm.ExecuteNonQuery();
                connection.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }
    }
}
