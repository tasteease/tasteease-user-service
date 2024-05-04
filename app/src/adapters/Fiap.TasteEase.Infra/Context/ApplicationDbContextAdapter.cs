﻿using Fiap.TasteEase.Application.Ports;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fiap.TasteEase.Infra.Context;

internal class ApplicationDbContextAdapter : IApplicationDbContext
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextAdapter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IApplicationDbContextTransaction> BeginTransactionAsync()
    {
        var tx = await _dbContext.Database.BeginTransactionAsync();
        return new ApplicationDbContextTransactionAdapter(tx);
    }

    public Task SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public Task CommitAsync(IApplicationDbContextTransaction tx)
    {
        return tx.CommitAsync();
    }
}

internal class ApplicationDbContextTransactionAdapter : IApplicationDbContextTransaction
{
    private readonly IDbContextTransaction _transaction;

    public ApplicationDbContextTransactionAdapter(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync()
    {
        return _transaction.CommitAsync();
    }

    public Task RollbackAsync()
    {
        return _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}