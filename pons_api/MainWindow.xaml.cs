using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace pons_api
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        hit selectedHit = new hit();
        rom selectedRom = new rom();
        arab selectedArab = new arab();
        translation selectedTranslation = new translation();
        int score = 0;
        Dictionary<int, string> languages;
        public MainWindow()
        {
            InitializeComponent();
            languages = DBLoadService.GetAllLanguages();
            CB_sourceLang.ItemsSource = languages.Values;
            CB_targetLang.ItemsSource = languages.Values;
            CB_vocLanguage.ItemsSource = languages.Values;
            CB_vocLanguageTarget.ItemsSource = languages.Values;
            CB_vocLanguage.SelectedIndex = 2;
            CB_sourceLang.SelectedIndex = 0;
            CB_targetLang.SelectedIndex = 2;
            CB_vocLanguageTarget.SelectedIndex = 0;
            TB_score.Text = score.ToString();
        }

        public static string apiRequest(string termToLookUp, string languageCode, string sourceLang)
        {
            string user = "pi_gmbh";
            string password = "e3fi3";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string endPoint = "https://api.pons.com/v1/dictionary";
            string query = endPoint + "?q=" + termToLookUp + "&l=" + languageCode + "&in=" + sourceLang;

            WebRequest request = WebRequest.Create(query);
            request.Headers.Add("X-Secret: bf54d04209fe20b0bf59889e8a5560d44617b224a953e4cf9baa70aacd6d7a62");
            request.Credentials = new NetworkCredential(user, password);

            try
            {
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void BTN_translate_Click(object sender, RoutedEventArgs e)
        {
            if (DBLoadService.GetTranslation(TB_input.Text).Count > 0)
            {
                TB_resullt.Text = DBLoadService.GetTranslation(TB_input.Text)[0];
            }
            else
            {
                TB_resullt.Text = getTranslationFromAPI(TB_input.Text);
            }
        }

        private string getTranslationFromAPI(string sword)
        {
            string response = apiRequest(sword, CB_sourceLang.Text + CB_targetLang.Text, CB_sourceLang.Text);
            if (response == null)
            {
                response = apiRequest(sword, CB_targetLang.Text + CB_sourceLang.Text, CB_sourceLang.Text);
            }
            try
            {
                List<language> r = JsonConvert.DeserializeObject<List<language>>(response);
                

                DBSaveService.SaveResponseToDB(r, CB_targetLang.Text);
                Regex removeHTMLtagsRegex = new Regex("<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
                int index = 0;
                foreach (translation translation in r[0].hits[0].roms[0].arabs[0].translations) {
                    r[0].hits[0].roms[0].arabs[0].translations[index].source = removeHTMLtagsRegex.Replace(translation.target, "");
                    index++;
                }

                LB_translations.ItemsSource = r[0].hits[0].roms[0].arabs[0].translations;

                return r[0].hits[0].roms[0].arabs[0].translations[0].target;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        private void BTN_vocTrainer_Click(object sender, RoutedEventArgs e)
        {
            if (TB_vocQuestion.Text != String.Empty)
            {
                if (DBLoadService.getVocTranslation(TB_vocQuestion.Text).Exists(x => x.Equals(TB_vocInput.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    score += 100;
                    MessageBox.Show("Your answer was correct", "Correct", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Your answer was wrong Press Yes to see Correct Answer", "Wrong", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    score -= 2;
                    if (result == MessageBoxResult.Yes)
                    {
                        string answer = DBLoadService.getVocTranslation(TB_vocQuestion.Text)[0];
                        MessageBox.Show("The correct answer is: " + answer, "Answer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            TB_score.Text = score.ToString();
            TB_vocInput.Text = "";

            var vocs = DBLoadService.getAllTranslations();

            Random rnd = new Random();
            int dice = rnd.Next(0, vocs.Count);

            TB_vocQuestion.Text = vocs[dice];
        }

        private void LB_translations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (selectedTranslation == null) {
                TB_source.Text = "";
            } else if (LB_translations.SelectedItem != null) {
                selectedTranslation = (translation)LB_translations.SelectedItem;
                TB_source.Text = selectedTranslation.source;
            }
        }
    }
}
