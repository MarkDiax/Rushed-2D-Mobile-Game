using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Rope ropeHookPrefab;
	public float ropeClimbSpeed;

	Rope _currentRope;

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (_currentRope != null) {
				Destroy(_currentRope.gameObject);
			}

			_currentRope = Instantiate(ropeHookPrefab, (Vector2)transform.position, Quaternion.identity);
			_currentRope.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}

		float vertical = Input.GetAxis("Vertical");
		if (_currentRope != null) {
			_currentRope.Traverse(vertical, ropeClimbSpeed);
		}
	}
}