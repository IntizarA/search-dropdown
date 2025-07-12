using System.Collections.Generic;

namespace App.Scripts.Models
{
    [System.Serializable]
    public class DummyUser
    {
        public int id;
        public string firstName;
        public string lastName;
        public string email;
    }

    [System.Serializable]
    public class DummyJsonUserResponse
    {
        public int total;
        public List<DummyUser> users;
    }
}