﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public static List<string> GetTranslation(string source)
        {
            try
            {
                List<string> results = new List<string>();
                var DBConnect = DBConnection.OpenConnection();
                string query = "SELECT target FROM translation WHERE source = '" + source + "'";
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
                File.AppendAllText("pons.log", ex.Message);
                return null;
            }

        }
        public static List<string> GetVocTranslation(string target, string targetLanguage)
        {
            try
            {
                List<string> results = new List<string>();
                var DBConnect = DBConnection.OpenConnection();
                Dictionary<int, string> languageDict = GetAllLanguages();
                string query = "SELECT source " +
                               "FROM translation " +
                               "WHERE target = '" + target + "' " +
                               "AND sourceLanguageId =" + GetLanguageId(languageDict,targetLanguage);
                MySqlCommand cmd = new MySqlCommand(query, DBConnect);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    results.Add(dataReader["source"].ToString());
                }
                return results;
            }
            catch (Exception ex)
            {
                File.AppendAllText("pons.log", ex.Message);
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

        public static List<string> GetAllTranslations(string targetLanguage, string sourceLanguage)
        {
            try
            {
                List<string> results = new List<string>();
                var DBConnect = DBConnection.OpenConnection();
                Dictionary<int, string> languageDict = GetAllLanguages();
                string query = "SELECT target " +
                               "FROM translation " +
                               "WHERE targetLanguageId =" + GetLanguageId(languageDict, targetLanguage) +
                               " AND sourceLanguageId =" + GetLanguageId(languageDict,sourceLanguage);
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
                File.AppendAllText("pons.log", ex.Message);
                return new List<string>();
            }
        }
        public static int GetLanguageId(Dictionary<int, string> languageDict, string lang)
        {
            foreach (var language in languageDict)
            {
                if (language.Value.Equals(lang))
                {
                    return language.Key;
                }
            }
            return 0;
        }
    }
}
