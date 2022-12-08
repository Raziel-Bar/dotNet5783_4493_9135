using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

/// <summary>
/// no object class. made only for its method that we will use in the other BlImplementation files
/// the class does a reflection copy from one object to another, copying all values that has same name and type in both objects
/// </summary>
/// <typeparam name="TFrom">The type of the object we copy from</typeparam>
/// <typeparam name="TTO">The type of the object we copy to</typeparam>
public static class PropertyCopier
{

    /*//    public static TTO CopyPropTo<TFrom, TTO>(this TFrom from, TTO to)
    //    {
    //        var fromProperties = from?.GetType().GetProperties();

    //        var toProperties = to?.GetType().GetProperties();

    //        Dictionary<string, PropertyInfo> properties = toProperties.ToDictionary(key => key.Name, value => value);

    //        if (fromProperties is null || toProperties is null)
    //        {
    //            throw new BO.UnexpectedException();
    //        }

    //        foreach (var fromProperty in fromProperties)
    //        {
    //            var value = fromProperty.GetValue(from);

    //            if (properties.ContainsKey(fromProperty.Name))
    //            {
    //                if (fromProperty.PropertyType == toProperty.PropertyType)
    //                    toProperty.SetValue(to, value);

    //                else
    //                {
    //                    Type typeNullAbleTo = Nullable.GetUnderlyingType(toProperty.PropertyType)!;

    //                    if (typeNullAbleTo is not null)
    //                        toProperty.SetValue(to, Enum.ToObject(typeNullAbleTo, value));
    //                }
    //            }
    //        }
    //        return to;
    //    }*/

    public static TTO CopyPropTo<TFrom, TTO>(this TFrom from, TTO to)
    {
        var fromProperties = from?.GetType().GetProperties();

        var toProperties = to?.GetType().GetProperties();

        if (fromProperties is null || toProperties is null) throw new BO.UnexpectedException();
                 
        foreach (var fromProperty in fromProperties)
        {
            var value = fromProperty.GetValue(from);

            foreach (var toProperty in toProperties)
            {
                if (fromProperty.Name == toProperty.Name)
                {

                    if (fromProperty.PropertyType == toProperty.PropertyType)
                        toProperty.SetValue(to, value);

                    else
                    {
                        Type typeNullAbleTo = Nullable.GetUnderlyingType(toProperty.PropertyType)!;

                        if (typeNullAbleTo is not null)
                            toProperty.SetValue(to, Enum.ToObject(typeNullAbleTo, value!));
                    }
                }
            }
        }

        return to;
    }

    public static TTO CopyPropToStruct<TFrom, TTO>(this TFrom from, TTO to) where TTO : struct => (TTO)from.CopyPropTo(to as object);

    public static IEnumerable<TTO> CopyPropToList<TFrom, TTO>(this IEnumerable<TFrom> froms) where TTO : new() => from item in froms select item.CopyPropTo(new TTO());

}