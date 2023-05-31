using EbaresMobile.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EbaresMobile.Services.Interface
{
    public interface IProdutoService
    {
        [Get("/Produtos")]
        Task<List<Produto>> BuscarProdutos();
        
        [Get("/Produtos/BuscarPeloPedido/{pedido}")]
        Task<List<Produto>> BuscaProdutosPeloPedido(int pedido);

        [Post("/Produtos")]
        Task<bool> EnviarProdutos(Mesa mesa);
    }
}
