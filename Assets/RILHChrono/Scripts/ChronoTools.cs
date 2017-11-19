#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: https://fr.linkedin.com/pub/roy-bodereau/b2/94/82b
************************************************************************************************************/
#endregion

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0 - Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion

namespace RILHChrono
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public static class ChronoTools
	{
		#region Fields
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Public Methods
		public static string ToChronoFormat(this float value)
		{
			int minutes = Mathf.Clamp(Mathf.FloorToInt(value / 60.0f), 0, 99);
			int seconds = Mathf.Clamp(Mathf.FloorToInt(value % 60.0f), 0, 59);

			return minutes.ToString("D2") + ":" + seconds.ToString("D2");
		}
		#endregion Public Methods

		#region Private Methods
		#endregion Private Methods
	}
}