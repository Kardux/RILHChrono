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

	public class ChronoManager : MonoBehaviour
	{
		#region Fields
		#region Serialized
		[SerializeField] private TMPro.TextMeshProUGUI _chronoText;
		#endregion Serialized


		#region Internals
		private float _chronoTimer = 0.0f;
		private bool _isChronoRunning = false;
		#endregion Internals
		#endregion Fields

		#region Properties
		#endregion Properties

		#region MonoBehaviour
		void Start ()
		{

		}

		void Update ()
		{
			if (_isChronoRunning)
			{
				_chronoTimer += Time.deltaTime * 10.0f;
				_chronoText.text = _chronoTimer.ToChronoFormat();
			}
		}
		#endregion MonoBehaviour

		#region Public Methods
		public void StopChrono()
		{
			if (_isChronoRunning)
			{
				_isChronoRunning = false;
			}
			else
			{
				Debug.LogWarning("Button stopping chrono shouldn't be available when chrono is not running already.");
			}
		}

		public void StartChrono()
		{
			if (_isChronoRunning == false)
			{
				_isChronoRunning = true;
			}
			else
			{
				Debug.LogWarning("Button starting chrono shouldn't be available when chrono is running already.");
			}
		}
		#endregion Public Methods

		#region Private Methods
		#endregion Private Methods
	}
}