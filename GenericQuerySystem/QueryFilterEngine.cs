using System.Collections.Generic;
using System.Linq;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Enums;
using GenericQuerySystem.Extensions;
using GenericQuerySystem.Interfaces;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem
{
    public class QueryFilterEngine<T> : IQueryFilterEngine<T> where T : class
    {
        private readonly IQueryBuilder<T> _queryBuilder;
        public QueryFilterEngine(
            IQueryBuilder<T> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }


        public IEnumerable<T> FilterCollection(IEnumerable<T> collection, QueryGroup queryGroup)
        {
            if (!queryGroup.HasChildren && queryGroup.Rules.Count <= 0) return collection;

            var predicate = _queryBuilder.BuildGroupPredicate(queryGroup);

            return collection.Where(x => predicate(x));
        }

        public IEnumerable<T> SortCollection(IEnumerable<T> collection, IList<QuerySorter> querySorters)
        {
            ConditionChecker.Requires(collection != null, "Occurrences list cannot be null.");

            if (querySorters == null || querySorters.Count == 0)
            {
                return collection;
            }

            var targetedType = typeof(T);
            IOrderedEnumerable<T> result = querySorters[0].SortingOperation == SortingOperation.Ascending
                ? collection.OrderBy(x => targetedType.GetPropertyValue(querySorters[0].Field, x))
                : collection.OrderByDescending(
                    x => targetedType.GetPropertyValue(querySorters[0].Field, x));

            if (querySorters.Count > 1)
            {
                result = querySorters.Skip(1).Aggregate(
                    (IOrderedEnumerable<T>)result,
                    (current, thenBy) => thenBy.SortingOperation == SortingOperation.Ascending
                        ? current.ThenBy(x => targetedType.GetPropertyValue(thenBy.Field, x))
                        : current.ThenByDescending(x => targetedType.GetPropertyValue(thenBy.Field, x)));
            }

            return result;
        }

        public IEnumerable<dynamic> FilterFields(IEnumerable<T> collection, string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                return collection;
            }

            return collection.Select(x => x.GetType().GetPropertiesAsObject(fields, x));
        }
    }
}