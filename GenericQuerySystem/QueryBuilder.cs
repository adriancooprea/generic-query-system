using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Enums;
using GenericQuerySystem.Interfaces;

namespace GenericQuerySystem
{
    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private readonly IQueryCompiler<T> _queryCompiler;

        public QueryBuilder(IQueryCompiler<T> queryCompiler)
        {
            _queryCompiler = queryCompiler;
        }

        public Predicate<T> BuildOrPredicate(Predicate<T> leftPredicate, Predicate<T> rightPredicate)
        {
            Contract.Requires(leftPredicate != null || rightPredicate != null, "At least one predicate must not be null.");

            if (leftPredicate == null)
            {
                return rightPredicate;
            }
            if (rightPredicate == null)
            {
                return leftPredicate;
            }

            return item => leftPredicate(item) || rightPredicate(item);
        }

        public Predicate<T> BuildAndPredicate(Predicate<T> leftPredicate, Predicate<T> rightPredicate)
        {
            Contract.Requires(leftPredicate != null || rightPredicate != null, "At least one predicate must not be null.");

            if (leftPredicate == null)
            {
                return rightPredicate;
            }
            if (rightPredicate == null)
            {
                return leftPredicate;
            }

            return item => leftPredicate(item) && rightPredicate(item);
        }

        public Predicate<T> BuildRulesPredicate(IList<QueryRule> queryRules)
        {
            if (queryRules == null || queryRules.Count == 0)
            {
                return item => true;
            }

            try
            {
                var rulesPredicate = BuildOrPredicate(null, item => _queryCompiler.CompileRule(queryRules[0])(item));
                rulesPredicate = queryRules.Aggregate(
                    rulesPredicate,
                    (current, rule) =>
                    {
                        var compiledRule = _queryCompiler.CompileRule(rule);
                        if (rule.LogicalOperation == LogicalOperation.AND)
                        {
                            return BuildAndPredicate(current, item => compiledRule(item));
                        }

                        return BuildOrPredicate(current, item => compiledRule(item));
                    });

                return rulesPredicate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return item => false;
            }
        }

        public Predicate<T> BuildGroupPredicate(QueryGroup queryGroup)
        {
            Contract.Requires(queryGroup != null, "Query group cannot be null.");

            if (!queryGroup.HasChildren)
            {
                return BuildRulesPredicate(queryGroup.Rules);
            }

            Predicate<T> groupPredicate = null;

            if (queryGroup.Rules.Count > 0)
            {
                groupPredicate = BuildRulesPredicate(queryGroup.Rules);
            }

            var groupChildrenPredicate = BuildGroupPredicate(queryGroup.InnerGroups[0]);
            if (queryGroup.InnerGroups.Count > 1)
            {
                groupChildrenPredicate = queryGroup.InnerGroups.Aggregate(
                    groupChildrenPredicate,
                    (current, group) =>
                    {
                    if (group.LogicalOperation == LogicalOperation.AND)
                        {
                            return BuildAndPredicate(current, BuildGroupPredicate(@group));
                        }

                        return BuildOrPredicate(current, BuildGroupPredicate(@group));
                    });
            }

            if (groupPredicate != null)
            {
                if (queryGroup.InnerGroups[0].LogicalOperation == LogicalOperation.AND)
                {
                    return BuildAndPredicate(groupPredicate, groupChildrenPredicate);
                }

                return BuildOrPredicate(groupPredicate, groupChildrenPredicate);
            }
            return item => groupChildrenPredicate(item);
        }

        public Predicate<T> BuildGroupsPredicate(IList<QueryGroup> queryGroups)
        {
            Contract.Requires(queryGroups != null && queryGroups.Count > 0, "Query groups cannot be null or empty.");

            var groupsPredicate = BuildGroupPredicate(queryGroups[0]);
            if (queryGroups.Count > 1)
            {
                groupsPredicate = queryGroups.Aggregate(
                    groupsPredicate,
                    (current, group) =>
                    {
                    if (group.LogicalOperation == LogicalOperation.AND)
                    {
                        return BuildAndPredicate(current, BuildGroupPredicate(@group));
                    }

                    return BuildOrPredicate(current, BuildGroupPredicate(@group));
                    });
            }

            return item => groupsPredicate(item);
        }
    }
}
