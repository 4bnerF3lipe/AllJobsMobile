using EbaresMobile.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Interface
{
    public interface IMesaService
    {
        [Get("/Mesa")]
        Task<List<Mesa>> BuscarMesasOcupadas();
        [Post("/Mesa")]
        Task<int> AbrirComanda(Mesa mesa);
    }
}
