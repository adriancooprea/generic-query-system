using GenericQuerySystem.Infrastructure;
using GenericQuerySystem.Interfaces;

namespace GenericQuerySystem
{
    public class QueryFactory<T> where T : class 
    {
        public static IQueryFilterEngine<T> BuildQueryFilterEngine()
        {
            return IoC.Get<IQueryFilterEngine<T>>();
        }
    }
}