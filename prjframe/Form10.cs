using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace prjframe
{
    public partial class signin : Form
    {
        OleDbConnection connection;
        public signin()
        {
            InitializeComponent();

            //string dbPath = "C:/Users/eyabe/Downloads/BD Framework/DatabaseHealthApp.accdb";
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
            

        }

        private void comboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboRole.SelectedItem != null)
            {
                string selectedRole = comboRole.SelectedItem.ToString();



                // les champs selon le role
                if (selectedRole == "Docteur")
                {
                    labelspecialite.Visible = true;
                    comboBoxSpecialite.Visible = true;

                    labeldatenaissance.Visible = false;
                    dateTimePicker1.Visible = false;
                    labelsexe.Visible = false;
                    comboSexe.Visible = false;
                    labelassurance.Visible = false;
                    textAssurance.Visible = false;
                    labeldepartement.Visible = false;
                    comboDepartement.Visible = false;
                }
                else if (selectedRole == "Patient")
                {
                    labeldatenaissance.Visible = true;
                    dateTimePicker1.Visible = true;
                    labelsexe.Visible = true;
                    comboSexe.Visible = true;
                    labelassurance.Visible = true;
                    textAssurance.Visible = true;

                    labelspecialite.Visible = false;
                    comboBoxSpecialite.Visible = false;
                    labeldepartement.Visible = false;
                    comboDepartement.Visible = false;


                }
                else if (selectedRole == "Secretaire")
                {
                    labeldepartement.Visible = true;
                    comboDepartement.Visible = true;

                    labeldatenaissance.Visible = false;
                    dateTimePicker1.Visible = false;
                    labelsexe.Visible = false;
                    comboSexe.Visible = false;
                    labelassurance.Visible = false;
                    textAssurance.Visible = false;
                    labelspecialite.Visible = false;
                    comboBoxSpecialite.Visible = false;
                }
            }

        }

        private bool ValidateForm()
        {   // Role
            if (string.IsNullOrEmpty(comboRole.SelectedItem?.ToString()))
            {
                MessageBox.Show("Veuillez entrer votre role");
                return false;
            }
            // Nom & Prénom
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                MessageBox.Show("Veuillez entrer votre nom et prénom.");
                return false;
            }

            // CIN ( 8 CHIFFRES)
            if (!Regex.IsMatch(txtCIN.Text, @"^\d{8}$"))
            {
                MessageBox.Show("Le numéro CIN doit contenir exactement 8 chiffres.");
                return false;
            }

            // Téléphone (8 CHIFFRES)
            if (!Regex.IsMatch(txtTelephone.Text, @"^\d{8}$"))
            {
                MessageBox.Show("Le numéro de téléphone doit contenir exactement 8 chiffres.");
                return false;
            }

            // Email (basic format)
            if (!Regex.IsMatch(txtMail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Veuillez entrer une adresse email valide.");
                return false;
            }

            // Mot de passe
            if (string.IsNullOrWhiteSpace(txtMDP.Text) || txtMDP.Text.Length < 6)
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 6 caractères.");
                return false;
            }
            if (comboRole.SelectedItem.ToString()== "Patient")
            {
                // sexe vide
                if (string.IsNullOrEmpty(comboSexe.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Veuillez entrer votre sexe");
                    return false;
                }

                // Date de naissance  vide ou represente la date la plus petite
                if (dateTimePicker1.Value == null || dateTimePicker1.Value == DateTime.MinValue)
                {
                    MessageBox.Show("Veuillez entrer une date de naissance valide");
                    return false;
                }
                // Assurance
                if (string.IsNullOrWhiteSpace(textAssurance.Text))
                {
                    MessageBox.Show("Veuillez entrer votre assurance.");
                    return false;
                }
            }
            if (comboRole.SelectedItem.ToString()== "Docteur")
            {
                // speacialite vide
                if (string.IsNullOrEmpty(comboBoxSpecialite.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Veuillez entrer votre specialite.");
                    return false;
                }
            }
            if (comboRole.SelectedItem.ToString() == "Secretaire") { 
            // departement vide
            if (string.IsNullOrEmpty(comboDepartement.SelectedItem?.ToString()))
            {
                MessageBox.Show("Veuillez entrer votre departement");
                return false;
            }
            }
            
            return true;
        }

        private void Valider_Click(object sender, EventArgs e)
        {
           if (ValidateForm())
            {
                string selectedRole = comboRole.SelectedItem.ToString();
                string nom = txtNom.Text;
                string prenom = txtPrenom.Text;
                string cin = txtCIN.Text;
                string telephone = txtTelephone.Text;
                string email = txtMail.Text;
                string mdp = txtMDP.Text;

                string specialite = null;
                string assurance = null;
                string sexe = null;
                string departement = null;
                DateTime dateNaissance = DateTime.MinValue;

                if (selectedRole == "Docteur")
                {
                    specialite = comboBoxSpecialite.SelectedItem?.ToString();
                }
                else if (selectedRole == "Patient")
                {
                    assurance = textAssurance.Text;
                    sexe = comboSexe.SelectedItem?.ToString();
                    dateNaissance = dateTimePicker1.Value;
                }
                else if (selectedRole == "Secretaire")
                {
                    departement = comboDepartement.SelectedItem?.ToString();
                }

                connection.Open();

                //si utilisateur existe 
                string checkQuery = "SELECT COUNT(*) FROM Utilisateur WHERE CIN = " + cin ;
                OleDbCommand checkCmd = new OleDbCommand(checkQuery, connection);
                OleDbDataReader checkreader = checkCmd.ExecuteReader();

                int count = 0;
                if (checkreader.Read())  
                {
                    count = checkreader.GetInt32(0); 
                }

                checkreader.Close();

                if (count > 0)
                {
                    MessageBox.Show("Un utilisateur avec le même CIN existe déjà!");
                    connection.Close();
                    return;
                }
         
                

                string query = "INSERT INTO Utilisateur (Nom, Prenom, Email, MotDePasse, Role, Telephone, CIN) " +
                                "VALUES ('" + nom + "','" + prenom + "','" + email + "','" + mdp + "','" + selectedRole + "','" + telephone + "','" + cin + "')";
                              

                OleDbCommand cmd = new OleDbCommand(query, connection);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Événement ajouté avec succès", "Success");
                }
                else
                {
                    MessageBox.Show("Échec de l'ajout de l'événement", "Error");
                }

                // cin doit etre unique
                string query2 = "SELECT IdUtilisateur FROM Utilisateur WHERE CIN = ?";
                OleDbCommand cmd2 = new OleDbCommand(query2, connection);
                cmd2.Parameters.AddWithValue("?", cin);

                OleDbDataReader reader = cmd2.ExecuteReader();
                int userId = 0;

                if (reader.Read())
                {
                    userId = reader.GetInt32(0);  
                }

                reader.Close();


                //inserer dans la table correspondante au role
                if (selectedRole == "Patient")
                {
                    string query3 = "INSERT INTO Patient (IdUtilisateur, DateNaissance, Sexe, Assurance) " +
                                    "VALUES (" + userId + ", '" + dateNaissance.ToString("yyyy-MM-dd") + "', '" + sexe + "', '" + assurance + "')";
                    OleDbCommand cmd3 = new OleDbCommand(query3, connection);
                    cmd3.ExecuteNonQuery();
                }
                else if (selectedRole == "Docteur")
                {
                    string query4 = "INSERT INTO Medecin (IdUtilisateur, Specialite) VALUES (" + userId + ", '" + specialite + "')";
                    OleDbCommand cmd4 = new OleDbCommand(query4, connection);
                    cmd4.ExecuteNonQuery();
                }
                else if (selectedRole == "Secretaire")
                {
                    string query5 = "INSERT INTO Secretaire (IdUtilisateur, Departement) VALUES (" + userId + ", '" + departement + "')";
                    OleDbCommand cmd5 = new OleDbCommand(query5, connection);
                    cmd5.ExecuteNonQuery();
                }

                MessageBox.Show("Inscription réussie !");
                connection.Close();

                
                txtNom.Clear();
                txtPrenom.Clear();
                txtCIN.Clear();
                txtTelephone.Clear();
                txtMail.Clear();
                txtMDP.Clear();
                textAssurance.Clear();
                comboBoxSpecialite.SelectedIndex = -1;
                comboDepartement.SelectedIndex = -1;
                comboSexe.SelectedIndex = -1;
                dateTimePicker1.Value = DateTime.Now;
                comboRole.SelectedIndex = -1;

                // Hide extra fields
                labelspecialite.Visible = false;
                comboBoxSpecialite.Visible = false;
                labeldatenaissance.Visible = false;
                dateTimePicker1.Visible = false;
                labelsexe.Visible = false;
                comboSexe.Visible = false;
                labelassurance.Visible = false;
                textAssurance.Visible = false;
                labeldepartement.Visible = false;
                comboDepartement.Visible = false;

                //aller vers le login form
                this.Hide();
                signup loginForm = new signup();
                loginForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'inscription. Veuillez réessayer une autre fois");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Êtes-vous sûr de vouloir annuler les modifications ?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
               
                ResetForm();

               
                MenuForm menu = new MenuForm();
                menu.Show(); 
                this.Hide(); 
            }
        }
            void ResetForm()
        {
            
            txtNom.Clear();
            txtPrenom.Clear();
            txtCIN.Clear();
            txtTelephone.Clear();
            txtMail.Clear();
            txtMDP.Clear();
            textAssurance.Clear();
           
            comboRole.SelectedIndex = -1;
            comboBoxSpecialite.SelectedIndex = -1;
            comboDepartement.SelectedIndex = -1;
            comboSexe.SelectedIndex = -1;
            
            dateTimePicker1.Value = DateTime.Now;
            
            labelspecialite.Visible = false;
            comboBoxSpecialite.Visible = false;
            labeldatenaissance.Visible = false;
            dateTimePicker1.Visible = false;
            labelsexe.Visible = false;
            comboSexe.Visible = false;
            labelassurance.Visible = false;
            textAssurance.Visible = false;
            labeldepartement.Visible = false;
            comboDepartement.Visible = false;
        }

        private void signin_Load(object sender, EventArgs e)
        {

        }
    }
}