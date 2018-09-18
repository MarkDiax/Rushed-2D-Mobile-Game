using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
	public int score;

	public void AddScore(int Addition) {
		score += Addition;
		Debug.Log("Score added: \t" + Addition + "\n"
			 + "Score total: \t" + score);
	}
}