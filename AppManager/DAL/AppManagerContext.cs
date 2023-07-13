using System;
using System.Collections.Generic;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core;

public partial class AppManagerContext : DbContext
{
    public AppManagerContext()
    {
    }

    public AppManagerContext(DbContextOptions<AppManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

    public virtual DbSet<ToDoTask> ToDoTasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies().UseSqlServer(GetConString());

    private string GetConString()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appSettings.json");
        var config = builder.Build();
        return config.GetConnectionString("DefaultConnection");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Project>()
        //    .HasMany(sc => sc.ProjectUsers)
        //    .WithOne(s => s.Project)
        //    .HasForeignKey(sc => sc.ProjectId);
        //modelBuilder.Entity<User>()
        //    .HasMany(sc => sc.ProjectUsers)
        //    .WithOne(s => s.User)
        //    .HasForeignKey(sc => sc.UserId);
        modelBuilder.Entity<ToDoTask>()
            .HasOne(sc => sc.Project)
            .WithMany(sc => sc.ToDoTasks)
            .HasForeignKey(sc => sc.ProjectId);
        modelBuilder.Entity<Project>()
            .HasMany(sc => sc.ToDoTasks)
            .WithOne(s => s.Project)
            .HasForeignKey(sc => sc.ProjectId);
        modelBuilder.Entity<Notification>()
            .HasOne(sc => sc.User)
            .WithMany(sc => sc.Notifications)
            .HasForeignKey(sc => sc.UserId);
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(sc => sc.User)
        //    .WithMany(sc => sc.ProjectUsers)
        //    .HasForeignKey(sc => sc.UserId);
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(sc => sc.Project)
        //    .WithMany(sc => sc.ProjectUsers)
        //    .HasForeignKey(sc => sc.ProjectId);

        modelBuilder.Entity<ProjectUser>().HasKey(sc => new { sc.ProjectId, sc.UserId });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
