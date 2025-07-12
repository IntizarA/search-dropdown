using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDataProvider<T>
{
    Task<List<T>> GetDataAsync(int page, int pageSize,string query=null);
    int TotalCount { get; }
}
