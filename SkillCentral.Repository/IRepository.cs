using System.Linq.Expressions;


namespace SkillCentral.Repository;
public interface IRepository
{
    #region GetAll
    IQueryable<T> GetAll<T>() where T : class;
    Task<IQueryable<T>> GetAllAsync<T>() where T : class;


    IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<IQueryable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    #endregion

    #region GetById
    T GetById<T>(int id) where T : class;
    T GetById<T>(params object[] compositKey) where T : class;
    T GetById<T>(string primaryKeyValue) where T : class;
    T GetById<T>(Expression<Func<T, bool>> predicate) where T : class;


    Task<T> GetByIdAsync<T>(int id) where T : class;
    Task<T> GetByIdAsync<T>(params object[] strCompositKey) where T : class;
    Task<T> GetByIdAsync<T>(string primaryKeyValue) where T : class;
    Task<T> GetByIdAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    #endregion

    #region Insert
    T Insert<T>(T entity) where T : class;
    Task<T> InsertAsync<T>(T entity) where T : class;

    List<T> Insert<T>(List<T> entityList) where T : class;
    Task<List<T>> InsertAsync<T>(List<T> entityList) where T : class;

    #endregion

    #region Update
    void Update<T>(T entity) where T : class;
    Task UpdateAsync<T>(T entity) where T : class;
    #endregion

}
