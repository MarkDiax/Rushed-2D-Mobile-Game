using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ScorePickup : MonoBehaviour
{
	public int score;
	public UnityEvent OnScorePickup;

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == Player.Instance.gameObject) {
			Player.Instance.playerData.AddScore(score);
			OnScorePickup.Invoke();

			Destroy(gameObject);
		}
	}
}