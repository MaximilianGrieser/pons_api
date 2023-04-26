using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace pons_api {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            string response = apiRequest("cut", "deen");
            List<translation> r = JsonConvert.DeserializeObject<List<translation>>(response);
            TB_resullt.Text = r[1].hits[0].roms[0].headword;
        }

        public static string apiRequest(string termToLookUp, string languageCode) {
            string user = "pi_gmbh";
            string password = "e3fi3"; 

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string endPoint = "https://api.pons.com/v1/dictionary";
            string query = endPoint + "?q=" + termToLookUp + "&l=" + languageCode;         

            WebRequest request = WebRequest.Create(query);
            request.Headers.Add("X-Secret: bf54d04209fe20b0bf59889e8a5560d44617b224a953e4cf9baa70aacd6d7a62");
            request.Credentials = new NetworkCredential(user, password);

            try {
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return responseFromServer;
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
