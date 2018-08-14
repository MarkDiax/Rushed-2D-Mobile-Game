using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
			colorAlpha = 0.15f;

		Color c = _image.color;
		c.a = colorAlpha;
		_image.color = c;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		Pressed = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		Pressed = false;
	}

	public bool Pressed {
		get; protected set;
	}

	public MobileInputManager.TouchInput TouchInput {
		get { return _touchInput; }
	}
}