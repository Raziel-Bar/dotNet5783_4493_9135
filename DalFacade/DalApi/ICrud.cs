using DO;
namespace DalApi;


// when the interface is public so its default for the שדות is public


public interface ICrud<T>
{
    int Add(T t);
    void Delete(int id);
    void Update(T t);
    T Get(int id);
    IEnumerable<T> GetList();
}
