using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ScorePickup : MonoBehaviour
{
	public int score;
	public UnityEvent OnScorePickup;

	private Tilemap _tilemap;

	private void Start() {
		_tilemap = GetComponent<Tilemap>();
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject == Player.Instance.gameObject) {
			Player.Instance.playerData.AddScore(score);
			OnScorePickup.Invoke();

			//Vector3Int pos = _tilemap.WorldToCell(collision.transform.position);
			////nullify the collided tile;
			//_tilemap.SetTile(pos, null);

			ContactPoint2D[] contacts = collision.contacts;

			for (int i = 0; i < contacts.Length; i++) {
				//Vector3Int pos = _tilemap.WorldToCell(contacts[i].otherCollider.transform.position);
				//print(collision.gameObject.GetComponent<Tile>());
				//_tilemap.SetTile(pos, null);

				print(contacts[i].normal);

				Vector3 hitPosition = Vector3.zero;
				hitPosition.x = contacts[i].point.x + contacts[i].normal.x;
				hitPosition.y = contacts[i].point.y +  contacts[i].normal.y;

				_tilemap.SetTile(_tilemap.WorldToCell(Vector3Int.RoundToInt(hitPosition)), null);
			}
		}
	}
}
