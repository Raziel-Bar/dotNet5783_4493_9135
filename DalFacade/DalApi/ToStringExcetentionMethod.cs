using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

                if (value is IEnumerable && value is not string)
                {
                    var items = (IEnumerable)value;
                    foreach (var item in items)
                    {
                        item.helpToStringProperty(stringBuilder);

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
