namespace BlApi;

public class Factory
{
    static public IBl Get() => new BlImplementation.Bl();

}
