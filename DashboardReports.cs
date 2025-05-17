using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Printing;

namespace ABC_Car_Traders
{
    public partial class DashboardReports : Form
    {
        public DashboardReports()
        {
            InitializeComponent();
        }

        private void DashboardReports_Load(object sender, EventArgs e)
        {
            btnReports.BackColor = Color.OrangeRed;
            FilldgvReports();
        }

        public void FilldgvReports()
        {
            try
            {
                //populate the datagridview            
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM orders", con);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvReports.DataSource = dt;

                DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
                DataGridViewImageColumn img = new DataGridViewImageColumn();
                DataGridViewTextBoxColumn userid = new DataGridViewTextBoxColumn();                
                id = (DataGridViewTextBoxColumn)dgvReports.Columns[0];
                img = (DataGridViewImageColumn)dgvReports.Columns[1];
                userid = (DataGridViewTextBoxColumn)dgvReports.Columns[8];
                id.Visible = false;
                img.Visible = false;
                userid.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM orders WHERE Date BETWEEN @from AND @to", con);

                cmd.Parameters.Add("@from", MySqlDbType.Date).Value = dtpFrom.Value;
                cmd.Parameters.Add("@to", MySqlDbType.Date).Value = dtpTo.Value;

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvReports.DataSource = dt;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            DashboardOrders dashboardorders = new DashboardOrders();
            dashboardorders.Show();
            this.Hide();
        }

        private void btnCars_Click(object sender, EventArgs e)
        {
            DashboardCars dashboardcars = new DashboardCars();
            dashboardcars.Show();
            this.Hide();
        }

        private void btnParts_Click(object sender, EventArgs e)
        {
            DashboardParts dashboardparts = new DashboardParts();
            dashboardparts.Show();
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            DashboardCustomers dashboardcustomers = new DashboardCustomers();
            dashboardcustomers.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            this.Show();
        }        

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
