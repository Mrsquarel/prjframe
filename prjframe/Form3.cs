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
            confirmBtn.HeaderText = "Confirmation";
            confirmBtn.Text = "Confirmer";
            confirmBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(confirmBtn);

            // Refuse Button Column
            DataGridViewButtonColumn refuseBtn = new DataGridViewButtonColumn();
            refuseBtn.Name = "Refuser";
            refuseBtn.HeaderText = "Annulation";
            refuseBtn.Text = "Refuser";
            refuseBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(refuseBtn);

            dataGridViewDemandes.RowHeadersVisible = false;

            dataGridViewDemandes.Rows.Add("2025-04-10 09:00", "Amira Ben Saïd", "Dr. Karim Haddad");
            dataGridViewDemandes.Rows.Add("2025-04-11 14:30", "Youssef Trabelsi", "Dr. Leila Mzoughi");
            dataGridViewDemandes.Rows.Add("2025-04-12 11:15", "Sami Ferjani", "Dr. Rania Gharbi");


            dataGridViewConfirmes.Columns.Add("DateRdv", "Date du Rendez-vous");
            dataGridViewConfirmes.Columns.Add("NomPatient", "Nom du Patient");
            dataGridViewConfirmes.Columns.Add("NomMedecin", "Nom du Médecin");

            // Optional: make all columns editable
            foreach (DataGridViewColumn col in dataGridViewConfirmes.Columns)
            {
                col.ReadOnly = false;
            }
            dataGridViewConfirmes.Rows.Add("2025-04-07 10:00", "Imen Jaziri", "Dr. Sofiene Mabrouk");
            dataGridViewConfirmes.Rows.Add("2025-04-08 11:30", "Khalil Bouazizi", "Dr. Salma Chebbi");
            dataGridViewConfirmes.Rows.Add("2025-04-09 15:00", "Aya Hammami", "Dr. Firas Zribi");

            dataGridViewConfirmes.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewConfirmes.AllowUserToAddRows = true;     // Allow adding new RDVs
            dataGridViewConfirmes.AllowUserToDeleteRows = true;  // Allow removing RDVs

            dataGridViewConfirmes.RowHeadersVisible = false;


            // Setup columns
            dataGridViewPaiements.Columns.Add("RdvID", "ID du Rendez-vous");
            dataGridViewPaiements.Columns.Add("NomPatient", "Nom du Patient");
            dataGridViewPaiements.Columns.Add("Montant", "Montant Payé (DT)");
            dataGridViewPaiements.Columns.Add("ModePaiement", "Mode de Paiement");
            dataGridViewPaiements.Columns.Add("DatePaiement", "Date de Paiement");
            dataGridViewPaiements.Columns.Add("Statut", "Statut");

            // Make columns editable
            foreach (DataGridViewColumn col in dataGridViewPaiements.Columns)
            {
                col.ReadOnly = false;
            }
            // Enable editing and flexible row management
            dataGridViewPaiements.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewPaiements.AllowUserToAddRows = true;
            dataGridViewPaiements.AllowUserToDeleteRows = true;

            // Optional: Hide the row headers for cleaner UI
            dataGridViewPaiements.RowHeadersVisible = false;


            dataGridViewDossiers.Columns.Add("CIN", "CIN");
            dataGridViewDossiers.Columns.Add("Nom", "Nom");
            dataGridViewDossiers.Columns.Add("Prenom", "Prénom");
            dataGridViewDossiers.Columns.Add("Adresse", "Adresse");
            dataGridViewDossiers.Columns.Add("Email", "Email");
            dataGridViewDossiers.Columns.Add("DateNaissance", "Date de Naissance");
            dataGridViewDossiers.Columns.Add("Assurance", "Assurance");

            dataGridViewDossiers.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewDossiers.AllowUserToAddRows = false;
            dataGridViewDossiers.RowHeadersVisible = false;

            dataGridViewDossiers.Rows.Add("12345678", "Sana", "Rekik", "Ariana, Rue El Fajr", "sana.rekik@mail.com", "1994-06-12", "CNAM");
            dataGridViewDossiers.Rows.Add("78901234", "Walid", "Mejri", "Tunis, Montplaisir", "walid.mejri@mail.com", "1990-09-23", "Privée");


        }

        private void profile_pic_Click(object sender, EventArgs e)
        {
            ProfileSecretary profile = new ProfileSecretary();
            profile.Show();

        }
        private void logout_out_Click_1(object sender, EventArgs e)
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
