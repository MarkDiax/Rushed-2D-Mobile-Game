using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class CustomButton : Button
{
	[System.Serializable]
	public class OnPressedEvent : UnityEvent { }

	[System.Serializable]
	public class OnPointerDownEvent : UnityEvent { }

	public OnPressedEvent onPressedEvent = new OnPressedEvent();
	public OnPointerDownEvent onPointerDown = new OnPointerDownEvent();

	bool _isPressed;

	private void Update() {
		_isPressed = IsPressed();

		if (_isPressed)
			onPressedEvent.Invoke();
	}

	public override void OnPointerDown(PointerEventData eventData) {
		base.OnPointerDown(eventData);

		onPointerDown.Invoke();
	}
}