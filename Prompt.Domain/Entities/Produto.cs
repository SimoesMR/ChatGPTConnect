namespace Prompt.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Estoque { get; set; } // Quantidade em estoque
        public string Categoria { get; set; } = string.Empty; // Categoria do produto
        public decimal Desconto { get; set; } // Desconto aplicado
    }

}
