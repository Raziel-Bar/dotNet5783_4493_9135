using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using Dal;

namespace BlImplementation;

/// <summary>
/// no object class. made only for its method that we will use in the other BlImplementation files
/// the class does a reflection copy from one object to another, copying all values that has same name and type in both objects
/// </summary>
/// <typeparam name="TFrom">The type of the object we copy from</typeparam>
/// <typeparam name="TTO">The type of the object we copy to</typeparam>
public class PropertyCopier<TFrom, TTO> 
{
    /// <summary>
    /// copies all from's values into to if they have same name and type (exception for DO/BO.WINERYS type where we copy just if the name is equal) 
    /// </summary>
    /// <param name="from">the object we copy from</param>
    /// <param name="to">the object we copy to</param>
    /// <exception cref="BO.UnexpectedException">for developers. not suppose to happen</exception>
    public static void Copy(TFrom from, TTO to)
    {
        var fromProperties = from?.GetType().GetProperties();
        var toProperties = to?.GetType().GetProperties();

        if (fromProperties is null || toProperties is null)
        {
            throw new BO.UnexpectedException();
        }

        foreach (var fromProperty in fromProperties)
        {
            foreach (var toProperty in toProperties)
            {
                if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                {
                    toProperty.SetValue(to, fromProperty.GetValue(from));
                    break;
                }
                if (fromProperty.Name == toProperty.Name && fromProperty.Name == "Category") // special case for the category prop since the DO.WINERYS enum is similar to the BO.WINERYS
                {
                    if (toProperty.PropertyType.Name == "DO.WINERYS") // we always convert to the to's type
                    {
                        toProperty.SetValue(to, (DO.WINERYS)fromProperty.GetValue(from)!);
                        break;
                    }
                    toProperty.SetValue(to, (BO.WINERYS)fromProperty.GetValue(from)!);
                    break;
                }
            }
        }
    }
}