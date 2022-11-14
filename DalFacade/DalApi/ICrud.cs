using DO;
namespace DalApi;

public interface ICrud <T>
{
    public bool Add(T t);
    public bool Delete(T t);
    public bool Update(T t);
    public T Get(int ID);
    public IEnumerable<T> PrintList();
}
