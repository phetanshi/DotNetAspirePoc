using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkillCentral.Repository;

public class Repository(DbContext database, IMapper mapper, ILogger<Repository> logger) : IRepository
{
    #region GetList
    public IQueryable<T> GetList<T>() where T : class
    {
        return database.Set<T>();
    }

    public IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return database.Set<T>().Where(predicate);
    }

    public async Task<IQueryable<T>> GetListAsync<T>() where T : class
    {
        var result = GetList<T>();
        return await Task.FromResult(result);
    }

    public async Task<IQueryable<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var result = GetList<T>(predicate);
        return await Task.FromResult(result);
    }
    #endregion

    #region GetSingle
    public T GetSingle<T>(int id) where T : class
    {
        return database.Set<T>().Find(id);
    }
    public T GetSingle<T>(params object[] compositKey) where T : class
    {
        return database.Set<T>().Find(compositKey);
    }
    public T GetSingle<T>(string primaryKeyValue) where T : class
    {
        return database.Set<T>().Find(primaryKeyValue);
    }
    public T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return database.Set<T>().FirstOrDefault(predicate);
    }


    public async Task<T> GetSingleAsync<T>(int id) where T : class
    {
        return await database.Set<T>().FindAsync(id);
    }
    public async Task<T> GetSingleAsync<T>(params object[] strCompositKey) where T : class
    {
        return await database.Set<T>().FindAsync(strCompositKey);
    }
    public async Task<T> GetSingleAsync<T>(string primaryKeyValue) where T : class
    {
        return await database.Set<T>().FindAsync(primaryKeyValue);
    }
    public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return await database.Set<T>().FirstOrDefaultAsync(predicate);
    }
    #endregion

    #region Create
    public T Create<T>(T entity) where T : class
    {
        database.Set<T>().Add(entity);
        database.SaveChanges();
        return entity;
    }

    public List<T> Create<T>(List<T> entityList) where T : class
    {
        database.Set<T>().AddRange(entityList);
        database.SaveChanges();
        return entityList;
    }

    public async Task<T> CreateAsync<T>(T entity) where T : class
    {
        await database.Set<T>().AddAsync(entity);
        await database.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> CreateAsync<T>(List<T> entityList) where T : class
    {
        await database.Set<T>().AddRangeAsync(entityList);
        await database.SaveChangesAsync();
        return entityList;
    }

    public async Task<TDest> CreateWithMapperAsync<TSource, TDest>(TSource source) where TDest : class
    {
        var entity = mapper.Map<TDest>(source);
        await database.Set<TDest>().AddAsync(entity);
        await database.SaveChangesAsync();
        return entity;
    }
    #endregion

    #region Update
    public int Update<T>(T entity) where T : class
    {
        database.Entry(entity).State = EntityState.Modified;
        return database.SaveChanges();
    }

    public async Task<int> UpdateAsync<T>(T entity) where T : class
    {
        database.Entry(entity).State = EntityState.Modified;
        return await database.SaveChangesAsync();
    }
    public int Update<T>(List<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            database.Entry(entity).State = EntityState.Modified;
        }
        return database.SaveChanges();
    }

    public async Task<int> UpdateAsync<T>(List<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            database.Entry(entity).State = EntityState.Modified;
        }
        return await database.SaveChangesAsync();
    }

    #endregion

    #region Delete
    public int Delete<T>(int id) where T : class
    {
        T? set = database.Set<T>().Find(id);
        if (set != null)
        {
            database.Set<T>().Remove(set);
            return database.SaveChanges();
        }
        return 0;
    }

    public async Task<int> DeleteAsync<T>(int id) where T : class
    {
        T? set = await database.Set<T>().FindAsync(id);
        if (set != null)
        {
            database.Set<T>().Remove(set);
            return await database.SaveChangesAsync();
        }
        return 0;
    }
    public int Delete<T>(params int[] ids) where T : class
    {
        foreach (var id in ids)
        {
            T? set = database.Set<T>().Find(id);
            if (set != null)
            {
                database.Set<T>().Remove(set);
            }
        }
        return database.SaveChanges();
    }

    public async Task<int> DeleteAsync<T>(params int[] ids) where T : class
    {
        foreach (int id in ids)
        {
            T? set = await database.Set<T>().FindAsync(id);
            if (set != null)
            {
                database.Set<T>().Remove(set);
            }
        }
        return await database.SaveChangesAsync();
    }

    public int Delete<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var sets = database.Set<T>().Where(predicate);
        foreach (var set in sets)
        {
            database.Set<T>().Remove(set);
        }
        return database.SaveChanges();
    }

    public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        var sets = database.Set<T>().Where(predicate);
        foreach (var set in sets)
        {
            database.Set<T>().Remove(set);
        }
        return await database.SaveChangesAsync();
    }
    #endregion
}
