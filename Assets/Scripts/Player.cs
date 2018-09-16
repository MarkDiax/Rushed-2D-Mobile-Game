using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public BoxCollider2D touchCollider;
	public GrappleGun grappleGun;

	public float grappleMoveSpeed;
	public float grappleReleaseVelocityMultiplier;
	Rigidbody2D _rigidbody;
	Vector2 _velocity;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {

		if (grappleGun.Target != null && grappleGun.Target.IsReady) {
			if (Vector2.Distance(grappleGun.transform.position, grappleGun.Target.transform.position) > 1f) {
				Vector2 moveDirection = grappleGun.Target.transform.position - grappleGun.transform.position;
				Vector2 moveNormalized = moveDirection.normalized;

				Move(moveNormalized, grappleMoveSpeed);
			}
			else {
				grappleGun.DestroyLine();
				_rigidbody.AddForce(_velocity * grappleReleaseVelocityMultiplier, ForceMode2D.Impulse);
			}
		}

		if (Input.GetKeyDown(KeyCode.W))
			_rigidbody.AddForce(Vector2.up * 500);
	}

	private void Move(Vector3 MoveDir, float Multiplier) {
		Vector2 oldPos = transform.position;
		transform.position += (MoveDir * Multiplier) * Time.deltaTime;
		_velocity = ((Vector2)transform.position - oldPos);
	}

	public void ShootGrapple() {
		grappleGun.ShootGrapple();
	}
}