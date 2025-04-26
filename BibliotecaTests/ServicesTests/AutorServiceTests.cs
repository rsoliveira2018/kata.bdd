using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.Dto.Autor;
using WebApi8.Models;
using WebApi8.Services.Autor;
using Xunit;

namespace WebApi8_UnitTesting.ServicesTests;

public class AutorServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AutorService _autorService;

    public AutorServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _autorService = new AutorService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task BuscarAutorPorId_DeveRetornarAutorQuandoExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.BuscarAutorPorId(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.NotNull(resultado.Dados);
        Assert.Equal("Machado", resultado.Dados.Nome);
    }

    [Fact]
    public async Task BuscarAutorPorId_DeveRetornarMensagemQuandoNaoExistir()
    {
        // Act
        var resultado = await _autorService.BuscarAutorPorId(99);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(resultado.Status);
        Assert.Equal("Nenhum registro localizado!", resultado.Mensagem);
    }

    [Fact]
    public async Task BuscarAutorPorIdLivro_DeveRetornarAutorQuandoLivroExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Fulano", Sobrenome = "Silva" };
        var livro = new LivroModel { Id = 1, Titulo = "Livro Teste", Autor = autor };

        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.BuscarAutorPorIdLivro(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.NotNull(resultado.Dados);
        Assert.Equal("Fulano", resultado.Dados.Nome);
    }

    [Fact]
    public async Task CriarAutor_DeveCriarNovoAutor()
    {
        // Arrange
        var autorCriacaoDto = new AutorCriacaoDto
        {
            Nome = "Novo",
            Sobrenome = "Autor"
        };

        // Act
        var resultado = await _autorService.CriarAutor(autorCriacaoDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        Assert.Contains(resultado.Dados, a => a.Nome == "Novo" && a.Sobrenome == "Autor");
    }

    [Fact]
    public async Task EditarAutor_DeveEditarAutorExistente()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Velho", Sobrenome = "Nome" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        var autorEdicaoDto = new AutorEdicaoDto
        {
            Id = 1,
            Nome = "NovoNome",
            Sobrenome = "NovoSobrenome"
        };

        // Act
        var resultado = await _autorService.EditarAutor(autorEdicaoDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        var autorAtualizado = await _context.Autores.FirstOrDefaultAsync(a => a.Id == 1);
        Assert.Equal("NovoNome", autorAtualizado.Nome);
    }

    [Fact]
    public async Task ExcluirAutor_DeveRemoverAutor()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "ParaExcluir", Sobrenome = "Teste" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.ExcluirAutor(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Status);
        var autorExcluido = await _context.Autores.FirstOrDefaultAsync(a => a.Id == 1);
        Assert.Null(autorExcluido);
    }
}
