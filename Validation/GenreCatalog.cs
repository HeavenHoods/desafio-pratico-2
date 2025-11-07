using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorLivrosLivraria.Validation
{
    public static class GenreCatalog
    {
        public static readonly HashSet<string> Allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ficção",
            "romance",
            "mistério",
            "fantasia",
            "aventura",
            "biografia",
            "história",
            "drama",
            "terror",
            "sci-fi",
            "poesia",
            "conto"
        };

        public static string AllowedList => string.Join(", ", Allowed.OrderBy(g => g));
    }
}