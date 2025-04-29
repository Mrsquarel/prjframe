using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjframe
{
    public partial class DoctorDashboard : Form
    {
        private OleDbConnection connection;
        private int _idUtilisateur;
        private int idPatient = 0;
        private int idRdv = 0;
        private int idDossier = 0;
        private int idConsultation = 0;
        public DoctorDashboard(int idUtilisateur)
        {
            InitializeComponent();
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
            _idUtilisateur = idUtilisateur;
        }

        private void Form1_Load(object sender, EventArgs e)
        {   //afficher jour
            date_label.Text = DateTime.Today.ToLongDateString();
           
            // datagridview emploi du temps
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Heure", "Heure");
            dataGridView1.Columns.Add("Patient", "Patient");

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.ReadOnly = true;
            }

            // datagridview notes consultation
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Add("Date", "Date");
            dataGridView2.Columns.Add("Notes", "Notes");

            // Ajuster les propriétés des colonnes avec des tailles fixes
            dataGridView2.Columns["Date"].Width = 80;
            dataGridView2.Columns["Notes"].Width = 313;

            

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;


            // datagridview historique document

            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Add("Date", "Date");
            dataGridView3.Columns.Add("Type", "Type");
            DataGridViewButtonColumn viewBtn_2 = new DataGridViewButtonColumn();
            viewBtn_2.Name = "Voir";
            viewBtn_2.HeaderText = "    Contenu";
            viewBtn_2.Text = "Voir";
            viewBtn_2.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(viewBtn_2);

           

            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //affihcer nom docteur et specialite


            string query = @"SELECT u.Nom, u.Prenom, m.Specialite 
                    FROM Medecin m
                    INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur
                    WHERE m.IdUtilisateur = ?";

            connection.Open();
            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("?", _idUtilisateur);

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
            //charger les rdv d'aujourdhui
            LoadDayAppointments(_idUtilisateur);
            //charger le prochain patient
            GetNextPatientOfDay(_idUtilisateur);
            //charger le dossier medical du prochain patient
            if (idPatient != 0)
            {
                LoadDossierMedical(idPatient);
            }
            else
            {
                richTextBox1.Text = "Aucun patient restant";
            }
            //charger l'historique medical du prochain patient et ses documents
            LoadConsultationHistory(idPatient);
            LoadDocumentHistory(idDossier);
            //afficher nbr patient
            nbr_cons.Text = dataGridView1.Rows.Count.ToString();
        }
        //***charger les rdv de cette semaine***//

        private void LoadDayAppointments(int doctorUserId)
        {
            // retourner le nom du jour courant en français (majuscule initiale)
            var fr = CultureInfo.GetCultureInfo("fr-FR");
            string rawDayName = fr.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string todayName = char.ToUpper(rawDayName[0]) + rawDayName.Substring(1);

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Heure", "Heure");
            dataGridView1.Columns.Add("Patient", "Patient");
            dataGridView1.Rows.Clear();

            const string sql = @"
        SELECT 
            r.IdRendezVous,
            p.HeureDebut, 
            uPat.Nom       AS PatientNom, 
            uPat.Prenom    AS PatientPrenom,
            pat.IdPatient 
        FROM 
            (
              (
                (
                  (
                    RendezVous AS r
                    INNER JOIN PlageHoraire AS p ON r.IdPlage     = p.IdPlage
                  )
                  INNER JOIN Patient     AS pat ON r.IdPatient   = pat.IdPatient
                )
                INNER JOIN Utilisateur AS uPat ON pat.IdUtilisateur = uPat.IdUtilisateur
              )
              INNER JOIN Medecin     AS m    ON r.IdMedecin    = m.IdMedecin
            )
            INNER JOIN Utilisateur AS uDoc ON m.IdUtilisateur = uDoc.IdUtilisateur
        WHERE 
            r.Statut             = TRUE
            AND p.Jour           = ?
            AND uDoc.IdUtilisateur = ?
        ORDER BY 
            p.HeureDebut";

            using (var cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", todayName);
                cmd.Parameters.AddWithValue("?", doctorUserId);

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idRdv = (int)reader["IdRendezVous"];
                        string heure = Convert.ToDateTime(reader["HeureDebut"]).ToString("HH:mm");
                        string patient = $"{reader["PatientNom"]} {reader["PatientPrenom"]}";
                        int patientIdUtilisateur = Convert.ToInt32(reader["IdPatient"]);

                        // Ajouter la ligne et stocker l'IdUtilisateur dans le Tag
                        int rowIndex = dataGridView1.Rows.Add(heure, patient);
                        dataGridView1.Rows[rowIndex].Tag = patientIdUtilisateur;
                    }
                }
                connection.Close();
            }
        }
        //********savoir le prochain patient *************  //
        private void GetNextPatientOfDay(int doctorId)
        {
            
            var fr = CultureInfo.GetCultureInfo("fr-FR");
            string raw = fr.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            string todayName = char.ToUpper(raw[0]) + raw.Substring(1);

            const string sql = @"
                SELECT TOP 1
                    pat.IdPatient,
                    uPat.Nom    AS NomPatient,
                    uPat.Prenom AS PrenomPatient,
                    p.HeureDebut,
                    r.IdRendezVous
                FROM 
                    (  
                      ((RendezVous AS r
                        INNER JOIN PlageHoraire AS p ON r.IdPlage = p.IdPlage)
                       INNER JOIN Patient     AS pat ON r.IdPatient = pat.IdPatient)
                      INNER JOIN Utilisateur AS uPat ON pat.IdUtilisateur = uPat.IdUtilisateur
                    )
                    INNER JOIN 
                      (Medecin AS m
                       INNER JOIN Utilisateur AS uDoc ON m.IdUtilisateur = uDoc.IdUtilisateur)
                    ON r.IdMedecin = m.IdMedecin
                WHERE 
                    r.Statut             = TRUE
                    AND p.Jour            = ?
                    AND uDoc.IdUtilisateur = ?
                    AND p.HeureDebut     >= ?
                ORDER BY 
                    p.HeureDebut";

            idPatient = 0; 

            connection.Open();
            using (var cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", todayName);
                cmd.Parameters.AddWithValue("?", doctorId);
                cmd.Parameters.AddWithValue("?", DateTime.Now.TimeOfDay);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idPatient = Convert.ToInt32(reader["IdPatient"]);
                        idRdv = Convert.ToInt32(reader["IdRendezVous"]);
                        string nom = reader["NomPatient"].ToString();
                        string prenom = reader["PrenomPatient"].ToString();
                        lblNextPatient.Text = $"{prenom} {nom}";
                    }
                }
            }
            connection.Close();

            if (idPatient == 0)
                lblNextPatient.Text = "Aucun patient restant";
        }

        private void LoadDossierMedical(int patientId)
        {
            const string sql = @"
                SELECT 
                    dm.IdDossier,
                    dm.IdPatient,
                    dm.DossierCreationDate,
                    u.Nom,
                    u.Prenom,
                    p.Sexe
                FROM 
                    (DossierMedical AS dm
                     INNER JOIN Patient AS p ON dm.IdPatient = p.IdPatient)
                    INNER JOIN Utilisateur AS u ON p.IdUtilisateur = u.IdUtilisateur
                WHERE 
                    dm.IdPatient = ?";

            using (var cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", patientId);
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string prenom = reader["Prenom"].ToString();
                        string sexe = reader["Sexe"].ToString();
                        DateTime dateCreate = Convert.ToDateTime(reader["DossierCreationDate"]);
                        idDossier = Convert.ToInt32(reader["IdDossier"]);

                        richTextBox1.Lines = new[]
                        {
                            $"Nom                 : {nom}",
                            $"Prénom             : {prenom}",
                            $"Sexe               : {sexe}",
                            $"DossierCreationDate : {dateCreate:g}"
                        };
                    }
                    else
                    {
                        richTextBox1.Text = "Aucun dossier médical trouvé pour ce patient.";
                    }
                }
                connection.Close();
            }

        }

        private void CreateNewDossier(int patientId)
        {
            string formattedDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string sql = $@"INSERT INTO DossierMedical (IdPatient, DossierCreationDate) 
                           VALUES ({patientId}, #{formattedDate}#)";

            
                connection.Open();
                using (var cmd = new OleDbCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

            string selectSql = @"SELECT IdDossier FROM DossierMedical WHERE IdPatient = ?";
            using (var cmd = new OleDbCommand(selectSql, connection))
            {
                cmd.Parameters.AddWithValue("?", patientId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idDossier = Convert.ToInt32(reader["IdDossier"]);
                    }
                    else
                    {
                        idDossier = 0;
                    }
                }
            }
            connection.Close();
            MessageBox.Show(
                "Dossier médical créé avec succès.",
                "Succès",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /********************afficher consultqtion histortique***************/
        private void LoadConsultationHistory(int patientId)
        {

            // Requête SQL pour récupérer les consultations du patient
            const string selectSql = @"
        SELECT ConsultationDate, Notes 
        FROM Consultation 
        WHERE IdPatient = ? 
        ORDER BY ConsultationDate DESC";

            connection.Open();
            using (var cmd = new OleDbCommand(selectSql, connection))
            {
                // Ajouter le paramètre IdPatient
                cmd.Parameters.Add("?", OleDbType.Integer).Value = patientId;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupérer les valeurs
                        DateTime consultationDate = reader.GetDateTime(0); // Date
                        string notes = reader.IsDBNull(1) ? string.Empty : reader.GetString(1); // Notes (gérer les valeurs NULL)

                        // Ajouter une ligne au DataGridView
                        dataGridView2.Rows.Add(consultationDate.ToString("dd/MM/yyyy"), notes);
                    }
                }
            }

            connection.Close();
        }
        private void LoadDocumentHistory(int dossierId)
        {
            dataGridView3.Rows.Clear();

            const string sqlDoc = @"
            SELECT 
                NomFichier,
                CheminFichier
            FROM Document
            WHERE IdDossier = ?";

            using (var cmd = new OleDbCommand(sqlDoc, connection))
            {
                cmd.Parameters.AddWithValue("?", dossierId);
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nomFichier = reader["NomFichier"].ToString();
                        string chemin = reader["CheminFichier"].ToString();

                        string date = "";
                        if (File.Exists(chemin))
                            date = File.GetLastWriteTime(chemin).ToString("dd/MM/yyyy");

                        int rowIndex = dataGridView3.Rows.Add(date, nomFichier);
                        dataGridView3.Rows[rowIndex].Tag = chemin;
                    }
                }
                connection.Close();
            }

            if (dataGridView3.Rows.Count == 0)
                dataGridView3.Rows.Add("", "Aucun document", null);
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

        private void profile_pic_Click(object sender, EventArgs e)
        {
            ProfileDoctor profile = new ProfileDoctor(_idUtilisateur);
            profile.Show();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateNewDossier(idPatient);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Patient_infos patient = new Patient_infos(idPatient,_idUtilisateur,idRdv);
            patient.Show();
            
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            const string sql = @"
                INSERT INTO Consultation 
                   (IdRendezVous, IdDossier, ConsultationDate, Notes ,IdPatient)
                VALUES (?,?,?,?,?)";

            connection.Open();
            using (var cmd = new OleDbCommand(sql, connection))
            {
                var p1 = cmd.Parameters.Add("?", OleDbType.Integer);
                p1.Value = idRdv;

                var p2 = cmd.Parameters.Add("?", OleDbType.Integer);
                p2.Value = idDossier;

                var p3 = cmd.Parameters.Add("?", OleDbType.Date);
                p3.Value = DateTime.Now;

                var p4 = cmd.Parameters.Add("?", OleDbType.VarChar);
                p4.Value = richTextBox2.Text.Trim();

                var p5 = cmd.Parameters.Add("?", OleDbType.Integer);
                p5.Value = idPatient;

                cmd.ExecuteNonQuery();
            }

            // Étape 2 : Récupérer l'IdConsultation de la nouvelle consultation
            const string selectSql = @"
            SELECT idConsultation
            FROM Consultation
            WHERE IdRendezVous = ?";
            using (var cmd = new OleDbCommand(selectSql, connection))
            {
                cmd.Parameters.AddWithValue("?", idRdv);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idConsultation = Convert.ToInt32(reader["idConsultation"]);
                    }
                    else
                    {
                        idConsultation = 0;
                    }
                }
            }

            // Étape 3 : Insérer une nouvelle facture pour cette consultation
            const string sqlInsertFacture = @"
            INSERT INTO Facture (IdConsultation, DatePaiement)
            VALUES (?, ?)";
            using (var cmd = new OleDbCommand(sqlInsertFacture, connection))
            {
                cmd.Parameters.Add("?", OleDbType.Integer).Value = idConsultation;
                cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                cmd.ExecuteNonQuery();
            }

            connection.Close();

            MessageBox.Show(
                "Consultation et facture enregistrées avec succès.",
                "Succès",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

           
            
            connection.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (idPatient == 0)
            {
                MessageBox.Show(
                    "Aucun patient sélectionné.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (idConsultation == 0)
            {
                MessageBox.Show(
                    "Veuillez d'abord enregistrer une consultation.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            
            prescription_form prescriptionForm = new prescription_form(
                idPatient,
                _idUtilisateur,
                idConsultation
            );
            prescriptionForm.ShowDialog();


        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ne traiter que le clic sur la colonne "Voir"
            if (e.RowIndex < 0 || dataGridView3.Columns[e.ColumnIndex].Name != "Voir")
                return;

            // Récupérer le chemin stocké dans Tag
            var chemin = dataGridView3.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(chemin) || !File.Exists(chemin))
            {
                MessageBox.Show("Fichier introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Ouvrir le fichier quel qu'il soit avec l'application par défaut
                var psi = new ProcessStartInfo
                {
                    FileName = chemin,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d’ouvrir le fichier :\n{ex.Message}", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Récupérer la ligne cliquée
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Récupérer l'IdUtilisateur du patient à partir du Tag et le stocker dans patientId
                if (row.Tag is int patientIdUtilisateur)
                {
                    idPatient = patientIdUtilisateur;
                }
                else
                {
                    idPatient = 0; // Réinitialiser à 0 si l'IdUtilisateur ne peut pas être récupéré
                }
            }
            if (idPatient == 0)
            {
                MessageBox.Show(
                    "Aucun patient sélectionné.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            else
            {   // Charger le dossier médical et l'historique de consultation du patient sélectionné
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                richTextBox1.Clear();
                LoadDossierMedical(idPatient);
                LoadConsultationHistory(idPatient);
                LoadDocumentHistory(idDossier);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}