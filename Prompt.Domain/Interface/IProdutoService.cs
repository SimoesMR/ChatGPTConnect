using Prompt.Domain.Entities;

namespace Prompt.Domain.Interface
{
    public interface IProdutoService
    {
        string GetProdutos();
        List<Produto> GetProdutosListObject();
        string GetProdutosPorCategoria(string categoria);
    }
}
