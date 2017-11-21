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
	using UnityEngine.EventSystems;

	[RequireComponent(typeof(RectTransform))]
	public class HelpDisplayDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region Fields
		#region Serialized
		[SerializeField] private float _minYScreenRatio = 0.5f;
		[SerializeField] private float _maxSnappingTime = 1.0f;
		[SerializeField] private AnimationCurve _snappingCurve = null;
		#endregion Serialized

		#region Internals
		private RectTransform _rectTransform = null;
		private bool _dragged = false;
		private IEnumerator _snappingCoroutine;
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
			if (_snappingCoroutine != null)
			{
				StopCoroutine(_snappingCoroutine);
				_snappingCoroutine = null;
			}

			_dragged = true;
			_starDragAnchor = _rectTransform.anchorMin.y;
		}

		public void OnDrag(PointerEventData eventData)
		{
			MoveYAnchors(_rectTransform, Mathf.Clamp(
				_starDragAnchor + (eventData.position.y - eventData.pressPosition.y) / Screen.height, 0.0f, 1.0f));
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_dragged = false;
			if (_snappingCoroutine == null)
			{
				if (_rectTransform.anchorMin.y < 0.5f)
				{
					_snappingCoroutine = SnapCoroutine(
						0.0f,
						 _rectTransform.anchorMin.y	/ 0.5f * _maxSnappingTime);
				}
				else
				{
					_snappingCoroutine = SnapCoroutine(
						1.0f, 
						(1.0f - _rectTransform.anchorMin.y)	/ 0.5f * _maxSnappingTime);
				}

				StartCoroutine(_snappingCoroutine);
			}
		}
		#endregion Handlers

		#region Private Methods
		private void MoveYAnchors(RectTransform rectTransform, float value)
		{
			rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, value);
			rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, value + 1.0f);
			rectTransform.anchoredPosition = Vector2.zero;
		}

		private IEnumerator SnapCoroutine(float targetAnchorValue, float duration)
		{
			float timer = 0.0f;
			float startAnchorValue = _rectTransform.anchorMin.y;
			float deltaAnchorValue = targetAnchorValue - startAnchorValue;

			while (timer < duration)
			{
				timer += Time.deltaTime;

				MoveYAnchors(
					_rectTransform, 
					startAnchorValue + deltaAnchorValue * _snappingCurve.Evaluate(timer / duration));
				yield return new WaitForEndOfFrame();
			}

			MoveYAnchors(_rectTransform, targetAnchorValue);
		}
		#endregion Private Methods
	}	
}