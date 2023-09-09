using System.Reflection;
using Common;
using Microsoft.EntityFrameworkCore;

namespace SqliteDatasource;

public sealed class ItemDataContext : DbContext
{
    public static ItemDataContext? Context;

    private ItemDataContext()
    {
    }

    public static ItemDataContext CreateContext()
    {
        if (Context is null)
        {
            Context = new ItemDataContext();
        }

        return Context;
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder
    ) =>
        optionsBuilder.UseSqlite(
            $"Data Source={Path.Combine(path1: Constants.DatabaseFolder, path2: Constants.AppDatabaseFileName)};");
}