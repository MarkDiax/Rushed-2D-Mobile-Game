using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
	public abstract class InputComponent : MonoBehaviour
	{
		public enum InputType
		{
			MouseAndKeyboard,
			Controller,
			Mobile,
		}

		public enum XboxControllerButtons
		{
			None,
			A,
			B,
			X,
			Y,
			Leftstick,
			Rightstick,
			View,
			Menu,
			LeftBumper,
			RightBumper,
		}


		public enum XboxControllerAxes
		{
			None,
			LeftstickHorizontal,
			LeftstickVertical,
			DpadHorizontal,
			DpadVertical,
			RightstickHorizontal,
			RightstickVertical,
			LeftTrigger,
			RightTrigger,
		}


		[Serializable]
		public class InputButton
		{
			public KeyCode key;
			public XboxControllerButtons controllerButton;
			public MobileInputManager.MobileInputKeys touchInput;
			public bool Down { get; protected set; }
			public bool Held { get; protected set; }
			public bool Up { get; protected set; }
			public bool Enabled {
				get { return m_Enabled; }
			}

			[SerializeField]
			protected bool m_Enabled = true;
			protected bool m_GettingInput = true;

			//This is used to change the state of a button (Down, Up) only if at least a FixedUpdate happened between the previous Frame
			//and this one. Since movement are made in FixedUpdate, without that an input could be missed it get press/release between fixedupdate
			bool m_AfterFixedUpdateDown;
			bool m_AfterFixedUpdateHeld;
			bool m_AfterFixedUpdateUp;

			protected static readonly Dictionary<int, string> k_ButtonsToName = new Dictionary<int, string>
			{
				{(int)XboxControllerButtons.A, "A"},
				{(int)XboxControllerButtons.B, "B"},
				{(int)XboxControllerButtons.X, "X"},
				{(int)XboxControllerButtons.Y, "Y"},
				{(int)XboxControllerButtons.Leftstick, "Leftstick"},
				{(int)XboxControllerButtons.Rightstick, "Rightstick"},
				{(int)XboxControllerButtons.View, "View"},
				{(int)XboxControllerButtons.Menu, "Menu"},
				{(int)XboxControllerButtons.LeftBumper, "Left Bumper"},
				{(int)XboxControllerButtons.RightBumper, "Right Bumper"},
			};

			public InputButton(KeyCode key, XboxControllerButtons controllerButton, MobileInputManager.MobileInputKeys touchInput) {
				this.key = key;
				this.controllerButton = controllerButton;
				this.touchInput = touchInput;
			}

			public void Get(bool fixedUpdateHappened, InputType inputType) {
				if (!m_Enabled) {
					Down = false;
					Held = false;
					Up = false;
					return;
				}

				if (!m_GettingInput)
					return;

				if (inputType == InputType.Controller) {
					if (fixedUpdateHappened) {
						Down = Input.GetButtonDown(k_ButtonsToName[(int)controllerButton]);
						Held = Input.GetButton(k_ButtonsToName[(int)controllerButton]);
						Up = Input.GetButtonUp(k_ButtonsToName[(int)controllerButton]);

						m_AfterFixedUpdateDown = Down;
						m_AfterFixedUpdateHeld = Held;
						m_AfterFixedUpdateUp = Up;
					}
					else {
						Down = Input.GetButtonDown(k_ButtonsToName[(int)controllerButton]) || m_AfterFixedUpdateDown;
						Held = Input.GetButton(k_ButtonsToName[(int)controllerButton]) || m_AfterFixedUpdateHeld;
						Up = Input.GetButtonUp(k_ButtonsToName[(int)controllerButton]) || m_AfterFixedUpdateUp;

						m_AfterFixedUpdateDown |= Down;
						m_AfterFixedUpdateHeld |= Held;
						m_AfterFixedUpdateUp |= Up;
					}
				}
				else if (inputType == InputType.MouseAndKeyboard) {
					if (fixedUpdateHappened) {
						Down = Input.GetKeyDown(key);
						Held = Input.GetKey(key);
						Up = Input.GetKeyUp(key);

						m_AfterFixedUpdateDown = Down;
						m_AfterFixedUpdateHeld = Held;
						m_AfterFixedUpdateUp = Up;
					}
					else {
						Down = Input.GetKeyDown(key) || m_AfterFixedUpdateDown;
						Held = Input.GetKey(key) || m_AfterFixedUpdateHeld;
						Up = Input.GetKeyUp(key) || m_AfterFixedUpdateUp;

						m_AfterFixedUpdateDown |= Down;
						m_AfterFixedUpdateHeld |= Held;
						m_AfterFixedUpdateUp |= Up;
					}
				}
				else if (inputType == InputType.Mobile) {
					if (fixedUpdateHappened) {
						Down = MobileInputManager.Instance.GetInput(touchInput);
						Held = MobileInputManager.Instance.GetInput(touchInput);
						Up = !Down;

						m_AfterFixedUpdateDown = Down;
						m_AfterFixedUpdateHeld = Held;
						m_AfterFixedUpdateUp = Up;
					}
					else {
						Down = MobileInputManager.Instance.GetInput(touchInput) || m_AfterFixedUpdateDown;
						Held = MobileInputManager.Instance.GetInput(touchInput) || m_AfterFixedUpdateHeld;
						Up = !Down || m_AfterFixedUpdateUp;

						m_AfterFixedUpdateDown |= Down;
						m_AfterFixedUpdateHeld |= Held;
						m_AfterFixedUpdateUp |= Up;
					}
				}
			}

			public void Enable() {
				m_Enabled = true;
			}

			public void Disable() {
				m_Enabled = false;
			}

			public void GainControl() {
				m_GettingInput = true;
			}

			public IEnumerator ReleaseControl(bool resetValues) {
				m_GettingInput = false;

				if (!resetValues)
					yield break;

				if (Down)
					Up = true;
				Down = false;
				Held = false;

				m_AfterFixedUpdateDown = false;
				m_AfterFixedUpdateHeld = false;
				m_AfterFixedUpdateUp = false;

				yield return null;

				Up = false;
			}
		}

		[Serializable]
		public class InputAxis
		{
			public KeyCode keyboardPositive;
			public KeyCode keyboardNegative;
			public XboxControllerAxes controllerAxis;
			public MobileInputManager.MobileInputKeys mobilePositive;
			public MobileInputManager.MobileInputKeys mobileNegative;
			public float Value { get; protected set; }
			public bool ReceivingInput { get; protected set; }
			public bool Enabled {
				get { return m_Enabled; }
			}

			protected bool m_Enabled = true;
			protected bool m_GettingInput = true;

			protected readonly static Dictionary<int, string> k_AxisToName = new Dictionary<int, string> {
				{(int)XboxControllerAxes.LeftstickHorizontal, "Leftstick Horizontal"},
				{(int)XboxControllerAxes.LeftstickVertical, "Leftstick Vertical"},
				{(int)XboxControllerAxes.DpadHorizontal, "Dpad Horizontal"},
				{(int)XboxControllerAxes.DpadVertical, "Dpad Vertical"},
				{(int)XboxControllerAxes.RightstickHorizontal, "Rightstick Horizontal"},
				{(int)XboxControllerAxes.RightstickVertical, "Rightstick Vertical"},
				{(int)XboxControllerAxes.LeftTrigger, "Left Trigger"},
				{(int)XboxControllerAxes.RightTrigger, "Right Trigger"},
			};

			public InputAxis(KeyCode keyboardPositive, KeyCode keyboardNegative, XboxControllerAxes controllerAxis, MobileInputManager.MobileInputKeys mobilePositive, MobileInputManager.MobileInputKeys mobileNegative) {
				this.keyboardPositive = keyboardPositive;
				this.keyboardNegative = keyboardNegative;
				this.controllerAxis = controllerAxis;
				this.mobilePositive = mobilePositive;
				this.mobileNegative = mobileNegative;
			}


			public void Get(InputType inputType) {
				if (!m_Enabled) {
					Value = 0f;
					return;
				}

				if (!m_GettingInput)
					return;

				bool positiveHeld = false;
				bool negativeHeld = false;

				if (inputType == InputType.Controller) {
					float value = Input.GetAxisRaw(k_AxisToName[(int)controllerAxis]);
					positiveHeld = value > Single.Epsilon;
					negativeHeld = value < -Single.Epsilon;
				}
				else if (inputType == InputType.MouseAndKeyboard) {
					positiveHeld = Input.GetKey(keyboardPositive);
					negativeHeld = Input.GetKey(keyboardNegative);
				}
				else if (inputType == InputType.Mobile) {
					positiveHeld = MobileInputManager.Instance.GetInput(mobilePositive);
					negativeHeld = MobileInputManager.Instance.GetInput(mobileNegative);
				}

				if (positiveHeld == negativeHeld)
					Value = 0f;
				else if (positiveHeld)
					Value = 1f;
				else
					Value = -1f;

				ReceivingInput = positiveHeld || negativeHeld;
			}

			public void Enable() {
				m_Enabled = true;
			}

			public void Disable() {
				m_Enabled = false;
			}

			public void GainControl() {
				m_GettingInput = true;
			}

			public void ReleaseControl(bool resetValues) {
				m_GettingInput = false;
				if (resetValues) {
					Value = 0f;
					ReceivingInput = false;
				}
			}
		}

		public InputType inputType = InputType.Mobile;

		bool m_FixedUpdateHappened;

		void Update() {
			GetInputs(m_FixedUpdateHappened || Mathf.Approximately(Time.timeScale, 0));

			m_FixedUpdateHappened = false;
		}

		void FixedUpdate() {
			m_FixedUpdateHappened = true;
		}

		protected abstract void GetInputs(bool fixedUpdateHappened);

		public abstract void GainControl();

		public abstract void ReleaseControl(bool resetValues = true);

		protected void GainControl(InputButton inputButton) {
			inputButton.GainControl();
		}

		protected void GainControl(InputAxis inputAxis) {
			inputAxis.GainControl();
		}

		protected void ReleaseControl(InputButton inputButton, bool resetValues) {
			StartCoroutine(inputButton.ReleaseControl(resetValues));
		}

		protected void ReleaseControl(InputAxis inputAxis, bool resetValues) {
			inputAxis.ReleaseControl(resetValues);
		}
	}
}
