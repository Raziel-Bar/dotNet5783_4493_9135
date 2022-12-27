namespace BlApi;

/// <summary>
/// simple factory for getting the data from the BL
/// </summary>
public class Factory
{
    static public IBl Get() => new BlImplementation.Bl();
}
