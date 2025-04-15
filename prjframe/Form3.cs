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


           
            dataGridViewPaiements.Columns.Add("RdvID", "ID du Rendez-vous");
            dataGridViewPaiements.Columns.Add("NomPatient", "Nom du Patient");
            dataGridViewPaiements.Columns.Add("Montant", "Montant Payé (DT)");
            dataGridViewPaiements.Columns.Add("ModePaiement", "Mode de Paiement");
            dataGridViewPaiements.Columns.Add("DatePaiement", "Date de Paiement");
            dataGridViewPaiements.Columns.Add("Statut", "Statut");

            // Make columns editable
            foreach (DataGridViewColumn col in dataGridViewPaiements.Columns)
            {
                col.ReadOnly = false;
            }
            // Enable editing and flexible row management
            dataGridViewPaiements.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewPaiements.AllowUserToAddRows = true;
            dataGridViewPaiements.AllowUserToDeleteRows = true;

            // Optional: Hide the row headers for cleaner UI
            dataGridViewPaiements.RowHeadersVisible = false;


            dataGridViewDossiers.Columns.Add("CIN", "CIN");
            dataGridViewDossiers.Columns.Add("Nom", "Nom");
            dataGridViewDossiers.Columns.Add("Prenom", "Prénom");
            dataGridViewDossiers.Columns.Add("Adresse", "Adresse");
            dataGridViewDossiers.Columns.Add("Email", "Email");
            dataGridViewDossiers.Columns.Add("DateNaissance", "Date de Naissance");
            dataGridViewDossiers.Columns.Add("Assurance", "Assurance");

            dataGridViewDossiers.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewDossiers.AllowUserToAddRows = false;
            dataGridViewDossiers.RowHeadersVisible = false;

            dataGridViewDossiers.Rows.Add("12345678", "Sana", "Rekik", "Ariana, Rue El Fajr", "sana.rekik@mail.com", "1994-06-12", "CNAM");
            dataGridViewDossiers.Rows.Add("78901234", "Walid", "Mejri", "Tunis, Montplaisir", "walid.mejri@mail.com", "1990-09-23", "Privée");

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


            LoadPendingAppointments();
            LoadConfirmedAppointments();

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
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", semaine_label);
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idRendezVous = Convert.ToInt32(reader["IdRendezVous"]);
                string jour = reader["Jour"].ToString();
                DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                string patientName = $"{reader["PatientPrenom"]} {reader["PatientNom"]}";
                string doctorName = $"Dr. {reader["DoctorPrenom"]} {reader["DoctorNom"]}";
                string formattedDateTime = $"{jour} {heureDebut:HH:mm}";

                int rowIndex = dataGridViewDemandes.Rows.Add();
                DataGridViewRow row = dataGridViewDemandes.Rows[rowIndex];
                row.Cells["IdRendezVous"].Value = idRendezVous;
                row.Cells["DateRdv"].Value = formattedDateTime;
                row.Cells["NomPatient"].Value = patientName;
                row.Cells["NomMedecin"].Value = doctorName;
            }
            reader.Close();
            connection.Close();
        }
        /***confirmer ou refuser le rdv***/
        private void dataGridViewDemandes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

           
            string columnName = dataGridViewDemandes.Columns[e.ColumnIndex].Name;

           int idRendezVous = Convert.ToInt32(dataGridViewDemandes.Rows[e.RowIndex].Cells["IdRendezVous"].Value);

            if (columnName == "Confirmer")
            {
                string updateQuery = "UPDATE RendezVous SET Statut = TRUE WHERE IdRendezVous = ?";
                connection.Open();
                OleDbCommand cmd = new OleDbCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("?", idRendezVous);
                cmd.ExecuteNonQuery();
                connection.Close();

                dataGridViewDemandes.Rows.RemoveAt(e.RowIndex);
            }
            else if (columnName == "Refuser")
            {
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir refuser ce rendez-vous ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
                string selectQuery = "SELECT IdPlage FROM RendezVous WHERE IdRendezVous = ?";
                connection.Open();
                OleDbCommand selectCmd = new OleDbCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("?", idRendezVous);
                int idPlage = Convert.ToInt32(selectCmd.ExecuteScalar());
                connection.Close();
                // Update the PlageHoraire table to release the slot (set Prise = No, i.e. 0)
                string updatePlageQuery = "UPDATE PlageHoraire SET Prise = FALSE WHERE IdPlage = ?";
                connection.Open();
                OleDbCommand cmdPlage = new OleDbCommand(updatePlageQuery, connection);
                cmdPlage.Parameters.AddWithValue("?", idPlage);
                cmdPlage.ExecuteNonQuery();
                connection.Close();

                dataGridViewDemandes.Rows.RemoveAt(e.RowIndex);
                string updateRdvQuery = "DELETE RendezVous WHERE IdRendezVous = ?";
                connection.Open();
                OleDbCommand cmdRdv = new OleDbCommand(updateRdvQuery, connection);
                cmdRdv.Parameters.AddWithValue("?", idRendezVous);
                cmdRdv.ExecuteNonQuery();
                connection.Close();

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
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("?", semaine_label);
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string jour = reader["Jour"].ToString();
                DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                string patientName = $"{reader["PatientPrenom"]} {reader["PatientNom"]}";
                string doctorName = $"Dr. {reader["DoctorPrenom"]} {reader["DoctorNom"]}";
                string formattedDateTime = $"{jour} {heureDebut:HH:mm}";

                int rowIndex = dataGridViewConfirmes.Rows.Add();
                DataGridViewRow row = dataGridViewConfirmes.Rows[rowIndex];
                row.Cells["DateRdv"].Value = formattedDateTime;
                row.Cells["NomPatient"].Value = patientName;
                row.Cells["NomMedecin"].Value = doctorName;
            }
            reader.Close();
            connection.Close();
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

  
    }
}
