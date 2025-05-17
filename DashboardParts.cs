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
    public partial class DashboardParts : Form
    {
        public DashboardParts()
        {
            InitializeComponent();
        }

        private void DashboardParts_Load(object sender, EventArgs e)
        {
            btnParts.BackColor = Color.OrangeRed;
            FilldgvParts("");            
        }

        public void FilldgvParts(string Search)
        {
            try
            {
                //populate the datagridview            
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM part WHERE CONCAT(Model, Price, AddDate)LIKE'%" + Search + "%'", con);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvParts.DataSource = dt;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            FilldgvParts(txtSearch.Text);
        }

        private void dgvParts_MouseClick(object sender, MouseEventArgs e)
        {
            Byte[] image = (Byte[])dgvParts.CurrentRow.Cells[0].Value;
            MemoryStream ms = new MemoryStream(image);
            pbImage.Image = Image.FromStream(ms);

            txtModel.Text = dgvParts.CurrentRow.Cells[1].Value.ToString();
            txtPrice.Text = dgvParts.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Choose Image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbImage.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = Program.GetConnection;
                MemoryStream ms = new MemoryStream();
                pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                byte[] image = ms.ToArray();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO abccar.part(Image, Model, Price) VALUES(@image, @model, @price)", con);
                cmd.Parameters.Add("@image", MySqlDbType.Blob);
                cmd.Parameters.Add("@model", MySqlDbType.VarChar, 100);
                cmd.Parameters.Add("@price", MySqlDbType.Int64, 20);

                cmd.Parameters["@image"].Value = image;
                cmd.Parameters["@model"].Value = txtModel.Text;
                cmd.Parameters["@price"].Value = txtPrice.Text;

                if (cmd.ExecuteNonQuery() == 1)
                {
                    FilldgvParts("");
                    txtModel.Text = "";
                    txtPrice.Text = "";
                    MessageBox.Show("New Part Added Succesfully");                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Already Added Part. Please Add New Part.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string model1 = dgvParts.CurrentRow.Cells[1].Value.ToString();
                MySqlConnection con = Program.GetConnection;
                MemoryStream ms = new MemoryStream();
                pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                byte[] image = ms.ToArray();

                MySqlCommand cmd = new MySqlCommand("UPDATE `part` SET `Image`=@image,`Model`=@model,`Price`=@price WHERE `Model`='" + model1 + "'", con);
                cmd.Parameters.Add("@image", MySqlDbType.Blob);
                cmd.Parameters.Add("@model", MySqlDbType.VarChar, 100);
                cmd.Parameters.Add("@price", MySqlDbType.Int64, 20);

                cmd.Parameters["@image"].Value = image;
                cmd.Parameters["@model"].Value = txtModel.Text;
                cmd.Parameters["@price"].Value = txtPrice.Text;

                if (cmd.ExecuteNonQuery() == 1)
                {
                    FilldgvParts("");
                    txtModel.Text = "";
                    txtPrice.Text = "";
                    MessageBox.Show("Edited Succesfully");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Already Added Part. Please Use Diffrent Model Name to Edit.");
            }
        }
        
        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            try
            {
                string model = dgvParts.CurrentRow.Cells[1].Value.ToString();
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `part` WHERE `Model`='" + model + "'", con);
                cmd.ExecuteNonQuery();

                FilldgvParts("");
                txtModel.Text = "";
                txtPrice.Text = "";                
                MessageBox.Show("Removed Succesfully");
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
            this.Show();
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
