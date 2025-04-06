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
    public partial class secretary_dashboard : Form
    {
        public secretary_dashboard()
        {
            InitializeComponent();
        }

        private void secretary_dashboard_Load(object sender, EventArgs e)
        {
            date_label.Text = DateTime.Today.ToLongDateString();
            // Setup columns
            dataGridViewDemandes.Columns.Add("DateRdv", "Date du Rendez-vous");
            dataGridViewDemandes.Columns.Add("NomPatient", "Patient");
            dataGridViewDemandes.Columns.Add("NomMedecin", "Médecin");

            // Confirm Button Column
            DataGridViewButtonColumn confirmBtn = new DataGridViewButtonColumn();
            confirmBtn.Name = "Confirmer";
            confirmBtn.HeaderText = "";
            confirmBtn.Text = "Confirmer";
            confirmBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(confirmBtn);

            // Refuse Button Column
            DataGridViewButtonColumn refuseBtn = new DataGridViewButtonColumn();
            refuseBtn.Name = "Refuser";
            refuseBtn.HeaderText = "";
            refuseBtn.Text = "Refuser";
            refuseBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(refuseBtn);

            dataGridViewDemandes.RowHeadersVisible = false;

        }


    }
}
