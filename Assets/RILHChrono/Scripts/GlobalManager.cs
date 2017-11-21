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
	using System.Collections;
	using System.Collections.Generic;

	public class GlobalManager : MonoBehaviour
	{
		#region Fields
		private Match _match;

		private Team _teamA;
		private Team _teamB;

		private Person _refereeeA;
		private Person _refereeeB;
		private Person _writer;
		private Person _chrono;

		#endregion Fields

		#region Properties
		#endregion Properties

		#region MonoBehaviour
		protected void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		protected void Start ()
		{

		}

		protected void Update ()
		{

		}
		#endregion MonoBehaviour

		#region Public Methods
		public void AddPlayer(bool isTeamA)
		{
			if (isTeamA)
			{
				
			}
			else
			{

			}
		}
		#endregion Public Methods

		#region Private Methods
		#endregion Private Methods
	}	
}