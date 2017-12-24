using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestService.Utils
{
    public static class Extensions
    {
        [ThreadStatic]
        private static Dictionary<Type, PropertyInfo[]> _copyInterfacePropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        public static void CopyPublicPropertyTo<T,Result>(this T source, Result destination)
        {
            Type type = typeof(T);
            
            // See if we have the public properties of the type cached. If not, get them and cache them.
            PropertyInfo[] properties;
            _copyInterfacePropertiesCache.TryGetValue(type, out properties);
            if (properties == null)
            {
                properties = GetInterfaceProperties(type);
                _copyInterfacePropertiesCache.Add(type, properties);
            }

            // Copy the properties.
            foreach (PropertyInfo propertyInfo in properties)
            {
                propertyInfo.GetSetMethod().Invoke(destination, new object[] { propertyInfo.GetGetMethod().Invoke(source, null) });
            }
        }

        private static PropertyInfo[] GetInterfaceProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (Type baseType in type.GetInterfaces())
            {
                ArrayAppender<PropertyInfo>(ref properties, GetInterfaceProperties(baseType));
            }

            // Done.
            return properties;
        }

        private static void ArrayAppender<T>(ref T[] array, T[] newItems)
        {
            Array.Resize<T>(ref array, array.Length + newItems.Length);
            Array.Copy(newItems, 0, array, array.Length - newItems.Length, newItems.Length);
        }
    }
}