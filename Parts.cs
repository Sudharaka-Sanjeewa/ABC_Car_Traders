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
    public partial class Parts : Form
    {
        public Parts()
        {
            InitializeComponent();
        }

        private void Parts_Load(object sender, EventArgs e)
        {
            this.lnklblName.Text = "Welcome, " + Program.Name;
            FilldgvParts("");
            pnlAdd.Hide();
        }

        public void FilldgvParts(string Search)
        {
            try
            {
                //populate the datagridview
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM part WHERE CONCAT(Model, Price)LIKE'%" + Search + "%'", con);
                DataTable tb = new DataTable();
                adp.Fill(tb);
                dgvParts.DataSource = tb;

                DataGridViewTextBoxColumn AddDate = new DataGridViewTextBoxColumn();
                AddDate = (DataGridViewTextBoxColumn)dgvParts.Columns[3];
                AddDate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvParts_MouseClick(object sender, MouseEventArgs e)
        {
            Byte[] image = (Byte[])dgvParts.CurrentRow.Cells[0].Value;
            MemoryStream ms = new MemoryStream(image);
            pbImage.Image = Image.FromStream(ms);

            lblModel.Text = dgvParts.CurrentRow.Cells[1].Value.ToString();
            lblPrice.Text = dgvParts.CurrentRow.Cells[2].Value.ToString();

            pnlAdd.Show();
            btnM.Hide();
            int qty = 1;
            lblQuantity.Text = qty.ToString();

            int price = Convert.ToInt32(lblPrice.Text);
            int count = Convert.ToInt32(lblQuantity.Text);
            int total = price * count;
            lblTotal.Text = total.ToString();
        }

        private void btnP_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(lblQuantity.Text);
            int price = Convert.ToInt32(lblPrice.Text);

            int newcount = count + 1;
            int total = price * newcount;

            lblQuantity.Text = newcount.ToString();
            lblTotal.Text = total.ToString();

            if (newcount < 5)
            {
                btnP.Show();
                btnM.Show();
            }
            else
            {
                btnP.Hide();
            }
        }

        private void btnM_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(lblQuantity.Text);
            int price = Convert.ToInt32(lblPrice.Text);

            int newcount = count - 1;
            int total = price * newcount;

            lblQuantity.Text = newcount.ToString();
            lblTotal.Text = total.ToString();

            if (newcount > 1)
            {
                btnM.Show();
                btnP.Show();
            }
            else
            {
                btnM.Hide();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int userid = Program.ID;
                string model = lblModel.Text;

                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM `cart` WHERE `Model`='" + model + "' && `UserID`='" + userid + "'", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd1);
                DataTable dt = new DataTable();

                if (adp.Fill(dt) > 0)
                {
                    MessageBox.Show("Product Already in the Cart");
                    FilldgvParts("");
                }
                else
                {
                    try
                    {                        
                        MemoryStream ms = new MemoryStream();
                        pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                        byte[] image = ms.ToArray();

                        MySqlCommand cmd = new MySqlCommand("INSERT INTO abccar.cart(Image, Model, Price, Quantity, Total, UserID) VALUES(@image, @model, @price, @quantity, @total, @userid)", con);
                        cmd.Parameters.Add("@image", MySqlDbType.Blob);
                        cmd.Parameters.Add("@model", MySqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@price", MySqlDbType.Int64, 20);
                        cmd.Parameters.Add("@quantity", MySqlDbType.Int32, 10);
                        cmd.Parameters.Add("@total", MySqlDbType.Int64, 20);
                        cmd.Parameters.Add("@userid", MySqlDbType.Int32, 10);

                        cmd.Parameters["@image"].Value = image;
                        cmd.Parameters["@model"].Value = lblModel.Text;
                        cmd.Parameters["@price"].Value = lblPrice.Text;
                        cmd.Parameters["@quantity"].Value = lblQuantity.Text;
                        cmd.Parameters["@total"].Value = lblTotal.Text;
                        cmd.Parameters["@userid"].Value = Program.ID;

                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Product Added to Cart Succesfully");
                            FilldgvParts("");
                        }
                        else
                        {
                            MessageBox.Show("Add to Cart Error");
                            FilldgvParts("");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        FilldgvParts("");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvParts("");
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilldgvParts(txtSearch.Text);
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

        private void btnOrders_Click(object sender, EventArgs e)
        {
            Orders order = new Orders();
            order.Show();
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
