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
    public partial class star : Form
    {
        public star()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                rectangleShape1.Width += 2;
                if (rectangleShape1.Width >= 500)
                {
                    timer1.Stop();
                    Form1 frm = new Form1();
                    frm.Show();
                    this.Hide();
                    
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        
    }
}
