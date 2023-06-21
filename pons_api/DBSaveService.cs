using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace pons_api
{
    public static class DBSaveService
    {
        public static void SaveResponseToDB(List<language> languages, string targetLanguage)
        {
            var DBConnect = DBConnection.OpenConnection();
            Dictionary<int, string> languageDict = DBLoadService.GetAllLanguages();
            Regex removeHTMLtagsRegex = new Regex("<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
            foreach (var translation in languages[0].hits[0].roms[0].arabs[0].translations)
            {
                string query = "INSERT INTO translation (target, source, sourceLanguageId, targetLanguageId) VALUES ('" + removeHTMLtagsRegex.Replace(translation.target, "") +
                                                        "','" + removeHTMLtagsRegex.Replace(translation.source, "") +
                                                        "'," + DBLoadService.GetLanguageId(languageDict, languages[0].lang).ToString() +
                                                        "," + DBLoadService.GetLanguageId(languageDict, targetLanguage).ToString() + ")";

                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
                cmd.ExecuteNonQuery();
            }
        }
       
    }
}
