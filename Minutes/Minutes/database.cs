using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace Minutes
{
    public class database
    {
        private void runQuery()
        {


        }

        private string server;
        private string databasen;
        private string uid;
        private string dbpassword;

        public Boolean Login(string username, string password)
        {
            server = "5.135.216.6";
            databasen = "stale_minutes";
            uid = "stale_stale";
            dbpassword = "Stale2018";

            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=5.135.216.6;uid=stale_stale;" +
                "pwd=Stale2018;database=stale_minutes;sslmode=none";

            conn = new MySql.Data.MySqlClient.MySqlConnection();

            try
            { 
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
               System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand(String.Format("SELECT * FROM users WHERE user_uid='{0}' OR user_email='{0}'", username), conn);
                MySqlDataReader myReader;
                myReader = cmd.ExecuteReader();

                if (myReader.Read())
                {
                    string returnhash = Convert.ToString(myReader[5]).Replace("$2y", "$2a");
                    if (BCrypt.Net.BCrypt.Verify(password, returnhash))
                    {
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        return false;
                    }
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                conn.Close();
                return false;
            }
        }
    }
}
