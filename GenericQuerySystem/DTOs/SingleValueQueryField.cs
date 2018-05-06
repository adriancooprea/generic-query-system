using System.Diagnostics.Contracts;

namespace GenericQuerySystem.DTOs
{
    public class SingleValueQueryField : QueryField
    {
        public SingleValueQueryField(string name, string type, object value) : base(name, type)
        {
            Contract.Requires(value != null);
            this.Value = value;
        }

        // Mocking
        protected SingleValueQueryField() {}

        public object Value { get; set; }
    }
}
