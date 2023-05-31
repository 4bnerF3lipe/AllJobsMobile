using EbaresMobile.Services.Interface;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Model
{
    public class StatusService 
    {
        public async Task<bool> VerificaStatus()
        {
			try
			{
                var service = RestService.For<IStatusService>(App.BaseUrl);
                return  await service.VerificaStatus();

			}
			catch (Exception ex)
			{
                return false;
			}
        }
        public async Task<bool> VerificaSenhaBanco(string senha)
        {
            try
            {
                var service = RestService.For<IStatusService>(App.BaseUrl);
                return await service.VerificaSenhaBanco(senha);

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
