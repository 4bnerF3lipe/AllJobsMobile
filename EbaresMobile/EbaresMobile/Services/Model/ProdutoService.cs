using EbaresMobile.Models;
using EbaresMobile.Services.Interface;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Model
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoService _produtoService;
        public ProdutoService()
        {
            _produtoService = RestService.For<IProdutoService>(App.BaseUrl);
        }

        public async Task<List<Produto>> BuscaProdutosPeloPedido(int pedido)
        {
            try
            {
                return await _produtoService.BuscaProdutosPeloPedido(pedido);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Produto>> BuscarProdutos()
        {
            try
            {
                return await _produtoService.BuscarProdutos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EnviarProdutos(Mesa mesa)
        {
            try
            {
                return await _produtoService.EnviarProdutos(mesa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
