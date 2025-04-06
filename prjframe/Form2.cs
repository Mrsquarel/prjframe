using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjframe
{
    public partial class adminpanel : Form
    {
        public adminpanel()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Heure", "heure");
            dataGridView1.Columns.Add("Patient", "patient");

            dataGridView1.Rows.Add("9:00 AM", "Patrick K.");
            dataGridView1.Rows.Add("10:30 AM", "Maria S.");
            dataGridView1.Rows.Add("11:00 AM", "John D.");
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void adminpanel_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

        }
    }
}
