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
	using UnityEngine.EventSystems;

	[RequireComponent(typeof(RectTransform))]
	public class HelpDisplayDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region Fields
		#region Serialized
		[SerializeField] private float _minYScreenRatio = 0.5f;
		#endregion Serialized

		#region Internals
		private RectTransform _rectTransform = null;
		private bool _dragged = false;
		private float _starDragAnchor = 0.0f;
		#endregion Internals
		#endregion Fields

		#region Properties
		#endregion Properties

		#region MonoBehaviour
		protected void Start ()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		protected void Update ()
		{

		}
		#endregion MonoBehaviour

		#region Handlers
		public void OnBeginDrag(PointerEventData eventData)
		{
			_starDragAnchor = _rectTransform.anchorMin.y;
		}

		public void OnDrag(PointerEventData eventData)
		{
			MoveYAnchors(_rectTransform, Mathf.Clamp(
				_starDragAnchor + (eventData.position.y - eventData.pressPosition.y) / Screen.height, 0.0f, 1.0f));
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
		#endregion Handlers

		#region Private Methods
		private void MoveYAnchors(RectTransform rectTransform, float value)
		{
			rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, value);
			rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, value + 1.0f);
			rectTransform.anchoredPosition = Vector2.zero;
		}
		#endregion Private Methods
	}	
}