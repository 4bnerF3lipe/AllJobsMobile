using Acr.UserDialogs;
using DLToolkit.Forms.Controls;
using EbaresMobile.Models;
using EbaresMobile.Paginas;
using EbaresMobile.Popups;
using EbaresMobile.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EbaresMobile
{
    public partial class App : Application
    {
        public static HttpClient BaseUrl { get; set; }
        public static bool Demo { get; private set; }
        public static List<Mesa> MesasOcupadas { get; private set; }
        public static List<Produto> ListaProdutos{ get; private set; }
        public App()
        {
            Demo = true;
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            ListaProdutos = new List<Produto>()
            {
                new Produto()
                {
                    Id = 1,
                    NomeProduto = "COCA COLA LATA",
                    Valor = 3.5
                },
                 new Produto()
                {
                    Id = 2,
                    NomeProduto = "COCA COLA 1L",
                    Valor = 6
                },
                  new Produto()
                {
                    Id = 3,
                    NomeProduto = "COCA COLA 2L",
                    Valor = 10
                },
                   new Produto()
                {
                    Id = 4,
                    NomeProduto = "Porcão de Batata 500g",
                    Valor = 20
                },
                    new Produto()
                {
                    Id = 5,
                    NomeProduto = "Porcão de Batata 1Kg",
                    Valor = 40
                },
                     new Produto()
                {
                    Id = 6,
                    NomeProduto = "Porcão de Batata com Chedar 500g",
                    Valor = 25
                },
                      new Produto()
                {
                    Id = 7,
                    NomeProduto = "Porcão de Batata com Chedar 1kg",
                    Valor = 50
                },
                       new Produto()
                {
                    Id = 8,
                    NomeProduto = "Coxinha de Frango",
                    Valor = 4.5
                },
                        new Produto()
                {
                    Id = 9,
                    NomeProduto = "X-TUDO",
                    Valor = 20
                },
                         new Produto()
                {
                    Id = 10,
                    NomeProduto = "X FRANGO",
                    Valor = 18
                }
            };

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Njk1ODUxQDMyMzAyZTMyMmUzMGNvai9yZks3UlVZcW10SzBIaC9zU0Y5YjFmbHNiZ29hdTdoZGdIT2Y3QWM9");

            InitializeComponent();
            if (!Demo)
            {
                CarregaConfigWeb();
                if (Preferences.ContainsKey("Host"))
                    App.BaseUrl.BaseAddress = new Uri($"https://{Preferences.Get("Host", null)}");
            }
            MainPage = new NavigationPage(new ComandasPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static void AdicionaMesa(Mesa m)
        {
            if (MesasOcupadas == null)
            {
                MesasOcupadas = new List<Mesa>();
            }
            
                MesasOcupadas.Add(m);

           
        }
        public static void CarregaConfigWeb()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = true;
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            App.BaseUrl = new HttpClient(clientHandler);

        }
        public static async Task<List<Produto>> AbrirCadastroPedido(string nomeMesa, int numeroComanda, Produto pedido = null)
        {
            try
            {
                var retorno = new TaskCompletionSource<List<Produto>>();

                var popup = new CadastroProdutoPopupPage(pedido, nomeMesa, numeroComanda.ToString());
                popup.Retorno += async (obj) =>
                {
                    var _produtoService = new ProdutoService();
                    Mesa m = MesasOcupadas.FirstOrDefault(i => i.Numero == numeroComanda);
                    if(m != null)
                    {
                        obj.ForEach(o => { o.Enviado = true; });
                        if(m.Produtos == null)
                        {
                            m.Produtos = new List<Produto>();
                        }
                        m.Produtos.AddRange(obj);
                        
                    }
                    

                    UserDialogs.Instance.ShowLoading("Enviando pedidos...");
                    await Task.Delay(1500);
                    UserDialogs.Instance.HideLoading();
                    retorno.SetResult(obj);
                };
                return await retorno.Task;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static async Task<Mesa> AbrirPopupEntry(int numero)
        {
            try
            {
                var retorno = new TaskCompletionSource<Mesa>();

                var popup = new EntryPopupPage(numero);
                popup.Retorno += (obj) =>
                {
                    retorno.SetResult(obj);
                };
                return await retorno.Task;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }
    }
}
