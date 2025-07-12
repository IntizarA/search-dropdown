using System.Collections.Generic;

namespace App.Scripts.Abstracts
{
    public interface IApiResponseParser<T>
    {
        public List<T> Parse(string json);
        int ExtractTotalCount(string json);
    }
}