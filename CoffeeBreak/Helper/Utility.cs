using System;
using SQLite;
using System.Collections;
using Xamarin.Forms;

namespace CoffeeBreak
{
	public class Utility
	{
		public Utility ()
		{
		}

		public static bool TableExists<T> (SQLiteConnection connection)
		{
			const string cmdText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = ?";
			var cmd = connection.CreateCommand (cmdText, typeof(T).Name);
			return cmd.ExecuteScalar<string> () != null;
		}



		public static User GetUser (SQLiteConnection connection)
		{
			var query = connection.Table<User> ().FirstOrDefault ();

			return query;
		}


		public static int SaveUser (SQLiteConnection connection, string usrName)
		{
			var usr = new User{ username = usrName};
			return connection.Insert (usr);
		}




	

	}
}

