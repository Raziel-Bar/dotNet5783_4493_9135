
using BlApi;

namespace BlImplementation;

sealed public class Bl : IBl
{

    public ICart Cart => new Cart();

    public IOrder Order =>  new Order();

    public IProduct Product =>  new Product();
}
