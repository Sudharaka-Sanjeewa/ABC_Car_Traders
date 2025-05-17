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
    public partial class Forgot : Form
    {
        public Forgot()
        {
            InitializeComponent();
        }        

        private void btnReset_Click(object sender, EventArgs e)
        {
            //Inputs
            string Email = txtEmail.Text;
            string Mobile = txtMobile.Text;            
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;
            string CPassword = txtCPassword.Text;

            //Input Validation
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Mobile) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(CPassword))
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
                try
                {
                    //Search for Availability of Duplicate Accounts in Customer Table
                    MySqlConnection con = Program.GetConnection;
                    MySqlCommand validation_cmd = new MySqlCommand("SELECT * FROM `customer` WHERE `Email`='" + Email + "' && `Mobile`='" + Mobile + "'", con);
                    MySqlDataAdapter adp = new MySqlDataAdapter(validation_cmd);
                    DataTable dt = new DataTable();

                    if (adp.Fill(dt) > 1)
                    {
                        MessageBox.Show("There are Duplicate Accounts contains same Email & Mobile. So, You can't Reset Username or Password right now. Please Contact Our Help Center. Thank You!");
                    }
                    else
                    {
                        //Update Username and Password to Customer Table
                        MySqlCommand cmd = new MySqlCommand("UPDATE `customer` SET `Username`='" + Username + "',`Password`='" + Password + "' WHERE `Email`='" + Email + "' && `Mobile`='" + Mobile + "'", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Username and Password Reseted Successfully. Please Login Now!");

                        Login login = new Login();
                        login.Show();
                        this.Hide();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Username Already Used.");
                }
            }
        }

        //Email Format
        private bool IsValidEmail(string Email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(Email);
        }

        //Mobile Format
        private bool IsValidMobile(string Mobile)
        {
            return Regex.Match(Mobile, @"^([0-9]{9})$").Success;
        }

        //Password Format
        private bool IsValidPassword(string Password)
        {
            if (Password.Length < 8) return false;
            if (!Password.Any(Char.IsUpper)) return false;
            if (!Password.Any(Char.IsLower)) return false;
            if (!Password.Any(Char.IsDigit)) return false;            

            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
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
