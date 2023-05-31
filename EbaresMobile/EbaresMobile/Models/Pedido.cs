using System;
using System.Collections.Generic;
using System.Text;

namespace EbaresMobile.Models
{
    public class Pedido
    {
        public int Codigo { get; set; }
        public List<Produto> Produtos { get; set; }
        public bool Finalizado { get; set; }
    }
}
