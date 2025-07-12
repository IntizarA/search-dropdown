using System.Collections.Generic;
using App.Scripts.Abstracts;
using App.Scripts.Models;
using UnityEngine;

namespace App.Scripts.Concretes
{
    public class DummyParser:IApiResponseParser<DummyUser>
    {
        public List<DummyUser> Parse(string json)
        {
            var response=JsonUtility.FromJson<DummyJsonUserResponse>(json);
            return response?.users??new List<DummyUser>();
        }

        public int ExtractTotalCount (string json)
        {
            var parsed = JsonUtility.FromJson<DummyJsonUserResponse>(json);
            return parsed.total;
        }
    }
}