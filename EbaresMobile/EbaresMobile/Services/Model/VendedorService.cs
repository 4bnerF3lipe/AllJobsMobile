using EbaresMobile.Models;
using EbaresMobile.Services.Interface;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Model
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorService _vendedorService;
        public VendedorService()
        {
            _vendedorService = RestService.For<IVendedorService>(App.BaseUrl);
        }

        public async Task<List<Atendente>> BuscarAtendentes()
        {
            try
            {
                return await _vendedorService.BuscarAtendentes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
