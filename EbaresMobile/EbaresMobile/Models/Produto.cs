using EbaresMobile.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using Xamarin.Forms;

namespace EbaresMobile.Models
{
    public class Produto : BaseViewModel
    {
        private int _quantidade;

        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public string Observacao { get; set; }
        public double Valor { get; set; }
        public int Quantidade { get { return _quantidade; } set { _quantidade = value; OnPropertyChanged("Quantidade"); } }
        [JsonIgnore]
        public double Total { get { return Valor * Quantidade; } }
        [JsonIgnore]
        public bool Enviado { get; set; } = false;
        [JsonIgnore]
        public Color CorEnviado
        {
            get
            {
                if (Enviado)
                    return Color.DarkGreen;
                else
                    return Color.DarkRed;
            }
        }


    }
}
