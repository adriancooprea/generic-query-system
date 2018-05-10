using System;
using System.Linq.Expressions;
using System.Reflection;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Extensions;
using GenericQuerySystem.Interfaces;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem
{
    internal class QueryCompiler<T> : IQueryCompiler<T> where T : class
    {
        public QueryCompiler()
        {
        }

        Predicate<T> IQueryCompiler<T>.CompileRule(QueryRule queryRule)
        {
            ConditionChecker.Requires(queryRule != null, "Query Rule cannot be null.");

            // When migrating the database, some fields of the old DTO might be missing.
            if (!typeof(T).HasProperty(queryRule.Field))
            {
                return null;
            }

            var targetedType = Expression.Parameter(typeof(T));

            var leftMember = Expression.Property(targetedType, queryRule.Field);
            var propertyType = typeof(T).GetProperty(queryRule.Field)?.PropertyType;

            if (propertyType == null)
            {
                return null;
            }

            Expression expression;

            // The given rule is binary(aka : equal, less than , gte, etc.)
            if (Enum.TryParse(Operations.Get(queryRule.FieldOperation), out ExpressionType binaryOperator))
            {
                var queriedValue = GetQueriedValue(queryRule, propertyType);
                expression = Expression.MakeBinary(binaryOperator, leftMember, queriedValue);
            }
            else
            {
                // The given rule is a method(aka: Contains, Equals, etc.
                var queriedMethod = GetQueriedMethod(queryRule, propertyType);
                if (queriedMethod == null)
                {
                    return null;
                }

                var rightMember = GetRightMember(queryRule, queriedMethod);

                if (propertyType == typeof(string))
                {
                    var toLowerMethodInfo = typeof(string).GetMethod("ToLower", new Type[] { });
                    Expression leftMemberLowerCase = Expression.Call(leftMember, toLowerMethodInfo ?? throw new MissingMethodException());
                    Expression rightMemberLowerCase = Expression.Call(rightMember, toLowerMethodInfo ?? throw new MissingMethodException());

                    expression = Expression.Call(leftMemberLowerCase, queriedMethod, rightMemberLowerCase);
                }
                else
                {
                    expression = Expression.Call(leftMember, queriedMethod, rightMember);
                }
            }

            return Expression.Lambda<Predicate<T>>(expression, targetedType).Compile();
        }

        private static MethodInfo GetQueriedMethod(QueryRule queryRule, Type propertyType)
        {
            var method = propertyType.GetMethod(
                Operations.Get(queryRule.FieldOperation),
                new[] { propertyType }
                );

            return method;
        }

        private static ConstantExpression GetQueriedValue(QueryRule queryRule, Type propertyType)
        {
            ConstantExpression queriedValue;
            if (propertyType.IsEnum)
            {
                queriedValue = Expression.Constant(Enum.Parse(propertyType, queryRule.Value.ToString()));
            }
            else if (propertyType == typeof(TimeSpan))
            {
                queriedValue = Expression.Constant(TimeSpan.Parse(queryRule.Value.ToString()));
            }
            else if (propertyType == typeof(string))
            {
                queriedValue = Expression.Constant(Convert.ChangeType(queryRule.Value.ToString().Trim().ToLower(), propertyType));
            }
            else
            {
                queriedValue = Expression.Constant(Convert.ChangeType(queryRule.Value, propertyType));
            }

            return queriedValue;
        }

        private static ConstantExpression GetRightMember(QueryRule queryRule, MethodInfo queriedMethod)
        {
            var methodParameterType = queriedMethod.GetParameters()[0].ParameterType;
            return Expression.Constant(Convert.ChangeType(queryRule.Value, methodParameterType));
        }
    }
}