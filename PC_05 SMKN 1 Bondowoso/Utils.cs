using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PC_05_SMKN_1_Bondowoso
{
    class Utils
    {
        public static string conn = @"Data Source=desktop-00eposj;Initial Catalog=PC_05;Integrated Security=True";
    }

    class Session
    {
        public static int id { set; get; }
        public static string username { set; get; }
        public static int level { set; get; }
    }

    class Logout
    {
        public static void logout()
        {
            Session.id = 0;
            Session.username = "";
            Session.level = 0;
        }
    }

    class Command
    {
        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand(com, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static DataTable getdata(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        
    }
    
    class Selected
    {
        public static string nik { set; get; }
    }


}
