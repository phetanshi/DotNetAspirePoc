using System.Linq.Expressions;


namespace SkillCentral.Repository;

public interface IRepository
{
    #region GetList

    /// <summary>
    /// Fetches list of items from database
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <returns>IQueryable<T></returns>
    IQueryable<T> GetList<T>() where T : class;

    /// <summary>
    /// Fetches list of items from database (Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <returns>Task<IQueryable<T>></returns>
    Task<IQueryable<T>> GetListAsync<T>() where T : class;

    /// <summary>
    /// Fetches list of items from database which are matched with give prediction
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns>IQueryable<T></returns>
    IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class;

    /// <summary>
    /// Fetches list of items from database which are matched with give prediction (Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns>Task<IQueryable<T>></returns>
    Task<IQueryable<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    #endregion

    #region GetSingle
    /// <summary>
    /// Fetches single object which is matched with given id (primary key)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="id">Primary key of a table</param>
    /// <returns>single object of given type</returns>
    T GetSingle<T>(int id) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given keys (composit key)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="compositKey"> combination of  composit key values</param>
    /// <returns>single object of given type</returns>
    T GetSingle<T>(params object[] compositKey) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given key value which is of string type (primary key)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="primaryKeyValue">String type primary key value</param>
    /// <returns>single object of given type</returns>
    T GetSingle<T>(string primaryKeyValue) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given prediction
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns>single object of given type</returns>
    T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given id (primary key) (Async method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="id">Primary key of a table</param>
    /// <returns>single object of given type</returns>
    Task<T> GetSingleAsync<T>(int id) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given keys (composit key) (Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="compositKey"> combination of  composit key values</param>
    /// <returns>single object of given type</returns>
    Task<T> GetSingleAsync<T>(params object[] strCompositKey) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given key value which is of string type (primary key) (Async method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="primaryKeyValue">String type primary key value</param>
    /// <returns>single object of given type</returns>
    Task<T> GetSingleAsync<T>(string primaryKeyValue) where T : class;

    /// <summary>
    /// Fetches single object which is matched with given prediction (Async method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns>single object of given type</returns>
    Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    #endregion

    #region Create

    /// <summary>
    /// Inserts a new record into the specified table.
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entity">Object that need to be inserted in database</param>
    /// <returns>object of given type</returns>
    T Create<T>(T entity) where T : class;

    /// <summary>
    /// Inserts a new record into the specified table.(Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entity">Object that need to be inserted in database</param>
    /// <returns>object of given type</returns>
    Task<T> CreateAsync<T>(T entity) where T : class;

    /// <summary>
    /// Inserts list of new records into the specified table.
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entityList">List of objects that need to be insterted into database</param>
    /// <returns></returns>
    List<T> Create<T>(List<T> entityList) where T : class;

    /// <summary>
    /// Inserts list of new records into the specified table. (Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entityList">List of objects that need to be insterted into database</param>
    /// <returns></returns>
    Task<List<T>> CreateAsync<T>(List<T> entityList) where T : class;

    #endregion

    #region Update

    /// <summary>
    /// Updates a record that already exists in the database
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entity">Object with updated values. Primary key value should be present</param>
    int Update<T>(T entity) where T : class;

    /// <summary>
    /// Updates a record that already exists in the database (Async)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entity">Object with updated values. Primary key value should be present</param>
    Task<int> UpdateAsync<T>(T entity) where T : class;

    /// <summary>
    /// Updates a list of records which are already exists in the database
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entities">List of objects with updated values. Primary key value should be present in each object</param>
    int Update<T>(List<T> entities) where T : class;

    /// <summary>
    /// Updates a list of records which are already exists in the database (Async)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="entities">List of objects with updated values. Primary key value should be present in each object</param>
    /// <returns></returns>
    Task<int> UpdateAsync<T>(List<T> entities) where T : class;

    #endregion

    #region Delete

    /// <summary>
    /// Delete a record from database table
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="id">Uniquely identified key value (primary key)</param>
    int Delete<T>(int id) where T : class;

    /// <summary>
    /// Delete a record from database table (Async method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="id">Uniquely identified key value (primary key)</param>
    Task<int> DeleteAsync<T>(int id) where T : class;

    /// <summary>
    /// Deleted a list of records whos primary key matches with the given ids
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="ids">primary key values array</param>
    int Delete<T>(params int[] ids) where T : class;

    /// <summary>
    /// Deleted a list of records whos primary key matches with the given ids (Async Method)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="ids">primary key values array</param>
    Task<int> DeleteAsync<T>(params int[] ids) where T : class;

    /// <summary>
    /// Deletes all records which are matched with given prediction.
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    int Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

    /// <summary>
    /// Deletes all records which are matched with given prediction. (Async)
    /// </summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    #endregion
}
