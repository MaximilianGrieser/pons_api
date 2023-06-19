using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace pons_api
{
    static public class DBLoadService
    {
        public static List<string> getTranslation(string text)
        {
            try
            {
                List<string> results = new List<string>();
                var DBConnect = DBConnection.OpenConnection();
                string query = "SELECT target FROM translation WHERE source = " + text;
                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    results.Add(dataReader["target"].ToString());
                }
                return results;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static Dictionary<int, string> GetAllLanguages()
        {
            Dictionary<int, string> results = new Dictionary<int, string>();
            var DBConnect = DBConnection.OpenConnection();
            string query = "SELECT * FROM lang";
            MySqlCommand cmd = new MySqlCommand(query, DBConnect);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                results.Add(Int32.Parse(dataReader["id"].ToString()), dataReader["description"].ToString());
            }
            return results;
        }

        public static List<string> getAllTranslations()
        {
            try
            {
                List<string> results = new List<string>();
                var DBConnect = DBConnection.OpenConnection();
                string query = "SELECT target FROM translation";
                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    results.Add(dataReader["target"].ToString());
                }
                return results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
