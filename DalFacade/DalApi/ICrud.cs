namespace DalApi;

/// <summary>
/// Genreric CRUD interface for our entities' databases
/// </summary>
/// <typeparam name="T">
/// will present entities
/// </typeparam>
public interface ICrud<T> where T : struct
{
    int Add(T t);

    void Delete(int id);

    void Update(T t);

    T Get(int id);

    T Get(Func<T?, bool>? func);

    IEnumerable<T?> GetList(Func<T?, bool>? func = null);
}
