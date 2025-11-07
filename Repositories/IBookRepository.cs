using System;
using System.Collections.Generic;
using GerenciadorLivrosLivraria.Models;

namespace GerenciadorLivrosLivraria.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book? GetById(Guid id);
        Book Create(Book book);
        bool Update(Book book);
        bool Delete(Guid id);
    }
}