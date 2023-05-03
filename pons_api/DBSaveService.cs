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
            Dictionary<int, string> languageDict = DBLoadService.GetAllLanguages();
            foreach (var translation in languages[0].hits[0].roms[0].arabs[0].translations)
            {

                string query = "INSERT INTO translation VALUES (" + translation.target +
                                                        "," + translation.source +
                                                        "," + languageDict.Where(x=> x.Value == languages[0].lang).ToString() +
                                                        "," + languageDict.Where(x=> x.Value == languages[1].lang).ToString() + ");";

                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
            }
        }
    }
}
