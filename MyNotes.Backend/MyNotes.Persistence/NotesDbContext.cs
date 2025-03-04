﻿using Microsoft.EntityFrameworkCore;
using MyNotes.Domain.Interfaces;
using MyNotes.Domain.Models;
using MyNotes.Persistence.EntityTypeConfigurations;

namespace MyNotes.Persistence;

public class NotesDbContext : DbContext, INotesDbContext
{
    public DbSet<Note> Notes { get; set; }

    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
