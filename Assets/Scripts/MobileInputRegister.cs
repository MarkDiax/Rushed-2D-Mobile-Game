using UnityEngine;
using Gamekit2D;
using System.Collections;
using UnityEngine.UI;

public class MobileInputRegister : MonoSingleton<MobileInputRegister>
{
	public Image DebugCursor;

	[System.Serializable]
	public class MobileInput
	{
		public MobileButton button;
		public InputComponent.MobileTouchAreas inputArea;
	}

	[SerializeField]
	private MobileInput[] _mobileInputs;

	public MobileButton GetButton(InputComponent.MobileTouchAreas inputArea) {
		for (int i = 0; i < _mobileInputs.Length; i++) {
			if (_mobileInputs[i].inputArea == inputArea)
				return _mobileInputs[i].button;
		}

		Debug.LogWarning("MobileInput: " + inputArea.ToString() + " not registered!");
		return null;
	}

	private void Update() {
		if (Input.touchCount > 0) {
			DebugCursor.transform.position = Input.GetTouch(0).position;
		}

		for (int i = 0; i < _mobileInputs.Length; i++) {
			print(_mobileInputs[i].button.name + " being pressed: " + _mobileInputs[i].button.Pressed);
		}
	}
}