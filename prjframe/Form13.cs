using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing.Imaging;
using PdfSharp.Drawing.Layout;
namespace prjframe
{
    public partial class prescription_form : Form
    {
        private int _idPatient;
        private int _idConsultation;
        private int _idDocteur;
        private OleDbConnection _connection;


        OleDbConnection connection;
        public prescription_form(int idPatient_dash, int idDocteur_dash, int idConsultation_dash)
        {
            string dbPath = Path.Combine(Application.StartupPath, "DatabaseHealthApp.accdb");
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";


            connection = new OleDbConnection(connectionString);
            InitializeComponent();
            _idPatient = idPatient_dash;
            _idDocteur = idDocteur_dash;
            _idConsultation = idConsultation_dash;
        }

        private void prescription_form_Load(object sender, EventArgs e)
        {
            LoadDoctorInfo();
            LoadPatientInfo();
            label4.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
        private void LoadDoctorInfo()
        {
            const string doctorSql = @"
                SELECT u.Nom, u.Prenom, u.Telephone, u.Email
                FROM Medecin m
                INNER JOIN Utilisateur u ON m.IdUtilisateur = u.IdUtilisateur
                WHERE m.IdUtilisateur = ?";
            connection.Open();
            using (var cmd = new OleDbCommand(doctorSql, connection))
            {
                cmd.Parameters.AddWithValue("?", _idDocteur);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string prenom = reader["Prenom"].ToString();
                        string telephone = reader["Telephone"].ToString();
                        string email = reader["Email"].ToString();

                        label1.Text = $"{prenom} {nom}";
                        label2.Text = telephone;
                        label3.Text = email;
                    }
                }
                connection.Close();
            }
        }

        private void LoadPatientInfo()
        {
            const string patientSql = @"
                SELECT u.Nom, u.Prenom
                FROM Patient p
                INNER JOIN Utilisateur u ON p.IdUtilisateur = u.IdUtilisateur
                WHERE p.IdPatient = ?";
            connection.Open();
            using (var cmd = new OleDbCommand(patientSql, connection))
            {
                cmd.Parameters.AddWithValue("?", _idPatient);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nom = reader["Nom"].ToString();
                        string prenom = reader["Prenom"].ToString();
                        string patientName = $"{nom} {prenom}";
                        label5.Text = patientName;
                    }
                }
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1) Prépare le dossier de stockage
            string prescriptionsDir = Path.Combine(Application.StartupPath, "Prescriptions");
            if (!Directory.Exists(prescriptionsDir))
                Directory.CreateDirectory(prescriptionsDir);

            // 2) Nom de fichier unique
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = $"Prescription_{_idConsultation}_{timestamp}.pdf";
            string filePath = Path.Combine(prescriptionsDir, fileName);

            // 3) Génération du PDF
            try
            {
                using (var pdf = new PdfDocument())
                {
                    pdf.Info.Title = "Prescription";
                    PdfPage page = pdf.AddPage();
                    page.Size = PdfSharp.PageSize.A4;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        double margin = 40;
                       
                        var hfFont = new XFont("Verdana", 8, XFontStyle.Italic);
                        double y = margin;

                        // ligne horizontale
                        gfx.DrawLine(XPens.Gray, margin, y, page.Width - margin, y);
                        y += 5;

                        // date
                        string dateStr = DateTime.Now.ToString("dd/MM/yyyy");
                        gfx.DrawString($"Date    : {dateStr}", hfFont, XBrushes.Black,
                            new XRect(margin, y, page.Width - 2 * margin, 15), XStringFormats.TopLeft);
                        y += 12;

                        // patient
                        string patientName = label5.Text;
                        gfx.DrawString($"Patient : {patientName}", hfFont, XBrushes.Black,
                            new XRect(margin, y, page.Width - 2 * margin, 15), XStringFormats.TopLeft);
                        y += 20;

                        // -------------------------------------------------------
                        // Corps de la prescription 
                        // -------------------------------------------------------
                        var bodyFont = new XFont("Verdana", 10, XFontStyle.Regular);
                        double currentY = y;

                        // Vérifier si richTextBox1 contient du texte
                        if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                        {
                            // Si le texte est vide, afficher un message temporaire dans le PDF
                            gfx.DrawString("Aucune prescription entrée.", bodyFont, XBrushes.Black,
                                new XRect(margin, currentY, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
                            currentY += 15;
                        }
                        else
                        {
                            // Normaliser les retours à la ligne et diviser le texte en lignes
                            string normalizedText = richTextBox1.Text.Trim().Replace("\r\n", "\n").Replace("\r", "\n");
                            string[] lines = normalizedText.Split('\n');

                            // Dessiner chaque ligne
                            foreach (string line in lines)
                            {
                                if (!string.IsNullOrWhiteSpace(line)) // Ignorer les lignes vides
                                {
                                    gfx.DrawString(
                                        line,
                                        bodyFont,
                                        XBrushes.Black,
                                        new XRect(margin, currentY, page.Width - 2 * margin, 20),
                                        XStringFormats.TopLeft
                                    );
                                    currentY += 15; // Espacement entre les lignes
                                }
                            }
                        }

                        // -------------------------------------------------------
                        // Footer : infos du médecin
                        // -------------------------------------------------------
                        var footerFont = new XFont("Verdana", 8, XFontStyle.Italic);
                        double footerY = Math.Max(page.Height - margin - 45, currentY + 20); 
                        gfx.DrawLine(XPens.Gray, margin, footerY, page.Width - margin, footerY);
                        footerY += 5;

                        gfx.DrawString($"Médecin : {label1.Text}", footerFont, XBrushes.Black,
                            new XRect(margin, footerY, page.Width - 2 * margin, 15), XStringFormats.TopLeft);
                        footerY += 12;

                        gfx.DrawString($"Tél     : {label2.Text}", footerFont, XBrushes.Black,
                            new XRect(margin, footerY, page.Width - 2 * margin, 15), XStringFormats.TopLeft);
                        footerY += 12;

                        gfx.DrawString($"Email   : {label3.Text}", footerFont, XBrushes.Black,
                            new XRect(margin, footerY, page.Width - 2 * margin, 15), XStringFormats.TopLeft);
                    }

                    pdf.Save(filePath);
                }

                // 4) Enregistre dans la table Prescription
                const string insertSql = @"
                    INSERT INTO Prescription (IdConsultation, CheminFichier)
                    VALUES (?, ?)";
                connection.Open();
                using (var cmd = new OleDbCommand(insertSql, connection))
                {
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = _idConsultation;
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = filePath;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();

                MessageBox.Show(
                    $"PDF généré et enregistré en base.\n{filePath}",
                    "Succès",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}
    
