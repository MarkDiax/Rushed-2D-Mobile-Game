using UnityEditor;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		CustomButton customButton = (CustomButton)target;
	}
}