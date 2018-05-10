using GenericQuerySystem.Interfaces;
using SimpleInjector;

namespace GenericQuerySystem.Infrastructure
{
    internal class IoC
    {
        internal static readonly Container Container;

        static IoC()
        {
            Container = new Container();
            RegisterDependencies();
        }

        internal static T Get<T>() where T : class
        {
            return Container.GetInstance<T>();
        }

        private static void RegisterDependencies()
        {
            Container.Register(typeof(IQueryCompiler<>), typeof(QueryCompiler<>));
            Container.Register(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            Container.Register(typeof(IQueryFilterEngine<>), typeof(QueryFilterEngine<>));
        }
    }
}