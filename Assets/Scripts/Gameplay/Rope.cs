using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
	public GameObject nodePrefab;
	public float moveSpeed = 1f;
	public float nodeSpawnDistance = 2f;
	public float nodeMinimumDistance = 1f;

	Player _player;
	List<GameObject> _ropeList;

	GameObject _lastNode;
	Vector2 _destination;
	bool _atDestination;

	private void Awake() {
		_player = FindObjectOfType<Player>();

		_lastNode = gameObject;
		_ropeList = new List<GameObject> {
			_lastNode
		};
	}

	public void SetDestination(Vector2 pDestination) {
		_destination = pDestination;
	}

	/*public void Traverse(float pDirection, float pSpeed) {	
		if (pDirection > 0) {
			if (_lastNode == gameObject)
				return;

			_player.transform.position = Vector2.MoveTowards(_player.transform.position, _lastNode.transform.position, pSpeed * pDirection);
		}
		else {
			Vector2 dir = (_player.transform.position - _lastNode.transform.position).normalized;
			dir *= nodeSpawnDistance;

			_player.transform.position = Vector2.MoveTowards(_player.transform.position, dir, pSpeed * pDirection);

			if (Vector2.Distance(_player.transform.position, _lastNode.transform.position) > nodeSpawnDistance)
				CreateNode();
		}
	}*/

	private void Update() {
		transform.position = Vector2.MoveTowards(transform.position, _destination, moveSpeed * Time.deltaTime);

		if (_lastNode != null && _lastNode != gameObject) {
			if ((Vector2.Distance(_player.transform.position, _lastNode.transform.position) < nodeMinimumDistance)) {
				_ropeList.Remove(_lastNode);
				Destroy(_lastNode);
				_lastNode = _ropeList[_ropeList.Count - 1];
				_lastNode.GetComponent<HingeJoint2D>().connectedBody = _player.GetComponent<Rigidbody2D>();
			}
		}


		if ((Vector2)transform.position == _destination) {
			_atDestination = true;
			_lastNode.GetComponent<HingeJoint2D>().connectedBody = _player.GetComponent<Rigidbody2D>();
		}
		/*
		if ((Vector2)transform.position != _destination) {
			if (Vector2.Distance(_player.transform.position, _lastNode.transform.position) > nodeSpawnDistance)
				CreateNode();
		}

		else if (!_atDestination) {
			_atDestination = true;
			_lastNode.GetComponent<HingeJoint2D>().connectedBody = _player.GetComponent<Rigidbody2D>();
		}
		*/
	}

	private void CreateNode() {
		Vector2 nodeDir = _player.transform.position - _lastNode.transform.position;
		Vector2 normalizedDir = nodeDir.normalized;
		Vector2 nodePos = (normalizedDir * nodeSpawnDistance) + (Vector2)_lastNode.transform.position;

		GameObject node = GameObject.Instantiate(nodePrefab, transform);
		node.transform.position = nodePos;

		_lastNode.GetComponent<HingeJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();

		_lastNode = node;
		_ropeList.Add(_lastNode);
	}

	public bool IsReady {
		get { return _atDestination; }
	}
}