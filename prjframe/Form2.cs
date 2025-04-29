using System;
using System.Collections;
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
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Excel = Microsoft.Office.Interop.Excel;

namespace prjframe
{
    public partial class adminpanel : Form
    {
        private int _idUtilisateur;
        private OleDbConnection connection;
        private int? newRowIndex = null;

        public adminpanel(int IdUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = IdUtilisateur;
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }

        private void adminpanel_Load(object sender, EventArgs e)
        {   //charger date d'aujourd'hui
            date_label.Text = DateTime.Today.ToLongDateString();
            //afficher nom admin 
            string query = @"SELECT Nom FROM Utilisateur WHERE IdUtilisateur = ?";

            connection.Open();


            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("?", _idUtilisateur);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        admin_name.Text = nom;


                    }
                }
            }
            connection.Close();
            // data grid view for users

            dataGridViewUsers.ColumnCount = 7;
            dataGridViewUsers.Columns[0].Name = "IdUTILISATEUR";
            dataGridViewUsers.Columns[1].Name = "Nom";
            dataGridViewUsers.Columns[2].Name = "Prénom";
            dataGridViewUsers.Columns[3].Name = "Email";
            dataGridViewUsers.Columns[4].Name = "Rôle";
            dataGridViewUsers.Columns[5].Name = "Téléphone";
            dataGridViewUsers.Columns[6].Name = "CIN";

            dataGridViewUsers.RowHeadersVisible = false;
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.MultiSelect = false;
            dataGridViewUsers.AllowUserToAddRows = false;
            dataGridViewUsers.Columns["IdUTILISATEUR"].ReadOnly = true;

            //charger les utilisateurs
            LoadUsers();

            //data grid view for disponibilites 
            dataGridViewDisponibilites.Columns.Clear();
            dataGridViewDisponibilites.Rows.Clear();
            dataGridViewDisponibilites.Columns.Add("IdPlage", "ID");
            dataGridViewDisponibilites.Columns["IdPlage"].Visible = false;
            dataGridViewDisponibilites.Columns.Add("SemaineDebut", "Semaine");
            dataGridViewDisponibilites.Columns.Add("Jour", "Jour");
            dataGridViewDisponibilites.Columns.Add("HeureDebut", "Heure Début");
            dataGridViewDisponibilites.Columns.Add("HeureFin", "Heure Fin");

            dataGridViewDisponibilites.Columns["SemaineDebut"].Width = 250;
            dataGridViewDisponibilites.Columns["Jour"].Width = 120;
            dataGridViewDisponibilites.Columns["HeureDebut"].Width = 100;
            dataGridViewDisponibilites.Columns["HeureFin"].Width = 100;


            DataGridViewButtonColumn validerBtn = new DataGridViewButtonColumn();
            validerBtn.Name = "Valider";
            validerBtn.HeaderText = "Action";
            validerBtn.Text = "Valider";
            validerBtn.UseColumnTextForButtonValue = true;
            dataGridViewDisponibilites.Columns.Add(validerBtn);

            DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
            deleteBtn.Name = "Supprimer";
            deleteBtn.HeaderText = "";
            deleteBtn.Text = "Supprimer";
            deleteBtn.UseColumnTextForButtonValue = true;
            dataGridViewDisponibilites.Columns.Add(deleteBtn);

            dataGridViewDisponibilites.RowHeadersVisible = false;

            //charger le combobox des médecins
            LoadDoctorNames();


            //data grid view for consultations
            dataGridViewConsultations.ColumnCount = 4;
            dataGridViewConsultations.Columns[0].Name = "Semaine";
            dataGridViewConsultations.Columns[1].Name = "Médecin";
            dataGridViewConsultations.Columns[2].Name = "Patient";
            dataGridViewConsultations.Columns[3].Name = "Date";

            dataGridViewConsultations.Columns[0].Width = 150;
            dataGridViewConsultations.Columns[1].Width = 150;
            dataGridViewConsultations.Columns[2].Width = 150;
            dataGridViewConsultations.Columns[3].Width = 150;


            foreach (DataGridViewColumn col in dataGridViewConsultations.Columns)
            {
                col.ReadOnly = true;
            }
            dataGridViewConsultations.AllowUserToAddRows = false;
            dataGridViewConsultations.AllowUserToDeleteRows = false;
            dataGridViewConsultations.RowHeadersVisible = false;


            LoadSemaineDebutComboBox();


            // Configure the chart
            chartConsultations.Series.Clear();
            chartConsultations.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("MainArea");
            chartConsultations.ChartAreas.Add(chartArea);

            Series series = new Series("Consultations")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            chartConsultations.Series.Add(series);

            chartConsultations.ChartAreas[0].AxisX.Title = "Semaine";
            chartConsultations.ChartAreas[0].AxisY.Title = "Nombre de Consultations";
            chartConsultations.ChartAreas[0].AxisX.Interval = 1;
            chartConsultations.ChartAreas[0].AxisY.Minimum = 0;

           
            UpdateChart();

        }
        private void LoadUsers()
        {
            dataGridViewUsers.Rows.Clear();

            string query = "SELECT IdUtilisateur, Nom, Prenom, Email , Role, Telephone, CIN " +
                          "FROM Utilisateur";

            connection.Open();

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int rowIndex = dataGridViewUsers.Rows.Add();

                        dataGridViewUsers.Rows[rowIndex].Cells["IdUTILISATEUR"].Value = reader["IdUtilisateur"];
                        dataGridViewUsers.Rows[rowIndex].Cells["Nom"].Value = reader["Nom"];
                        dataGridViewUsers.Rows[rowIndex].Cells["Prénom"].Value = reader["Prenom"];
                        dataGridViewUsers.Rows[rowIndex].Cells["Email"].Value = reader["Email"];
                        dataGridViewUsers.Rows[rowIndex].Cells["Rôle"].Value = reader["Role"];
                        dataGridViewUsers.Rows[rowIndex].Cells["Téléphone"].Value = reader["Telephone"];
                        dataGridViewUsers.Rows[rowIndex].Cells["CIN"].Value = reader["CIN"];
                    }

                    if (dataGridViewUsers.Rows.Count == 0)
                    {
                        MessageBox.Show("Aucun utilisateur trouvé.",
                                        "Information",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }

            connection.Close();
        }
        private void LoadDoctorNames()
        {


            string query = "SELECT m.IdMedecin, u.Nom, u.Prenom " +
                           "FROM Medecin m " +
                           "INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur";

            OleDbCommand cmd = new OleDbCommand(query, connection);
            connection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();

            comboBoxDoctors.Items.Clear();

            while (reader.Read())
            {
                string doctorName = reader["Nom"].ToString() + " " + reader["Prenom"].ToString();
                int doctorId = Convert.ToInt32(reader["IdMedecin"]);

                //ajouter le médecin sous forme de cle/ valeur pour faciliter la manipulation des donnees au combobox
                comboBoxDoctors.Items.Add(new KeyValuePair<int, string>(doctorId, doctorName));
            }

            reader.Close();
            connection.Close();


            if (comboBoxDoctors.Items.Count == 0)
            {
                MessageBox.Show("Aucun médecin trouvé.");
            }


        }
        private void LoadSemaineDebutComboBox()
        {
            try
            {
                const string sqlLoadSemaineDebut = @"
                SELECT DISTINCT SemaineDebut
                FROM PlageHoraire
                ORDER BY SemaineDebut";

                using (var cmd = new OleDbCommand(sqlLoadSemaineDebut, connection))
                {
                    Console.WriteLine("Executing sqlLoadSemaineDebut to load SemaineDebut values.");
                    connection.Open();
                    Console.WriteLine("Connection opened for sqlLoadSemaineDebut.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        comboBoxSemaineDebut.Items.Clear();
                        comboBoxSemaineDebut.Items.Add("All"); 
                        while (reader.Read())
                        {
                            string semaineDebut = reader["SemaineDebut"].ToString();
                            comboBoxSemaineDebut.Items.Add(semaineDebut);
                            Console.WriteLine($"SemaineDebut added to comboBox: {semaineDebut}");
                        }
                    }
                    connection.Close();
                    Console.WriteLine("Connection closed after sqlLoadSemaineDebut.");
                }

                // selectionner all par defaut
                comboBoxSemaineDebut.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadSemaineDebutComboBox: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue lors du chargement des semaines : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed after error.");
                }
            }
        }
        private void LoadConsultations(string semaineDebutFilter)
        {
            var consults = new List<(int RdvId, DateTime Date, string SemaineDebut)>();
            string sqlRaw = @"
        SELECT 
            c.IdRendezVous, 
            c.ConsultationDate,
            ph.SemaineDebut
        FROM 
            (Consultation AS c
             INNER JOIN RendezVous   AS r  ON c.IdRendezVous = r.IdRendezVous)
            INNER JOIN PlageHoraire AS ph ON r.IdPlage       = ph.IdPlage";
            if (!string.IsNullOrEmpty(semaineDebutFilter) && semaineDebutFilter != "All")
                sqlRaw += " WHERE ph.SemaineDebut = ?";

            using (var cmd = new OleDbCommand(sqlRaw, connection))
            {
                if (sqlRaw.Contains("WHERE"))
                    cmd.Parameters.AddWithValue("?", semaineDebutFilter);

                connection.Open();
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        consults.Add((
                            RdvId: Convert.ToInt32(rd["IdRendezVous"]),
                            Date: Convert.ToDateTime(rd["ConsultationDate"]),
                            SemaineDebut: rd["SemaineDebut"].ToString()
                        ));
                connection.Close();
            }

            dataGridViewConsultations.Rows.Clear();
            if (consults.Count == 0) return;

            //  Récupérer les noms des médecins
            var rdvIds = consults.Select(c => c.RdvId).Distinct().ToList();
            string inRdv = "(" + string.Join(",", rdvIds) + ")";
            var medNames = new Dictionary<int, string>();
            string sqlMed = $@"
            SELECT 
                r.IdRendezVous,
                u.Nom & ' ' & u.Prenom AS FullName
            FROM (RendezVous AS r
                  INNER JOIN Medecin    AS m   ON r.IdMedecin    = m.IdMedecin)
                 INNER JOIN Utilisateur AS u   ON m.IdUtilisateur = u.IdUtilisateur
            WHERE r.IdRendezVous IN {inRdv}";
            using (var cmd = new OleDbCommand(sqlMed, connection))
            {
                connection.Open();
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        medNames[(int)rd["IdRendezVous"]] = rd["FullName"].ToString();
                connection.Close();
            }

            //  Récupérer les noms des patients
            var rdvToPat = new Dictionary<int, int>();
            string sqlRdvPat = $@"
            SELECT IdRendezVous, IdPatient
            FROM RendezVous
            WHERE IdRendezVous IN {inRdv}";
            using (var cmd = new OleDbCommand(sqlRdvPat, connection))
            {
                connection.Open();
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        rdvToPat[(int)rd["IdRendezVous"]] = Convert.ToInt32(rd["IdPatient"]);
                connection.Close();
            }

            var patIds = rdvToPat.Values.Distinct().ToList();
            string inPat = "(" + string.Join(",", patIds) + ")";
            var patNames = new Dictionary<int, string>();
            string sqlPat = $@"
            SELECT 
                p.IdPatient,
                u.Nom & ' ' & u.Prenom AS FullName
            FROM Patient      AS p
            INNER JOIN Utilisateur AS u ON p.IdUtilisateur = u.IdUtilisateur
            WHERE p.IdPatient IN {inPat}";
            using (var cmd = new OleDbCommand(sqlPat, connection))
            {
                connection.Open();
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        patNames[(int)rd["IdPatient"]] = rd["FullName"].ToString();
                connection.Close();
            }

            // 4 Construire les lignes du DataGridView, en utilisant SemaineDebut
            foreach (var (rdvId, date, semDebut) in consults)
            {
                string semaine = semDebut;  // <-- directement depuis la colonne SemaineDebut
                string medName = medNames.TryGetValue(rdvId, out var m) ? m : "—";
                int patId = rdvToPat.TryGetValue(rdvId, out var pid) ? pid : 0;
                string patName = patNames.TryGetValue(patId, out var p) ? p : "—";
                string formattedDate = date.ToString("dd/MM/yyyy");

                dataGridViewConsultations.Rows.Add(
                    semaine, medName, patName, formattedDate
                );
            }
        }




        private void profile_pic_Click(object sender, EventArgs e)
        {

            ProfileAdmin profile = new ProfileAdmin(_idUtilisateur);
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


        private void LoadDisponibilitesForDoctor(int doctorId)
        {
            dataGridViewDisponibilites.Rows.Clear();

            string query = "SELECT IdPlage,Jour, HeureDebut, HeureFin, SemaineDebut " +
                          "FROM PlageHoraire " +
                          "WHERE IdMedecin = ? AND EstValide=?";

            connection.Open();

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("?", doctorId);
                cmd.Parameters.AddWithValue("?", false);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int rowIndex = dataGridViewDisponibilites.Rows.Add();


                        DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                        DateTime heureFin = Convert.ToDateTime(reader["HeureFin"]);

                        dataGridViewDisponibilites.Rows[rowIndex].Cells["IdPlage"].Value = reader["IdPlage"];
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["Jour"].Value = reader["Jour"];
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["HeureDebut"].Value = heureDebut.ToString("HH:mm"); 
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["HeureFin"].Value = heureFin.ToString("HH:mm");
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["SemaineDebut"].Value = reader["SemaineDebut"];
                    }

                    if (dataGridViewDisponibilites.Rows.Count == 0)
                    {
                        MessageBox.Show("No availability data found for this doctor",
                                     "Information",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                    }
                }
            }

            connection.Close();
        }

        private void comboBoxDoctors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDoctors.SelectedItem == null)
            {
                dataGridViewDisponibilites.Rows.Clear();
                return;
            }

            KeyValuePair<int, string> selectedDoctor = (KeyValuePair<int, string>)comboBoxDoctors.SelectedItem;

            int doctorId = selectedDoctor.Key;
            LoadDisponibilitesForDoctor(doctorId);
        }

        private void dataGridViewDisponibilites_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {    //ignorer les clicks sur les cellules qui ne sont pas des boutons
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = (DataGridView)sender;
            int idPlage = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["IdPlage"].Value);
            //si admin click sur valider
            if (grid.Columns[e.ColumnIndex].Name == "Valider")
            {   //maj de  la colonne estvalide
                string updateQuery = "UPDATE PlageHoraire SET EstValide = true WHERE IdPlage = ?";

                connection.Open();
                using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("?", idPlage);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                //colorer la ligne de dispo validée
                grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                MessageBox.Show("Disponibilité validée!");
            }
            //si admin click sur supprimer
            else if (grid.Columns[e.ColumnIndex].Name == "Supprimer")
            {
                if (MessageBox.Show("Supprimer cette disponibilité?", "Confirmation",
                                  MessageBoxButtons.YesNo) == DialogResult.Yes)
                {//si oui supprimer la ligne 
                    string deleteQuery = "DELETE FROM PlageHoraire WHERE IdPlage = ?";

                    connection.Open();
                    using (OleDbCommand cmd = new OleDbCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("?", idPlage);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    //enlever la ligne du datagrid view
                    grid.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("Disponibilité supprimée!");
                }
            }
        }

        private void ajouter_btn_Click(object sender, EventArgs e)
        {
            if (ajouter_btn.Text == "Ajouter Utilisateur")
            {
                // Remove any previous unsaved row
                if (newRowIndex.HasValue && newRowIndex.Value < dataGridViewUsers.Rows.Count)
                {
                    dataGridViewUsers.Rows.RemoveAt(newRowIndex.Value);
                }

                // Add a new row to the DataGridView and allow the admin to edit it
                newRowIndex = dataGridViewUsers.Rows.Add();
                dataGridViewUsers.CurrentCell = dataGridViewUsers.Rows[newRowIndex.Value].Cells["Nom"]; // Set focus to the first editable cell
                dataGridViewUsers.BeginEdit(true); // Start editing mode
                ajouter_btn.Text = "Enregistrer"; // Change button text to "Save"
            }
            else // Button text is "Enregistrer"
            {
                if (!newRowIndex.HasValue || newRowIndex.Value >= dataGridViewUsers.Rows.Count)
                {
                    MessageBox.Show("No new row to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ajouter_btn.Text = "Ajouter Utilisateur"; // Reset button text
                    newRowIndex = null;
                    return;
                }

                // Validate the new row
                DataGridViewRow row = dataGridViewUsers.Rows[newRowIndex.Value];
                if (string.IsNullOrWhiteSpace(row.Cells["Nom"].Value?.ToString()) ||
                    string.IsNullOrWhiteSpace(row.Cells["Prénom"].Value?.ToString()) ||
                    string.IsNullOrWhiteSpace(row.Cells["Email"].Value?.ToString()) ||
                    string.IsNullOrWhiteSpace(row.Cells["Rôle"].Value?.ToString()) ||
                    string.IsNullOrWhiteSpace(row.Cells["Téléphone"].Value?.ToString()) ||
                    string.IsNullOrWhiteSpace(row.Cells["CIN"].Value?.ToString()))
                {
                    MessageBox.Show("All fields are required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridViewUsers.Rows.Remove(row); // Remove the incomplete row
                    newRowIndex = null;
                    ajouter_btn.Text = "Ajouter Utilisateur"; // Reset button text
                    return;
                }

                // Insert the new user into the database, setting MotDePasse to the Email value
                connection.Open();
                string query = "INSERT INTO Utilisateur (Nom, Prenom, Email, MotDePasse, Role, Telephone, CIN) " +
                               "VALUES (?, ?, ?, ?, ?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("?", row.Cells["Nom"].Value.ToString());
                    cmd.Parameters.AddWithValue("?", row.Cells["Prénom"].Value.ToString());
                    cmd.Parameters.AddWithValue("?", row.Cells["Email"].Value.ToString());
                    cmd.Parameters.AddWithValue("?", row.Cells["Email"].Value.ToString()); // MotDePasse (same as Email)
                    cmd.Parameters.AddWithValue("?", row.Cells["Rôle"].Value.ToString());
                    cmd.Parameters.AddWithValue("?", row.Cells["Téléphone"].Value.ToString());
                    cmd.Parameters.AddWithValue("?", row.Cells["CIN"].Value.ToString());
                    cmd.ExecuteNonQuery();
                }
                connection.Close();

                newRowIndex = null; // Clear the new row index
                ajouter_btn.Text = "Ajouter Utilisateur"; // Reset button text to "Ajouter Utilisateur"
                LoadUsers(); // Refresh DataGridView
            }
        }

        private void supprimer_btn_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Vous etes sure de supprimer cet utilisateur", "Confirmer Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int userId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["IdUTILISATEUR"].Value);
                    connection.Open();
                    string query = "DELETE FROM Utilisateur WHERE IdUtilisateur = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("?", userId);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();

                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Selectionnez un utlisateur à supprimer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxSemaineDebut_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSemaineDebut = comboBoxSemaineDebut.SelectedItem?.ToString();
            Console.WriteLine($"ComboBoxSemaineDebut selection changed: {selectedSemaineDebut}");
            LoadConsultations(selectedSemaineDebut);
        }


        private void UpdateChart()
        {
            try
            {
                string sql = @"
                SELECT 
                    m.Specialite,
                    COUNT(*) AS ConsultationCount
                FROM (Consultation AS c
                INNER JOIN RendezVous AS r ON c.IdRendezVous = r.IdRendezVous)
                INNER JOIN Medecin AS m ON r.IdMedecin = m.IdMedecin
                GROUP BY m.Specialite";

                var consultationsByDept = new Dictionary<string, int>();

                using (var cmd = new OleDbCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dept = reader["Specialite"].ToString();
                            int count = Convert.ToInt32(reader["ConsultationCount"]);
                            consultationsByDept[dept] = count;
                        }
                    }
                    connection.Close();
                }

                // Update the chart
                chartConsultations.Series["Consultations"].Points.Clear();
                foreach (var kvp in consultationsByDept.OrderBy(k => k.Key))
                {
                    chartConsultations.Series["Consultations"].Points.AddXY(kvp.Key, kvp.Value);
                }

                chartConsultations.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateChart: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue lors de la mise à jour du graphique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed after error.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a SaveFileDialog to let the user choose where to save the PDF
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Export Chart as PDF";
                    saveFileDialog.FileName = "ConsultationsByDepartment.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the chart as a temporary PNG file
                        string tempImagePath = Path.Combine(Path.GetTempPath(), "chart.png");
                        chartConsultations.SaveImage(tempImagePath, ChartImageFormat.Png);
                        Console.WriteLine($"Chart saved as image: {tempImagePath}");

                        // Create a new PDF document
                        using (Document document = new Document(PageSize.A4, 50, 50, 50, 50))
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                            document.Open();

                            // Add a title to the PDF using FontFactory
                            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 18f, iTextSharp.text.Font.BOLD);
                            Paragraph title = new Paragraph("Consultations by Department", titleFont);
                            title.Alignment = Element.ALIGN_CENTER;
                            document.Add(title);
                            document.Add(new Paragraph("\n"));

                            // Add the chart image to the PDF
                            iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(tempImagePath);
                            chartImage.ScaleToFit(document.PageSize.Width - 100, document.PageSize.Height - 100);
                            chartImage.Alignment = Element.ALIGN_CENTER;
                            document.Add(chartImage);

                            document.Close();
                            writer.Close();
                            Console.WriteLine($"PDF exported successfully: {saveFileDialog.FileName}");
                        }

                        // Clean up the temporary image file
                        if (File.Exists(tempImagePath))
                        {
                            File.Delete(tempImagePath);
                            Console.WriteLine($"Temporary image deleted: {tempImagePath}");
                        }

                        MessageBox.Show("Chart exported as PDF successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnExportPDF_Click: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue lors de l'exportation en PDF : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the DataGridView has data
                if (dataGridViewConsultations.Rows.Count == 0)
                {
                    MessageBox.Show("Aucune donnée à exporter.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Create a SaveFileDialog to let the user choose where to save the Excel file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveFileDialog.Title = "Export Consultations as Excel";
                    saveFileDialog.FileName = "Consultations.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create a new Excel application
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook workbook = excelApp.Workbooks.Add();
                        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                        worksheet.Name = "Consultations";

                        // Export column headers
                        for (int i = 0; i < dataGridViewConsultations.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dataGridViewConsultations.Columns[i].HeaderText;
                            worksheet.Cells[1, i + 1].Font.Bold = true;
                        }

                        // Export data rows
                        for (int i = 0; i < dataGridViewConsultations.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridViewConsultations.Columns.Count; j++)
                            {
                                var cellValue = dataGridViewConsultations.Rows[i].Cells[j].Value;
                                worksheet.Cells[i + 2, j + 1] = cellValue != null ? cellValue.ToString() : "";
                            }
                        }

                        // Auto-fit columns
                        worksheet.Columns.AutoFit();

                        // Save the Excel file
                        workbook.SaveAs(saveFileDialog.FileName);
                        workbook.Close();
                        excelApp.Quit();

                        // Release COM objects to avoid memory leaks
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                        Console.WriteLine($"Excel file exported successfully: {saveFileDialog.FileName}");
                        MessageBox.Show("Consultations exported as Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnExportExcel_Click: {ex.Message}");
                MessageBox.Show($"Une erreur est survenue lors de l'exportation en Excel : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
