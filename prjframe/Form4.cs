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
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prjframe
{
    public partial class PatientDashboard : Form
    {
        private int _idUtilisateur;
        private int idPatient = 0;
        private int DoctorId = 0;
        private OleDbConnection connection;
       

        public PatientDashboard(int idUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = idUtilisateur;
           
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            connection = new OleDbConnection(connectionString);
        }

        private void Patient_Load(object sender, EventArgs e)
        {
            dataGridViewHistorique.Columns.Clear();

           
            dataGridViewHistorique.Columns.Add("colDocteur", "Nom du Docteur");
            dataGridViewHistorique.Columns.Add("colConsultation", "Nom de Consultation");
            dataGridViewHistorique.Columns.Add("colNotes", "Notes");

            DataGridViewButtonColumn dossierBtn = new DataGridViewButtonColumn();
            dossierBtn.Name = "colDossier";
            dossierBtn.HeaderText = "Dossier";
            dossierBtn.Text = "Voir";
            dossierBtn.UseColumnTextForButtonValue = true;
            dataGridViewHistorique.Columns.Add(dossierBtn);

            DataGridViewButtonColumn prescriptionBtn = new DataGridViewButtonColumn();
            prescriptionBtn.Name = "colPrescription";
            prescriptionBtn.HeaderText = "Prescription";
            prescriptionBtn.Text = "Voir";
            prescriptionBtn.UseColumnTextForButtonValue = true;
            dataGridViewHistorique.Columns.Add(prescriptionBtn);

            // Add rows (dummy data)
            dataGridViewHistorique.Rows.Add("Dr. Karim", "Consultation Générale", "Suivi régulier...");
            dataGridViewHistorique.Rows.Add("Dr. Nadia", "Cardiologie", "ECG effectué...");

            dataGridViewHistorique.RowHeadersVisible = false;

            //***************afficher la semaine**************************//
            // Date d'aujourd'hui
            DateTime aujourdHui = DateTime.Today;

            // Calcul du lundi de la semaine actuelle
            int joursDepuisLundi = (int)aujourdHui.DayOfWeek - 1;
            if (joursDepuisLundi < 0) joursDepuisLundi = 6; // Si dimanche

            DateTime lundi = aujourdHui.AddDays(-joursDepuisLundi);
            DateTime samedi = lundi.AddDays(5); // Samedi = lundi + 5 jours

            // Formatage des dates
            string texte = $"DU {lundi:dddd d MMMM yyyy} au {samedi:dddd d MMMM yyyy}";

            label_week.Text = texte;

            // Charger idPatient
            

            string query = "SELECT IdPatient FROM Patient WHERE IdUtilisateur = ?";
            connection.Open();
            OleDbCommand cmd2 = new OleDbCommand(query, connection);
            cmd2.Parameters.AddWithValue("?", _idUtilisateur);
            OleDbDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                idPatient = (int)reader2["IdPatient"];
            }
            connection.Close();
            //************************prendre rdv*******************************/
            LoadSpecialites();


        }
        //charger les specilaites des docteurs pour prendre un rdv
        private void LoadSpecialites()
        {
            comboBoxSpecialite.Items.Clear();
            string query = "SELECT DISTINCT Specialite FROM Medecin";

            connection.Open();
            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxSpecialite.Items.Add(reader["Specialite"].ToString());
                    }
                }
            }
            connection.Close();
        }
        //apres la selection d'une specialite , on charge la liste des medecins
        private void comboBoxSpecialite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSpecialite.SelectedItem == null) return;
            string selectedSpecialty = comboBoxSpecialite.SelectedItem.ToString();
            LoadDoctorNames(selectedSpecialty);
        }

        private void LoadDoctorNames(string specialty)
        {
            string query = "SELECT m.IdMedecin, u.Nom, u.Prenom " +
                            "FROM Medecin m " +
                            "INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur " +
                            "WHERE m.Specialite = ?";

            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", specialty);

            connection.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            comboBoxDoctors.Items.Clear();

            while (reader.Read())
            {
                string doctorName = reader["Nom"].ToString() + " " + reader["Prenom"].ToString();
                int doctorId = Convert.ToInt32(reader["IdMedecin"]);
                comboBoxDoctors.Items.Add(new KeyValuePair<int, string>(doctorId, doctorName));
            }

            reader.Close();
            connection.Close();

            if (comboBoxDoctors.Items.Count == 0)
            {
                MessageBox.Show("Aucun médecin trouvé.");
            }

            comboBoxDoctors.DisplayMember = "Value";
            comboBoxDoctors.ValueMember = "Key";
        }
        //on recupere lid du docteur
        private void comboBoxDoctors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDoctors.SelectedItem == null) return;
            KeyValuePair<int, string> selectedDoctor = (KeyValuePair<int, string>)comboBoxDoctors.SelectedItem;
            DoctorId = selectedDoctor.Key; 
            LoadAvailableRDVs(DoctorId);
        }
        //choix d'un rdv 
        private void valider_btn_Click(object sender, EventArgs e)
        {
            if (comboBoxDoctors.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un médecin.");
                return;
            }

            KeyValuePair<int, string> selectedDoctor = (KeyValuePair<int, string>)comboBoxDoctors.SelectedItem;
            DoctorId = selectedDoctor.Key;
            LoadAvailableRDVs(DoctorId); ;
        }
        private void LoadAvailableRDVs(int doctorId)
        {
            checkedListBoxRDV.Items.Clear(); 
            checkedListBoxRDV.DisplayMember = "Display"; 

            DateTime now = DateTime.Now;
            DateTime tomorrow = now.Date.AddDays(1); // le patient peut prendre un rdv à partir de demain
            int daysLeft = 6 - (int)now.DayOfWeek;

            Dictionary<string, int> frenchDayToIndex = new Dictionary<string, int>
            {
                { "Dimanche", 0 },
                { "Lundi", 1 },
                { "Mardi", 2 },
                { "Mercredi", 3 },
                { "Jeudi", 4 },
                { "Vendredi", 5 },
                { "Samedi", 6 }
            };

            
            string query = "SELECT IdPlage, Jour, HeureDebut, HeureFin, SemaineDebut " +
                           "FROM PlageHoraire " +
                           "WHERE IdMedecin = ? AND EstValide = true AND Prise = false AND SemaineDebut = ?";

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("?", doctorId);
                cmd.Parameters.AddWithValue("?", label_week.Text); 
                connection.Open();

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idPlage = Convert.ToInt32(reader["IdPlage"]);
                        string jour = reader["Jour"].ToString();
                        string heureDebutStr = reader["HeureDebut"].ToString();
                        string heureFinStr = reader["HeureFin"].ToString();

                        if (frenchDayToIndex.ContainsKey(jour))
                        {
                            int jourIndex = frenchDayToIndex[jour];
                            int todayIndex = (int)now.DayOfWeek;
                            int dayOffset = (jourIndex - todayIndex + 7) % 7;

                            DateTime rdvDate = now.Date.AddDays(dayOffset);

                            if (dayOffset > 0 && dayOffset <= daysLeft)
                            {
                                // Parse HeureDebut et HeureFin pour extraire seulement la partie des heures (HH:mm)
                                DateTime heureDebut = Convert.ToDateTime(heureDebutStr);
                                DateTime heureFin = Convert.ToDateTime(heureFinStr);

                                string formattedHeureDebut = heureDebut.ToString("H:mm");
                                string formattedHeureFin = heureFin.ToString("H:mm");

                               
                                string rdv = $"{jour} {formattedHeureDebut} - {formattedHeureFin}";

                                // stocker lidplage avec la chiane du rdv pour faciliter la reservation
                                checkedListBoxRDV.Items.Add(new { Display = rdv, IdPlage = idPlage }, false);
                            }
                        }
                    }
                }

                connection.Close();
            }

            checkedListBoxRDV.Refresh(); 

            if (checkedListBoxRDV.Items.Count == 0)
                MessageBox.Show("Aucun rendez-vous disponible à réserver pour les jours suivants.");
        }

        private void checkedListBoxRDV_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBoxRDV.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        checkedListBoxRDV.SetItemChecked(i, false);
                    }
                }
            }
        }

        /****reserver un rdv****/
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBoxRDV.CheckedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un rendez-vous.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            var selectedItem = checkedListBoxRDV.CheckedItems[0];
            int idPlage = (int)selectedItem.GetType().GetProperty("IdPlage").GetValue(selectedItem);

            connection.Open();
            string updateQuery = "UPDATE PlageHoraire SET Prise = true WHERE IdPlage = ?";
            using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, connection))
            {
                updateCmd.Parameters.AddWithValue("?", idPlage);
                updateCmd.ExecuteNonQuery();
            }

            string insertQuery = "INSERT INTO RendezVous (IdMedecin, IdPatient, IdPlage) " +
                                 "VALUES (?, ?, ?)";
            using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, connection))
            {
                insertCmd.Parameters.AddWithValue("?", DoctorId); 
                insertCmd.Parameters.AddWithValue("?", idPatient); 
                insertCmd.Parameters.AddWithValue("?", idPlage);
                insertCmd.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show("Vous seriez notifés de la reservation de votre rdv!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadAvailableRDVs(DoctorId); 
        }


        private void profile_pic_Click(object sender, EventArgs e)
        {
            ProfilePatient profile = new ProfilePatient(_idUtilisateur);
            profile.Show();
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
