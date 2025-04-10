using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjframe
{
    public partial class ProfileSecretary : Form
    {
        private int _idUtilisateur; 
        public ProfileSecretary(int idUtilisateur)
        {
            InitializeComponent();
            _idUtilisateur = idUtilisateur;
        }

        private void profile_pic_Click(object sender, EventArgs e)
        {
            secretary_dashboard dashboard = new secretary_dashboard(_idUtilisateur);
            dashboard.Show();
        }
        bool isEditMode = false;
        private void modifier_btn_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                // Enter Edit Mode
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                textBox7.ReadOnly = false;
                modifier_btn.Text = "Sauvegarder";
                isEditMode = true;
            }
            else
            {
                // Save and Return to View Mode
                // TODO: Validate & save to DB or memory

                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                modifier_btn.Text = "Modifier";

                isEditMode = false;

                MessageBox.Show("Profil mis à jour avec succès !");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            secretary_dashboard dashboard = new secretary_dashboard(_idUtilisateur);
            dashboard.Show();
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
                // Open MenuForm
                MenuForm menu = new MenuForm();
                menu.Show();

                // Close current dashboard
                this.Close();
            }

        }

        private void ProfileSecretary_Load(object sender, EventArgs e)
        {

        }
    }
}
