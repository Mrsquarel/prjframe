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
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DoctorDashboard doctorDashboard = new DoctorDashboard();
            doctorDashboard.Show();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            adminpanel adminDashboard = new adminpanel();
            adminDashboard.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            secretary_dashboard secretaryDashboard = new secretary_dashboard();
            secretaryDashboard.Show();
        }
    }
}
