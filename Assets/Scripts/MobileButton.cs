using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	Image image;

	void Start() {
		image = GetComponent<Image>();
	}

	void Update() {
		float colorAlpha = 0f;

		if (Pressed)
			colorAlpha = 0.25f;

		Color c = image.color;
		c.a = colorAlpha;
		image.color = c;
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
}