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
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            this.lnklblName.Text = "Welcome, " + Program.Name;
            FilldgvOrder("");
        }

        public void FilldgvOrder(string Search)
        {
            try
            {
                //populate the datagridview
                int userid = Program.ID;
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM orders WHERE `UserID`=" + userid + " && CONCAT(Model, Price, Quantity, Total)LIKE'%" + Search + "%'", con);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvOrder.DataSource = dt;

                DataGridViewTextBoxColumn ID = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn UserID = new DataGridViewTextBoxColumn();
                ID = (DataGridViewTextBoxColumn)dgvOrder.Columns[0];
                UserID = (DataGridViewTextBoxColumn)dgvOrder.Columns[8];
                ID.Visible = false;
                UserID.Visible = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }        

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilldgvOrder(txtSearch.Text);
        }

        private void lnklblName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            cart.Show();
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
