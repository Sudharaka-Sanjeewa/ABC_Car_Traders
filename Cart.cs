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
    public partial class Cart : Form
    {
        public Cart()
        {
            InitializeComponent();
        }

        private void Cart_Load(object sender, EventArgs e)
        {
            this.lnklblName.Text = "Welcome, " + Program.Name;            
            FilldgvCart("");
            pnlPurchase.Hide();
        }

        public void FilldgvCart(string Search)
        {
            try
            {
                //populate the datagridview
                int userid = Program.ID;
                MySqlConnection con = Program.GetConnection;
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM cart WHERE `UserID`=" + userid + " && CONCAT(Model, Price, Quantity, Total)LIKE'%" + Search + "%'", con);
                DataTable tb = new DataTable();
                adp.Fill(tb);
                dgvCart.DataSource = tb;

                DataGridViewTextBoxColumn ID = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn UserID = new DataGridViewTextBoxColumn();
                ID = (DataGridViewTextBoxColumn)dgvCart.Columns[0];
                UserID = (DataGridViewTextBoxColumn)dgvCart.Columns[6];
                ID.Visible = false;
                UserID.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCart_MouseClick(object sender, MouseEventArgs e)
        {
            Byte[] image = (Byte[])dgvCart.CurrentRow.Cells[1].Value;
            MemoryStream ms = new MemoryStream(image);
            pbImage.Image = Image.FromStream(ms);

            lblModel.Text = dgvCart.CurrentRow.Cells[2].Value.ToString();
            lblPrice.Text = dgvCart.CurrentRow.Cells[3].Value.ToString();
            lblQuantity.Text = dgvCart.CurrentRow.Cells[4].Value.ToString();
            lblTotal.Text = dgvCart.CurrentRow.Cells[5].Value.ToString();

            pnlPurchase.Show();            
            int qty = Convert.ToInt32(lblQuantity.Text);
            if (qty == 1)
            {
                btnM.Hide();
            }
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

            try
            {
                int userid = Program.ID;
                string model = lblModel.Text;                
                int quantity = Convert.ToInt32(lblQuantity.Text);
                int totals = Convert.ToInt32(lblTotal.Text);

                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `cart` SET `Quantity`=" + quantity + ",`Total`=" + totals + " WHERE `Model`='" + model + "' && `UserID`='" + userid + "'", con);
                cmd.ExecuteNonQuery();
                FilldgvCart("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            try
            {
                int userid = Program.ID;
                string model = lblModel.Text;
                int quantity = Convert.ToInt32(lblQuantity.Text);
                int totals = Convert.ToInt32(lblTotal.Text);

                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `cart` SET `Quantity`=" + quantity + ",`Total`=" + totals + " WHERE `Model`='" + model + "' && `UserID`='" + userid + "'", con);
                cmd.ExecuteNonQuery();
                FilldgvCart("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                FilldgvCart("");
            }
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = Program.GetConnection;
                MemoryStream ms = new MemoryStream();
                pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                byte[] image = ms.ToArray();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO abccar.orders(Image, Model, Price, Quantity, Total, UserID) VALUES(@image, @model, @price, @quantity, @total, @userid)", con);
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
                    try
                    {
                        int userid = Program.ID;
                        string model = lblModel.Text;

                        MySqlCommand cmd1 = new MySqlCommand("DELETE FROM `cart` WHERE `Model`='" + model + "' && `UserID`='" + userid + "'", con);
                        cmd1.ExecuteNonQuery();

                        pnlPurchase.Hide();
                        FilldgvCart("");
                        MessageBox.Show("Order Placed Succesfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Order Place Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int userid = Program.ID;
                string model = lblModel.Text;

                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `cart` WHERE `Model`='" + model + "' && `UserID`='" + userid + "'", con);
                cmd.ExecuteNonQuery();

                pnlPurchase.Hide();
                FilldgvCart("");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilldgvCart(txtSearch.Text);
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
