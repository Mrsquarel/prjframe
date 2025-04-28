using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prjframe
{
    public partial class Patient_infos : Form
    {
        private OleDbConnection connection;
        private int _idUtilisateur;
        private int _idDocteur;
        private int dossierId = 0;
        private int _idrdv = 0;
        private int? newHistoryRow = null;
        private int? newDocRow = null;

        public Patient_infos(int IdUtilisateur, int idDocteur , int idrdv)
        {
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
            _idUtilisateur = IdUtilisateur;
            InitializeComponent();
            _idDocteur = idDocteur;
            _idrdv = idrdv;
        }

        private void Patient_infos_Load(object sender, EventArgs e)
        {

            ///**************charger les donnees du patient************///
            OpenDossierMedical();
            dossierId = getIdDossier(_idUtilisateur);
           
            // datagrid view historique_consultation
            dataGridViewHistorique.Columns.Clear();
            dataGridViewHistorique.Rows.Clear();
            dataGridViewHistorique.Columns.Add("DateConsultation", "Date de Consultation");
            dataGridViewHistorique.Columns.Add("Notes", "Notes");

            dataGridViewHistorique.AllowUserToAddRows = false;
            dataGridViewHistorique.EditMode = DataGridViewEditMode.EditOnEnter; // Permettre l'édition manuelle
            dataGridViewHistorique.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Sélectionner toute la ligne
            dataGridViewHistorique.Columns["DateConsultation"].Width = 150; // Largeur fixe
            dataGridViewHistorique.Columns["Notes"].Width = 419;
            dataGridViewHistorique.Columns["DateConsultation"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewHistorique.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewHistorique.Columns["Notes"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewHistorique.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewHistorique.RowHeadersVisible = false;

            // datagrid view documents
            dataGridViewDocuments.Columns.Clear();
            dataGridViewDocuments.Rows.Clear();
            dataGridViewDocuments.Columns.Add("NomFichier", "Nom du document");
            dataGridViewDocuments.Columns.Add("CheminFichier", "Chemin du fichier");

            var btnVoir = new DataGridViewButtonColumn
            {
                Name = "Voir",
                HeaderText = "Action",
                Text = "Voir",
                UseColumnTextForButtonValue = true
            };
            dataGridViewDocuments.Columns.Add(btnVoir);

            dataGridViewDocuments.AllowUserToAddRows = false;
            dataGridViewDocuments.EditMode = DataGridViewEditMode.EditOnEnter; // Permettre l'édition manuelle
            dataGridViewDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Sélectionner toute la ligne
            dataGridViewDocuments.Columns["NomFichier"].Width = 150; // Largeur fixe
            dataGridViewDocuments.Columns["CheminFichier"].Width = 441; 
            dataGridViewDocuments.Columns["Voir"].Width = 50;
            dataGridViewDocuments.Columns["NomFichier"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDocuments.Columns["CheminFichier"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDocuments.Columns["Voir"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewDocuments.RowHeadersVisible = false;

            LoadDocuments();
            LoadHistory();

        }
        private void OpenDossierMedical()
        {
            // 1) Requête des infos patient + dossier
            const string patientSql = @"
            SELECT 
                dm.DossierCreationDate,
                u.Nom,
                u.Prenom,
                u.Telephone,    
                p.Sexe,
                p.DateNaissance,
                p.Assurance      
            FROM 
                (DossierMedical AS dm
                 INNER JOIN Patient     AS p  ON dm.IdPatient      = p.IdPatient)
                INNER JOIN Utilisateur  AS u  ON p.IdUtilisateur    = u.IdUtilisateur
            WHERE 
                dm.IdPatient = ?";

            using (var cmd = new OleDbCommand(patientSql, connection))
            {
                cmd.Parameters.AddWithValue("?", _idUtilisateur);
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtNom.Text = reader["Nom"].ToString();
                        txtPrenom.Text = reader["Prenom"].ToString();
                        txtTelephone.Text = reader["Telephone"].ToString();
                        txtSexe.Text = reader["Sexe"].ToString();
                        txtDateNaisance.Text = Convert.ToDateTime(reader["DateNaissance"])
                                                .ToShortDateString();
                        txtAssurance.Text = reader["Assurance"].ToString();
                    }
                    else
                    {
                        connection.Close();
                        this.Close();
                        return;
                    }
                }
                connection.Close();
            }
        }

        private int getIdDossier(int _idUtilisateur)
        {
            //requete pour recuperer l'id du dossier medical

            const string sqlGetDossier = @"
            SELECT dm.IdDossier
            FROM DossierMedical AS dm
            INNER JOIN Patient AS p 
              ON dm.IdPatient = p.IdPatient
            WHERE p.IdPatient = ?";
            using (var cmd1 = new OleDbCommand(sqlGetDossier, connection))
            {
                cmd1.Parameters.AddWithValue("?", _idUtilisateur);
                connection.Open();
                using (var reader = cmd1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dossierId = Convert.ToInt32(reader["IdDossier"]);
                    }
                }
                connection.Close();
                return dossierId;
            }

        }
        private void LoadHistory()
        {
            dataGridViewHistorique.Rows.Clear();
            if (dossierId == 0) return;

            const string sql = @"
            SELECT IdConsultation, ConsultationDate, Notes
            FROM Consultation
            WHERE IdDossier = ?
            ORDER BY ConsultationDate";
            using (var cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", dossierId);
                connection.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var dt = Convert.ToDateTime(r["ConsultationDate"]).ToString("g");
                        var notes = r["Notes"].ToString();
                        int idx = dataGridViewHistorique.Rows.Add(dt, notes);
                        // Stocke l'ID pour suppression si besoin
                        dataGridViewHistorique.Rows[idx].Tag = Convert.ToInt32(r["IdConsultation"]);
                    }
                }
                connection.Close();
            }
        }

        private void LoadDocuments()
        {
            if (dossierId == 0) return;

            const string sql = @"
            SELECT IdDocument, NomFichier, CheminFichier
            FROM Document
            WHERE IdDossier = ?";
            using (var cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", dossierId);
                connection.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var name = r["NomFichier"].ToString();
                        var path = r["CheminFichier"].ToString();
                        int idx = dataGridViewDocuments.Rows.Add(name, path, "Voir");
                        dataGridViewDocuments.Rows[idx].Tag = Convert.ToInt32(r["IdDocument"]);
                    }
                }
                connection.Close();
            }
        }

        private void profile_pic_Click(object sender, EventArgs e)
        {
            DoctorDashboard dashboard = new DoctorDashboard(_idDocteur);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (btnAddHistory.Text == "Ajouter")
            {
                // Supprime l'ancienne ligne non-sauvée si elle existe
                if (newHistoryRow.HasValue)
                    dataGridViewHistorique.Rows.RemoveAt(newHistoryRow.Value);

                // Ajoute une ligne blanche
                newHistoryRow = dataGridViewHistorique.Rows.Add();
                var row = dataGridViewHistorique.Rows[newHistoryRow.Value];
                row.DefaultCellStyle.BackColor = Color.LightYellow; // Indiquer que c'est une nouvelle ligne

                // Pas besoin de BeginEdit avec EditOnEnter
                btnAddHistory.Text = "Enregistrer";
            }
            else
            {
                // Validation
                if (!newHistoryRow.HasValue || newHistoryRow.Value >= dataGridViewHistorique.Rows.Count)
                {
                    MessageBox.Show("Rien à enregistrer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnAddHistory.Text = "Ajouter";
                    newHistoryRow = null;
                    return;
                }

                var row = dataGridViewHistorique.Rows[newHistoryRow.Value];
                string sDate = row.Cells["DateConsultation"].Value?.ToString();
                string notes = row.Cells["Notes"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(sDate) || string.IsNullOrWhiteSpace(notes) || !DateTime.TryParse(sDate, out DateTime dt))
                {
                    MessageBox.Show("Tous les champs sont obligatoires et la date doit être valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridViewHistorique.Rows.RemoveAt(newHistoryRow.Value);
                    newHistoryRow = null;
                    btnAddHistory.Text = "Ajouter";
                    return;
                }

                // INSERT en base
                const string insert = @"
                INSERT INTO Consultation (IdRendezVous,IdDossier, ConsultationDate, Notes,IdPatient)
                VALUES (?, ?, ?,?,?)";
                using (var cmd = new OleDbCommand(insert, connection))
                {
                    cmd.Parameters.AddWithValue("?", _idrdv);
                    cmd.Parameters.AddWithValue("?", dossierId);
                    cmd.Parameters.AddWithValue("?", dt);
                    cmd.Parameters.AddWithValue("?", notes);
                    cmd.Parameters.AddWithValue("?", _idUtilisateur);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                // Reset
                newHistoryRow = null;
                btnAddHistory.Text = "Ajouter";
                LoadHistory();
            }
        }

        private void btnDeleteHistory_Click(object sender, EventArgs e)
        {

            if (dataGridViewHistorique.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une consultation à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridViewHistorique.SelectedRows[0];
            if (row.Tag is int idCons)
            {
                // Ligne existante dans la base de données
                if (MessageBox.Show("Supprimer cette consultation ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                const string del = "DELETE FROM Consultation WHERE IdConsultation = ?";
                using (var cmd = new OleDbCommand(del, connection))
                {
                    cmd.Parameters.AddWithValue("?", idCons);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                LoadHistory();
            }
            else if (newHistoryRow.HasValue && row.Index == newHistoryRow.Value)
            {
                // Ligne nouvellement ajoutée (non sauvegardée)
                dataGridViewHistorique.Rows.RemoveAt(newHistoryRow.Value);
                newHistoryRow = null;
                btnAddHistory.Text = "Ajouter";
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cette ligne.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddDocument_Click(object sender, EventArgs e)
        {
            if (btnAddDocument.Text == "Ajouter")
            {
                if (newDocRow.HasValue)
                    dataGridViewDocuments.Rows.RemoveAt(newDocRow.Value);

                newDocRow = dataGridViewDocuments.Rows.Add();
                var row = dataGridViewDocuments.Rows[newDocRow.Value];
                row.DefaultCellStyle.BackColor = Color.LightYellow; // Indiquer que c'est une nouvelle ligne

                // Pas besoin de BeginEdit avec EditOnEnter
                btnAddDocument.Text = "Enregistrer";
            }
            else
            {
                if (!newDocRow.HasValue || newDocRow.Value >= dataGridViewDocuments.Rows.Count)
                {
                    MessageBox.Show("Rien à enregistrer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnAddDocument.Text = "Ajouter";
                    newDocRow = null;
                    return;
                }

                var row = dataGridViewDocuments.Rows[newDocRow.Value];
                string name = row.Cells["NomFichier"].Value?.ToString();
                string path = row.Cells["CheminFichier"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show("Tous les champs sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridViewDocuments.Rows.RemoveAt(newDocRow.Value);
                    newDocRow = null;
                    btnAddDocument.Text = "Ajouter";
                    return;
                }

                // Valider que le fichier existe
                if (!File.Exists(path))
                {
                    MessageBox.Show("Le chemin du fichier est invalide ou le fichier n'existe pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                const string insert = @"
                INSERT INTO Document (IdDossier, NomFichier, CheminFichier)
                VALUES (?, ?, ?)";
                using (var cmd = new OleDbCommand(insert, connection))
                {
                    cmd.Parameters.AddWithValue("?", dossierId);
                    cmd.Parameters.AddWithValue("?", name);
                    cmd.Parameters.AddWithValue("?", path);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                newDocRow = null;
                btnAddDocument.Text = "Ajouter";
                LoadDocuments();
            }
        }

        private void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            if (dataGridViewDocuments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un document à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridViewDocuments.SelectedRows[0];
            if (row.Tag is int idDoc)
            {
                // Ligne existante dans la base de données
                if (MessageBox.Show("Supprimer ce document ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                const string del = "DELETE FROM Document WHERE IdDocument = ?";
                using (var cmd = new OleDbCommand(del, connection))
                {
                    cmd.Parameters.AddWithValue("?", idDoc);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                LoadDocuments();
            }
            else if (newDocRow.HasValue && row.Index == newDocRow.Value)
            {
                // Ligne nouvellement ajoutée (non sauvegardée)
                dataGridViewDocuments.Rows.RemoveAt(newDocRow.Value);
                newDocRow = null;
                btnAddDocument.Text = "Ajouter";
            }
            else
            {
                MessageBox.Show("Impossible de supprimer cette ligne.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDocuments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewDocuments.Columns[e.ColumnIndex].Name != "Voir")
                return;

            var tag = dataGridViewDocuments.Rows[e.RowIndex].Tag as int?;
            string path;

            path = dataGridViewDocuments.Rows[e.RowIndex].Cells["CheminFichier"].Value.ToString();

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show("Fichier introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
