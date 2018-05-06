using System;
using System.Collections.Generic;
using GenericQuerySystem.DTOs;

namespace GenericQuerySystem.Interfaces
{
    public interface IQueryBuilder<T> where T : class
    {
        Predicate<T> BuildAndPredicate(Predicate<T> leftPredicate, Predicate<T> rightPredicate);

        Predicate<T> BuildOrPredicate(Predicate<T> leftPredicate, Predicate<T> rightPredicate);

        Predicate<T> BuildRulesPredicate(IList<QueryRule> queryRules);

        Predicate<T> BuildGroupPredicate(QueryGroup queryGroup);

        Predicate<T> BuildGroupsPredicate(IList<QueryGroup> queryGroups);
    }
}
