using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Regex removeHTMLtagsRegex = new Regex("<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
            foreach (var translation in languages[0].hits[0].roms[0].arabs[0].translations)
            {
                string query = "INSERT INTO translation (target, sourche, id VALUES (" + translation.target +
                                                        "," + translation.source +
                                                        "," + languageDict.Values.Where(x=> x == languages[0].lang).ToString() +
                                                        "," + languageDict.Keys.Where(x=> x.Equals(languages[1].lang)).ToString() + ");";

                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
                cmd.ExecuteNonQuery();
            }
        }
        public static int GetLanguageId(Dictionary<int, string> languageDict, string lang) {
            foreach (var language in languageDict) {
                if (language.Value.Equals(lang)) {
                    return language.Key;
                }
            }
            return 0;
        }
    }
}
