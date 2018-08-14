using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	Image _image;

	[SerializeField]
	MobileInputManager.TouchInput _touchInput;

	void Start() {
		_image = GetComponent<Image>();

		MobileInputManager.Instance.Register(this);
	}

	void Update() {
		float colorAlpha = 0f;

		if (Pressed)
			colorAlpha = 0.25f;

		Color c = _image.color;
		c.a = colorAlpha;
		_image.color = c;
	}

	public bool Pressed {
		get; protected set;
	}

	public void OnPointerDown(PointerEventData eventData) {
		Pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		Pressed = false;
	}

	public void OnPointerClick(PointerEventData eventData) {
		Pressed = true;
	}

	public MobileInputManager.TouchInput TouchInput {
		get {
			return _touchInput;
		}
	}
}