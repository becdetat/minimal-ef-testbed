using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalEF
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object args)
        {
            if (args == null)
            {
                return new Dictionary<string, object>();
            }

            return TypeDescriptor.GetProperties(args).Cast<PropertyDescriptor>()
                .ToDictionary(
                    property => property.Name,
                    property => property.GetValue(args));
        }
    }
}
