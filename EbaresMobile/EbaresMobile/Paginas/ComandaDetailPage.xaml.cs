using EbaresMobile.Models;
using EbaresMobile.ViewModels.Paginas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EbaresMobile.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComandaDetailPage : ContentPage
    {
        ComandaDetailViewModel _comandaDetailViewModel;
        public ComandaDetailPage(Comanda comanda)
        {
            InitializeComponent();
            _comandaDetailViewModel = BindingContext as ComandaDetailViewModel;
            _comandaDetailViewModel.ComandaAtual = comanda;
            _comandaDetailViewModel.Navigation = Navigation;
            _comandaDetailViewModel.ComandaAtual.ComandaDetailViewModel = _comandaDetailViewModel;
        }
       
        protected override bool OnBackButtonPressed()
        {
            _comandaDetailViewModel.ComandaAtual.Voltar();
            return true;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _comandaDetailViewModel.ComandaAtual.BuscarProdutos();
       
        }

    }
}