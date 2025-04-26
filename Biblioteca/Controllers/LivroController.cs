﻿using Microsoft.AspNetCore.Mvc;
using WebApi8.Dto.Livro;
using WebApi8.Models;
using WebApi8.Services.Livro;

namespace WebApi8.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LivroController : Controller
{
    private readonly ILivroInterface _livroInterface;
    public LivroController(ILivroInterface livroInterface)
    {
        _livroInterface = livroInterface;
    }

    [HttpGet("ListarLivros")]
    public async Task<ActionResult<ResponseModel<List<LivroModel>>>> ListarLivros()
    {
        var livros = await _livroInterface.ListarLivros();

        return Ok(livros);
    }

    [HttpGet("BuscarLivroPorId/{idLivro}")]
    public async Task<ActionResult<ResponseModel<LivroModel>>> BuscarLivroPorId(int idLivro)
    {
        var livro = await _livroInterface.BuscarLivroPorId(idLivro);

        return Ok(livro);
    }

    [HttpGet("BuscarLivrosPorIdAutor/{idAutor}")]
    public async Task<ActionResult<ResponseModel<List<LivroModel>>>> BuscarLivrosPorIdAutor(int idAutor)
    {
        var livros = await _livroInterface.BuscarLivrorPorIdAutor(idAutor);

        return Ok(livros);
    }

    [HttpPost("CriarLivro")]
    public async Task<ActionResult<ResponseModel<List<LivroModel>>>> CriarLivro(LivroCriacaoDto livroCriacaoDto)
    {
        var livros = await _livroInterface.CriarLivro(livroCriacaoDto);
        return Ok(livros);
    }

    [HttpPut("EditarLivro")]
    public async Task<ActionResult<ResponseModel<List<LivroModel>>>> EditarLivro(LivroEdicaoDto livroEdicaoDto)
    {
        var livros = await _livroInterface.EditarLivro(livroEdicaoDto);
        return Ok(livros);
    }
    [HttpDelete("ExcluirLivro")]
    public async Task<ActionResult<ResponseModel<List<LivroModel>>>> ExcluirLivro(int idLivro)
    {
        var livros = await _livroInterface.ExcluirLivro(idLivro);
        return Ok(livros);
    }
}
