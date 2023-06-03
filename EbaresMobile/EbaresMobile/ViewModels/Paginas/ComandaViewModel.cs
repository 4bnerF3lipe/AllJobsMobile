using Acr.UserDialogs;
using EbaresMobile.Interface;
using EbaresMobile.Models;
using EbaresMobile.Paginas;
using EbaresMobile.Services.Model;
using EbaresMobile.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EbaresMobile.ViewModels.Paginas
{
    public class ComandaViewModel : BaseViewModel, IRetornoDetalheComanda
    {

        #region Propriedades
        private bool _comandaAtiva;
        private bool _informaIp;
        public bool InformaIp { get { return _informaIp; } set { _informaIp = value; OnPropertyChanged("InformaIp"); } }
        private bool _isRefreshing;
        private string ip;
        private string senhaBanco;
        private string mesa;
        private List<Comanda> _listaGeral;
        private ObservableCollection<Comanda> _listaComandas;
        private bool demo;
        #endregion

        #region Encapsulamento
        public string Ip { get { return ip; } set { ip = value; OnPropertyChanged("Ip"); } }
        public string SenhaBanco { get { return senhaBanco; } set { senhaBanco = value; OnPropertyChanged("SenhaBanco"); } }
        public string Mesa { get { return mesa; } set { mesa = value; OnPropertyChanged("Mesa"); } }
        public bool IsRefreshing { get { return _isRefreshing; } set { _isRefreshing = value; OnPropertyChanged("IsRefreshing"); } }
        public int Disponiveis { get { return ListaComandas != null ? ListaComandas.Count((item) => item.ComandaDisponivel) : 0; } }
        public int Ocupados { get { return ListaComandas != null ? ListaComandas.Count((item) => !item.ComandaDisponivel) : 0; } }
        public bool ComandaAtiva { get { return _comandaAtiva; } set { _comandaAtiva = value; OnPropertyChanged("ComandaAtiva"); } }
        public ObservableCollection<Comanda> ListaComandas { get { return _listaComandas; } set { _listaComandas = value; OnPropertyChanged("ListaComandas"); } }
        public INavigation Navigation { get; set; }
        #endregion

        #region Commands
        private Command _conectarCommand;
        public Command ConectarCommand => _conectarCommand ?? (_conectarCommand = new Command(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(Ip) || string.IsNullOrWhiteSpace(Ip))
                {
                    throw new Exception("Informe o ip do servidor local");
                }
                else if (string.IsNullOrEmpty(SenhaBanco) || string.IsNullOrWhiteSpace(SenhaBanco))
                {
                    throw new Exception("Informe a senha da base de dados");
                }
                else
                {
                    App.BaseUrl.BaseAddress = new Uri("https://" + Ip);

                    await VerificaConexao();

                }


            }

            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Atenção", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }));
        private Command _buscarMesaCommand;
        public Command BuscarMesaCommand => _buscarMesaCommand ?? (_buscarMesaCommand = new Command(() =>
        {
            try
            {
                var mesaTemp = Convert.ToInt32(Mesa);

                var mesaEncontrada = ListaComandas.FirstOrDefault(i => i.NumeroComanda == mesaTemp);
                if (mesaEncontrada != null)
                {
                    mesaEncontrada.AbrirDetalheComandaCommand.Execute(mesaEncontrada);
                }
                else
                {
                    UserDialogs.Instance.Alert("Mesa não encontrada!", "Atenção", "OK");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Não foi possível buscar a mesa!", "Atenção", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }));
        private Command _atualizarCommand;
        public Command AtualizarCommand => _atualizarCommand ?? (_atualizarCommand = new Command(async () =>
        {
            try
            {
                IsRefreshing = true;
                await AtualizaMesas();
                IsRefreshing = false;
            }

            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Atenção", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }));
        #endregion

        #region Construtor 
        public ComandaViewModel()
        {
            Ip = "127.0.0.1";
            SenhaBanco = "SenhaBancoAqui";
        }
        #endregion
        #region Metodos
        private async Task VerificaConexao()
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Verificando IP...");
                await Task.Delay(1500);
                UserDialogs.Instance.ShowLoading("Verificando Conexão com o Banco...");
                await Task.Delay(1500);
                await AtualizaMesas();
            }
            catch (Exception ex)
            {
            }
        }
        public async void CarregaDados()
        {

            try
            {
                _listaGeral = new List<Comanda>();
                for (int i = 0; i < 180; i++)
                {
                    var comanda = new Comanda(true, i + 1, Navigation, this);
                    _listaGeral.Add(comanda);
                }
                ListaComandas = new ObservableCollection<Comanda>(_listaGeral);
                await AtualizaMesas();
                OnPropertyChanged("Disponiveis");
                OnPropertyChanged("Ocupados");

            }
            catch (Exception ex)
            {

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<bool> AtualizaMesas()
        {
            try
            {
                var result = new TaskCompletionSource<bool>();

                UserDialogs.Instance.ShowLoading("Atualizando mesas...");
                var mesaService = new MesaService();
                ListaComandas.ToList().ForEach(i =>
                { i.ComandaDisponivel = true; i.NomeMesa = null; });
                var consulta = App.MesasOcupadas;
                if (consulta != null && consulta.Count > 0)
                {
                    foreach (var item in consulta)
                    {
                        var mesa = ListaComandas.FirstOrDefault(i => i.NumeroComanda == item.Numero);
                        if (mesa != null)
                        {
                            mesa.NomeMesa = item.Nome;
                            mesa.ComandaDisponivel = false;
                            mesa.NumeroPedido = item.Pedido;
                        }
                    }
                    OnPropertyChanged("Disponiveis");
                    OnPropertyChanged("Ocupados");
                    OnPropertyChanged("ListaComandas");
                    result.SetResult(true);
                }
                else
                    result.SetResult(true);

                return await result.Task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public void RetornoDetalheComanda(Comanda comanda)
        {
            try
            {
                var templist = ListaComandas.ToList();
                for (int i = 0; i < templist.Count; i++)
                {

                    if (templist[i].NumeroComanda == comanda.NumeroComanda)
                        templist[i] = comanda;
                }
                ListaComandas = new ObservableCollection<Comanda>(templist);
                OnPropertyChanged("Disponiveis");
                OnPropertyChanged("Ocupados");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
        }
        #endregion
    }
}
