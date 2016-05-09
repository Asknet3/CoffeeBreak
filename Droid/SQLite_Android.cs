using System;
using Xamarin.Forms;
using System.IO;
using SQLite;
using CoffeeBreak.Droid;

[assembly: Dependency (typeof(SQLite_Android))]
namespace CoffeeBreak.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

		public SQLiteConnection GetConnection(){
			var sqliteFilename="db_coffeeBreak";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var path = Path.Combine (documentsPath, sqliteFilename);


			// creo la connection
			var conn = new SQLiteConnection(path);

			// Restituisco la connessione al DB
			return conn;
		}
	}
}

