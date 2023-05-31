using EbaresMobile.Models;
using EbaresMobile.Services.Interface;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Model
{
    public class MesaService : IMesaService
    {
        private readonly IMesaService _mesaService;
        public MesaService()
        {
            _mesaService = RestService.For<IMesaService>(App.BaseUrl);
        }
        public async Task<List<Mesa>> BuscarMesasOcupadas()
        {
            try
            {
                return await _mesaService.BuscarMesasOcupadas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> AbrirComanda(Mesa mesa)
        {
            try
            {
                return await _mesaService.AbrirComanda(mesa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
