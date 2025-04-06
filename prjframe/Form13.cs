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
    public partial class nv_patient : Form
    {
        public nv_patient()
        {
            InitializeComponent();
        }

        private void profile_pic_Click(object sender, EventArgs e)
        {
            DoctorDashboard dashboard = new DoctorDashboard();
            dashboard.Show();
        }

        private void logout_out_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
       "Voulez-vous vraiment vous déconnecter ?",
       "Déconnexion",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Open MenuForm
                MenuForm menu = new MenuForm();
                menu.Show();

                // Close current dashboard
                this.Close();
            }
        }

        private void nv_patient_Load(object sender, EventArgs e)
        {
            dataGridViewHistorique.Columns.Add("DateConsultation", "Date de Consultation");
            dataGridViewHistorique.Columns.Add("Notes", "Notes");
            dataGridViewHistorique.Columns.Add("NomDocument", "Nom du Document");
            dataGridViewHistorique.Columns.Add("DateDocument", "Date Document");




            // Add "Voir" button column
            DataGridViewButtonColumn btnVoir = new DataGridViewButtonColumn();
            btnVoir.Name = "Voir";
            btnVoir.HeaderText = "Action";
            btnVoir.Text = "Voir";
            btnVoir.UseColumnTextForButtonValue = true;
            dataGridViewHistorique.Columns.Add(btnVoir);

            dataGridViewHistorique.Columns["DateConsultation"].Width = 150;
            dataGridViewHistorique.Columns["NomDocument"].Width = 250;
            dataGridViewHistorique.Columns["DateDocument"].Width = 150;
            dataGridViewHistorique.Columns["Voir"].Width = 100;

            // Fill for Notes column
            dataGridViewHistorique.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewHistorique.RowHeadersVisible = false;

           
        }
    }
}
