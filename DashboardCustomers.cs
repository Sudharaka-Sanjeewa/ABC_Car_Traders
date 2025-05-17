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
using System.Text.RegularExpressions;

namespace ABC_Car_Traders
{
    public partial class DashboardCustomers : Form
    {
        public DashboardCustomers()
        {
            InitializeComponent();
        }

        private void DashboardCustomers_Load(object sender, EventArgs e)
        {
            btnCustomers.BackColor = Color.OrangeRed;
            FilldgvCustomers("");
        }

        public void FilldgvCustomers(string Search)
        {
            try
            {
                //populate the datagridview            
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM customer WHERE CONCAT(Name, Email, Mobile, Address)LIKE'%" + Search + "%'", con);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvCustomers.DataSource = dt;

                DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn username = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn password = new DataGridViewTextBoxColumn();
                id = (DataGridViewTextBoxColumn)dgvCustomers.Columns[0];
                username = (DataGridViewTextBoxColumn)dgvCustomers.Columns[1];
                password = (DataGridViewTextBoxColumn)dgvCustomers.Columns[2];
                id.Visible = false;
                username.Visible = false;
                password.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            FilldgvCustomers(txtSearch.Text);
        }

        private void dgvCustomers_MouseClick(object sender, MouseEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[3].Value.ToString();
            txtEmail.Text = dgvCustomers.CurrentRow.Cells[4].Value.ToString();
            txtMobile.Text = dgvCustomers.CurrentRow.Cells[5].Value.ToString();
            txtAddress.Text = dgvCustomers.CurrentRow.Cells[6].Value.ToString();
        }        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Email = txtEmail.Text;
            string Mobile = txtMobile.Text;
            string Address = txtAddress.Text;

            //Input Validation
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Mobile) || string.IsNullOrEmpty(Address))
            {
                MessageBox.Show("Please Fill in All Fields");
            }
            //Email Format Validation
            else if (!IsValidEmail(Email))
            {
                MessageBox.Show("Please Enter Valid Email");
            }
            //Mobile Format Validation
            else if (!IsValidMobile(Mobile))
            {
                MessageBox.Show("Mobile must be contain only 9 Digits and without 0 first");
            }
            else
            {
                try
                {
                    MySqlConnection con = Program.GetConnection;
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `customer`(`Name`, `Email`, `Mobile`, `Address`) VALUES ('" + Name + "','" + Email + "','" + Mobile + "','" + Address + "')", con);
                    cmd.ExecuteNonQuery();

                    FilldgvCustomers("");
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtMobile.Text = "";
                    txtAddress.Text = "";
                    MessageBox.Show("New Customer Added Succesfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells[0].Value);
            string Name = txtName.Text;
            string Email = txtEmail.Text;
            string Mobile = txtMobile.Text;
            string Address = txtAddress.Text;

            //Input Validation
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Mobile) || string.IsNullOrEmpty(Address))
            {
                MessageBox.Show("Please Fill in All Fields");
            }
            //Email Format Validation
            else if (!IsValidEmail(Email))
            {
                MessageBox.Show("Please Enter Valid Email");
            }
            //Mobile Format Validation
            else if (!IsValidMobile(Mobile))
            {
                MessageBox.Show("Mobile must be contain only 9 Digits and without 0 first");
            }
            else
            {
                try
                {
                    MySqlConnection con = Program.GetConnection;
                    MySqlCommand cmd = new MySqlCommand("UPDATE `customer` SET `Name`='" + Name + "',`Email`='" + Email + "',`Mobile`='" + Mobile + "',`Address`='" + Address + "' WHERE `ID`=" + id + "", con);
                    cmd.ExecuteNonQuery();

                    FilldgvCustomers("");
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtMobile.Text = "";
                    txtAddress.Text = "";
                    MessageBox.Show("Edited Succesfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }
        
        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells[0].Value);
            try
            {                
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `customer` WHERE `ID`=" + id + "", con);
                cmd.ExecuteNonQuery();

                FilldgvCustomers("");
                txtName.Text = "";
                txtEmail.Text = "";
                txtMobile.Text = "";
                txtAddress.Text = "";
                MessageBox.Show("Removed Succesfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsValidEmail(string Email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(Email);
        }
        private bool IsValidMobile(string Mobile)
        {
            return Regex.Match(Mobile, @"^([0-9]{9})$").Success;
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
            this.Show();
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
