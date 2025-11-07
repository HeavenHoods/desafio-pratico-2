namespace GerenciadorLivrosLivraria.Models
{
    public class Book : InventoryItem
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}