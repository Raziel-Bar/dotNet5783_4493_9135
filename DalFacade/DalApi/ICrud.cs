using DO;
namespace DalApi;

/// <summary>
/// Genreric CRUD interface for our entities' databases
/// </summary>
/// <typeparam name="T">
/// will present entities
/// </typeparam>
public interface ICrud<T>
{
    int Add(T t);
    void Delete(int id);
    void Update(T t);
    T Get(int id);
    IEnumerable<T> GetList();
}
