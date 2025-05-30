﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JampotCapstone.Models;

namespace JampotCapstone.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Models.File> Files { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<TextElement> TextElements { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<Page> Pages { get; set; }
    
    public DbSet<PagePosition> PagePositions { get; set; }
}