using System.Diagnostics.Contracts;

namespace GenericQuerySystem.DTOs
{
    public abstract class QueryField
    {
        public QueryField(
            string name, 
            string type)
        {
            Contract.Requires(string.IsNullOrEmpty(name) == false);
            Contract.Requires(string.IsNullOrEmpty(type) == false);
            this.Name = name;
            this.Type = type;
        }

        // Mocking
        protected QueryField() {}

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
