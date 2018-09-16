using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MobileInputManager : MonoSingleton<MobileInputManager>
{
	public enum MobileInputKeys
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

	List<MobileButton> _mobileButtons;
	List<MobileInputKeys> _registeredInputs;

	[HideInInspector]
	public SwipeInput Swipe;

	private void Awake() {
		Swipe = gameObject.AddComponent<SwipeInput>();
		_mobileButtons = new List<MobileButton>();
		_registeredInputs = new List<MobileInputKeys>();
	}

	public void ClickTest() {
		print("click");

	}

	public bool GetInput(MobileInputKeys input, bool checkForOtherInputs) {
		return GetInput(input);
	}

	public bool GetInput(MobileInputKeys input) {
		//check all MobileButtons
		for (int i = 0; i < _mobileButtons.Count; i++) {
			if (_mobileButtons[i].TouchInput == input) {
				return _mobileButtons[i].Pressed;
			}
		}

		//Check all swipes
		#region Swipes
		if (input == MobileInputKeys.SwipeLeft)
			return Swipe.SwipeLeft;
		if (input == MobileInputKeys.SwipeRight)
			return Swipe.SwipeRight;
		if (input == MobileInputKeys.SwipeUp)
			return Swipe.SwipeUp;
		if (input == MobileInputKeys.SwipeDown)
			return Swipe.SwipeDown;
		if (input == MobileInputKeys.Tap)
			return Swipe.Tap;
		#endregion

		return false;
	}


	public void RegisterButton(MobileButton Button) {
		if (!_mobileButtons.Contains(Button))
			_mobileButtons.Add(Button);
	}

	public Vector2 GetWorldPointOfMousePosition() {
		Camera cam = Camera.main;

		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		float distanceFromCamera = Mathf.Abs(cam.transform.position.z);

		Vector3 weirdTriplet = new Vector3(mouseX, mouseY, distanceFromCamera);
		Vector2 worldPos = cam.ScreenToWorldPoint(weirdTriplet);

		return worldPos;
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

			CheckSwipes();
		}

		private void CheckSwipes() {
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