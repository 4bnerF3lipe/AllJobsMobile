using Acr.UserDialogs;
using EbaresMobile.Models;
using EbaresMobile.Services.Model;
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
    public partial class ObservacaoPopupPage : PopupPage
    {
        public Action<string> Retorno { get; internal set; }
        public ObservacaoPopupPage(string observacao )
        {
            InitializeComponent();
            ObservacaoMesaEditor.Text = observacao??"";
            PopupNavigation.Instance.PushAsync(this, false);
        }
 
        private async void AdicionarObservacao(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(false);
            Retorno(ObservacaoMesaEditor.Text);

        }
    }
}