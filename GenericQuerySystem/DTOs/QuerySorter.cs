using GenericQuerySystem.Enums;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem.DTOs
{
    public class QuerySorter
    {
        public QuerySorter(string field, SortingOperation sortingOperation)
        {
            ConditionChecker.Requires(string.IsNullOrEmpty(field) == false);

            this.Field = field;
            this.SortingOperation = sortingOperation;
        }

        public long Id { get; set; }

        public string Field { get; set; }

        public SortingOperation SortingOperation { get; set; }
    }
}
