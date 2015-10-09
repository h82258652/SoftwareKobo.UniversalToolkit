using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SoftwareKobo.UniversalToolkit.Utils
{
    public static class ExpressionResolver
    {
        public static string ResolvePropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("not support expression.", nameof(propertyExpression));
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("member is not property.", nameof(propertyExpression));
            }

            return property.Name;
        }
    }
}