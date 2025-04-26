﻿using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data;

public class AppDbContext : DbContext
{


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {

    }

    public virtual DbSet<Models.AutorModel> Autores { get; set; }
    public virtual DbSet<Models.LivroModel> Livros { get; set; }
}
