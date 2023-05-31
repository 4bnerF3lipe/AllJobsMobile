using EbaresMobile.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EbaresMobile.ViewModels.Paginas
{
    public class PedidoComandaViewModel:BaseViewModel
    {
        #region Encapsulamento
        public INavigation Navigation { get; set; }
        #endregion
        #region Commands
        private Command _voltarCommand;
        public Command VoltarCommand => _voltarCommand ?? (_voltarCommand = new Command(async () =>
        {
            await Navigation.PopAsync(false);
        }));

        private Command _salvarPedidoCommand;
        public Command SalvarPedidoCommand => _salvarPedidoCommand ?? (_salvarPedidoCommand = new Command(async () =>
        {
            await Navigation.PopAsync(false);
        }));

        private Command _escolhaProdutoCommand;
        public Command EscolhaProdutoCommand => _escolhaProdutoCommand ?? (_escolhaProdutoCommand = new Command(async () =>
        {
            //var retornoProduto = await App.AbrirCadastroPedido();
        }));

       
        #endregion
    }
}
