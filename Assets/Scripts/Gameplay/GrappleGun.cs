using UnityEngine;
using System.Collections;

public class GrappleGun : MonoBehaviour
{
	public Rope ropeHookPrefab;

	private Rope _currentRope;

	private LineRenderer _lineRenderer;
	private Vector3[] _linePositions = new Vector3[2];

	private void Start() {
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.enabled = false;
	}

	public void ShootGrapple() {
		if (_currentRope != null) {
			DestroyLine();
		}

		_currentRope = Instantiate(ropeHookPrefab, (Vector2)transform.position, Quaternion.identity);
		_currentRope.SetDestination(MobileInputManager.Instance.GetWorldPointOfMousePosition());
		_lineRenderer.enabled = true;
	}

	public void RenderLine() {
		_linePositions[0] = transform.position;
		_linePositions[1] = _currentRope.transform.position;

		_lineRenderer.SetPositions(_linePositions);
	}

	public void Update() {
		if (_currentRope != null) {
			RenderLine();
		}
	}

	public void DestroyLine() {
		Destroy(_currentRope.gameObject);
		_lineRenderer.enabled = false;
	}

	public Rope Target {
		get { return _currentRope; }
	}
}