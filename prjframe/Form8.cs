using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjframe
{
    public partial class ProfilePatient : Form
    {
        private int _idUtilisateur;
        private int idPatient;
        public ProfilePatient(int  IdUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = IdUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }
        
        private OleDbConnection connection;
        private void ProfilePatient_Load(object sender, EventArgs e)
        {
        date_label.Text = DateTime.Today.ToLongDateString();

            //afficher nom paient 
            string query = @"SELECT u.Nom, u.Prenom
            From  Utilisateur u 
            WHERE u.IdUtilisateur = ?";

        connection.Open();


            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("?", _idUtilisateur);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string prenom = reader["Prenom"].ToString();
                        string nom_complet = $"{nom} {prenom}";
                        patient_name.Text = nom_complet;
                       

                    }
}
            }
            connection.Close();
            LoadPatientInfo();

        }
        //************charger les informations du patient***********************//

        private void LoadPatientInfo()
{
        string query = @"SELECT u.Nom, u.Prenom, u.CIN, u.Telephone, u.Email, u.MotDePasse, p.Sexe , p.DateNaissance, p.Assurance
             FROM Utilisateur u
             INNER JOIN Patient p ON u.IdUtilisateur = p.IdUtilisateur
             WHERE u.IdUtilisateur = ?";

        using (OleDbCommand cmd = new OleDbCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("?", _idUtilisateur);
            connection.Open();

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtNom.Text = reader["Nom"].ToString();
                    txtPrenom.Text = reader["Prenom"].ToString();
                    txtCIN.Text = reader["CIN"].ToString();
                    txtTelephone.Text = reader["Telephone"].ToString();
                    txtMail.Text = reader["Email"].ToString();
                    txtMotDePasse.Text = reader["MotDePasse"].ToString();
                    txtAssurance.Text = reader["Assurance"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(reader["DateNaissance"]);    
                    string sexe = reader["Sexe"].ToString();
                    comboBoxSexe.SelectedItem = sexe;
                }
            }

            connection.Close();
        }
}
        private void UpdatePatientInfo()
        {
            string updateQuery = @"UPDATE Utilisateur u
                                   INNER JOIN Patient p ON u.IdUtilisateur = p.IdUtilisateur
                                   SET u.Telephone = ?, u.Email = ?, u.MotDePasse = ?, p.DateNaissance = ? , p.Assurance = ?
                                   WHERE u.IdUtilisateur = ?";

            using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
            {
                connection.Open();

                // Add parameters for the update query
                cmd.Parameters.AddWithValue("?", txtTelephone.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMail.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMotDePasse.Text.Trim());
                cmd.Parameters.AddWithValue("?", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("?", txtAssurance.Text.Trim());
                cmd.Parameters.AddWithValue("?", _idUtilisateur);

                // Execute the update query
                cmd.ExecuteNonQuery();

                connection.Close();
                MessageBox.Show("Informations mises à jour avec succès.");
            }
        }
        bool isEditMode = false;
        private void modifier_btn_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                // Activer le mode modification
                txtTelephone.ReadOnly = false;
                txtMail.ReadOnly = false;
                txtAssurance.ReadOnly = false;
                txtMotDePasse.ReadOnly = false;
                dateTimePicker1.Enabled = true;
                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {
                // Mettre à jour les données en base
                UpdatePatientInfo();
                // Désactiver les champs après mise à jour

                txtTelephone.ReadOnly = true;
                txtMail.ReadOnly = true;
                txtAssurance.ReadOnly = true;
                txtMotDePasse.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                modifier_btn.Text = "Modifier";

                isEditMode = false;
            }
        }

        private void annuler_btn_Click(object sender, EventArgs e)
        {
            // Remettre les champs en lecture seule
            txtTelephone.ReadOnly = true;
            txtMail.ReadOnly = true;
            txtMotDePasse.ReadOnly = true;
            dateTimePicker1.Enabled = false;

            // Réinitialiser le bouton Modifier
            modifier_btn.Text = "Modifier";
            isEditMode = false;

            // Recharger les données depuis la base
            LoadPatientInfo();

            MessageBox.Show("Modifications annulées.");
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PatientDashboard dashboard = new PatientDashboard(_idUtilisateur);
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
