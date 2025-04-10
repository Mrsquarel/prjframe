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

            // Add normal text columns
            dataGridViewHistorique.Columns.Add("colDocteur", "Nom du Docteur");
            dataGridViewHistorique.Columns.Add("colConsultation", "Nom de Consultation");
            dataGridViewHistorique.Columns.Add("colNotes", "Notes");

            // Add Dossier button column
            DataGridViewButtonColumn dossierBtn = new DataGridViewButtonColumn();
            dossierBtn.Name = "colDossier";
            dossierBtn.HeaderText = "Dossier";
            dossierBtn.Text = "Voir";
            dossierBtn.UseColumnTextForButtonValue = true;
            dataGridViewHistorique.Columns.Add(dossierBtn);

            // Add Prescription button column
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



            //prendre rdv
            LoadSpecialites();
            LoadCurrentWeekDates();

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
        private void LoadCurrentWeekDates()
        {
            DateTime today = DateTime.Today;
            int daysToMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysToMonday < 0) daysToMonday += 7;

            DateTime monday = today.AddDays(-daysToMonday);

            for (int i = 0; i < 6; i++) 
            {
                comboBoxDate.Items.Add(monday.AddDays(i).ToString("dd/MM/yyyy"));
            }
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
                // Open MenuForm
                MenuForm menu = new MenuForm();
                menu.Show();

                // Close current dashboard
                this.Close();
            }
        }

        private void comboBoxSpecialite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSpecialite.SelectedItem == null) return;

            string selectedSpecialty = comboBoxSpecialite.SelectedItem.ToString();

            string query = "SELECT m.IdMedecin, u.Nom, u.Prenom " +
                           "FROM Medecin m " +
                           "INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur " +
                           "WHERE m.Specialite = ? " +
                           "ORDER BY u.Nom, u.Prenom";

            comboBoxDoctors.Items.Clear();

            connection.Open();
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", selectedSpecialty);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string doctorName = $"{reader["Nom"]} {reader["Prenom"]}";
                int doctorId = Convert.ToInt32(reader["IdMedecin"]);
                comboBoxDoctors.Items.Add(new KeyValuePair<int, string>(doctorId, doctorName));
            }

            reader.Close();
            connection.Close();

            // Configure combobox to display doctor names
            comboBoxDoctors.DisplayMember = "Value";
            comboBoxDoctors.ValueMember = "Key";

            if (comboBoxDoctors.Items.Count == 0)
            {
                MessageBox.Show("Aucun médecin trouvé dans cette spécialité.");
            }
        }
    }
}
