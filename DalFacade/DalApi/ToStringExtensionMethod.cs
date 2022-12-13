using System.Collections;
using System.Reflection;
using System.Text;
namespace DalApi;

public static class ToStringExtensionMethod
{
    /// <summary>
    /// developer method to pring the data of a given entity - extension of the ToString() for all entities
    /// </summary>
    /// <typeparam name="Entity">the given entity type</typeparam>
    /// <param name="entity">the given entity object</param>
    /// <returns>the final string with all the entity's data</returns>
    public static string ToStringProperty<Entity>(this Entity entity)
    {
        StringBuilder stringBuilder = new StringBuilder(); // we use string builder so we won't create multiple strings with every change we do
        string result = entity.helpToStringProperty(stringBuilder).ToString();
        stringBuilder.Clear();
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <param name="entity">the given entity object</param>
    /// <param name="stringBuilder">the string we build with all data</param>
    /// <returns>the final string with all the entity's data</returns>
    private static StringBuilder helpToStringProperty<Entity>(this Entity entity, StringBuilder stringBuilder)
    {
        IEnumerable<PropertyInfo> propertyInfos = entity!.GetType().GetProperties(); // we extract all props

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            object? value = propertyInfo.GetValue(entity);

            if (value is IEnumerable && value is not string)
            {
                var items = (IEnumerable)value;

                var innerItemsType = propertyInfo.PropertyType.GetGenericArguments().Single();

                foreach (var item in items)
                {
                    if (innerItemsType.IsValueType)
                        stringBuilder.Append(item.ToString() + "\n");

                    else
                        item.helpToStringProperty(stringBuilder); // we add the inner content of the Ienumerable using the same method with recursion
                }
            }
            else
            {
                stringBuilder.Append(propertyInfo.Name);
                stringBuilder.Append(": ");
                stringBuilder.Append(value);
                stringBuilder.Append('\n');
            }
        }
        return stringBuilder;
    }
}
