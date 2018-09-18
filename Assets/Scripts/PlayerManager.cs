using UnityEngine;
using System.Collections;

public class PlayerManager : MonoSingleton<PlayerManager>
{
	static Player _player;

	public static Player PlayerInstance {
		get {
			if (_player == null)
				_player = FindObjectOfType<Player>();

			return _player;
		}
	}
}