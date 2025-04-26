using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Biblioteca.Dto.Livro;
using Biblioteca.Models;
using Biblioteca.Services.Livro;
using Xunit;

namespace BibliotecaTests.ServicesTests;

public class LivroServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly LivroService _livroService;

    public LivroServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _livroService = new LivroService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task BuscarLivroPorId_DeveRetornarLivroQuandoExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.BuscarLivroPorId(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.NotNull(resultado.Dados);
        Assert.Equal("Dom Casmurro", resultado.Dados.Titulo);
    }

    [Fact]
    public async Task BuscarLivroPorId_DeveRetornarMensagemQuandoNaoExistir()
    {
        // Act
        var resultado = await _livroService.BuscarLivroPorId(99);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(resultado.Status);
        Assert.Equal("Nenhum registro localizado!", resultado.Mensagem);
    }

    [Fact]
    public async Task BuscarLivrosPorIdAutor_DeveRetornarLivrosQuandoAutorExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.BuscarLivrorPorIdAutor(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.NotEmpty(resultado.Dados);
        Assert.Equal("Dom Casmurro", resultado.Dados[0].Titulo);
    }

    [Fact]
    public async Task CriarLivro_DeveCriarNovoLivro()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        var livroCriacaoDto = new LivroCriacaoDto
        {
            Titulo = "Memórias Póstumas de Brás Cubas",
            idAutor = 1
        };

        // Act
        var resultado = await _livroService.CriarLivro(livroCriacaoDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.Contains(resultado.Dados, l => l.Titulo == "Memórias Póstumas de Brás Cubas");
    }

    [Fact]
    public async Task EditarLivro_DeveEditarLivroExistente()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        var livroEdicaoDto = new LivroEdicaoDto
        {
            Id = 1,
            Titulo = "Memórias Póstumas de Brás Cubas",
            idAutor = 1
        };

        // Act
        var resultado = await _livroService.EditarLivro(livroEdicaoDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        var livroEditado = await _context.Livros.FirstOrDefaultAsync(l => l.Id == 1);
        Assert.Equal("Memórias Póstumas de Brás Cubas", livroEditado.Titulo);
    }

    [Fact]
    public async Task ExcluirLivro_DeveRemoverLivro()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.ExcluirLivro(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        var livroRemovido = await _context.Livros.FirstOrDefaultAsync(l => l.Id == 1);
        Assert.Null(livroRemovido);
    }

    [Fact]
    public async Task ListarLivros_DeveRetornarListaDeLivros()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro1 = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        var livro2 = new LivroModel { Id = 2, Titulo = "Memórias Póstumas de Brás Cubas", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.AddRange(livro1, livro2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.ListarLivros();

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.Equal(2, resultado.Dados.Count);
    }
}
