using System;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
namespace Minutes
{
    public class database
    {
        private string server;
        private string databasen;
        private string uid;
        private string dbpassword;
        MySqlConnection conn;
        string myConnectionString;


        public Boolean Login(string username, string password)
        {
            server = "localhost";
            databasen = "minutes";
            uid = "root";
            dbpassword = "";
            myConnectionString = String.Format("server={0};uid={1};pwd={2};database={3};sslmode=none",server,uid,dbpassword,databasen);
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

        public void Update(string pinfo, string useruid, string operation)
        {
            server = "localhost";
            databasen = "minutes";
            uid = "root";
            dbpassword = "";
            myConnectionString = String.Format("server={0};uid={1};pwd={2};database={3};sslmode=none", server, uid, dbpassword, databasen);
            conn = new MySql.Data.MySqlClient.MySqlConnection();

            if (conn is object)
            {
                conn.Close();
            }

            try
            {
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            string programinfo = "";

            if (operation == "add")
            {
               programinfo = String.Format("{0}{1}{2}", pinfo, Environment.NewLine, Read(useruid));
            }
            else if (operation == "remove")
            {
                programinfo = Read(useruid).Replace(pinfo, "");
                programinfo = Regex.Replace(programinfo, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            }

            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE users SET usr_programs = @programinfo WHERE user_uid= @useruid"), conn);
            cmd.Parameters.AddWithValue("@programinfo", programinfo);
            cmd.Parameters.AddWithValue("useruid", useruid);
            cmd.ExecuteNonQuery();
        }

        public string Read(string useruid)
        {
            server = "localhost";
            databasen = "minutes";
            uid = "root";
            dbpassword = "";
            myConnectionString = String.Format("server={0};uid={1};pwd={2};database={3};sslmode=none", server, uid, dbpassword, databasen);
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

            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT usr_programs FROM users WHERE user_uid='{0}' OR user_email='{0}'", useruid), conn);
            MySqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            string data = "";
            while (myReader.Read())
            {  // <<- here
                data = (myReader["usr_programs"].ToString());
            }  // <<- here
            myReader.Close();
            return data;
        }

        public void RemoveProgram(string pinfo)
        {
            Update(pinfo, "JohnDoe", "remove");
        }

        public void AddProgram(string pinfo)
        {
            Update(pinfo, "JohnDoe", "add");
        }
    }
}
