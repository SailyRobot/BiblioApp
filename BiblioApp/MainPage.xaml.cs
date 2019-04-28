using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using BiblioApp.Model;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using static Newtonsoft.Json.JsonConvert;

namespace BiblioApp
{

    public partial class MainPage : ContentPage
    {
        // List<Livre> livres;

        private ObservableCollection<Livre> Livres { get; set; } = new ObservableCollection<Livre>();
        string FichierLivre = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Livres.txt");

        public MainPage()
        {
            InitializeComponent();
            GestionClick();
            ChargerLivres();
        }


        void GestionClick()
        {
            listeLivre.ItemSelected += (sender, e) => {
                if (listeLivre.SelectedItem != null)
                {
                    var detailsLivre = new DetailsLivre();
                    var livre = listeLivre.SelectedItem as Livre;
                    detailsLivre.BindingContext = livre;
                    this.Navigation.PushAsync(detailsLivre);
                    listeLivre.SelectedItem = null;
                    //todo mettre en place la page détail
                }
            };
        }


        void ChargerLivres()
        {
            if (File.Exists(FichierLivre))
            {
                string livresJson = File.ReadAllText(FichierLivre);
                if (!string.IsNullOrEmpty(livresJson))
                {
                    Livres = DeserializeObject<ObservableCollection<Livre>>(livresJson);
                    listeLivre.ItemsSource = Livres;
                }
            }
        }


        public async void Ajout(string isbn)
        {

            Livre livre = await Api(isbn);

            if (livre != null)
            {
                Livres.Add(livre);
                SaveString(Livres);
            }
            else
               await DisplayAlert("Attention", "Ce livre ne figure pas dans notre base de donnée", "ok");
        }


        async void ScanClick(object sender, System.EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    Ajout(result.Text);
                    //todo mettre un beau texte
                });
            };
        }


        public async Task<Livre> Api(string isbn)
        {

            var ApiBook = new HttpClient();
            var uri = "https://openlibrary.org/api/books?bibkeys=ISBN:" + isbn + "&jscmd=details&format=json";

            try
            {
                var response = await ApiBook.GetStringAsync(uri);
                var json = JObject.Parse(response);
                var jLivre = json.GetValue("ISBN:" + isbn);
                var livre = DeserializeObject<Livre>(jLivre.ToString());
                return livre;
            }
            catch (Exception e)
            {
                //fixme changer message d'erreur
                await DisplayAlert("Erreur", e.Message,  "ok");
            }
            return null;
        }


        public void SaveString(ObservableCollection<Livre> livres)
        {
            string json = SerializeObject(livres);
            File.WriteAllText(FichierLivre, json);
        }

        public Command<Livre> RemoveCommand
        {
            get
            {
                return new Command<Livre>((livre) =>
               {
                   Livres.Remove(livre);
               });
            }
        }

        void RemoveItem(object sender, EventArgs e)
        {
            var button = sender as Button;

            var livre = button.BindingContext as Livre;

            RemoveCommand.Execute(livre);

            SaveString(Livres);
         
        }
    }
}
