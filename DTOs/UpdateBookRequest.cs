using System.ComponentModel.DataAnnotations;
using GerenciadorLivrosLivraria.Validation;

namespace GerenciadorLivrosLivraria.DTOs
{
    public class UpdateBookRequest
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [AllowedGenre]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}