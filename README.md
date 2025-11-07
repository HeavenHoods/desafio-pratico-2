# Gerenciador de Livros — Guia de Uso

Este projeto é uma API REST em .NET 8 para gerenciar livros de uma livraria, com CRUD completo, validações, documentação via Swagger e uma interface web simples (estática) para consumo dos endpoints.

## Executando o projeto

1. Pré-requisitos: .NET SDK 8.
2. Na pasta do projeto, execute:

```
dotnet run
```

Ao iniciar em ambiente de desenvolvimento, você verá:
- API e Swagger: `http://localhost:5220/swagger`
- Interface web (UI): `http://localhost:5220/`

> Observação: também há perfil HTTPS (`https://localhost:7189`) no `launchSettings.json`.

---

## Swagger — Documentação e Testes

- Abra `http://localhost:5220/swagger`.
- Lá você pode:
  - Explorar todos os endpoints (`GET`, `POST`, `PUT`, `DELETE`).
  - Expandir um endpoint, clicar em “Try it out”, preencher os campos e enviar a requisição.
  - Ver o corpo de resposta e os códigos de status.

### Endpoints principais

- `POST /api/books` — Cria um novo livro.
- `GET /api/books` — Lista livros (com filtros opcionais).
- `GET /api/books/{id}` — Busca por ID.
- `PUT /api/books/{id}` — Atualiza um livro.
- `DELETE /api/books/{id}` — Exclui um livro.

### Regras de negócio e validações

- `title` e `author`: entre 2 e 120 caracteres; não pode haver duplicidade da combinação `title + author`.
- `genre`: deve estar entre os gêneros permitidos (case-insensitive):
  - `ficção`, `romance`, `mistério`, `fantasia`, `aventura`, `biografia`, `história`, `drama`, `terror`, `sci-fi`, `poesia`, `conto`.
- `price >= 0` e `stock >= 0`.
- Timestamps: `CreatedAt` é definido na criação; `UpdatedAt` é atualizado em alterações.

### Exemplos rápidos (curl)

Criar:
```
curl -X POST http://localhost:5220/api/books \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Dom Casmurro",
    "author": "Machado de Assis",
    "genre": "romance",
    "price": 39.9,
    "stock": 10
  }'
```

Listar com filtros:
```
curl "http://localhost:5220/api/books?author=machado&genre=romance&minPrice=20&maxPrice=50"
```

Buscar por ID:
```
curl http://localhost:5220/api/books/{id}
```

Atualizar:
```
curl -X PUT http://localhost:5220/api/books/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Dom Casmurro",
    "author": "Machado de Assis",
    "genre": "romance",
    "price": 42.9,
    "stock": 12
  }'
```

Excluir:
```
curl -X DELETE http://localhost:5220/api/books/{id}
```

---

## Interface Web (UI)

- Abra `http://localhost:5220/`.
- Recursos disponíveis:
  - Formulário “Novo Livro” para criar registros.
  - Seção “Filtros” para listar por `title`, `author`, `genre`, `minPrice`, `maxPrice`, `minStock`, `maxStock`.
  - Tabela com ações “Editar” (inline) e “Excluir”.
- Comportamento:
  - Ao criar ou editar, a lista é atualizada automaticamente.
  - Erros são exibidos como avisos (validações, conflitos 409, erros 500).

### Dicas de uso

- Para editar, clique em “Editar” na linha desejada, altere os campos e salve.
- Para excluir, clique em “Excluir” e confirme.
- Para refinar a busca, use “Aplicar filtros”; “Limpar” remove os filtros.

---

## Códigos de Status

- `200 OK`: consultas e (se configurado) atualização com retorno de dados.
- `201 Created`: criação de novo recurso.
- `204 No Content`: atualização ou exclusão sem conteúdo no corpo.
- `400 Bad Request`: validações inválidas/dados incorretos.
- `404 Not Found`: recurso não encontrado.
- `409 Conflict`: conflito de dados (ex.: `title + author` já existente).
- `500 Internal Server Error`: erro inesperado; o pipeline retorna `ProblemDetails` em JSON.

---

## Estrutura relevante

- `Controllers/BooksController.cs`: endpoints e regras de negócio (duplicidade, filtros, validações via DTOs).
- `DTOs/*`: contratos de entrada e saída com DataAnnotations.
- `Models/*`: domínio com herança (`BaseEntity` → `InventoryItem` → `Book`) e timestamps.
- `Repositories/InMemoryBookRepository.cs`: CRUD em memória (preenche `CreatedAt`/`UpdatedAt`).
- `Program.cs`: DI, controllers, Swagger e tratamento global de exceções.
- `wwwroot/index.html`: interface web.

---

## Observações e próximos passos

- Persistência: atualmente o repositório é em memória. Para dados persistentes, substitua por EF Core e banco de dados (posso auxiliar na migração).
- Melhorias: paginação e ordenação na listagem, exportação CSV/JSON, testes automatizados.