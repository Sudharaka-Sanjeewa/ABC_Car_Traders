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

namespace ABC_Car_Traders
{
    public partial class DashboardOrders : Form
    {
        public DashboardOrders()
        {
            InitializeComponent();
        }

        private void DashboardOrders_Load(object sender, EventArgs e)
        {
            btnOrders.BackColor = Color.OrangeRed;
            FilldgvOrder("");
        }

        public void FilldgvOrder(string Search)
        {
            //populate the datagridview            
            MySqlConnection con = Program.GetConnection;
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM orders WHERE CONCAT(Model, Price, Quantity, Total, Date, Status)LIKE'%" + Search + "%'", con);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgvOrder.DataSource = dt;

            DataGridViewImageColumn Img = new DataGridViewImageColumn();
            DataGridViewTextBoxColumn UserID = new DataGridViewTextBoxColumn();
            Img = (DataGridViewImageColumn)dgvOrder.Columns[1];
            UserID = (DataGridViewTextBoxColumn)dgvOrder.Columns[8];
            Img.Visible = false;
            UserID.Visible = false;
            pnlAddress.Hide();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilldgvOrder(txtSearch.Text);
        }

        private void dgvOrder_MouseClick(object sender, MouseEventArgs e)
        {
            pnlAddress.Show();
            int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[8].Value);
            string status = dgvOrder.CurrentRow.Cells[7].Value.ToString();

            if (status == "Pending")
            {
                btnConfirm.Show();
                btnCanceled.Show();
                btnDelivered.Hide();
            }
            else if (status == "Confirmed")
            {
                btnDelivered.Show();
                btnConfirm.Hide();
                btnCanceled.Hide();
            }
            else
            {
                btnConfirm.Hide();
                btnCanceled.Hide();
                btnDelivered.Hide();
            }

            try
            {
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `customer` WHERE `ID`=" + id + "", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                lblName.Text = dt.Rows[0][3].ToString();
                lblAddress.Text = dt.Rows[0][6].ToString();
                lblEmail.Text = dt.Rows[0][4].ToString();
                lblMobile.Text = dt.Rows[0][5].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[0].Value);
                string status = "Confirmed";
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `orders` SET `Status`='" + status + "' WHERE `ID`=" + id + "", con);
                cmd.ExecuteNonQuery();
                FilldgvOrder("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvOrder("");
            }
        }

        private void btnDelivered_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[0].Value);
                string status = "Delivered";
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `orders` SET `Status`='" + status + "' WHERE `ID`=" + id + "", con);
                cmd.ExecuteNonQuery();
                FilldgvOrder("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvOrder("");
            }
        }

        private void btnCanceled_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[0].Value);
                string status = "Canceled";
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `orders` SET `Status`='" + status + "' WHERE `ID`=" + id + "", con);
                cmd.ExecuteNonQuery();
                FilldgvOrder("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvOrder("");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[0].Value);
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM `orders` WHERE `ID`=" + id + "", con);
                cmd1.ExecuteNonQuery();
                FilldgvOrder("");
                MessageBox.Show("Order Removed Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvOrder("");
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
            this.Show();
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
