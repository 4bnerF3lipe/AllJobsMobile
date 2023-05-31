using Acr.UserDialogs;
using DLToolkit.Forms.Controls;
using EbaresMobile.Models;
using EbaresMobile.Paginas;
using EbaresMobile.Popups;
using EbaresMobile.Services.Model;
using System;
using System.Collections.Generic;
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

        public App()
        {
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;


            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Njk1ODUxQDMyMzAyZTMyMmUzMGNvai9yZks3UlVZcW10SzBIaC9zU0Y5YjFmbHNiZ29hdTdoZGdIT2Y3QWM9");

            InitializeComponent();
            CarregaConfigWeb();
            if (Preferences.ContainsKey("Host"))
                App.BaseUrl.BaseAddress = new Uri($"https://{Preferences.Get("Host", null)}");
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
                    Mesa m = new Mesa()
                    {
                        Numero = numeroComanda,
                        //Pedido = NumeroPedido,*
                        Produtos = obj
                    };

                    UserDialogs.Instance.ShowLoading("Enviando pedidos...");
                    var retornoService = await _produtoService.EnviarProdutos(m);
                    if (retornoService)
                    {
                        obj.ForEach((i) => { i.Enviado = true; });
                    }
                    else
                        throw new Exception("Não foi possivel enviar os pedidos!");
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
