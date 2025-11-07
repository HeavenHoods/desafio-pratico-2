namespace GerenciadorLivrosLivraria.Models
{
    public abstract class InventoryItem : BaseEntity
    {
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}