using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Documents;

namespace pons_api
{
    static public class DBLoadService
    {
        public static List<string> getTransaltion(string text)
        {
            List<string> results = new List<string>();
            var DBConnect = DBConnection.OpenConnection();
            string query = "SELECT target FROM arab WHERE source = " + text;
            MySqlCommand cmd = new MySqlCommand(query, DBConnect);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                results.Add(dataReader["target"].ToString());
            }
            return results;
        }
    }
}
