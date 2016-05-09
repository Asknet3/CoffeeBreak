using System;
using SQLite;

namespace CoffeeBreak
{
	public class User
	{
		public User(){
			
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set;}
		public string username { get; set;}
	}
}

