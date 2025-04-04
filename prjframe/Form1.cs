using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace prjframe
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            OleDbConnection conn = new OleDbConnection();
            string ChaineConnexion = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=bdcm.accdb";
            conn.ConnectionString = ChaineConnexion;
            


        }

        private void Form1_Load(object sender, EventArgs e)
            {

            }
    }
}
