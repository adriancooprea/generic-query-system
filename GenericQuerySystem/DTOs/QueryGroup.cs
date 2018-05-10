using System.Collections.Generic;
using GenericQuerySystem.Enums;

namespace GenericQuerySystem.DTOs
{
    public class QueryGroup
    {
        public QueryGroup(IList<QueryRule> rules, IList<QueryGroup> innerGroups, IList<QuerySorter> sortingRules, LogicalOperation logicalOperation)
        {
            this.Rules = rules;
            this.InnerGroups = innerGroups;
            this.LogicalOperation = logicalOperation;
            this.SortingRules = sortingRules;
        }

        public QueryGroup(IList<QueryRule> rules) : this(rules, new List<QueryGroup>(), new List<QuerySorter>(), LogicalOperation.And)
        {
        }

        public QueryGroup(QueryRule queryRule) : this(new List<QueryRule> { queryRule }, new List<QueryGroup>(), new List<QuerySorter>(),
            LogicalOperation.And)
        {
        }

        public QueryGroup() : this(new List<QueryRule>(), new List<QueryGroup>(), new List<QuerySorter>(), LogicalOperation.And)
        {
        }

        public QueryGroup And(QueryRule queryRule)
        {
            queryRule.LogicalOperation = LogicalOperation.And;
            this.Rules.Add(queryRule);

            return this;
        }

        public QueryGroup And(QueryGroup queryGroup)
        {
            queryGroup.LogicalOperation = LogicalOperation.And;
            this.InnerGroups.Add(queryGroup);

            return this;
        }

        public QueryGroup Or(QueryGroup queryGroup)
        {
            queryGroup.LogicalOperation = LogicalOperation.Or;
            this.InnerGroups.Add(queryGroup);

            return this;
        }

        public QueryGroup Or(QueryRule queryRule)
        {
            queryRule.LogicalOperation = LogicalOperation.Or;
            this.Rules.Add(queryRule);

            return this;
        }

        public QueryGroup AscendingBy(QuerySorter querySorter)
        {
            querySorter.SortingOperation = SortingOperation.Ascending;
            this.SortingRules.Add(querySorter);

            return this;
        }

        public QueryGroup DescendingBy(QuerySorter querySorter)
        {
            querySorter.SortingOperation = SortingOperation.Descending;
            this.SortingRules.Add(querySorter);

            return this;
        }

        public long Id { get; set; }

        public IList<QueryGroup> InnerGroups { get; set; }

        public IList<QueryRule> Rules { get; set; }

        public IList<QuerySorter> SortingRules { get; set; }

        public LogicalOperation LogicalOperation { get; set; }

        public bool HasChildren => InnerGroups.Count > 0;
    }
}