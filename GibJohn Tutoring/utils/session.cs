using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GibJohn_Tutoring
{
    internal class session
    {
        private static string connStr = "server=ND-COMPSCI;" +
                 "user=tl_sly;" +
                 "database=tl_sly_gibjohn;" +
                 "port=3306;" +
                 "password=";
        private static MySqlConnection conn;
        public static void connect()
        {
            conn = new MySqlConnection(connStr + passwords.getPass());
        }
        public static MySqlConnection getConnection()
        {
            return conn;
        }
        public static void open()
        {
            conn.Open();
        }
        public static void close()
        {
            conn.Close();
        }
    }
}
