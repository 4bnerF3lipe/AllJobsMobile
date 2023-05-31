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
    public partial class EntryPopupPage : PopupPage
    {
        int _numero;
        private List<Atendente> consulta;
        public Action<Mesa> Retorno { get; internal set; }
        public EntryPopupPage(int numero)
        {
            InitializeComponent();
            BuscarAtendentes().GetAwaiter();
            _numero = numero;
            PopupNavigation.Instance.PushAsync(this, false);
        }
        private async Task BuscarAtendentes()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Buscando Atendentes...");
                var service = new VendedorService();
                consulta = await service.BuscarAtendentes();

                if (consulta != null && consulta.Count > 0)
                {
                    nomeVendedorComboBox.ComboBoxSource = consulta.Select(item => item.Nome).ToList();
                }
                else
                {
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
        private async void AbrirMesa(object sender, EventArgs e)
        {
            try
            {
                var service = new MesaService();
                var nome = NomeMesaEntry.Text;
                if (string.IsNullOrEmpty(nome))
                    throw new Exception("Digite um nome para a mesa");
                if(nomeVendedorComboBox.SelectedItem == null)
                    throw new Exception("Selecione um vendedor");
                var vendedor = consulta.FirstOrDefault(item => item.Nome == nomeVendedorComboBox.SelectedItem.ToString());
                var mesa = new Mesa()
                {
                    Atendente = vendedor.Id,
                    Nome = nome,
                    Numero = _numero,
                };
                UserDialogs.Instance.ShowLoading("Abrindo comanda...");
            var result =  await service.AbrirComanda(mesa);
                if (result!= -1)
                {
                    mesa.Pedido = result;
                    Retorno(mesa);
                }
                else
                    throw new Exception("Não foi possivel abrir a comanda!");
                await PopupNavigation.Instance.PopAsync(false);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", ex.Message, "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }
    }
}