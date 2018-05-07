using System.Collections.Generic;
using GenericQuerySystem.Enums;

namespace GenericQuerySystem.DTOs
{
    public class QueryGroup
    {
        public QueryGroup(IList<QueryRule> rules, IList<QueryGroup> innerGroups, LogicalOperation logicalOperation)
        {
            this.Rules = rules;
            this.InnerGroups = innerGroups;
            this.LogicalOperation = logicalOperation;
        }

        public QueryGroup(IList<QueryRule> rules) : this(rules, new List<QueryGroup>(), LogicalOperation.And) {}

        public QueryGroup() : this(new List<QueryRule>(), new List<QueryGroup>(), LogicalOperation.And) {}

        public long Id { get; set; }

        public IList<QueryGroup> InnerGroups { get; set; }

        public IList<QueryRule> Rules { get; set; }

        public IList<QuerySorter> SortingRules { get; set; }

        public LogicalOperation LogicalOperation { get; set; }

        public bool HasChildren => InnerGroups.Count > 0;
    }
}
