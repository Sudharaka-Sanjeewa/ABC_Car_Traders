using System;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ABC_Car_Traders
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }

        public static MySqlConnection GetConnection
        {
            get
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["abccarConnection"].ConnectionString;
                MySqlConnection con = new MySqlConnection(ConnectionString);
                con.Open();
                return con;
            }
        }

        public static string Name { get; set; }
        public static int ID { get; set; }
    }
}





