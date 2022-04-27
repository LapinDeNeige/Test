using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Test_1
{
    //Data Base Handler

    public class DBClient
    {
        public List<dbContacts> dbContacts;
        private SqlConnection cn;

        private string sqlConnectionString = "Data Source=DESKTOP-47QNAB5\\SQLEXPRESS;" +
               "Initial Catalog=UsersDB;" +
               "Integrated Security=SSPI;" +
               "Encrypt=false;";                  //Строка подключения к базе данных

        private dbContacts contacts;

        public DBClient()
        {
            dbContacts = new List<dbContacts>();
            contacts = new dbContacts();
        }
        public void updateClient(string name, string surname, string fathersname, string phone)
        {
            contacts.updateContacts(name, surname, fathersname, phone);
        }
        private bool sqlConnect()
        {
            cn = new SqlConnection();
            cn.ConnectionString = sqlConnectionString;
            try
            {
                if (cn.State != System.Data.ConnectionState.Open)
                    cn.Open();
            }
            catch
            {
                //Console.WriteLine("Error Database");
                return false;
            }

            //Console.WriteLine("Database connetion succesfully");
            return true;

        }

        private void sqlClose()
        {
            if (cn.State == System.Data.ConnectionState.Open)
                cn.Close();
        }

        private bool readDatabase()
        {
            if (cn.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    dbContacts.Clear();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tbl ORDER BY ID", cn);
                    cmd.ExecuteNonQuery();

                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        
                        string nm = rd.GetString(0);
                        string srnm = rd.GetString(1);
                        string fthrnm = rd.GetString(2);
                        string phn = rd.GetString(3);
                        string id = rd.GetInt32(4).ToString();
                        dbContacts.Add(new dbContacts() { Name = nm, Surname = srnm, Fathersname = fthrnm, Phone = phn,Id=id });
                    }
                }
                catch
                {
                    return false;
                    // Console.WriteLine("Failed to execute sql command");
                }

                return true;
            }

            return false;

        }
        public bool readSql()
        {
            if (!sqlConnect())
                return false;
            if (!readDatabase())
            {
                sqlClose();
                return false;
            }
            sqlClose();
            return true;
        }

        private bool deleteById(string id)
        {
            string sqlCmd = "DELETE FROM tbl WHERE ID IN (" + id +")";
            SqlCommand cmd = new SqlCommand(sqlCmd);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool editById(string id )
        {
            string sqlCmd = "DELETE FROM tbl WHERE ID IN (" + id + ")";
            SqlCommand cmd = new SqlCommand(sqlCmd);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool addToDatabase()
        {

            if (cn.State == System.Data.ConnectionState.Open)
            {
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("INSERT INTO tbl VALUES (@name,@surname,@fathersname,@phone )", cn);
                    //cmd.Parameters.Add(new SqlParameter("ID", 3));
                    cmd.Parameters.Add(new SqlParameter("name", contacts.Name));
                    cmd.Parameters.Add(new SqlParameter("surname", contacts.Surname));
                    cmd.Parameters.Add(new SqlParameter("fathersname", contacts.Fathersname));
                    cmd.Parameters.Add(new SqlParameter("phone", contacts.Phone));

                    cmd.ExecuteNonQuery();

                }

                
                catch
                {
                    return false;
                    // Console.WriteLine("Failed to execute command");
                }
               
                return true;
            }
            return false;
        }

        public bool  sqlDelete(string id)
        {
            if (!sqlConnect())
                return false;
            deleteById(id);
            sqlClose();

            return true;
        }
        public bool sqlAdd()
        {
            if (!sqlConnect())
                return false;
            if (!addToDatabase())
            {
                sqlClose();
                return false;
            }
            sqlClose();
            return true;
        }
    }
}
