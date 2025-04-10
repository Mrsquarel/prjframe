using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prjframe
{
    public partial class Patient_infos : Form
    {
        private int _idUtilisateur;
        public Patient_infos(int IdUtilisateur)
        {
            _idUtilisateur = IdUtilisateur;
            InitializeComponent();
        }

        private void Patient_infos_Load(object sender, EventArgs e)
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

            dataGridViewHistorique.Rows.Add("06/04/2025", "Maux de tête fréquents", "Scanner_IRM.pdf", "06/04/2025");
            dataGridViewHistorique.Rows.Add("28/03/2025", "Douleurs thoraciques", "RadioThorax.png", "28/03/2025");

        }
        bool isEditMode = false;    
        private void modifier_btn_Click(object sender, EventArgs e)
        {

            if (!isEditMode)
            {
                
                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {
                // Save and Return to View Mode
                // TODO: Validate & save to DB or memory

                modifier_btn.Text = "Modifier";

                isEditMode = false;

                MessageBox.Show("Profil mis à jour avec succès !");
            }
        }

        private void profile_pic_Click(object sender, EventArgs e)
        {
            DoctorDashboard dashboard = new DoctorDashboard(_idUtilisateur);
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
    }
}
