using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace OwinWebApiSelfHost.Helpers
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor property in properties)
            {
                expando.Add(property.Name, property.GetValue(value));
            }
            return (ExpandoObject) expando;
        }

        public static IDictionary<string, object> ToDictionary(this object d)
        {
            return d.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(d, null));
        }
    }
}