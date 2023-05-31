using EbaresMobile.Models;
using EbaresMobile.Paginas;
using EbaresMobile.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EbaresMobile.ViewModels.Paginas
{
    public class ComandaDetailViewModel : BaseViewModel
    {
        #region Propriedades
        private Comanda _comandaAtual;
        private ObservableCollection<Produto> _produtosComanda;
        public ObservableCollection<Produto> ProdutosComanda { get { return _produtosComanda; } set { _produtosComanda = value; } }
        private double _totalPedidos;

        public double TotalPedidos { get { return _totalPedidos; } set { _totalPedidos = value; OnPropertyChanged("TotalPedidos"); } }

        #endregion

        #region Encapsulamento
        public INavigation Navigation { get; set; }
        public Comanda ComandaAtual { get { return _comandaAtual; } set { _comandaAtual = value; OnPropertyChanged("ComandaAtual"); } }

        #endregion

        #region Comandos
        private Command _novaComandaCommand;
        public Command NovaComandaCommand => _novaComandaCommand ?? (_novaComandaCommand = new Command(async () =>
        {
            var produtos = await App.AbrirCadastroPedido(ComandaAtual.NomeMesa,ComandaAtual.NumeroComanda);
            produtos.ForEach(p =>
            {
                ComandaAtual.AdicionarProduto(p);
            });
            TotalPedidos = ComandaAtual.PedidosComanda.ToList().Sum(item => item.Total);
        }));

        #endregion

        public ComandaDetailViewModel()
        {
        }

        public void AtualizaTotalPedidos()
        {

            TotalPedidos = ComandaAtual.PedidosComanda.ToList().Sum(item => item.Total);
            OnPropertyChanged("TotalPedidos");
        }
    }
}
