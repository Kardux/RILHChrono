#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: https://fr.linkedin.com/pub/roy-bodereau/b2/94/82b
************************************************************************************************************/
#endregion Author

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0
Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion Copyright

namespace RILHChrono
{
	using UnityEngine;
    using System;
	using System.Collections;
	using System.Collections.Generic;

    public class Match
	{
		public int number;
		public string location;
		public string competitionType;
		public string group;
		public DateTime date;
	}

	public class Team
	{
		public Color color;
		public string name;

		public List<Player> players;
		public List<Official> officials;
	}

	public class Person
	{
		public int licenseNumber;
		public string name;
	}

	public class Official : Person
	{
		public enum Letter
		{
			A, B, C, D, E, F
		}

		public Letter letter;
	}

	public class Player : Person
	{
		public enum Status
		{
			Goal,
			Captain,
			Assistant
		}

		public int number;
	}
}