using System.ComponentModel.DataAnnotations;

namespace GerenciadorLivrosLivraria.Validation
{
    public class AllowedGenreAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var genre = value as string;
            if (genre == null)
            {
                return new ValidationResult("O gênero é obrigatório.");
            }

            if (!GenreCatalog.Allowed.Contains(genre))
            {
                return new ValidationResult($"Gênero inválido. Permitidos: {GenreCatalog.AllowedList}.");
            }

            return ValidationResult.Success;
        }
    }
}