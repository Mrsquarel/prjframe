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
    public partial class DoctorDashboardForm : Form
    {

        public DoctorDashboardForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {   //to display the current date
            date_label.Text = DateTime.Today.ToLongDateString();
            //defining the columns of the datagridview
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Heure", "heure");
            dataGridView1.Columns.Add("Patient", "patient");

            dataGridView1.Rows.Add("9:00 AM", "Patrick K.");
            dataGridView1.Rows.Add("10:30 AM", "Maria S.");
            dataGridView1.Rows.Add("11:00 AM", "John D.");

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.ReadOnly = true;
            }
        }

        
    }
}