namespace prjframe
{
    partial class PatientDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label_week = new System.Windows.Forms.Label();
            this.checkedListBoxRDV = new System.Windows.Forms.CheckedListBox();
            this.valider_btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxDoctors = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSpecialite = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewHistorique = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.date_label = new System.Windows.Forms.Label();
            this.nbr_cons = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.profile_pic = new System.Windows.Forms.PictureBox();
            this.logout_out = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.specialite = new System.Windows.Forms.Label();
            this.patient_name = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorique)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.profile_pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logout_out)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 671);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(35, 172);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1210, 487);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label_week);
            this.tabPage1.Controls.Add(this.checkedListBoxRDV);
            this.tabPage1.Controls.Add(this.valider_btn);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.comboBoxDoctors);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.comboBoxSpecialite);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1202, 458);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Prendre un rendez-vous";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_week
            // 
            this.label_week.AutoSize = true;
            this.label_week.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_week.Location = new System.Drawing.Point(486, 183);
            this.label_week.Name = "label_week";
            this.label_week.Size = new System.Drawing.Size(64, 18);
            this.label_week.TabIndex = 12;
            this.label_week.Text = "semaine";
            // 
            // checkedListBoxRDV
            // 
            this.checkedListBoxRDV.FormattingEnabled = true;
            this.checkedListBoxRDV.Location = new System.Drawing.Point(291, 213);
            this.checkedListBoxRDV.Name = "checkedListBoxRDV";
            this.checkedListBoxRDV.Size = new System.Drawing.Size(584, 174);
            this.checkedListBoxRDV.TabIndex = 11;
            // 
            // valider_btn
            // 
            this.valider_btn.Location = new System.Drawing.Point(714, 119);
            this.valider_btn.Name = "valider_btn";
            this.valider_btn.Size = new System.Drawing.Size(161, 30);
            this.valider_btn.TabIndex = 10;
            this.valider_btn.Text = "Valider";
            this.valider_btn.UseVisualStyleBackColor = true;
            this.valider_btn.Click += new System.EventHandler(this.valider_btn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(288, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 18);
            this.label6.TabIndex = 9;
            this.label6.Text = "Les horaires disponibles";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(698, 407);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(177, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "Reserver le rendez vous";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBoxDoctors
            // 
            this.comboBoxDoctors.FormattingEnabled = true;
            this.comboBoxDoctors.Location = new System.Drawing.Point(489, 77);
            this.comboBoxDoctors.Name = "comboBoxDoctors";
            this.comboBoxDoctors.Size = new System.Drawing.Size(386, 24);
            this.comboBoxDoctors.TabIndex = 3;
            this.comboBoxDoctors.SelectedIndexChanged += new System.EventHandler(this.comboBoxDoctors_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(278, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Selectionner un medecin";
            // 
            // comboBoxSpecialite
            // 
            this.comboBoxSpecialite.FormattingEnabled = true;
            this.comboBoxSpecialite.Items.AddRange(new object[] {
            "Cardiologie",
            "Dermatologie",
            "Pédiatrie",
            "Neurologie",
            "Orthopédie",
            "Gynécologie",
            "Ophtalmologie",
            "Psychiatrie",
            "Endocrinologie",
            "Gastroentérologie",
            "Oncologie",
            "Urologie",
            "Rhumatologie",
            "Chirurgie générale",
            "Médecine interne"});
            this.comboBoxSpecialite.Location = new System.Drawing.Point(489, 30);
            this.comboBoxSpecialite.Name = "comboBoxSpecialite";
            this.comboBoxSpecialite.Size = new System.Drawing.Size(386, 24);
            this.comboBoxSpecialite.TabIndex = 1;
            this.comboBoxSpecialite.SelectedIndexChanged += new System.EventHandler(this.comboBoxSpecialite_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(278, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Selectionner une specialite";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewHistorique);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1202, 458);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mes consultations";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewHistorique
            // 
            this.dataGridViewHistorique.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistorique.Location = new System.Drawing.Point(46, 52);
            this.dataGridViewHistorique.Name = "dataGridViewHistorique";
            this.dataGridViewHistorique.ReadOnly = true;
            this.dataGridViewHistorique.RowHeadersWidth = 51;
            this.dataGridViewHistorique.RowTemplate.Height = 24;
            this.dataGridViewHistorique.Size = new System.Drawing.Size(1102, 387);
            this.dataGridViewHistorique.TabIndex = 16;
            this.dataGridViewHistorique.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewHistorique_CellContentClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(240, 20);
            this.label9.TabIndex = 13;
            this.label9.Text = "Historique des consultation";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.date_label);
            this.panel3.Controls.Add(this.nbr_cons);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Controls.Add(this.profile_pic);
            this.panel3.Controls.Add(this.logout_out);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.specialite);
            this.panel3.Controls.Add(this.patient_name);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1284, 126);
            this.panel3.TabIndex = 13;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(815, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 28);
            this.label1.TabIndex = 11;
            this.label1.Text = "Date d\'aujourd\'hui :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // date_label
            // 
            this.date_label.AutoSize = true;
            this.date_label.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.date_label.Location = new System.Drawing.Point(1058, 78);
            this.date_label.Name = "date_label";
            this.date_label.Size = new System.Drawing.Size(23, 28);
            this.date_label.TabIndex = 10;
            this.date_label.Text = "0";
            this.date_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nbr_cons
            // 
            this.nbr_cons.AutoSize = true;
            this.nbr_cons.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nbr_cons.Location = new System.Drawing.Point(999, 39);
            this.nbr_cons.Name = "nbr_cons";
            this.nbr_cons.Size = new System.Drawing.Size(0, 28);
            this.nbr_cons.TabIndex = 9;
            this.nbr_cons.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::prjframe.Properties.Resources.icons8_patient_32;
            this.pictureBox3.Location = new System.Drawing.Point(22, 34);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(44, 41);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // profile_pic
            // 
            this.profile_pic.Image = global::prjframe.Properties.Resources.icons8_invité_homme_32;
            this.profile_pic.Location = new System.Drawing.Point(1116, 9);
            this.profile_pic.Name = "profile_pic";
            this.profile_pic.Size = new System.Drawing.Size(56, 58);
            this.profile_pic.TabIndex = 7;
            this.profile_pic.TabStop = false;
            this.profile_pic.Click += new System.EventHandler(this.profile_pic_Click);
            // 
            // logout_out
            // 
            this.logout_out.Image = global::prjframe.Properties.Resources.icons8_connexion_32;
            this.logout_out.Location = new System.Drawing.Point(1188, 9);
            this.logout_out.Name = "logout_out";
            this.logout_out.Size = new System.Drawing.Size(57, 58);
            this.logout_out.TabIndex = 6;
            this.logout_out.TabStop = false;
            this.logout_out.Click += new System.EventHandler(this.logout_out_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(756, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 28);
            this.label2.TabIndex = 4;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // specialite
            // 
            this.specialite.AutoSize = true;
            this.specialite.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.specialite.Location = new System.Drawing.Point(75, 78);
            this.specialite.Name = "specialite";
            this.specialite.Size = new System.Drawing.Size(0, 28);
            this.specialite.TabIndex = 2;
            this.specialite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // patient_name
            // 
            this.patient_name.AutoSize = true;
            this.patient_name.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patient_name.Location = new System.Drawing.Point(263, 26);
            this.patient_name.Name = "patient_name";
            this.patient_name.Size = new System.Drawing.Size(215, 45);
            this.patient_name.TabIndex = 3;
            this.patient_name.Text = "nom_patient";
            this.patient_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(72, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(200, 45);
            this.label10.TabIndex = 2;
            this.label10.Text = "Bienvenue  ";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PatientDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 671);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitter1);
            this.Name = "PatientDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Patient";
            this.Load += new System.EventHandler(this.Patient_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorique)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.profile_pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logout_out)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSpecialite;
        private System.Windows.Forms.ComboBox comboBoxDoctors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label nbr_cons;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox profile_pic;
        private System.Windows.Forms.PictureBox logout_out;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label specialite;
        private System.Windows.Forms.Label patient_name;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckedListBox checkedListBoxRDV;
        private System.Windows.Forms.Button valider_btn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridViewHistorique;
        private System.Windows.Forms.Label label_week;
        private System.Windows.Forms.Label date_label;
        private System.Windows.Forms.Label label1;
    }
}