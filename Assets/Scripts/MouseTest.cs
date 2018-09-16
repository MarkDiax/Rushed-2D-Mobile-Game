using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour {

	void Update () {
		gameObject.transform.position = CalculateWorldPointOfMouseClick();
	}

	Vector2 CalculateWorldPointOfMouseClick() {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		float distanceFromCamera = Mathf.Abs(Camera.main.transform.position.z);

		Vector3 weirdTriplet = new Vector3(mouseX, mouseY, distanceFromCamera);
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(weirdTriplet);

		return worldPos;
	}
}
