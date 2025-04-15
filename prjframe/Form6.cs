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
    public partial class ProfileAdmin : Form
    {
        private OleDbConnection connection;
        private int _idUtilisateur;
        public ProfileAdmin(int IdUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = IdUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }
        
        
       

        private void ProfileAdmin_Load(object sender, EventArgs e)
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
                        admin_name.Text = nom_complet;


                    }
                }
            }
            connection.Close();
            LoadAdminInfo();

        }
        //************charger les informations du admin***********************//

        private void LoadAdminInfo()
        {
            string query = @"SELECT u.Nom, u.Prenom, u.CIN, u.Telephone, u.Email, u.MotDePasse
             FROM Utilisateur u
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
                    }
                }

                connection.Close();
            }
        }
        private void UpdateAdminInfo()
        {
            string updateQuery = @"UPDATE Utilisateur u
                                   SET u.Telephone = ?, u.Email = ?, u.MotDePasse = ?
                                   WHERE u.IdUtilisateur = ?";

            using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
            {
                connection.Open();

                // Add parameters for the update query
                cmd.Parameters.AddWithValue("?", txtTelephone.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMail.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMotDePasse.Text.Trim());
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
                txtTelephone.ReadOnly = false;
                txtMail.ReadOnly = false;
                txtMotDePasse.ReadOnly = false;
                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {   UpdateAdminInfo(); 
                txtTelephone.ReadOnly = true;
                txtMail.ReadOnly = true;
                txtMotDePasse.ReadOnly = true;
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
            

            // Réinitialiser le bouton Modifier
            modifier_btn.Text = "Modifier";
            isEditMode = false;

            // Recharger les données depuis la base
            LoadAdminInfo();

            MessageBox.Show("Modifications annulées.");
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminpanel admindashboard = new adminpanel(_idUtilisateur);
            admindashboard.Show();
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
