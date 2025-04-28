using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            /*****charger nom patient *****************/
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
            /*******charger la date *********/
            date_label.Text = DateTime.Today.ToLongDateString();

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


            string query2 = "SELECT IdPatient FROM Patient WHERE IdUtilisateur = ?";
            connection.Open();
            OleDbCommand cmd2 = new OleDbCommand(query2, connection);
            cmd2.Parameters.AddWithValue("?", _idUtilisateur);
            OleDbDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                idPatient = (int)reader2["IdPatient"];
            }
            connection.Close();

            //************************prendre rdv*******************************/
            LoadSpecialites();



            //************charger l'historique des consultations**********/
            dataGridViewHistorique.Columns.Clear();
            Console.WriteLine("DataGridView columns cleared.");

            // Ajouter les colonnes
            dataGridViewHistorique.Columns.Add("colDocteur", "Nom du Docteur");
            dataGridViewHistorique.Columns.Add("colConsultation", "Nom de Consultation");
            dataGridViewHistorique.Columns.Add("colNotes", "Notes");

        

            DataGridViewButtonColumn prescriptionBtn = new DataGridViewButtonColumn();
            prescriptionBtn.Name = "colPrescription";
            prescriptionBtn.HeaderText = "Prescription";
            prescriptionBtn.Text = "Voir";
            prescriptionBtn.UseColumnTextForButtonValue = true;
            dataGridViewHistorique.Columns.Add(prescriptionBtn);

            Console.WriteLine("DataGridView columns added.");

            // Configurer les propriétés du DataGridView
            dataGridViewHistorique.RowHeadersVisible = false;
            dataGridViewHistorique.AllowUserToAddRows = false;
            dataGridViewHistorique.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Appliquer le style
            dataGridViewHistorique.EnableHeadersVisualStyles = false; // Désactiver le style par défaut des en-têtes
            dataGridViewHistorique.BorderStyle = BorderStyle.None; // Supprimer la bordure extérieure
            dataGridViewHistorique.BackgroundColor = Color.White; // Fond blanc

            // Style des en-têtes de colonnes
            dataGridViewHistorique.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // Bleu foncé
            dataGridViewHistorique.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Texte blanc
            dataGridViewHistorique.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Police en gras
            dataGridViewHistorique.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centrer le texte
            dataGridViewHistorique.ColumnHeadersDefaultCellStyle.Padding = new Padding(5); // Espacement interne

            // Style des cellules
            dataGridViewHistorique.DefaultCellStyle.Font = new Font("Segoe UI", 9); // Police standard
            dataGridViewHistorique.DefaultCellStyle.Padding = new Padding(3); // Espacement interne
            dataGridViewHistorique.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Alignement à gauche pour le texte
            dataGridViewHistorique.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 174, 219); // Couleur de sélection
            dataGridViewHistorique.DefaultCellStyle.SelectionForeColor = Color.White; // Texte blanc en sélection

            // Style des lignes alternées
            dataGridViewHistorique.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Gris clair pour lignes alternées

            // Style spécifique pour les colonnes de boutons
            dataGridViewHistorique.Columns["colPrescription"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Ajuster la hauteur des lignes
            dataGridViewHistorique.RowTemplate.Height = 30; // Hauteur de ligne de 30 pixels

            // Faire remplir les colonnes tout l'espace du DataGridView
            dataGridViewHistorique.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Optionnel : Ajuster les proportions des colonnes (si vous voulez des largeurs spécifiques)
            dataGridViewHistorique.Columns["colDocteur"].FillWeight = 30; // 30% de l'espace
            dataGridViewHistorique.Columns["colConsultation"].FillWeight = 20; // 20% de l'espace
            dataGridViewHistorique.Columns["colNotes"].FillWeight = 30; // 30% de l'espace
            dataGridViewHistorique.Columns["colPrescription"].FillWeight = 20; 

            // Supprimer les lignes de grille pour un look plus propre
            dataGridViewHistorique.CellBorderStyle = DataGridViewCellBorderStyle.None;


            try
            {
                // Étape 1 : Récupérer le dossierId du patient
                int dossierId = 0;
                const string sqlDossier = @"
            SELECT dm.IdDossier
            FROM DossierMedical AS dm
            INNER JOIN Patient AS p 
                ON dm.IdPatient = p.IdPatient
            WHERE p.IdUtilisateur = ?";
                using (var cmd = new OleDbCommand(sqlDossier, connection))
                {
                    cmd.Parameters.AddWithValue("?", _idUtilisateur);
                    Console.WriteLine("Executing sqlDossier with IdUtilisateur: " + _idUtilisateur);
                    connection.Open();
                    Console.WriteLine("Connection opened for sqlDossier.");
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        dossierId = Convert.ToInt32(result);
                        Console.WriteLine($"Dossier ID retrieved: {dossierId}");
                    }
                    else
                    {
                        Console.WriteLine("No dossier found for IdUtilisateur: " + _idUtilisateur);
                        MessageBox.Show("Aucun dossier médical trouvé pour cet utilisateur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    connection.Close();
                    Console.WriteLine("Connection closed after sqlDossier.");
                }

                if (dossierId == 0)
                {
                    Console.WriteLine("Aborting: dossierId is 0.");
                    return;
                }

                // Étape 2 : Charger la liste des consultations
                var consultations = new List<(int IdDossier, int IdRendezVous, string Notes)>();
                int consultationId = 0;
                const string sqlCons = @"
                SELECT c.IdConsultation,c.IdDossier, c.IdRendezVous, c.Notes
                FROM Consultation AS c
                WHERE c.IdDossier = ?";
                using (var cmd = new OleDbCommand(sqlCons, connection))
                {
                    cmd.Parameters.AddWithValue("?", dossierId);
                    Console.WriteLine($"Executing sqlCons with IdDossier: {dossierId}");
                    connection.Open();
                    Console.WriteLine("Connection opened for sqlCons.");
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                           consultationId= Convert.ToInt32(rd["IdConsultation"]);
                           var consultation = (
                                IdDossier: Convert.ToInt32(rd["IdDossier"]),
                                IdRendezVous: Convert.ToInt32(rd["IdRendezVous"]),
                                Notes: rd["Notes"].ToString()
                            );
                            consultations.Add(consultation);
                            Console.WriteLine($"Consultation found: IdDossier={consultation.IdDossier}, IdRendezVous={consultation.IdRendezVous}, Notes={consultation.Notes}");
                        }
                    }
                    connection.Close();
                    Console.WriteLine("Connection closed after sqlCons.");
                }

                Console.WriteLine($"Total consultations found: {consultations.Count}");
                if (consultations.Count == 0)
                {
                    Console.WriteLine("No consultations found for dossierId: " + dossierId);
                    MessageBox.Show("Aucune consultation trouvée pour ce dossier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Étape 3 : Charger le chemin de la prescription (s’il y en a qu’une)
                string prescriptionPath = null;
                const string sqlPresc = @"
                SELECT TOP 1 CheminFichier 
                FROM Prescription
                WHERE   IdConsultation = ? 
                 ";
               
                using (var cmd = new OleDbCommand(sqlPresc, connection))
                {
                    cmd.Parameters.AddWithValue("?", consultationId);
                    connection.Open();
                    Console.WriteLine("Connection opened for sqlPresc.");
                    prescriptionPath = cmd.ExecuteScalar() as string;
                    Console.WriteLine($"Prescription Path: {(prescriptionPath ?? "No prescription found")}");
                    connection.Close();
                    Console.WriteLine("Connection closed after sqlPresc.");
                }
               

                // Étape 4 : Pour chaque consultation, récupérer le médecin
                const string sqlDoctor = @"
            SELECT
                uDoc.Nom AS DoctorNom,
                uDoc.Prenom AS DoctorPrenom,
                uDoc.IdUtilisateur AS DoctorIdUtilisateur
            FROM
                ((RendezVous AS r
                INNER JOIN Medecin AS m ON r.IdMedecin = m.IdMedecin)
                INNER JOIN Utilisateur AS uDoc ON m.IdUtilisateur = uDoc.IdUtilisateur)
            WHERE
                r.IdRendezVous = ?";

                // Parcourir les consultations et peupler le DataGridView
                foreach (var consultation in consultations)
                {
                    string doctorName = null;
                    int doctorIdUtilisateur = 0;

                    // Récupérer les informations du médecin pour cette consultation
                    using (var cmd = new OleDbCommand(sqlDoctor, connection))
                    {
                        cmd.Parameters.AddWithValue("?", consultation.IdRendezVous);
                        Console.WriteLine($"Executing sqlDoctor with IdRendezVous: {consultation.IdRendezVous}");
                        connection.Open();
                        Console.WriteLine("Connection opened for sqlDoctor.");
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                doctorName = $"{rd["DoctorNom"]} {rd["DoctorPrenom"]}";
                                doctorIdUtilisateur = Convert.ToInt32(rd["DoctorIdUtilisateur"]);
                                Console.WriteLine($"Doctor found: {doctorName}, IdUtilisateur: {doctorIdUtilisateur}");
                            }
                            else
                            {
                                Console.WriteLine($"No doctor found for IdRendezVous: {consultation.IdRendezVous}");
                            }
                        }
                        connection.Close();
                        Console.WriteLine("Connection closed after sqlDoctor.");
                    }

                    // Préparer les données pour la ligne
                    string consultationName = "Consultation Générale"; // Remplacer par un champ réel si disponible
                    string notes = consultation.Notes;
                    int idDossier = consultation.IdDossier;

                    // Ajouter la ligne au DataGridView
                    int rowIndex = dataGridViewHistorique.Rows.Add(doctorName, consultationName, notes);
                    Console.WriteLine($"Row added to DataGridView: Doctor={doctorName}, Consultation={consultationName}, Notes={notes}, RowIndex={rowIndex}");

                    // Stocker IdDossier, DoctorIdUtilisateur et CheminFichier dans le Tag
                    dataGridViewHistorique.Rows[rowIndex].Tag = new
                    {
                        IdDossier = idDossier,
                        DoctorIdUtilisateur = doctorIdUtilisateur,
                        PrescriptionPath = prescriptionPath
                    };
                    Console.WriteLine($"Row Tag set: IdDossier={idDossier}, DoctorIdUtilisateur={doctorIdUtilisateur}, PrescriptionPath={(prescriptionPath ?? "null")}");
                }

                Console.WriteLine($"DataGridView population complete. Total rows: {dataGridViewHistorique.Rows.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Patient_Load: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed after error.");
                }
            }

            

    

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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridViewHistorique_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Récupérer la ligne cliquée
            DataGridViewRow row = dataGridViewHistorique.Rows[e.RowIndex];
            var rowData = row.Tag as dynamic; // Utiliser dynamic pour accéder aux propriétés de l'objet anonyme
            if (rowData == null) return;

           if (dataGridViewHistorique.Columns[e.ColumnIndex].Name == "colPrescription")
            {
                string prescriptionPath = rowData.PrescriptionPath;
                if (!string.IsNullOrEmpty(prescriptionPath) && File.Exists(prescriptionPath))
                {
                    // Ouvrir le fichier de prescription
                    Process.Start(new ProcessStartInfo(prescriptionPath) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("Aucune prescription trouvée ou fichier introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
