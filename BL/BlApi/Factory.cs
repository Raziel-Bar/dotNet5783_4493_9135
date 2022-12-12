using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public class Factory
{
    static public IBl Get() => new BlImplementation.Bl();   // שלב 4.1 ככה?

}
