using EbaresMobile.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Interface
{
    public interface IStatusService
    {
        [Get("/Status")]
        Task<bool> VerificaStatus();
        [Post("/Banco")]
        Task<bool> VerificaSenhaBanco(string senha);
    }
}
