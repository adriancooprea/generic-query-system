using System.Diagnostics.Contracts;
using GenericQuerySystem.Enums;

namespace GenericQuerySystem.DTOs
{
    public class QuerySorter
    {
        public QuerySorter(string field, SortingOperation sortingOperation)
        {
            Contract.Requires(string.IsNullOrEmpty(field) == false);

            this.Field = field;
            this.SortingOperation = sortingOperation;
        }

        // Mocking
        protected QuerySorter() {}

        public long Id { get; set; }

        public string Field { get; set; }

        public SortingOperation SortingOperation { get; set; }
    }
}
