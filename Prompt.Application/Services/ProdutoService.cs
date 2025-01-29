using Prompt.Domain.Entities;
using Prompt.Domain.Interface;
using System.Text.Json;

namespace Prompt.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        public string GetProdutos()
        {
            List<Produto> produtos = new List<Produto>
            {
            new Produto { Id = 1, Nome = "Produto A", Preco = 10.99m, Descricao = "Descrição do Produto A", Estoque = 5, Categoria = "Eletrônicos", Desconto = 0 },
            new Produto { Id = 2, Nome = "Produto B", Preco = 20.49m, Descricao = "Descrição do Produto B", Estoque = 0, Categoria = "Casa", Desconto = 5 },
            new Produto { Id = 3, Nome = "Produto C", Preco = 30.00m, Descricao = "Descrição do Produto C", Estoque = 3, Categoria = "Eletrônicos", Desconto = 10 }
            };

            return JsonSerializer.Serialize(produtos);
        }

        public List<Produto> GetProdutosListObject()
        {
            return new List<Produto>
            {
            new Produto { Id = 1, Nome = "Produto A", Preco = 10.99m, Descricao = "Descrição do Produto A", Estoque = 5, Categoria = "Eletronico", Desconto = 0 },
            new Produto { Id = 2, Nome = "Produto B", Preco = 20.49m, Descricao = "Descrição do Produto B", Estoque = 0, Categoria = "Casa", Desconto = 5 },
            new Produto { Id = 3, Nome = "Produto C", Preco = 30.00m, Descricao = "Descrição do Produto C", Estoque = 3, Categoria = "Eletronico", Desconto = 10 }
            };
        }

        public string GetProdutosPorCategoria(string categoria)
        {
            List<Produto> produtos = GetProdutosListObject().Where(p => p.Categoria.ToUpper().Equals(categoria.ToUpper(), StringComparison.OrdinalIgnoreCase)).ToList();
            return JsonSerializer.Serialize(produtos); // Retorna uma lista vazia se não houver produtos
        }
    }
}
