using Acr.UserDialogs;
using EbaresMobile.Models;
using EbaresMobile.Popups;
using EbaresMobile.Services.Interface;
using EbaresMobile.Services.Model;
using EbaresMobile.ViewModels.Core;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EbaresMobile.ViewModels.Popup
{
    public class CadastroProdutoPoupViewModel : BaseViewModel
    {
        private string _nomeMesa;
        public string NomeMesa { get { return _nomeMesa; } set { _nomeMesa = value; OnPropertyChanged("NomeMesa"); } }

        private string _numeroMesa;
        public string NumeroMesa { get { return _numeroMesa; } set { _numeroMesa = value; OnPropertyChanged("NumeroMesa"); } }

        private ProdutoService _produtoService;
        private List<Produto> _listaGeral;

        private Produto _produto;
        public Produto Produto { get { return _produto; } set { _produto = value; OnPropertyChanged("Produto"); } }

        private ObservableCollection<Produto> _produtosLista;
        public ObservableCollection<Produto> ProdutosLista { get { return _produtosLista; } set { _produtosLista = value; OnPropertyChanged("ProdutosLista"); } }

        public List<Produto> ListaSelecionados = new List<Produto>();
        private double _totalPedidos;
        public double TotalPedidos
        {
            get
            {
                return _totalPedidos;
            }
            set { _totalPedidos = value; OnPropertyChanged("TotalPedidos"); }
        }

        private string _campoPesquisa;

        public string CampoPesquisa { get { return _campoPesquisa; } set { _campoPesquisa = value; OnPropertyChanged("CampoPesquisa"); } }

        private Command _adicionarQuantidadeCommand;
        public Command AdicionarQuantidadeCommand => _adicionarQuantidadeCommand ?? (_adicionarQuantidadeCommand = new Command<Produto>((p) =>
        {
            try
            {

                if (ListaSelecionados.Any(i => i.Id == p.Id))
                {
                    ListaSelecionados.FirstOrDefault(i => i.Id == p.Id).Quantidade += 1;

                }
                else
                {
                    p.Quantidade++;
                    ListaSelecionados.Add(p);
                }

                TotalPedidos = ListaSelecionados.Where(i => i.Quantidade >= 1).Sum(i => i.Total);
                OnPropertyChanged("ProdutosLista");
            }
            catch (Exception ex)
            {

                throw;
            }
        }));

        private Command _diminuirQuantidadeCommand;
        public Command DiminuirQuantidadeCommand => _diminuirQuantidadeCommand ?? (_diminuirQuantidadeCommand = new Command<Produto>((p) =>
        {
            try
            {
                if (p.Quantidade >= 1)
                {
                    var obj = ListaSelecionados.FirstOrDefault(i => i.Id == p.Id);
                    if (obj != null && obj.Quantidade > 1)
                    {
                        ListaSelecionados.FirstOrDefault(i => i.Id == p.Id).Quantidade--;
                    }
                    else
                    {
                        p.Quantidade--;
                        ListaSelecionados.Remove(p);
                    }
                    TotalPedidos = ListaSelecionados.Where(i => i.Quantidade >= 1).Sum(i => i.Total);
                    OnPropertyChanged("ProdutosLista");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }));

        private Command _adicionarObservacaoCommand;
        public Command AdicionarObservacaoCommand => _adicionarObservacaoCommand ?? (_adicionarObservacaoCommand = new Command<Produto>((p) =>
        {
            try
            {
                var popup = new ObservacaoPopupPage(p.Observacao);
                popup.Retorno += (obs) =>
                {
                    if (ListaSelecionados.Any(i => i.Id == p.Id))
                    {
                        ListaSelecionados.FirstOrDefault(i => i.Id == p.Id).Observacao = obs;
                    }
                    else
                    {
                        p.Observacao = obs;
                        ListaSelecionados.Add(p);
                    }
              
                    OnPropertyChanged("ProdutosLista");
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }));
        public CadastroProdutoPoupViewModel()
        {
            CarregaListaProdutos().GetAwaiter();
        }

        private async Task CarregaListaProdutos()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Buscando Produtos...");
                _produtoService = new ProdutoService();
                var consulta = await _produtoService.BuscarProdutos();
                if (consulta != null && consulta.Count > 0)
                {
                    _listaGeral = consulta;
                    ProdutosLista = new ObservableCollection<Produto>(_listaGeral);
                }
                else
                {
                    ProdutosLista = new ObservableCollection<Produto>();
                    throw new Exception("Não foi possivel buscar os produtos!");
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
        }

        public void Pesquisa()
        {
            if (string.IsNullOrEmpty(CampoPesquisa))
            {
                ProdutosLista = new ObservableCollection<Produto>(_listaGeral);
            }
            else
            {
                var pesquisa = _listaGeral.Where(item => item.NomeProduto.ToUpper().Contains(CampoPesquisa.ToUpper()));
                ProdutosLista = new ObservableCollection<Produto>(pesquisa);
            }
            TotalPedidos = ListaSelecionados.Where(i => i.Quantidade >= 1).Sum(i => i.Total);
        }
    }
}
