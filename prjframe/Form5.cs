using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjframe
{
    public partial class ProfileDoctor : Form
    {   private int _IdUtilisateur;
        private int idMedecin=0;
        public ProfileDoctor(int IdUtilisateur)
        {
            InitializeComponent();
            _IdUtilisateur = IdUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }
        
        
        private void ProfileDoctor_Load(object sender, EventArgs e)
        {//afficher la date d'aujourd'hui
            date_label.Text = DateTime.Today.ToLongDateString();

            dataGridViewSchedule.Columns.Clear();
            dataGridViewSchedule.Rows.Clear();
            //tableau des dispos

            DataGridViewTextBoxColumn heureCol = new DataGridViewTextBoxColumn();
            heureCol.HeaderText = "Heure";
            heureCol.Name = "Heure";
            heureCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewSchedule.Columns.Add(heureCol);


            string[] jours = { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" };
            foreach (string jour in jours)
            {
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                col.HeaderText = jour;
                col.Name = jour;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewSchedule.Columns.Add(col);
            }


            DateTime startTime = DateTime.Today.AddHours(9);
            for (int i = 0; i <= 16; i++)
            {
                string timeLabel = startTime.AddMinutes(i * 30).ToString("HH:mm");
                int rowIndex = dataGridViewSchedule.Rows.Add();
                dataGridViewSchedule.Rows[rowIndex].Cells["Heure"].Value = timeLabel;
            }


            dataGridViewSchedule.AllowUserToAddRows = false;
            dataGridViewSchedule.RowHeadersVisible = false;


            //determnier la semaine de travail
            // Date d'aujourd'hui
            DateTime aujourdHui = DateTime.Today;

            // Calcul du lundi de la semaine actuelle
            int joursDepuisLundi = (int)aujourdHui.DayOfWeek - 1;
            if (joursDepuisLundi < 0) joursDepuisLundi = 6; // Si dimanche

            DateTime lundi = aujourdHui.AddDays(-joursDepuisLundi);
            DateTime samedi = lundi.AddDays(5); // Samedi = lundi + 5 jours

            // Formatage des dates
            string texte = $"DU {lundi:dddd d MMMM yyyy} au {samedi:dddd d MMMM yyyy}";

            // Affichage dans le GroupBox
            groupBoxDisponibilite.Text = texte;


            //recuperer id medecin
            string query2 = "SELECT IdMedecin from Medecin WHERE IdUtilisateur = ?";
            connection.Open();
            OleDbCommand cmd2 = new OleDbCommand(query2, connection);
            cmd2.Parameters.AddWithValue("?", _IdUtilisateur);
            OleDbDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                idMedecin = (int)reader2["IdMedecin"];
            }
            connection.Close();

            LoadDisponibiliteFromDatabase();
            //afficher nom et specialite du medecin 
            string query = @"SELECT u.Nom, u.Prenom, m.Specialite 
                    FROM Medecin m
                    INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur
                    WHERE m.IdUtilisateur = ?";

            connection.Open();


            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("?", _IdUtilisateur);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string specialiteValue = reader["Specialite"].ToString();

                        doctor_name.Text = nom;
                        specialite.Text = specialiteValue;

                    }
                }
            }
            connection.Close();
            LoadDoctorInfo();
        }



        //************charger les informations du medecin***********************//

        private void LoadDoctorInfo()
        {
            string query = @"SELECT u.Nom, u.Prenom, u.CIN, u.Telephone, u.Email, u.MotDePasse, m.Specialite
             FROM Utilisateur u
             INNER JOIN Medecin m ON u.IdUtilisateur = m.IdUtilisateur
             WHERE u.IdUtilisateur = ?";

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("?", _IdUtilisateur);
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

                        string specialite = reader["Specialite"].ToString();
                        comboBoxSpecialite.SelectedItem = specialite;
                    }
                }

                connection.Close();
            }
        }




        //************charger les dispos du medecin***********************//
        private void LoadDisponibiliteFromDatabase()
        {
            bool dataFound = false;
            // vider la table
            foreach (DataGridViewRow row in dataGridViewSchedule.Rows)
            {
                for (int col = 1; col < row.Cells.Count; col++)
                {
                    row.Cells[col].Value = false;
                }
            }

            string query = "SELECT Jour, HeureDebut FROM PlageHoraire WHERE IdMedecin = ? AND SemaineDebut = ?";

                connection.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("?", idMedecin);
                    cmd.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        dataFound = true;
                           
                            string jour = reader["Jour"].ToString();  

                            // Exttraire juste la partie heure from datetime
                            DateTime heureDebut;
                            if (DateTime.TryParse(reader["HeureDebut"].ToString(), out heureDebut))
                            {
                                string heureFormatee = heureDebut.ToString("HH:mm");

                                // chercher la ligne ayant a meme heure
                                foreach (DataGridViewRow row in dataGridViewSchedule.Rows)
                                {
                                    if (row.Cells["Heure"].Value != null &&
                                        row.Cells["Heure"].Value.ToString() == heureFormatee)
                                    {
                                        if (dataGridViewSchedule.Columns.Contains(jour))
                                        {
                                            row.Cells[jour].Value = true;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            if (!dataFound)
            {
                MessageBox.Show("Aucune disponibilité trouvée pour cette semaine.\nLe tableau a été réinitialisé.",
                                     "Information",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
            }
            connection.Close();
        }
        /// <summary>
        ///modifier , annuler les modifications des informations personnelles
        /// </summary>
        private void UpdateDoctorInfo()
        {
            string updateQuery = @"UPDATE Utilisateur u
                           INNER JOIN Medecin m ON u.IdUtilisateur = m.IdUtilisateur
                           SET u.Telephone = ?, u.Email = ?, u.MotDePasse = ?, m.Specialite = ?
                           WHERE u.IdUtilisateur = ?";

            using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
            {
                connection.Open();

                // Add parameters for the update query
                cmd.Parameters.AddWithValue("?", txtTelephone.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMail.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtMotDePasse.Text.Trim());
                cmd.Parameters.AddWithValue("?", comboBoxSpecialite.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("?", _IdUtilisateur);

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
                txtMotDePasse.ReadOnly = false;
                comboBoxSpecialite.Enabled = true;

                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {
                // Mettre à jour les données en base
                UpdateDoctorInfo();

                // Désactiver les champs après mise à jour
                txtTelephone.ReadOnly = true;
                txtMail.ReadOnly = true;
                txtMotDePasse.ReadOnly = true;
                comboBoxSpecialite.Enabled = false;

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
            comboBoxSpecialite.Enabled = false;

            // Réinitialiser le bouton Modifier
            modifier_btn.Text = "Modifier";
            isEditMode = false;

            // Recharger les données depuis la base
            LoadDoctorInfo();

            MessageBox.Show("Modifications annulées.");
        }


        /// <summary>
        /// remplir la table de dipos , modifier ou annuler 
        /// </summary>

        bool isEditMode2 = false;
        private OleDbConnection connection;

        private void button2_Click(object sender, EventArgs e)
        {
           
            if (!isEditMode2) //changer en sauvgarder
            {
                dataGridViewSchedule.ReadOnly = false;
                isEditMode2 = true;
                btnValiderDisponibilite.Text = "Sauvegarder";
            }
            else //retour a modifier
            {
                SaveChanges();
                dataGridViewSchedule.ReadOnly = true;
                isEditMode2 = false;
                btnValiderDisponibilite.Text = "Modifier";
                
            }
        }


        private void SaveChanges()
        {
            // savoir si ladmin a validé les dispos --> EstValide=true
            string validationQuery = "SELECT COUNT(*) FROM PlageHoraire " +
                                   "WHERE IdMedecin = ? AND SemaineDebut = ? AND EstValide = true";

            connection.Open();
            using (OleDbCommand validationCmd = new OleDbCommand(validationQuery, connection))
            {
                validationCmd.Parameters.AddWithValue("?", idMedecin);
                validationCmd.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);
                int validatedCount = Convert.ToInt32(validationCmd.ExecuteScalar());
                //si count>0 on ne peut plus modifier
                if (validatedCount > 0)
                {
                    MessageBox.Show("Modification impossible. Certaines plages sont déjà validées.",
                                  "Information",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    connection.Close();
                    return;
                }
            }
            connection.Close();

            // sinon on enregistrer les changements
            foreach (DataGridViewRow row in dataGridViewSchedule.Rows)
            {
                if (row.Cells["Heure"].Value != null)
                {
                    string heureDebut = row.Cells["Heure"].Value.ToString();
                    string heureFin = DateTime.Parse(heureDebut).AddMinutes(30).ToString("HH:mm");

                    for (int col = 1; col < dataGridViewSchedule.Columns.Count; col++)
                    {
                        string jour = dataGridViewSchedule.Columns[col].Name;
                        bool isChecked = Convert.ToBoolean(row.Cells[col].Value);

                        
                        bool slotExists = false;
                        string checkQuery = "SELECT COUNT(*) FROM PlageHoraire " +
                                          "WHERE IdMedecin = ? AND Jour = ? AND HeureDebut = ? AND SemaineDebut = ?";

                        connection.Open();
                        using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("?", idMedecin);
                            checkCmd.Parameters.AddWithValue("?", jour);
                            checkCmd.Parameters.AddWithValue("?", heureDebut);
                            checkCmd.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);
                            slotExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                        }
                        connection.Close();

                        // Perform appropriate action
                        if (isChecked)
                        {
                            if (slotExists)
                            {
                                UpdateRow(jour, heureDebut, heureFin);
                            }
                            else
                            {
                                InsertRow(jour, heureDebut, heureFin);
                            }
                        }
                        else if (slotExists)
                        {
                            DeleteRow(jour, heureDebut);
                        }
                    }
                }
            }
            MessageBox.Show("Disponibilité sauvegardée/Mise à jour avec succès!");
        }

        private void UpdateRow(string jour, string heureDebut, string heureFin)
        {
            string queryUpdate = "UPDATE PlageHoraire SET HeureFin = ? WHERE IdMedecin = ? AND Jour = ? AND HeureDebut = ? AND SemaineDebut = ?";
            using (OleDbCommand cmdUpdate = new OleDbCommand(queryUpdate, connection))
            {
                cmdUpdate.Parameters.AddWithValue("?", heureFin);
                cmdUpdate.Parameters.AddWithValue("?", idMedecin); 
                cmdUpdate.Parameters.AddWithValue("?", jour);
                cmdUpdate.Parameters.AddWithValue("?", heureDebut);
                cmdUpdate.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);

                connection.Open();
                cmdUpdate.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void InsertRow(string jour, string heureDebut, string heureFin)
        {
            string queryInsert = "INSERT INTO PlageHoraire (IdMedecin, Jour, HeureDebut, HeureFin, SemaineDebut) VALUES (?, ?, ?, ?, ?)";
            using (OleDbCommand cmdInsert = new OleDbCommand(queryInsert, connection))
            {
                cmdInsert.Parameters.AddWithValue("?", idMedecin); 
                cmdInsert.Parameters.AddWithValue("?", jour);
                cmdInsert.Parameters.AddWithValue("?", heureDebut);
                cmdInsert.Parameters.AddWithValue("?", heureFin);
                cmdInsert.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);

                connection.Open();
                cmdInsert.ExecuteNonQuery();
                connection.Close();
            }
        }
        private void DeleteRow(string jour, string heureDebut)
        {
            string queryDelete = "DELETE FROM PlageHoraire WHERE IdMedecin = ? AND Jour = ? AND HeureDebut = ? AND SemaineDebut = ?";

            OleDbCommand cmdDelete = new OleDbCommand(queryDelete, connection);

            cmdDelete.Parameters.AddWithValue("?", idMedecin); 
            cmdDelete.Parameters.AddWithValue("?", jour);
            cmdDelete.Parameters.AddWithValue("?", heureDebut);
            cmdDelete.Parameters.AddWithValue("?", groupBoxDisponibilite.Text);

            connection.Open();
            cmdDelete.ExecuteNonQuery();
            connection.Close();
        }


        private void btnAnnulerDisponibilite_Click(object sender, EventArgs e)
        {
            ResetChanges();
        }
        private void ResetChanges()
        {
            LoadDisponibiliteFromDatabase();
        }
        private void profile_pic_Click(object sender, EventArgs e)
        {
            DoctorDashboard dashboard = new DoctorDashboard(_IdUtilisateur);
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

                MenuForm menu = new MenuForm();
                menu.Show();


                this.Close();
            }
        }

       
    }
}

