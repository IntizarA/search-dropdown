using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Scripts.Models;

namespace App.Scripts.Concretes
{
	public class LocalDataProvider : IDataProvider<DummyUser>
	{
	private List<DummyUser> users = new()
{
    new() { id = 1, firstName = "John", lastName = "Doe", email = "john.doe@example.com" },
    new() { id = 2, firstName = "Jane", lastName = "Doe", email = "jane.doe@example.com" },
    new() { id = 3, firstName = "Lock", lastName = "Trump", email = "lock.trump@example.com" },
    new() { id = 4, firstName = "Jack", lastName = "Mercury", email = "jack.mercury@example.com" },
    new() { id = 5, firstName = "Kate", lastName = "Tomlinson", email = "kate.tomlinson@example.com" },
    new() { id = 6, firstName = "Mike", lastName = "Stone", email = "mike.stone@example.com" },
    new() { id = 7, firstName = "Alice", lastName = "Johnson", email = "alice.johnson@example.com" },
    new() { id = 8, firstName = "Robert", lastName = "Brown", email = "robert.brown@example.com" },
    new() { id = 9, firstName = "Emily", lastName = "Clark", email = "emily.clark@example.com" },
    new() { id = 10, firstName = "Steve", lastName = "Rogers", email = "steve.rogers@example.com" },
    new() { id = 11, firstName = "Bruce", lastName = "Banner", email = "bruce.banner@example.com" },
    new() { id = 12, firstName = "Natasha", lastName = "Romanoff", email = "natasha.romanoff@example.com" },
    new() { id = 13, firstName = "Tony", lastName = "Stark", email = "tony.stark@example.com" },
    new() { id = 14, firstName = "Peter", lastName = "Parker", email = "peter.parker@example.com" },
    new() { id = 15, firstName = "Wanda", lastName = "Maximoff", email = "wanda.maximoff@example.com" },
    new() { id = 16, firstName = "Scott", lastName = "Lang", email = "scott.lang@example.com" },
    new() { id = 17, firstName = "Hope", lastName = "van Dyne", email = "hope.vandyne@example.com" },
    new() { id = 18, firstName = "Stephen", lastName = "Strange", email = "stephen.strange@example.com" },
    new() { id = 19, firstName = "T'Challa", lastName = "Udaku", email = "tchalla.udaku@example.com" },
    new() { id = 20, firstName = "Carol", lastName = "Danvers", email = "carol.danvers@example.com" },
    new() { id = 21, firstName = "Nick", lastName = "Fury", email = "nick.fury@example.com" },
    new() { id = 22, firstName = "Phil", lastName = "Coulson", email = "phil.coulson@example.com" },
    new() { id = 23, firstName = "Peggy", lastName = "Carter", email = "peggy.carter@example.com" },
    new() { id = 24, firstName = "Sam", lastName = "Wilson", email = "sam.wilson@example.com" },
    new() { id = 25, firstName = "Bucky", lastName = "Barnes", email = "bucky.barnes@example.com" },
    new() { id = 26, firstName = "Clint", lastName = "Barton", email = "clint.barton@example.com" },
    new() { id = 27, firstName = "Laura", lastName = "Barton", email = "laura.barton@example.com" },
    new() { id = 28, firstName = "Shuri", lastName = "Udaku", email = "shuri.udaku@example.com" },
    new() { id = 29, firstName = "Okoye", lastName = "Wakanda", email = "okoye.wakanda@example.com" },
    new() { id = 30, firstName = "Mantis", lastName = "Guardians", email = "mantis.guardians@example.com" },
    new() { id = 31, firstName = "Drax", lastName = "Destroyer", email = "drax.destroyer@example.com" },
    new() { id = 32, firstName = "Rocket", lastName = "Raccoon", email = "rocket.raccoon@example.com" },
    new() { id = 33, firstName = "Groot", lastName = "Tree", email = "groot.tree@example.com" },
    new() { id = 34, firstName = "Gamora", lastName = "Zen", email = "gamora.zen@example.com" },
    new() { id = 35, firstName = "Nebula", lastName = "Titan", email = "nebula.titan@example.com" },
    new() { id = 36, firstName = "Yondu", lastName = "Udonta", email = "yondu.udonta@example.com" },
    new() { id = 37, firstName = "Quill", lastName = "Star-Lord", email = "quill.starlord@example.com" },
    new() { id = 38, firstName = "Maria", lastName = "Hill", email = "maria.hill@example.com" },
    new() { id = 39, firstName = "Rhodey", lastName = "Rhodes", email = "rhodey.rhodes@example.com" },
    new() { id = 40, firstName = "Happy", lastName = "Hogan", email = "happy.hogan@example.com" },
    new() { id = 41, firstName = "Wong", lastName = "Master", email = "wong.master@example.com" },
    new() { id = 42, firstName = "Thanos", lastName = "Titan", email = "thanos.titan@example.com" },
    new() { id = 43, firstName = "Loki", lastName = "Odinson", email = "loki.odinson@example.com" },
    new() { id = 44, firstName = "Thor", lastName = "Odinson", email = "thor.odinson@example.com" },
    new() { id = 45, firstName = "Heimdall", lastName = "Asgard", email = "heimdall.asgard@example.com" },
    new() { id = 46, firstName = "Jane", lastName = "Foster", email = "jane.foster@example.com" },
    new() { id = 47, firstName = "Darcy", lastName = "Lewis", email = "darcy.lewis@example.com" },
    new() { id = 48, firstName = "Erik", lastName = "Selvig", email = "erik.selvig@example.com" },
    new() { id = 49, firstName = "May", lastName = "Parker", email = "may.parker@example.com" },
    new() { id = 50, firstName = "Ned", lastName = "Leeds", email = "ned.leeds@example.com" },
};

		public int TotalCount => users.Count;
		public async Task<List<DummyUser>> GetDataAsync (int page, int pageSize, string query = null)
		{
			await Task.Yield ();

			if (page < 1 || pageSize < 1)
				return new ();

			IEnumerable<DummyUser> filtered = users;

			if (!string.IsNullOrWhiteSpace (query))
			{
				query = query.ToLower ();
				filtered = filtered.Where (u => 
					!string.IsNullOrEmpty (u.email) && u.email.ToLower ().Contains (query));
			}

			List<DummyUser> result = filtered
				.Skip ((page - 1) * pageSize)
				.Take (pageSize)
				.ToList ();

			return result;
		}

	}
}