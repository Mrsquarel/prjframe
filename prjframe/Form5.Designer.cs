namespace prjframe
{
    partial class ProfileDoctor
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.date_label = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.profile_pic = new System.Windows.Forms.PictureBox();
            this.logout_out = new System.Windows.Forms.PictureBox();
            this.specialite = new System.Windows.Forms.Label();
            this.doctor_name = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxSpecialite = new System.Windows.Forms.ComboBox();
            this.txtMotDePasse = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.annuler_btn = new System.Windows.Forms.Button();
            this.modifier_btn = new System.Windows.Forms.Button();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtCIN = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxDisponibilite = new System.Windows.Forms.GroupBox();
            this.btnAnnulerDisponibilite = new System.Windows.Forms.Button();
            this.btnValiderDisponibilite = new System.Windows.Forms.Button();
            this.dataGridViewSchedule = new System.Windows.Forms.DataGridView();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.profile_pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logout_out)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBoxDisponibilite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSchedule)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.Controls.Add(this.date_label);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Controls.Add(this.profile_pic);
            this.panel3.Controls.Add(this.logout_out);
            this.panel3.Controls.Add(this.specialite);
            this.panel3.Controls.Add(this.doctor_name);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1280, 126);
            this.panel3.TabIndex = 1;
            // 
            // date_label
            // 
            this.date_label.AutoSize = true;
            this.date_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_label.Location = new System.Drawing.Point(1008, 78);
            this.date_label.Name = "date_label";
            this.date_label.Size = new System.Drawing.Size(23, 28);
            this.date_label.TabIndex = 11;
            this.date_label.Text = "0";
            this.date_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(823, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(179, 28);
            this.label6.TabIndex = 10;
            this.label6.Text = "Date d\'aujourd\'hui:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::prjframe.Properties.Resources.icons8_médecin_321;
            this.pictureBox3.Location = new System.Drawing.Point(22, 34);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(44, 41);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // profile_pic
            // 
            this.profile_pic.Image = global::prjframe.Properties.Resources.icons8_menu_utilisateur_homme_32;
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
            // specialite
            // 
            this.specialite.AutoSize = true;
            this.specialite.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.specialite.Location = new System.Drawing.Point(75, 78);
            this.specialite.Name = "specialite";
            this.specialite.Size = new System.Drawing.Size(96, 28);
            this.specialite.TabIndex = 2;
            this.specialite.Text = "Spécialité";
            this.specialite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // doctor_name
            // 
            this.doctor_name.AutoSize = true;
            this.doctor_name.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doctor_name.Location = new System.Drawing.Point(356, 26);
            this.doctor_name.Name = "doctor_name";
            this.doctor_name.Size = new System.Drawing.Size(224, 45);
            this.doctor_name.TabIndex = 3;
            this.doctor_name.Text = "nom_docteur";
            this.doctor_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(72, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 45);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bienvenue Dr. ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSpecialite);
            this.groupBox1.Controls.Add(this.txtMotDePasse);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.annuler_btn);
            this.groupBox1.Controls.Add(this.modifier_btn);
            this.groupBox1.Controls.Add(this.txtMail);
            this.groupBox1.Controls.Add(this.txtTelephone);
            this.groupBox1.Controls.Add(this.txtPrenom);
            this.groupBox1.Controls.Add(this.txtNom);
            this.groupBox1.Controls.Add(this.txtCIN);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 479);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informations Personnelles";
            // 
            // comboBoxSpecialite
            // 
            this.comboBoxSpecialite.Enabled = false;
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
            "Rhumatologie"});
            this.comboBoxSpecialite.Location = new System.Drawing.Point(215, 296);
            this.comboBoxSpecialite.Name = "comboBoxSpecialite";
            this.comboBoxSpecialite.Size = new System.Drawing.Size(259, 30);
            this.comboBoxSpecialite.TabIndex = 16;
            // 
            // txtMotDePasse
            // 
            this.txtMotDePasse.Location = new System.Drawing.Point(215, 358);
            this.txtMotDePasse.Name = "txtMotDePasse";
            this.txtMotDePasse.ReadOnly = true;
            this.txtMotDePasse.Size = new System.Drawing.Size(259, 28);
            this.txtMotDePasse.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(61, 358);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 22);
            this.label10.TabIndex = 14;
            this.label10.Text = "Mot de Passe:";
            // 
            // annuler_btn
            // 
            this.annuler_btn.Location = new System.Drawing.Point(353, 431);
            this.annuler_btn.Name = "annuler_btn";
            this.annuler_btn.Size = new System.Drawing.Size(121, 41);
            this.annuler_btn.TabIndex = 13;
            this.annuler_btn.Text = "Annuler";
            this.annuler_btn.UseVisualStyleBackColor = true;
            this.annuler_btn.Click += new System.EventHandler(this.annuler_btn_Click);
            // 
            // modifier_btn
            // 
            this.modifier_btn.Location = new System.Drawing.Point(118, 431);
            this.modifier_btn.Name = "modifier_btn";
            this.modifier_btn.Size = new System.Drawing.Size(139, 41);
            this.modifier_btn.TabIndex = 12;
            this.modifier_btn.Text = "Modifier";
            this.modifier_btn.UseVisualStyleBackColor = true;
            this.modifier_btn.Click += new System.EventHandler(this.modifier_btn_Click);
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(215, 243);
            this.txtMail.Name = "txtMail";
            this.txtMail.ReadOnly = true;
            this.txtMail.Size = new System.Drawing.Size(259, 28);
            this.txtMail.TabIndex = 10;
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(215, 198);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.ReadOnly = true;
            this.txtTelephone.Size = new System.Drawing.Size(259, 28);
            this.txtTelephone.TabIndex = 9;
            // 
            // txtPrenom
            // 
            this.txtPrenom.Location = new System.Drawing.Point(215, 149);
            this.txtPrenom.Name = "txtPrenom";
            this.txtPrenom.ReadOnly = true;
            this.txtPrenom.Size = new System.Drawing.Size(259, 28);
            this.txtPrenom.TabIndex = 8;
            // 
            // txtNom
            // 
            this.txtNom.Location = new System.Drawing.Point(215, 97);
            this.txtNom.Name = "txtNom";
            this.txtNom.ReadOnly = true;
            this.txtNom.Size = new System.Drawing.Size(259, 28);
            this.txtNom.TabIndex = 7;
            // 
            // txtCIN
            // 
            this.txtCIN.Location = new System.Drawing.Point(215, 41);
            this.txtCIN.Name = "txtCIN";
            this.txtCIN.ReadOnly = true;
            this.txtCIN.Size = new System.Drawing.Size(259, 28);
            this.txtCIN.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 299);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 22);
            this.label9.TabIndex = 5;
            this.label9.Text = "Spécialité:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(61, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 22);
            this.label8.TabIndex = 4;
            this.label8.Text = "Mail:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 22);
            this.label7.TabIndex = 3;
            this.label7.Text = "Télèphone:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 22);
            this.label4.TabIndex = 2;
            this.label4.Text = "CIN:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 22);
            this.label3.TabIndex = 1;
            this.label3.Text = "Prénom:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom:";
            // 
            // groupBoxDisponibilite
            // 
            this.groupBoxDisponibilite.Controls.Add(this.btnAnnulerDisponibilite);
            this.groupBoxDisponibilite.Controls.Add(this.btnValiderDisponibilite);
            this.groupBoxDisponibilite.Controls.Add(this.dataGridViewSchedule);
            this.groupBoxDisponibilite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDisponibilite.Location = new System.Drawing.Point(548, 164);
            this.groupBoxDisponibilite.Name = "groupBoxDisponibilite";
            this.groupBoxDisponibilite.Size = new System.Drawing.Size(722, 478);
            this.groupBoxDisponibilite.TabIndex = 3;
            this.groupBoxDisponibilite.TabStop = false;
            this.groupBoxDisponibilite.Text = "Disponibilité Par Semaine";
            // 
            // btnAnnulerDisponibilite
            // 
            this.btnAnnulerDisponibilite.Location = new System.Drawing.Point(415, 431);
            this.btnAnnulerDisponibilite.Name = "btnAnnulerDisponibilite";
            this.btnAnnulerDisponibilite.Size = new System.Drawing.Size(121, 41);
            this.btnAnnulerDisponibilite.TabIndex = 15;
            this.btnAnnulerDisponibilite.Text = "Annuler";
            this.btnAnnulerDisponibilite.UseVisualStyleBackColor = true;
            this.btnAnnulerDisponibilite.Click += new System.EventHandler(this.btnAnnulerDisponibilite_Click);
            // 
            // btnValiderDisponibilite
            // 
            this.btnValiderDisponibilite.Location = new System.Drawing.Point(180, 431);
            this.btnValiderDisponibilite.Name = "btnValiderDisponibilite";
            this.btnValiderDisponibilite.Size = new System.Drawing.Size(139, 41);
            this.btnValiderDisponibilite.TabIndex = 14;
            this.btnValiderDisponibilite.Text = "Modifier";
            this.btnValiderDisponibilite.UseVisualStyleBackColor = true;
            this.btnValiderDisponibilite.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridViewSchedule
            // 
            this.dataGridViewSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewSchedule.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSchedule.Location = new System.Drawing.Point(7, 28);
            this.dataGridViewSchedule.Name = "dataGridViewSchedule";
            this.dataGridViewSchedule.RowHeadersWidth = 51;
            this.dataGridViewSchedule.RowTemplate.Height = 24;
            this.dataGridViewSchedule.Size = new System.Drawing.Size(709, 397);
            this.dataGridViewSchedule.TabIndex = 0;
            // 
            // ProfileDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 655);
            this.Controls.Add(this.groupBoxDisponibilite);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Name = "ProfileDoctor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProfileDoctor";
            this.Load += new System.EventHandler(this.ProfileDoctor_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.profile_pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logout_out)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxDisponibilite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSchedule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox profile_pic;
        private System.Windows.Forms.PictureBox logout_out;
        private System.Windows.Forms.Label specialite;
        private System.Windows.Forms.Label doctor_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button modifier_btn;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.TextBox txtCIN;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button annuler_btn;
        private System.Windows.Forms.TextBox txtMotDePasse;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBoxDisponibilite;
        private System.Windows.Forms.DataGridView dataGridViewSchedule;
        private System.Windows.Forms.Label date_label;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAnnulerDisponibilite;
        private System.Windows.Forms.Button btnValiderDisponibilite;
        private System.Windows.Forms.ComboBox comboBoxSpecialite;
    }
}