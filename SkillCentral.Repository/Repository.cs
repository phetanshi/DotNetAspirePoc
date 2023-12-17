using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkillCentral.Repository;

public class Repository : IRepository
{
    private readonly ILogger<Repository> _logger;

    public Repository(DbContext database, ILogger<Repository> logger)
    {
        Database = database;
        this._logger = logger;
    }

    public DbContext Database { get; }

    #region GetAll
    public IQueryable<T> GetAll<T>() where T : class
    {
        return Database.Set<T>();
    }

    public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return Database.Set<T>().Where(predicate);
    }

    public async Task<IQueryable<T>> GetAllAsync<T>() where T : class
    {
        var result = GetAll<T>();
        return await Task.FromResult(result);
    }

    public async Task<IQueryable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var result = GetAll<T>(predicate);
        return await Task.FromResult(result);
    }
    #endregion

    #region GetById
    public T GetById<T>(int id) where T : class
    {
        return Database.Set<T>().Find(id);
    }
    public T GetById<T>(params object[] compositKey) where T : class
    {
        return Database.Set<T>().Find(compositKey);
    }
    public T GetById<T>(string primaryKeyValue) where T : class
    {
        return Database.Set<T>().Find(primaryKeyValue);
    }
    public T GetById<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return Database.Set<T>().FirstOrDefault(predicate);
    }


    public async Task<T> GetByIdAsync<T>(int id) where T : class
    {
        return await Database.Set<T>().FindAsync(id);
    }
    public async Task<T> GetByIdAsync<T>(params object[] strCompositKey) where T : class
    {
        return await Database.Set<T>().FindAsync(strCompositKey);
    }
    public async Task<T> GetByIdAsync<T>(string primaryKeyValue) where T : class
    {
        return await Database.Set<T>().FindAsync(primaryKeyValue);
    }
    public async Task<T> GetByIdAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return await Database.Set<T>().FirstOrDefaultAsync(predicate);
    }
    #endregion

    #region Insert
    public T Insert<T>(T entity) where T : class
    {
        Database.Set<T>().Add(entity);
        Database.SaveChanges();
        return entity;
    }

    public List<T> Insert<T>(List<T> entityList) where T : class
    {
        Database.Set<T>().AddRange(entityList);
        Database.SaveChanges();
        return entityList;
    }

    public async Task<T> InsertAsync<T>(T entity) where T : class
    {
        await Database.Set<T>().AddAsync(entity);
        await Database.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> InsertAsync<T>(List<T> entityList) where T : class
    {
        await Database.Set<T>().AddRangeAsync(entityList);
        await Database.SaveChangesAsync();
        return entityList;
    }
    #endregion

    #region Update
    public void Update<T>(T entity) where T : class
    {
        Database.Entry(entity).State = EntityState.Modified;
        Database.SaveChanges();
    }

    public async Task UpdateAsync<T>(T entity) where T : class
    {
        Database.Entry(entity).State = EntityState.Modified;
        await Database.SaveChangesAsync();
    }
    #endregion
}
