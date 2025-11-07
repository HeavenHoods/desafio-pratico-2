using System;
using System.Collections.Generic;
using GerenciadorLivrosLivraria.Models;

namespace GerenciadorLivrosLivraria.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly Dictionary<Guid, Book> _store = new();

        public InMemoryBookRepository()
        {
            var b1 = new Book { Title = "Dom Casmurro", Author = "Machado de Assis", Genre = "romance", Price = 39.9m, Stock = 10 };
            var b2 = new Book { Title = "O Alienista", Author = "Machado de Assis", Genre = "ficção", Price = 29.9m, Stock = 5 };
            _store[b1.Id] = b1;
            _store[b2.Id] = b2;
        }

        public IEnumerable<Book> GetAll() => _store.Values;

        public Book? GetById(Guid id) => _store.TryGetValue(id, out var book) ? book : null;

        public Book Create(Book book)
        {
            var now = DateTime.UtcNow;
            book.CreatedAt = now;
            book.UpdatedAt = now;
            _store[book.Id] = book;
            return book;
        }

        public bool Update(Book book)
        {
            if (!_store.ContainsKey(book.Id)) return false;
            book.UpdatedAt = DateTime.UtcNow;
            _store[book.Id] = book;
            return true;
        }

        public bool Delete(Guid id) => _store.Remove(id);
    }
}