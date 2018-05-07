using System;
using GenericQuerySystem.DTOs;

namespace GenericQuerySystem.Interfaces
{
    public interface IQueryCompiler<in T> where T : class
    {
        Predicate<T> CompileRule(QueryRule queryRule);
    }
}
