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
using System.Text.RegularExpressions;

namespace ABC_Car_Traders
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            this.lnklblName.Text = "Welcome, " + Program.Name;
            int id = Program.ID;
            try
            {
                //Search Loged Customer Profile Details from Customer Table
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `customer` WHERE `ID`=" + id + "", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                txtName.Text = dt.Rows[0][3].ToString();
                txtEmail.Text = dt.Rows[0][4].ToString();
                txtMobile.Text = dt.Rows[0][5].ToString();
                txtAddress.Text = dt.Rows[0][6].ToString();
                txtPassword.Text = dt.Rows[0][2].ToString();
                txtCPassword.Text = dt.Rows[0][2].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Email = txtEmail.Text;
            string Mobile = txtMobile.Text;
            string Address = txtAddress.Text;            
            string Password = txtPassword.Text;
            string CPassword = txtCPassword.Text;

            //Input Validation
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Mobile) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(CPassword))
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
            //Password Format Validation(length and Strong)
            else if (!IsValidPassword(Password))
            {
                MessageBox.Show("Password must be at least 8 Characters Long and contain at least 1 Uppercase Letter, 1 lowercase Letter & 1 Number");
            }
            //Is Match Password & Confirm Password
            else if (Password != CPassword)
            {
                MessageBox.Show("Passwords not Matched");
            }
            else
            {
                //Update Customer Details to Customer Table
                int id = Program.ID;
                MySqlConnection con = Program.GetConnection;
                MySqlCommand cmd = new MySqlCommand("UPDATE `customer` SET `Password`='" + Password + "',`Name`='" + Name + "',`Email`='" + Email + "',`Mobile`='" + Mobile + "',`Address`='" + Address + "' WHERE `id`=" + id + "", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Edited Successfully.");                                
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

        private bool IsValidPassword(string Password)
        {
            if (Password.Length < 8) return false;
            if (!Password.Any(Char.IsUpper)) return false;
            if (!Password.Any(Char.IsLower)) return false;
            if (!Password.Any(Char.IsDigit)) return false;            

            return true;
        }

        private void lnklblName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Show();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
