using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace pons_api
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, string> languages;
        public MainWindow()
        {
            InitializeComponent();
            languages = DBLoadService.GetAllLanguages();
            CB_sourceLang.ItemsSource = languages.Values;
            CB_targetLang.ItemsSource = languages.Values;
            CB_vocLanguage.ItemsSource = languages.Values;
            CB_vocLanguage.SelectedIndex = 2;
            CB_sourceLang.SelectedIndex = 0;
            CB_targetLang.SelectedIndex = 2;
        }

        public static string apiRequest(string termToLookUp, string languageCode)
        {
            string user = "pi_gmbh";
            string password = "e3fi3";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string endPoint = "https://api.pons.com/v1/dictionary";
            string query = endPoint + "?q=" + termToLookUp + "&l=" + languageCode;

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
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private void BTN_translate_Click(object sender, RoutedEventArgs e)
        {
            if (DBLoadService.getTranslation(TB_input.Text) != null)
            {
                TB_resullt.Text = DBLoadService.getTranslation(TB_input.Text)[0];
            }
            else
            {
                TB_resullt.Text = getTranslationFromAPI(TB_input.Text);
            }
        }

        private string getTranslationFromAPI(string sword)
        {
            try
            {

                string response = apiRequest(sword, "deen");
                List<language> r = JsonConvert.DeserializeObject<List<language>>(response);

                DBSaveService.SaveResponseToDB(r);

                Regex removeHTMLtagsRegex = new Regex("<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>");
                return removeHTMLtagsRegex.Replace(r[0].hits[0].roms[0].arabs[0].translations[0].target, "");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private void BTN_vocTrainer_Click(object sender, RoutedEventArgs e)
        {
            if (TB_vocQuestion.Text != String.Empty)
            {
                if (DBLoadService.getTranslation(TB_vocQuestion.Text).Contains(TB_vocInput.Text))
                {
                    MessageBox.Show("Your answer was correct", "Correct", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Your answer was wrong", "Wrong", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("The correct answers are: " + DBLoadService.getTranslation(TB_vocQuestion.Text), "Answer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            var vocs = DBLoadService.getAllTranslations();

            Random rnd = new Random();
            int dice = rnd.Next(0, vocs.Count);

            TB_vocQuestion.Text = vocs[dice];
        }
    }
}
