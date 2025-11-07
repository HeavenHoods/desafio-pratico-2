using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GerenciadorLivrosLivraria.Repositories;
using GerenciadorLivrosLivraria.Models;
using GerenciadorLivrosLivraria.DTOs;

namespace GerenciadorLivrosLivraria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookResponse>> GetAll(
            [FromQuery] string? title,
            [FromQuery] string? author,
            [FromQuery] string? genre,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? minStock,
            [FromQuery] int? maxStock)
        {
            var items = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(title))
                items = items.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(author))
                items = items.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(genre))
                items = items.Where(b => string.Equals(b.Genre, genre, StringComparison.OrdinalIgnoreCase));
            if (minPrice.HasValue)
                items = items.Where(b => b.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                items = items.Where(b => b.Price <= maxPrice.Value);
            if (minStock.HasValue)
                items = items.Where(b => b.Stock >= minStock.Value);
            if (maxStock.HasValue)
                items = items.Where(b => b.Stock <= maxStock.Value);

            var response = items.Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                Price = b.Price,
                Stock = b.Stock,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            });
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<BookResponse> GetById(Guid id)
        {
            var book = _repository.GetById(id);
            if (book == null) return NotFound();
            var response = new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Price = book.Price,
                Stock = book.Stock,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<BookResponse> Create([FromBody] CreateBookRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var normalizedTitle = request.Title.Trim();
            var normalizedAuthor = request.Author.Trim();
            var duplicate = _repository.GetAll().Any(b =>
                string.Equals(b.Title.Trim(), normalizedTitle, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(b.Author.Trim(), normalizedAuthor, StringComparison.OrdinalIgnoreCase));
            if (duplicate)
            {
                return Conflict(new { message = "Já existe um livro com mesmo título e autor." });
            }

            var book = new Book
            {
                Title = normalizedTitle,
                Author = normalizedAuthor,
                Genre = request.Genre,
                Price = request.Price,
                Stock = request.Stock
            };

            var created = _repository.Create(book);
            var response = new BookResponse
            {
                Id = created.Id,
                Title = created.Title,
                Author = created.Author,
                Genre = created.Genre,
                Price = created.Price,
                Stock = created.Stock,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateBookRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var existing = _repository.GetById(id);
            if (existing == null) return NotFound();

            var normalizedTitle = request.Title.Trim();
            var normalizedAuthor = request.Author.Trim();

            var duplicate = _repository.GetAll().Any(b =>
                b.Id != id &&
                string.Equals(b.Title.Trim(), normalizedTitle, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(b.Author.Trim(), normalizedAuthor, StringComparison.OrdinalIgnoreCase));
            if (duplicate)
            {
                return Conflict(new { message = "Já existe outro livro com mesmo título e autor." });
            }

            existing.Title = normalizedTitle;
            existing.Author = normalizedAuthor;
            existing.Genre = request.Genre;
            existing.Price = request.Price;
            existing.Stock = request.Stock;

            var ok = _repository.Update(existing);
            if (!ok) return NotFound();

            var response = new BookResponse
            {
                Id = existing.Id,
                Title = existing.Title,
                Author = existing.Author,
                Genre = existing.Genre,
                Price = existing.Price,
                Stock = existing.Stock,
                CreatedAt = existing.CreatedAt,
                UpdatedAt = existing.UpdatedAt
            };

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _repository.Delete(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}