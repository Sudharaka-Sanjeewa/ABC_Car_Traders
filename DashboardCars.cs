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
    public partial class DashboardCars : Form
    {
        public DashboardCars()
        {
            InitializeComponent();
        }

        private void DashboardCars_Load(object sender, EventArgs e)
        {
            btnCars.BackColor = Color.OrangeRed;
            FilldgvCars("");
        }

        public void FilldgvCars(string Search)
        {
            try
            {
                //populate the datagridview            
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM car WHERE CONCAT(Model, Price, AddDate)LIKE'%" + Search + "%'", con);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvCars.DataSource = dt;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            FilldgvCars(txtSearch.Text);
        }

        private void dgvCars_MouseClick(object sender, MouseEventArgs e)
        {
            Byte[] image = (Byte[])dgvCars.CurrentRow.Cells[0].Value;
            MemoryStream ms = new MemoryStream(image);
            pbImage.Image = Image.FromStream(ms);

            txtModel.Text = dgvCars.CurrentRow.Cells[1].Value.ToString();
            txtPrice.Text = dgvCars.CurrentRow.Cells[2].Value.ToString();
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

                MySqlCommand cmd = new MySqlCommand("INSERT INTO abccar.car(Image, Model, Price) VALUES(@image, @model, @price)", con);
                cmd.Parameters.Add("@image", MySqlDbType.Blob);
                cmd.Parameters.Add("@model", MySqlDbType.VarChar, 100);
                cmd.Parameters.Add("@price", MySqlDbType.Int64, 20);

                cmd.Parameters["@image"].Value = image;
                cmd.Parameters["@model"].Value = txtModel.Text;
                cmd.Parameters["@price"].Value = txtPrice.Text;

                if (cmd.ExecuteNonQuery() == 1)
                {
                    FilldgvCars("");
                    txtModel.Text = "";
                    txtPrice.Text = "";
                    MessageBox.Show("New Car Added Succesfully");                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Already Added Model. Please Add New Car.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string model1 = dgvCars.CurrentRow.Cells[1].Value.ToString();
                MySqlConnection con = Program.GetConnection;
                MemoryStream ms = new MemoryStream();
                pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                byte[] image = ms.ToArray();

                MySqlCommand cmd = new MySqlCommand("UPDATE `car` SET `Image`=@image,`Model`=@model,`Price`=@price WHERE `Model`='" + model1 + "'", con);
                cmd.Parameters.Add("@image", MySqlDbType.Blob);
                cmd.Parameters.Add("@model", MySqlDbType.VarChar, 100);
                cmd.Parameters.Add("@price", MySqlDbType.Int64, 20);

                cmd.Parameters["@image"].Value = image;
                cmd.Parameters["@model"].Value = txtModel.Text;
                cmd.Parameters["@price"].Value = txtPrice.Text;

                if (cmd.ExecuteNonQuery() == 1)
                {
                    FilldgvCars("");
                    txtModel.Text = "";
                    txtPrice.Text = "";
                    MessageBox.Show("Edited Succesfully");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Already Added Model. Please Use Diffrent Model Name to Edit.");
            }
        }
        
        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            try
            {
                string model = dgvCars.CurrentRow.Cells[1].Value.ToString();
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `car` WHERE `Model`='" + model + "'", con);
                cmd.ExecuteNonQuery();

                FilldgvCars("");
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
            this.Show();
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
