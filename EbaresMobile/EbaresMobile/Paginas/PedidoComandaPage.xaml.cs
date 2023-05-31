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
    public partial class PedidoComandaPage : ContentPage
    {
        PedidoComandaViewModel _pedidoComandaViewModel;
        public PedidoComandaPage()
        {
            InitializeComponent();
            _pedidoComandaViewModel = BindingContext as PedidoComandaViewModel;
            _pedidoComandaViewModel.Navigation = Navigation;
        }
    }
}