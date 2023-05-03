using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace pons_api
{
    public static class DBSaveService
    {
        public static void SaveResponseToDB(List<language> languages)
        {
            var DBConnect = DBConnection.OpenConnection();
            string query = "INSERT INTO arab VALUES (" + languages[0].hits[0].roms[0].arabs[0].translations[0].target + 
                                                    "," + languages[0].hits[0].roms[0].arabs[0].translations[0].source + ");";
            MySqlCommand cmd = new MySqlCommand(query, DBConnect);
        }
    }
}
