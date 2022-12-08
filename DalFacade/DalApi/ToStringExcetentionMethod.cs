using System.Collections;
using System.Reflection;
using System.Text;

namespace Tools
{
    public static class ToStringExcetentionMethod
    {

        public static string ToStringProperty<Entity>(this Entity entity)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string result = entity.helpToStringProperty(stringBuilder).ToString();
            stringBuilder.Clear();
            return result;
        }

        private static StringBuilder helpToStringProperty<Entity>(this Entity entity, StringBuilder stringBuilder)
        {
            IEnumerable<PropertyInfo> propertyInfos = entity!.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object? value = propertyInfo.GetValue(entity);

                if (value is IEnumerable && value is not string) // please call bob the builder...
                {
                    var items = (IEnumerable)value;

                    var innerItemsType = propertyInfo.PropertyType.GetGenericArguments().Single();

                    foreach (var item in items)
                    {
                        if (innerItemsType.IsValueType)
                            stringBuilder.Append(item.ToString() + "\n");

                        else
                             helpToStringProperty(item, stringBuilder);
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
}
