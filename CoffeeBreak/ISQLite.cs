using System;
using SQLite;

namespace CoffeeBreak
{
	
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

