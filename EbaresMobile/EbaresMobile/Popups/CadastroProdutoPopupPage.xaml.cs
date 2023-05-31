using EbaresMobile.Models;
using EbaresMobile.ViewModels.Popup;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EbaresMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroProdutoPopupPage : PopupPage
    {
        CadastroProdutoPoupViewModel _cadastroProdutoPoupViewModel;
        public CadastroProdutoPopupPage(Produto produto, string nomeMesa, string numeroMesa)
        {

            InitializeComponent();
            PopupNavigation.Instance.PushAsync(this, false);
            _cadastroProdutoPoupViewModel = BindingContext as CadastroProdutoPoupViewModel;
            _cadastroProdutoPoupViewModel.NomeMesa = nomeMesa;
            _cadastroProdutoPoupViewModel.NumeroMesa = numeroMesa;
            _cadastroProdutoPoupViewModel.Produto = produto;
        }

        public Action<List<Produto>> Retorno { get; internal set; }

        private void Salvar(object sender, EventArgs e)
        {
            try
            {
                var itens = _cadastroProdutoPoupViewModel.ListaSelecionados;
                if (itens == null || itens.Count() == 0)
                    throw new Exception("Nenhum item selecionado!");

                Retorno(itens);
                PopupNavigation.Instance.PopAsync(false);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Atenção:", ex.Message, "OK");
            }
        }

        private void PesquisaTempoReal(object sender, TextChangedEventArgs e)
        {
            _cadastroProdutoPoupViewModel.Pesquisa();
        }
    }
}