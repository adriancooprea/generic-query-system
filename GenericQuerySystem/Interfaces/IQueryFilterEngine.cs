using System.Collections.Generic;
using GenericQuerySystem.DTOs;

namespace GenericQuerySystem.Interfaces
{
    public interface IQueryFilterEngine<T> where T : class 
    {
        IEnumerable<T> FilterCollection(IEnumerable<T> collection, QueryGroup queryGroup);

        IEnumerable<T> SortCollection(IEnumerable<T> collection, IList<QuerySorter> querySorters);

        IEnumerable<dynamic> FilterFields(IEnumerable<T> collection, string[] fields);
    }
}