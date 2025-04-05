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
            //defining the columns of the datagridview emploi du temps
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Heure", "Heure");
            dataGridView1.Columns.Add("Patient", "Patient");

            dataGridView1.Rows.Add("9:00 AM", "Patrick K.");
            dataGridView1.Rows.Add("10:30 AM", "Maria S.");
            dataGridView1.Rows.Add("11:00 AM", "John D.");

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.ReadOnly = true;
            }

            //defining the columns of the datagridview historique consultation
            // Clear existing setup
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();

            // Add columns
            dataGridView2.Columns.Add("Date", "Date");
            dataGridView2.Columns.Add("Type", "Type");

            // Add View button column
            DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();
            viewBtn.Name = "Voir";
            viewBtn.HeaderText = "Contenu";
            viewBtn.Text = "Voir";
            viewBtn.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(viewBtn);

            // Add sample rows
            dataGridView2.Rows.Add("03/25/2025", "Follow-up");
            dataGridView2.Rows.Add("03/01/2025", "Initial Consult");

            // Optional styling
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ////defining the columns of the datagridview historique document
            // Clear existing setup
            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();

            // Add columns
            dataGridView3.Columns.Add("Date", "Date");
            dataGridView3.Columns.Add("Type", "Type");

            // Add View button column
            DataGridViewButtonColumn viewBtn_2 = new DataGridViewButtonColumn();
            viewBtn_2.Name = "Voir";
            viewBtn_2.HeaderText = "    Contenu";
            viewBtn_2.Text = "Voir";
            viewBtn_2.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(viewBtn_2);

            // Add sample rows
            dataGridView3.Rows.Add("03/25/2025", "Scanner");
            dataGridView3.Rows.Add("03/01/2025", "IRM");

            // Optional styling
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        
    }
}