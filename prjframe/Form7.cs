using System;
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
    public partial class ProfileSecretary : Form
    {
        private int _idUtilisateur;
        private int idSecretaire;
        public ProfileSecretary(int idUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = idUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }
        private void ProfileSecretary_Load(object sender, EventArgs e)
        {   //afficher la date d'aujourd'hui
            date_label.Text = DateTime.Today.ToLongDateString();

            //afficher nom et specialite de secretaire
            string query = @"SELECT u.Nom, u.Prenom, s.Departement
        FROM Secretaire s
        INNER JOIN Utilisateur u ON s.IdUtilisateur = u.IdUtilisateur
        WHERE s.IdUtilisateur = ?";

            connection.Open();


            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("?", _idUtilisateur);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string specialiteValue = reader["Departement"].ToString();

                        secretary_name.Text = nom;
                        dept_label.Text = specialiteValue;

                    }
                }
            }
            connection.Close();
            LoadSecretaryInfo();

        }
        //************charger les informations de secretaire***********************//

        private void LoadSecretaryInfo()
        {
            string query = @"SELECT u.Nom, u.Prenom, u.CIN, u.Telephone, u.Email, u.MotDePasse, s.Departement
             FROM Utilisateur u
             INNER JOIN Secretaire s ON u.IdUtilisateur = s.IdUtilisateur
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

                        string specialite = reader["Departement"].ToString();
                        comboBoxDept.SelectedItem = specialite;
                    }
                }

                connection.Close();
            }
        }
        private void UpdateSecretaryInfo()
        {
            string updateQuery = @"UPDATE Utilisateur u
                           INNER JOIN Secretaire s ON u.IdUtilisateur = s.IdUtilisateur
                           SET u.Telephone = ?, u.Email = ?, u.MotDePasse = ?, s.Departement = ?
                           WHERE u.IdUtilisateur = ?";

            using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
            {
                connection.Open();

                // Add parameters for the update query
                cmd.Parameters.AddWithValue("?", txtTelephone.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMail.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMotDePasse.Text.Trim());
                cmd.Parameters.AddWithValue("?", comboBoxDept.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("?", _idUtilisateur);

                // Execute the update query
                cmd.ExecuteNonQuery();

                connection.Close();
                MessageBox.Show("Informations mises à jour avec succès.");
            }
        }
        private void modifier_btn_Click_1(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                // Activer le mode modification
                txtTelephone.ReadOnly = false;
                txtMail.ReadOnly = false;
                txtMotDePasse.ReadOnly = false;
                comboBoxDept.Enabled = true;

                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {
                // Mettre à jour les données en base
                UpdateSecretaryInfo();

                // Désactiver les champs après mise à jour
                txtTelephone.ReadOnly = true;
                txtMail.ReadOnly = true;
                txtMotDePasse.ReadOnly = true;
                comboBoxDept.Enabled = false;

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
            comboBoxDept.Enabled = false;

            // Réinitialiser le bouton Modifier
            modifier_btn.Text = "Modifier";
            isEditMode = false;

            // Recharger les données depuis la base
            LoadSecretaryInfo();

            MessageBox.Show("Modifications annulées.");

        }
        private void profile_pic_Click(object sender, EventArgs e)
        {
            secretary_dashboard dashboard = new secretary_dashboard(_idUtilisateur);
            dashboard.Show();
        }
        bool isEditMode = false;
        private OleDbConnection connection;

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            secretary_dashboard dashboard = new secretary_dashboard(_idUtilisateur);
            dashboard.Show();
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
