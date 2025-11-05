using System.Linq.Expressions;
using System.Reflection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Extensions
{
    public static class EfFilterExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, QueryFilter[] filters)
            where T : class
        {
            if (filters == null || filters.Length == 0) return query;

            var param = Expression.Parameter(typeof(T), "e");
            Expression? totalBody = null;

            foreach (var f in filters)
            {
                var prop = GetPropertyOrThrow<T>(f.Column);
                var propExpr = Expression.Property(param, prop);
                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                Expression? columnPredicate = f.Operation switch
                {
                    Operation.eq => BuildEqSet(propExpr, underlyingType, f.Values),
                    Operation.like => BuildLike(propExpr, f.Values),
                    Operation.gt => BuildCompare(propExpr, underlyingType, f.Values, "gt"),
                    Operation.gte => BuildCompare(propExpr, underlyingType, f.Values, "gte"),
                    Operation.lt => BuildCompare(propExpr, underlyingType, f.Values, "lt"),
                    Operation.lte => BuildCompare(propExpr, underlyingType, f.Values, "lte"),
                    _ => throw new NotSupportedException($"Unsupported operation: {f.Operation}")
                };

                totalBody = totalBody is null ? columnPredicate : Expression.AndAlso(totalBody, columnPredicate);
            }

            if (totalBody is null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(totalBody, param);
            return query.Where(lambda);
        }

        private static Expression BuildEqSet(MemberExpression propExpr, Type underlyingType, string[] rawValues)
        {
            var validValues = rawValues.Where(v => !string.IsNullOrWhiteSpace(v)).ToArray();
            if (validValues.Length == 0)
                return Expression.Constant(false);

            var typedArray = Array.CreateInstance(underlyingType, validValues.Length);
            for (int i = 0; i < validValues.Length; i++)
                typedArray.SetValue(ConvertTo(validValues[i], underlyingType), i);

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(underlyingType);
            var listConst = Expression.Constant(typedArray, enumerableType);

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(underlyingType);

            Expression valueExpr = propExpr.Type == underlyingType
                ? propExpr
                : Expression.Convert(propExpr, underlyingType);

            return Expression.Call(containsMethod, listConst, valueExpr);
        }


        private static Expression BuildLike(MemberExpression propExpr, string[] values)
        {
            if (values.Length != 1)
                throw new InvalidOperationException("LIKE accepts exactly one value.");

            var pattern = Expression.Constant($"%{values[0]}%");
            var efFunctions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))!);

            var likeMethod = typeof(DbFunctionsExtensions)
                .GetMethod(nameof(DbFunctionsExtensions.Like),
                    new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;

            var nullCheck = Expression.NotEqual(propExpr, Expression.Constant(null, typeof(string)));
            var likeCall = Expression.Call(likeMethod, efFunctions, propExpr, pattern);

            return Expression.AndAlso(nullCheck, likeCall);
        }

        private static Expression BuildCompare(MemberExpression propExpr, Type underlyingType, string[] values, string op)
        {
            if (values.Length != 1)
                throw new InvalidOperationException($"Operation '{op}' accepts exactly one value.");

            var rhs = Expression.Constant(ConvertTo(values[0], underlyingType), underlyingType);
            Expression left = propExpr.Type == underlyingType
                ? propExpr
                : Expression.Convert(propExpr, underlyingType);

            return op switch
            {
                "gt" => Expression.GreaterThan(left, rhs),
                "gte" => Expression.GreaterThanOrEqual(left, rhs),
                "lt" => Expression.LessThan(left, rhs),
                "lte" => Expression.LessThanOrEqual(left, rhs),
                _ => throw new NotSupportedException(op)
            };
        }

        private static PropertyInfo GetPropertyOrThrow<T>(string name)
        {
            var prop = typeof(T).GetProperty(name);
            if (prop == null)
                throw new InvalidOperationException($"Property '{name}' not found on type {typeof(T).Name}.");
            return prop;
        }

        private static object ConvertTo(string input, Type targetType)
        {
            if (targetType.IsEnum)
            {
                if (int.TryParse(input, out var enumInt))
                    return Enum.ToObject(targetType, enumInt);

                return Enum.Parse(targetType, input, true);
            }

            if (targetType == typeof(string)) return input;
            if (targetType == typeof(int)) return int.Parse(input);
            if (targetType == typeof(long)) return long.Parse(input);
            if (targetType == typeof(decimal)) return decimal.Parse(input);
            if (targetType == typeof(bool)) return bool.Parse(input);
            if (targetType == typeof(DateTime)) return DateTime.Parse(input);

            return Convert.ChangeType(input, targetType);
        }
    }
}
