using Acr.UserDialogs;
using EbaresMobile.Interface;
using EbaresMobile.Paginas;
using EbaresMobile.Services.Model;
using EbaresMobile.ViewModels.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using EbaresMobile.ViewModels.Paginas;

namespace EbaresMobile.Models
{
    public class Comanda : BaseViewModel
    {
        #region Propriedades
        private bool _comandaDisponivel;
        private int _numeroComanda;
        private ObservableCollection<Produto> _pedidosComanda;
        private Command _abrirDetalheComandaCommand;
        private Command _mudarComandaCommand;
        private Command _fecharComandaCommand;
        private Command _voltarCommand;
        private IRetornoDetalheComanda _retorno;
        private ProdutoService _produtoService;
        private string _nomeMesa;

        #endregion

        #region Encapsulamento
        private string _nomeAtendente;
        public double Total { get; set; }
        public string NomeAtendente { get { return _nomeAtendente; } set { _nomeAtendente = value; OnPropertyChanged("NomeAtendente"); } }

        public bool ComandaDisponivel { get { return _comandaDisponivel; } set { _comandaDisponivel = value; OnPropertyChanged("ComandaDisponivel"); OnPropertyChanged("ComandaAberta"); } }
        public bool ComandaAberta { get { return !ComandaDisponivel; } }
        public string NomeMesa
        {
            get
            {
                if (string.IsNullOrEmpty(_nomeMesa))
                    return "Disponivel";
                else
                    return _nomeMesa;
            }
            set { _nomeMesa = value; OnPropertyChanged("NomeMesa"); }
        }
        public int NumeroComanda { get { return _numeroComanda; } set { _numeroComanda = value; OnPropertyChanged("NumeroComanda"); } }
        public INavigation Navigation { get; set; }
        public int NumeroPedido { get; set; }
        public bool EnvioPedidos
        {
            get
            {
                if (PedidosComanda != null && PedidosComanda.Count > 0)
                {
                    return PedidosComanda.Any(i => i.Enviado == false);
                }
                else
                    return false;
            }

        }
        public ObservableCollection<Produto> PedidosComanda { get { return _pedidosComanda; } set { _pedidosComanda = value; OnPropertyChanged("PedidosComanda"); } }
        public ComandaDetailViewModel ComandaDetailViewModel { get; set; }

        #endregion

        #region Construtor
        public Comanda(bool comandaAberta, int numeroComanda, INavigation navegacao, IRetornoDetalheComanda retorno)
        {
            ComandaDisponivel = comandaAberta;
            NumeroComanda = numeroComanda;
            Navigation = navegacao;
            _retorno = retorno;
        }
        #endregion

        #region Command
        public Command AbrirDetalheComandaCommand => _abrirDetalheComandaCommand ?? (_abrirDetalheComandaCommand = new Command<Comanda>(async (comanda) =>
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Atualizando mesas...");
                var mesaService = new MesaService();
                var consulta = await mesaService.BuscarMesasOcupadas();
                if (consulta != null && consulta.Count > 0)
                {

                    var mesa = consulta.FirstOrDefault(i => i.Numero == comanda.NumeroComanda);
                    if (mesa != null)
                    {
                        comanda.NomeMesa = mesa.Nome;
                        comanda.NumeroPedido = mesa.Pedido;
                    }

                }
                Navigation.PushAsync(new ComandaDetailPage(comanda), false);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }));
        private Command _enviarPedidosComandaCommand;
        public Command EnviarPedidosComandaCommand => _enviarPedidosComandaCommand ?? (_enviarPedidosComandaCommand = new Command(async () =>
        {
            try
            {
                if (await App.Current.MainPage.DisplayAlert("Atenção", "Deseja enviar os pedidos?", "SIM", "NÃO"))
                {
                    _produtoService = new ProdutoService();
                    Mesa m = new Mesa()
                    {
                        Numero = NumeroComanda,
                        Pedido = NumeroPedido,
                        Produtos = new List<Produto>()
                    };
                    PedidosComanda.Where(i => i.Enviado == false).ToList().ForEach((i) =>
                    {
                        m.Produtos.Add(i);
                    });
                    UserDialogs.Instance.ShowLoading("Enviando pedidos...");
                    var retorno = await _produtoService.EnviarProdutos(m);
                    if (retorno)
                    {
                        PedidosComanda.Where(i => i.Enviado == false).ToList().ForEach((i) =>
                        {
                            i.Enviado = true;
                        });
                        OnPropertyChanged("PedidosComanda");
                        OnPropertyChanged("EnvioPedidos");
                    }
                    else
                        throw new Exception("Não foi possivel enviar os pedidos!");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }));
        public Command MudarComandaCommand => _mudarComandaCommand ?? (_mudarComandaCommand = new Command<Comanda>(async (comanda) =>
        {
            try
            {
                PedidosComanda = new ObservableCollection<Produto>();
                var result = await App.AbrirPopupEntry(NumeroComanda);
                NomeMesa = result.Nome;
                NumeroComanda = result.Numero;
                NumeroPedido = result.Pedido;
                ComandaDisponivel = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
        }));
        public Command VoltarCommand => _voltarCommand ?? (_voltarCommand = new Command<Comanda>((comanda) =>
        {
            try
            {
                Voltar();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
        }));
        #endregion

        #region Metodos
        public void Voltar()
        {
            _retorno.RetornoDetalheComanda(this);
            Navigation.PopAsync(false);
        }

        public async void BuscarProdutos()
        {
            try
            {
                if (!ComandaDisponivel)
                {
                    UserDialogs.Instance.ShowLoading("Atualizando comanda...");
                    _produtoService = new ProdutoService();
                    var consulta = await _produtoService.BuscaProdutosPeloPedido(NumeroPedido);
                    consulta.ForEach(x => { x.Enviado = true; });
                    this.PedidosComanda = new ObservableCollection<Produto>(consulta);
                    ComandaDetailViewModel.AtualizaTotalPedidos();
                }

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

        public void AdicionarProduto(Produto p)
        {
            PedidosComanda.Add(p);

        }
        #endregion
    }
}
