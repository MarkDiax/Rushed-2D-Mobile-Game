using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MobileInputManager : MonoSingleton<MobileInputManager>
{
	public enum TouchInput
	{
		None,
		TouchLeft,
		TouchRight,
		SwipeUp,
		SwipeDown,
		SwipeLeft,
		SwipeRight,
		Tap,
	}

	[SerializeField]
	Image _debugCursor;

	List<MobileButton> _mobileButtons;

	[HideInInspector]
	public SwipeInput Swipe;

	private void Awake() {
		Swipe = gameObject.AddComponent<SwipeInput>();
		_mobileButtons = new List<MobileButton>();
	}

	public bool GetInput(TouchInput input) {
		//check all MobileButtons
		for (int i = 0; i < _mobileButtons.Count; i++) {
			if (_mobileButtons[i].TouchInput == input) {
				return _mobileButtons[i].Pressed;
			}
		}

		//Check all swipes
		#region Swipes
		if (input == TouchInput.SwipeLeft)
			return Swipe.SwipeLeft;
		if (input == TouchInput.SwipeRight)
			return Swipe.SwipeRight;
		if (input == TouchInput.SwipeUp)
			return Swipe.SwipeUp;
		if (input == TouchInput.SwipeDown)
			return Swipe.SwipeDown;
		if (input == TouchInput.Tap)
			return Swipe.Tap;
		#endregion

		return false;
	}

	public void Register(MobileButton Button) {
		if (!_mobileButtons.Contains(Button))
			_mobileButtons.Add(Button);
	}

	public class SwipeInput : MonoBehaviour
	{
		bool _isDragging;
		Vector2 _startTouch;
		int _swipeDeadzone = 125;

		void Update() {
			Tap = SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
			#region Standalone Input
			if (Input.GetMouseButtonDown(0)) {
				Tap = true;
				_isDragging = true;
				_startTouch = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0))
				Reset();

			#endregion

			#region Mobile Input
			if (Input.touchCount > 0) {
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					Tap = true;
					_isDragging = true;
					_startTouch = Input.GetTouch(0).position;
				}
				else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
					Reset();
				}
			}

			#endregion

			//Calculating SwipeDelta
			SwipeDelta = Vector2.zero;
			if (_isDragging) {
				if (Input.touchCount > 0)
					SwipeDelta = Input.GetTouch(0).position - _startTouch;
				else if (Input.GetMouseButton(0))
					SwipeDelta = (Vector2)Input.mousePosition - _startTouch;
			}

			//Check for deadzone
			if (SwipeDelta.magnitude > _swipeDeadzone) {
				float x = SwipeDelta.x;
				float y = SwipeDelta.y;

				if (Mathf.Abs(x) > Mathf.Abs(y)) {
					//X axis
					if (x < 0) {
						SwipeLeft = true;
						print("SwipeLeft!");
					}
					else {
						SwipeRight = true;
						print("SwipeRight");
					}
				}
				else {
					//Y axis
					if (y < 0) {
						SwipeDown = true;
						print("SwipeDown!");
					}
					else {
						SwipeUp = true;
						print("SwipeUp!");
					}
				}
			}
		}

		private void Reset() {
			_isDragging = false;
			_startTouch = SwipeDelta = Vector2.zero;
		}

		public Vector2 SwipeDelta { get; private set; }

		public bool Tap { get; private set; }
		public bool SwipeLeft { get; private set; }
		public bool SwipeRight { get; private set; }
		public bool SwipeUp { get; private set; }
		public bool SwipeDown { get; private set; }
	}
}