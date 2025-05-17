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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Inputs
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;

            //Input Validation
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please Fill in Both Fields");
            }
            //Admin Validation
            else if (Username == "admin" && Password == "admin123")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                try
                {
                    //Search Customer From Customer Table
                    MySqlConnection con = Program.GetConnection;
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM `customer` WHERE `Username`='" + Username + "' && `Password`='" + Password + "'", con);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    
                    if(adp.Fill(dt) == 1)
                    {
                        txtUsername.Text = dt.Rows[0][1].ToString();
                        txtPassword.Text = dt.Rows[0][2].ToString();
                        string id = dt.Rows[0][0].ToString();
                        int ID = Convert.ToInt32(id);
                        string Name = dt.Rows[0][3].ToString();

                        if (Username == txtUsername.Text && Password == txtPassword.Text)
                        {
                            Program.Name = Name;
                            Program.ID = ID;
                            Home home = new Home();
                            home.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username and or Password");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username and or Password");
                    }                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void lnklblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private void lnklblForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forgot forgot = new Forgot();
            forgot.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
