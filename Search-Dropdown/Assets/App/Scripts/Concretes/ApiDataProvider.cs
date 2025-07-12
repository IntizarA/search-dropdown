using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.Abstracts;
using UnityEngine;
using UnityEngine.Networking;

namespace App.Scripts.Concretes
{
    public class ApiDataProvider<T>:IDataProvider<T>
    {
        public int TotalCount { get; private set; }
        private string baseUrl;
        private readonly IApiResponseParser<T> _parser;

        public ApiDataProvider(string url, IApiResponseParser<T> parser)
        {
            baseUrl = url;
            _parser = parser;
        }


        public async Task<List<T>> GetDataAsync(int page, int pageSize,string query=null)
        {
            int skip=(page-1)*pageSize;
            
            string url=$"{baseUrl}/search?q={(string.IsNullOrEmpty (query) ? "" : UnityWebRequest.EscapeURL(query))}" +
                       $"&limit={pageSize}&skip={skip}";
            try
            {
                using (UnityWebRequest request=UnityWebRequest.Get(url))
                {
                    var operation = request.SendWebRequest();
                    while (!operation.isDone)
                        await Task.Yield();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"API Error:{request.error}");
                        return new List<T>();
                    }
                    string json = request.downloadHandler.text;
                    TotalCount = _parser.ExtractTotalCount(json);
                  return _parser.Parse(json);

                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error:{exception.Message}");
                return new List<T>();
            }
        }

    }
}