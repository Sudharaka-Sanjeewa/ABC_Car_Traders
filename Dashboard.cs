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

namespace ABC_Car_Traders
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.OrangeRed;

            string statusp = "Pending";
            string statusd = "Delivered";
            MySqlConnection con = Program.GetConnection;
            try
            {
                MySqlCommand new_orders_cmd = new MySqlCommand("SELECT COUNT(*) FROM orders WHERE Status='" + statusp + "'", con);
                MySqlCommand total_orders_cmd = new MySqlCommand("SELECT COUNT(*) FROM orders", con);
                MySqlCommand total_sales_cmd = new MySqlCommand("SELECT SUM(Total) FROM orders WHERE Status='" + statusd + "'", con);
                MySqlCommand total_customers_cmd = new MySqlCommand("SELECT COUNT(*) FROM customer", con);
                MySqlCommand total_cars_cmd = new MySqlCommand("SELECT COUNT(*) FROM car", con);
                MySqlCommand total_parts_cmd = new MySqlCommand("SELECT COUNT(*) FROM part", con);

                btnNewOrders.Text = new_orders_cmd.ExecuteScalar().ToString();
                btnTotalOrders.Text = total_orders_cmd.ExecuteScalar().ToString();
                btnSales.Text = total_sales_cmd.ExecuteScalar().ToString();
                btnTotalCustomers.Text = total_customers_cmd.ExecuteScalar().ToString();
                btnTotalCars.Text = total_cars_cmd.ExecuteScalar().ToString();
                btnTotalParts.Text = total_parts_cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Show();
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
            DashboardReports dashboardreports = new DashboardReports();
            dashboardreports.Show();
            this.Hide();
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
