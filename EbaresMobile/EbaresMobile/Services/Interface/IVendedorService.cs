using EbaresMobile.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Interface
{
    public interface IVendedorService
    {
        [Get("/Atendente")]
        Task<List<Atendente>> BuscarAtendentes();
    }
}
