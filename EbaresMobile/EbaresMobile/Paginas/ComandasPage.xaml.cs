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
    public partial class ComandasPage : ContentPage
    {
        ComandaViewModel _comandasViewModel;
        public ComandasPage()
        {
            InitializeComponent();
            _comandasViewModel = BindingContext as ComandaViewModel;
            _comandasViewModel.Navigation = Navigation;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _comandasViewModel.CarregaDados();
        }

    }
}