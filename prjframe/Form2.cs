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

namespace prjframe
{
    public partial class adminpanel : Form
    {
        private int _idUtilisateur;
        private OleDbConnection connection;

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
            dataGridViewUsers.ColumnCount = 6;
            dataGridViewUsers.Columns[0].Name = "CIN";
            dataGridViewUsers.Columns[1].Name = "Nom";
            dataGridViewUsers.Columns[2].Name = "Prénom";
            dataGridViewUsers.Columns[3].Name = "Email";
            dataGridViewUsers.Columns[4].Name = "Téléphone";
            dataGridViewUsers.Columns[5].Name = "Rôle";

            dataGridViewUsers.RowHeadersVisible = false;


            // Example data
            string[] row1 = { "12345678", "Dupont", "Alice", "alice@example.com", "20234567", "Admin" };
            string[] row2 = { "11223344", "Ben Ali", "Karim", "karim@example.com", "98765432", "Médecin" };
            dataGridViewUsers.Rows.Add(row1);
            dataGridViewUsers.Rows.Add(row2);


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


            // Example rows
            string[] row5 = { "Semaine 1", "Dr. Martin", "Fatma A.", "08/04/2025" };
            string[] row6 = { "Semaine 1", "Dr. Karim", "Sofien B.", "10/04/2025" };
            string[] row7 = { "Semaine 2", "Dr. Martin", "Anis C.", "15/04/2025" };

            dataGridViewConsultations.Rows.Add(row5);
            dataGridViewConsultations.Rows.Add(row6);
            dataGridViewConsultations.Rows.Add(row7);

            dataGridViewConsultations.RowHeadersVisible = false;


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
                          "WHERE IdMedecin = ?";

            connection.Open();

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("?", doctorId);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int rowIndex = dataGridViewDisponibilites.Rows.Add();


                        DateTime heureDebut = Convert.ToDateTime(reader["HeureDebut"]);
                        DateTime heureFin = Convert.ToDateTime(reader["HeureFin"]);

                        dataGridViewDisponibilites.Rows[rowIndex].Cells["IdPlage"].Value = reader["IdPlage"];
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["Jour"].Value = reader["Jour"];
                        dataGridViewDisponibilites.Rows[rowIndex].Cells["HeureDebut"].Value = heureDebut.ToString("HH:mm");//enlever la date 
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

      
    }
}

