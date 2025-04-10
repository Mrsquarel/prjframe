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
    public partial class signup : Form
    {
        public static int IdUtilisateur = 0;
        private OleDbConnection connection;

        public signup()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
        }

        private void Valider_Click(object sender, EventArgs e)
        {
            connection.Open();

            string query = "SELECT MotDePasse, Role , Idutilisateur FROM Utilisateur WHERE Email = '" + emailtxt.Text + "'";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                // Email exists, check password
                if (reader["MotDePasse"].ToString() == mdptxt.Text)
                {
                    MessageBox.Show("Bienvenue !");
                    this.Hide();
                    IdUtilisateur = Convert.ToInt32(reader["IdUtilisateur"]);

                    string role = reader["Role"].ToString();
                    if (role == "Admin")
                    {
                        new adminpanel(IdUtilisateur).Show();
                    }
                    else if (role == "Patient")
                    {
                        new PatientDashboard(IdUtilisateur).Show();
                    }
                    else if (role == "Secretaire")
                    {
                        new secretary_dashboard(IdUtilisateur).Show();
                    }
                    else if (role == "Docteur")
                    {
                        new DoctorDashboard(IdUtilisateur).Show();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Mot de passe incorrect.");
                }
            }
            else
            {
                MessageBox.Show("Email incorrect ou utilisateur inexistant.");
            }

            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            emailtxt.Text = "";
            mdptxt.Text = "";

            
            this.Hide();

            
            MenuForm menu = new MenuForm();  
            menu.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string email = emailtxt.Text.Trim();

            
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Veuillez entrer votre adresse e-mail.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            connection.Open();
            string query = "SELECT COUNT(*) FROM Utilisateur WHERE Email = '" + email + "'";
            OleDbCommand command = new OleDbCommand(query, connection);
            int emailCount = Convert.ToInt32(command.ExecuteScalar());  

            if (emailCount > 0)
            {
                MessageBox.Show("Un e-mail vous sera envoyé pour réinitialiser votre mot de passe.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                MenuForm menu = new MenuForm();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Cet e-mail n'est pas enregistré dans notre système.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            connection.Close();
        }
    }
}
