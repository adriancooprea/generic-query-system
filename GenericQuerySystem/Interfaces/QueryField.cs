using GenericQuerySystem.Utils;

namespace GenericQuerySystem.Interfaces
{
    public abstract class QueryField
    {
        protected QueryField(
            string name,
            string type)
        {
            ConditionChecker.Requires(string.IsNullOrEmpty(name) == false);
            ConditionChecker.Requires(string.IsNullOrEmpty(type) == false);
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
