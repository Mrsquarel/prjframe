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
    public partial class ProfileDoctor : Form
    {
        public ProfileDoctor()
        {
            InitializeComponent();
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

        private void ProfileDoctor_Load(object sender, EventArgs e)
        {
            // Clear previous columns and rows
            dataGridViewSchedule.Columns.Clear();
            dataGridViewSchedule.Rows.Clear();

            // Add the "Heure" column
            DataGridViewTextBoxColumn heureCol = new DataGridViewTextBoxColumn();
            heureCol.HeaderText = "Heure";
            heureCol.Name = "Heure";
            heureCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewSchedule.Columns.Add(heureCol);

            // Add day columns with AutoSizeMode.Fill
            string[] jours = { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" };
            foreach (string jour in jours)
            {
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                col.HeaderText = jour;
                col.Name = jour;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewSchedule.Columns.Add(col);
            }

            // Fill rows with time slots from 09:00 to 17:00 (every 30 min)
            DateTime startTime = DateTime.Today.AddHours(9);
            for (int i = 0; i <= 16; i++)
            {
                string timeLabel = startTime.AddMinutes(i * 30).ToString("HH:mm");
                int rowIndex = dataGridViewSchedule.Rows.Add();
                dataGridViewSchedule.Rows[rowIndex].Cells["Heure"].Value = timeLabel;
            }

            // Basic formatting
            dataGridViewSchedule.AllowUserToAddRows = false;
            dataGridViewSchedule.RowHeadersVisible = false;
        }



        private void profile_pic_Click(object sender, EventArgs e)
        {
            DoctorDashboard dashboard = new DoctorDashboard();
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
                // Open MenuForm
                MenuForm menu = new MenuForm();
                menu.Show();

                // Close current dashboard
                this.Close();
            }
        }
        bool isEditMode2 = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isEditMode2)
            {
                button2.Text = "Sauvegarder";
                isEditMode2 = true;
            }
            else
            {
                button2.Text = "Modifier";
            }
        }

      
    }
}

