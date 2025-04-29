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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;

namespace prjframe
{
    public partial class secretary_dashboard : Form
    {
        private int _idUtilisateur;
        private OleDbConnection connection;

        public secretary_dashboard(int idUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = idUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }

        private void secretary_dashboard_Load(object sender, EventArgs e)
        {
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
                        String prenom = reader["Prenom"].ToString();
                        string specialiteValue = reader["Departement"].ToString();
                        secretary_name.Text = nom + " " + prenom;
                        dept_label.Text = specialiteValue;
                    }
                }
            }
            connection.Close();

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
            semaine_label.Text = texte;


            //datagrid view rdv en attente
            DataGridViewTextBoxColumn idRdvCol = new DataGridViewTextBoxColumn();
            idRdvCol.Name = "IdRendezVous";
            idRdvCol.HeaderText = "IdRendezVous";
            idRdvCol.Visible = false;
            dataGridViewDemandes.Columns.Add(idRdvCol);


            dataGridViewDemandes.Columns.Add("DateRdv", "Date du Rendez-vous");
            dataGridViewDemandes.Columns.Add("NomPatient", "Patient");
            dataGridViewDemandes.Columns.Add("NomMedecin", "Médecin");


            DataGridViewButtonColumn confirmBtn = new DataGridViewButtonColumn();
            confirmBtn.Name = "Confirmer";
            confirmBtn.HeaderText = "Confirmation";
            confirmBtn.Text = "Confirmer";
            confirmBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(confirmBtn);


            DataGridViewButtonColumn refuseBtn = new DataGridViewButtonColumn();
            refuseBtn.Name = "Refuser";
            refuseBtn.HeaderText = "Annulation";
            refuseBtn.Text = "Refuser";
            refuseBtn.UseColumnTextForButtonValue = true;
            dataGridViewDemandes.Columns.Add(refuseBtn);

            dataGridViewDemandes.RowHeadersVisible = false;

            //datagrid view rdv confirmes

            dataGridViewConfirmes.Columns.Add("DateRdv", "Date du Rendez-vous");
            dataGridViewConfirmes.Columns.Add("NomPatient", "Nom du Patient");
            dataGridViewConfirmes.Columns.Add("NomMedecin", "Nom du Médecin");

            foreach (DataGridViewColumn col in dataGridViewConfirmes.Columns)
            {
                col.ReadOnly = false;
            }


            dataGridViewConfirmes.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewConfirmes.AllowUserToAddRows = true;
            dataGridViewConfirmes.AllowUserToDeleteRows = true;

            dataGridViewConfirmes.RowHeadersVisible = false;

            //data grid view paiements
            dataGridViewPaiements.Columns.Clear();

            // Ajouter les colonnes
            dataGridViewPaiements.Columns.Add("RdvID", "ID du Rendez-vous");
            dataGridViewPaiements.Columns.Add("NomPatient", "Nom du Patient");
            dataGridViewPaiements.Columns.Add("Montant", "Montant Payé (DT)");
            dataGridViewPaiements.Columns.Add("DatePaiement", "Date de Paiement");

            DataGridViewButtonColumn validerBtn = new DataGridViewButtonColumn();
            validerBtn.Name = "colValider";
            validerBtn.HeaderText = "Valider";
            validerBtn.Text = "Valider";
            validerBtn.UseColumnTextForButtonValue = true;
            dataGridViewPaiements.Columns.Add(validerBtn);

            DataGridViewButtonColumn genererFactureBtn = new DataGridViewButtonColumn();
            genererFactureBtn.Name = "colGenererFacture";
            genererFactureBtn.HeaderText = "Générer Facture";
            genererFactureBtn.Text = "Générer";
            genererFactureBtn.UseColumnTextForButtonValue = true;
            dataGridViewPaiements.Columns.Add(genererFactureBtn);


            // Configurer le DataGridView comme non éditable
            foreach (DataGridViewColumn col in dataGridViewPaiements.Columns)
            {
                col.ReadOnly = true;
            }
            dataGridViewPaiements.AllowUserToAddRows = false;
            dataGridViewPaiements.AllowUserToDeleteRows = false;
            dataGridViewPaiements.RowHeadersVisible = false;



            //datagrid view dossiers
            dataGridViewDossiers.Columns.Add("CIN", "CIN");
            dataGridViewDossiers.Columns.Add("Nom", "Nom");
            dataGridViewDossiers.Columns.Add("Prenom", "Prénom");
            dataGridViewDossiers.Columns.Add("Email", "Email");
            dataGridViewDossiers.Columns.Add("DateNaissance", "Date de Naissance");
            dataGridViewDossiers.Columns.Add("Assurance", "Assurance");

            dataGridViewDossiers.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewDossiers.AllowUserToAddRows = false;
            dataGridViewDossiers.RowHeadersVisible = false;




            LoadPendingAppointments();
            LoadConfirmedAppointments();
            LoadAllDossiers();
            LoadPaiementInfo();
        }
        /*****charger les demandes rdv*****/
        private void LoadPendingAppointments()
        {
            dataGridViewDemandes.Rows.Clear();

            string query = @"
            SELECT 
                r.IdRendezVous, 
                p.Jour, 
                p.HeureDebut, 
                up.Nom AS PatientNom, 
                up.Prenom AS PatientPrenom, 
                up.Email  AS PatientEmail,
                ud.Nom AS DoctorNom, 
                ud.Prenom AS DoctorPrenom
            FROM 
                (((RendezVous AS r
                INNER JOIN PlageHoraire AS p ON r.IdPlage = p.IdPlage)
                INNER JOIN Patient AS pat ON r.IdPatient = pat.IdPatient)
                INNER JOIN Utilisateur AS up ON pat.IdUtilisateur = up.IdUtilisateur)
                INNER JOIN (Medecin AS m
                INNER JOIN Utilisateur AS ud ON m.IdUtilisateur = ud.IdUtilisateur)
            ON r.IdMedecin = m.IdMedecin
            WHERE r.Statut = FALSE AND p.SemaineDebut = ?";

            connection.Open();
            var cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", semaine_label.Text);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idRendezVous = Convert.ToInt32(reader["IdRendezVous"]);
                    string jour = reader["Jour"].ToString();
                    DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                    string patientName = $"{reader["PatientPrenom"]} {reader["PatientNom"]}";
                    string patientEmail = reader["PatientEmail"].ToString();
                    string doctorName = $"Dr. {reader["DoctorPrenom"]} {reader["DoctorNom"]}";
                    string formattedDateTime = $"{jour} {heureDebut:HH:mm}";

                    int rowIndex = dataGridViewDemandes.Rows.Add();
                    DataGridViewRow row = dataGridViewDemandes.Rows[rowIndex];
                    row.Cells["IdRendezVous"].Value = idRendezVous;
                    row.Cells["DateRdv"].Value = formattedDateTime;
                    row.Cells["NomPatient"].Value = patientName;
                    row.Cells["NomMedecin"].Value = doctorName;

                    // Tag  contient  l'email
                    row.Tag = patientEmail;
                }
            }
            connection.Close();
        }
        /***confirmer ou refuser le rdv***/
        private void dataGridViewDemandes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dataGridViewDemandes.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dataGridViewDemandes.Rows[e.RowIndex];
            string formattedDate = row.Cells["DateRdv"].Value.ToString();
            string patientName = row.Cells["NomPatient"].Value.ToString();
            string doctorName = row.Cells["NomMedecin"].Value.ToString();
            int idRendezVous = Convert.ToInt32(row.Cells["IdRendezVous"].Value);

            if (columnName == "Confirmer")
            {
                // --- 1) Update en base ---
                string updateQuery = "UPDATE [RendezVous] SET [Statut] = TRUE WHERE [IdRendezVous] = ?";
                connection.Open();
                using (var cmd = new OleDbCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("?", idRendezVous);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();


                string patientEmail = row.Tag as string;  // email stocké en Tag

                // --- 3) Préparation du sujet et du corps du mail ---
                string subject = "Confirmation de votre rendez-vous";
                string body = $@"
                <p>Bonjour {patientName},</p>
                <p>Votre rendez-vous du <strong>{formattedDate}</strong> a bien été confirmé.</p>
                <p>À bientôt !</p>
    ";

                // --- 4) Envoi de l’e‑mail avec feedback utilisateur ---
                try
                {
                    emailService.SendEmail(patientEmail, subject, body);
                    MessageBox.Show(
                        $"Email envoyé à : {patientEmail}",
                        "Succès",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Erreur envoi email : {ex.Message}",
                        "Erreur SMTP",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                // --- 5) Ajout dans le grid Confirmés ---
                int idx = dataGridViewConfirmes.Rows.Add();
                var confRow = dataGridViewConfirmes.Rows[idx];
                confRow.Cells["DateRdv"].Value = formattedDate;
                confRow.Cells["NomPatient"].Value = patientName;
                confRow.Cells["NomMedecin"].Value = doctorName;

                // --- 6) Suppression de la ligne en attente ---
                dataGridViewDemandes.Rows.RemoveAt(e.RowIndex);
            }
            else if (columnName == "Refuser")
            {
                // Refus : même logique qu'avant (confirmation, libération plage, suppression RDV)
                var result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir refuser ce rendez-vous ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result != DialogResult.Yes) return;

                // Récupérer l'IdPlage
                string selectQuery = "SELECT [IdPlage] FROM [RendezVous] WHERE [IdRendezVous] = ?";
                int idPlage;
                connection.Open();
                using (var selectCmd = new OleDbCommand(selectQuery, connection))
                {
                    selectCmd.Parameters.AddWithValue("?", idRendezVous);
                    idPlage = Convert.ToInt32(selectCmd.ExecuteScalar());
                }
                connection.Close();

                // Libérer la plage
                string updatePlageQuery = "UPDATE [PlageHoraire] SET [Prise] = FALSE WHERE [IdPlage] = ?";
                connection.Open();
                using (var cmdPlage = new OleDbCommand(updatePlageQuery, connection))
                {
                    cmdPlage.Parameters.AddWithValue("?", idPlage);
                    cmdPlage.ExecuteNonQuery();
                }
                connection.Close();

                // Supprimer le rendez-vous
                string deleteQuery = "DELETE FROM [RendezVous] WHERE [IdRendezVous] = ?";
                connection.Open();
                using (var cmdDelete = new OleDbCommand(deleteQuery, connection))
                {
                    cmdDelete.Parameters.AddWithValue("?", idRendezVous);
                    cmdDelete.ExecuteNonQuery();
                }
                connection.Close();

                // Retirer la ligne du grid
                dataGridViewDemandes.Rows.RemoveAt(e.RowIndex);
            }
        }
        /****charger la liste des rdv confirmes*****/
        private void LoadConfirmedAppointments()
        {
            dataGridViewConfirmes.Rows.Clear();

            string query = @"
    SELECT 
        r.IdRendezVous, 
        p.Jour, 
        p.HeureDebut, 
        up.Nom AS PatientNom, 
        up.Prenom AS PatientPrenom, 
        ud.Nom AS DoctorNom, 
        ud.Prenom AS DoctorPrenom
    FROM 
        (((RendezVous AS r
        INNER JOIN PlageHoraire AS p ON r.IdPlage = p.IdPlage)
        INNER JOIN Patient AS pat ON r.IdPatient = pat.IdPatient)
        INNER JOIN Utilisateur AS up ON pat.IdUtilisateur = up.IdUtilisateur)
        INNER JOIN (Medecin AS m
        INNER JOIN Utilisateur AS ud ON m.IdUtilisateur = ud.IdUtilisateur)
    ON r.IdMedecin = m.IdMedecin
    WHERE r.Statut = TRUE AND p.SemaineDebut = ?";

            connection.Open();
            var cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", semaine_label.Text);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string jour = reader["Jour"].ToString();
                    DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                    string patientName = $"{reader["PatientPrenom"]} {reader["PatientNom"]}";
                    string doctorName = $"Dr. {reader["DoctorPrenom"]} {reader["DoctorNom"]}";
                    string formattedDateTime = $"{jour} {heureDebut:HH:mm}";

                    int rowIndex = dataGridViewConfirmes.Rows.Add();
                    var row = dataGridViewConfirmes.Rows[rowIndex];
                    row.Cells["DateRdv"].Value = formattedDateTime;
                    row.Cells["NomPatient"].Value = patientName;
                    row.Cells["NomMedecin"].Value = doctorName;
                }
            }
            connection.Close();
        }
        // Charger les dossiers administartifs des patients 
        private void LoadAllDossiers()
        {
            dataGridViewDossiers.Rows.Clear();

            string sql = @"
            SELECT 
                u.CIN, 
                u.Nom, 
                u.Prenom, 
                u.Email, 
                p.DateNaissance, 
                p.Assurance
            FROM 
                Patient AS p
                INNER JOIN Utilisateur AS u ON p.IdUtilisateur = u.IdUtilisateur";

            using (var cmd = new OleDbCommand(sql, connection))
            {
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataGridViewDossiers.Rows.Add(
                            reader["CIN"].ToString(),
                            reader["Nom"].ToString(),
                            reader["Prenom"].ToString(),
                            reader["Email"].ToString(),
                            Convert.ToDateTime(reader["DateNaissance"]).ToShortDateString(),
                            reader["Assurance"].ToString()
                        );
                    }
                }
            }
            connection.Close();
        }
        private void SearchDossiers(string term)
        {
            //  Normalisation du terme : suppression des espaces superflus et gestion du null
            term = term?.Trim() ?? "";

            //  Pour chaque ligne du DataGridView
            foreach (DataGridViewRow row in dataGridViewDossiers.Rows)
            {
                //  Récupération sécurisée des valeurs des cellules
                string cin = row.Cells["CIN"].Value?.ToString() ?? "";
                string nom = row.Cells["Nom"].Value?.ToString() ?? "";
                string prenom = row.Cells["Prenom"].Value?.ToString() ?? "";

                //  Vérification si l'une des colonnes contient le terme (insensible à la casse)
                bool match =
                    cin.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    nom.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    prenom.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

                // Affichage ou masquage de la ligne selon le résultat
                row.Visible = match;
            }
        }
        //  charger les informations de paiements (facture cree lors d'enregistrement d'une consultation
        private void LoadPaiementInfo()
        {
            try
            {
                // Charger les factures non payées (StatutPaiement = false) avec NomPatient et RdvID
                const string sqlLoadFactures = @"
            SELECT 
                f.IdFacture,
                r.IdRendezVous,
                u.Nom + ' ' + u.Prenom AS NomPatient,
                f.Montant,
                f.DatePaiement,
                f.StatutPaiement
            FROM ((((Facture AS f
            INNER JOIN Consultation AS c ON f.IdConsultation = c.IdConsultation)
            INNER JOIN RendezVous AS r ON c.IdRendezVous = r.IdRendezVous)
            INNER JOIN DossierMedical AS dm ON c.IdDossier = dm.IdDossier)
            INNER JOIN Patient AS p ON dm.IdPatient = p.IdPatient)
            INNER JOIN Utilisateur AS u ON p.IdUtilisateur = u.IdUtilisateur
            WHERE f.StatutPaiement = false";
                using (var cmd = new OleDbCommand(sqlLoadFactures, connection))
                {
                    Console.WriteLine("Executing sqlLoadFactures to load unpaid factures with NomPatient.");
                    connection.Open();
                    Console.WriteLine("Connection opened for sqlLoadFactures.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool statut = Convert.ToBoolean(reader["StatutPaiement"]);
                            int rowIndex = dataGridViewPaiements.Rows.Add(
                                reader["IdRendezVous"].ToString(),
                                reader["NomPatient"].ToString(),
                                reader["Montant"].ToString(),
                                Convert.ToDateTime(reader["DatePaiement"]).ToShortDateString()
                            );
                            // Stocker IdFacture dans le Tag
                            dataGridViewPaiements.Rows[rowIndex].Tag = new { IdFacture = Convert.ToInt32(reader["IdFacture"]) };
                            Console.WriteLine($"Row added to dataGridViewPaiements: IdFacture={reader["IdFacture"]}, RdvID={reader["IdRendezVous"]}, NomPatient={reader["NomPatient"]}, Montant={reader["Montant"]}, StatutPaiement={statut}, RowIndex={rowIndex}");
                        }
                    }
                    connection.Close();
                    Console.WriteLine("Connection closed after sqlLoadFactures.");
                }

                dataGridViewPaiements.Refresh();
                Console.WriteLine($"DataGridViewPaiements population complete. Total rows: {dataGridViewPaiements.Rows.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadPaiementInfo: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed after error.");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Rechercher Patient")
            {
                SearchDossiers(textBox_search.Text);
                btnSearch.Text = "Rafraîchir";
            }
            else
            {
                LoadAllDossiers();
                textBox_search.Clear();
                btnSearch.Text = "Rechercher Patient";
            }
        }
        private void profile_pic_Click(object sender, EventArgs e)
        {
            ProfileSecretary profile = new ProfileSecretary(_idUtilisateur);
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
                MenuForm menu = new MenuForm();
                menu.Show();


                this.Close();
            }
        }

        private void dataGridViewPaiements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dataGridViewPaiements.Rows[e.RowIndex];
            var rowData = row.Tag as dynamic;
            if (rowData == null || rowData.IdFacture == null) return;

            int idFacture = rowData.IdFacture;

            try
            {
                // Bouton "Valider"
                if (dataGridViewPaiements.Columns[e.ColumnIndex].Name == "colValider")
                {
                    const string sqlValidate = @"
                UPDATE Facture 
                SET StatutPaiement = ?, DatePaiement = ?
                WHERE IdFacture = ?";
                    using (var cmd = new OleDbCommand(sqlValidate, connection))
                    {
                        cmd.Parameters.Add("?", OleDbType.Boolean).Value = true; // StatutPaiement = true
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now; // DatePaiement
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = idFacture; // IdFacture
                        Console.WriteLine($"Executing sqlValidate for IdFacture: {idFacture}");
                        connection.Open();
                        Console.WriteLine("Connection opened for sqlValidate.");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected by validation: {rowsAffected}");
                        connection.Close();
                        Console.WriteLine("Connection closed after sqlValidate.");

                        if (rowsAffected > 0)
                        {
                            // Mettre à jour la ligne dans le DataGridView
                            row.Cells["DatePaiement"].Value = DateTime.Now.ToShortDateString();
                            row.DefaultCellStyle.BackColor = Color.LightGreen; // Couleur verte
                                                                               // Retirer la ligne car StatutPaiement est maintenant true
                           // dataGridViewPaiements.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Paiement validé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                // Bouton "Générer Facture"
                else if (dataGridViewPaiements.Columns[e.ColumnIndex].Name == "colGenererFacture")
                {
                    // Debug: Log column names to verify
                    string columnNames = string.Join(", ", dataGridViewPaiements.Columns.Cast<DataGridViewColumn>().Select(c => c.Name));
                    Console.WriteLine($"DataGridView columns: {columnNames}");

                    string baseDir = Application.StartupPath;                          // ou Environment.CurrentDirectory
                    string facturesDir = Path.Combine(baseDir, "Factures");
                    if (!Directory.Exists(facturesDir))
                        Directory.CreateDirectory(facturesDir);

                    // 2) Construire le chemin complet du PDF dans ce dossier
                    string pdfPath = Path.Combine(facturesDir, $"Facture_{idFacture}.pdf");
                    Document doc = new Document(PageSize.A4);
                    try
                    {
                        PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
                        doc.Open();

                        // Titre
                        iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                        Paragraph title = new Paragraph("Facture", titleFont);
                        title.Alignment = Element.ALIGN_CENTER;
                        doc.Add(title);
                        doc.Add(new Paragraph("\n"));

                        // Tableau avec les détails
                        PdfPTable table = new PdfPTable(2);
                        table.WidthPercentage = 80;
                        table.SetWidths(new float[] { 1, 2 });

                        iTextSharp.text.Font cellFont = FontFactory.GetFont("Arial", 12);
                        table.AddCell(new PdfPCell(new Phrase("ID Facture", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(idFacture.ToString(), cellFont))); // Use rowData.IdFacture
                        table.AddCell(new PdfPCell(new Phrase("ID Rendez-vous", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(row.Cells["RdvID"].Value?.ToString() ?? "N/A", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase("Patient", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(row.Cells["NomPatient"].Value?.ToString() ?? "N/A", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase("Montant (DT)", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(row.Cells["Montant"].Value?.ToString() ?? "N/A", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase("Date de Paiement", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(row.Cells["DatePaiement"].Value?.ToString() ?? "N/A", cellFont)));

                        doc.Add(table);
                        doc.Close();

                        // Ouvrir le PDF
                        Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                        Console.WriteLine($"PDF generated: {pdfPath}");
                        MessageBox.Show($"Facture générée : {pdfPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating PDF: {ex.Message}");
                        MessageBox.Show($"Erreur lors de la génération de la facture : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CellContentClick: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed after error.");
                }
            }
        }
    }
}
