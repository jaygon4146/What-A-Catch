using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public abstract class InputUtilities : MonoBehaviour
{
    [System.Serializable]
    public class InputAction
    {
        public string ActionName = "MISSING";

        public InputAction(string name)
        {
            ActionName = name;
        }

        List<string> AxisNames = new List<string>();
        List<string> ButtonNames = new List<string>();
        List<KeyCode> KeyCodes = new List<KeyCode>();

        public void AddAxis(string name)
        {
            AxisNames.Add(name);
        }
        public void AddButton(string name)
        {
            ButtonNames.Add(name);
        }
        public void AddKeyCode(KeyCode key)
        {
            KeyCodes.Add(key);
        }

        public float axisFloat;

        public bool buttonDown;
        public bool buttonHeld;
        public bool buttonUp;

        public bool keyDown;
        public bool keyHeld;
        public bool keyUp;

        public void GetInput()
        {
            //==================================================
            #region GetAxis
            axisFloat = 0;
            foreach (string axis in AxisNames)
            {
                float tAxis = Input.GetAxis(axis);
                axisFloat = tAxis;
            }
            #endregion
            //==================================================
            #region GetButton
            buttonDown = false;
            buttonUp = false;
            foreach (string button in ButtonNames)
            {                
                bool tButtonDown = Input.GetButtonDown(button);
                bool tButtonHeld = Input.GetButton(button);
                bool tButtonUp = Input.GetButtonUp(button);                           

                if (tButtonDown)
                {
                    buttonDown = true;
                    buttonHeld = true;
                    buttonUp = false;
                }
                if (tButtonHeld)
                {
                }
                if (tButtonUp)
                {
                    buttonDown = false;
                    buttonHeld = false;
                    buttonUp = true;
                }
            }
            #endregion
            //==================================================
            #region GetKey
            keyDown = false;
            keyUp = false;
            foreach (KeyCode key in KeyCodes)
            {
                bool tKeyDown = Input.GetKeyDown(key);
                bool tKeyHeld = Input.GetKey(key);
                bool tKeyUp = Input.GetKeyUp(key);

                if (tKeyDown)
                {
                    keyDown = true;
                    keyHeld = true;
                    keyUp = false;
                }
                if (tKeyHeld)
                {
                }
                if (tKeyUp)
                {
                    keyDown = false;
                    keyHeld = false;
                    keyUp = true;
                }
            }
            #endregion
            //==================================================
        }

        public void GetCrossPlatformInput()
        {
            //==================================================
            #region GetAxis
            axisFloat = 0;
            foreach (string axis in AxisNames)
            {
                float tAxis = CrossPlatformInputManager.GetAxis(axis);
                axisFloat = tAxis;
            }
            #endregion
            //==================================================
            #region GetButton
            buttonDown = false;
            buttonUp = false;
            foreach (string button in ButtonNames)
            {

                bool tButtonDown = CrossPlatformInputManager.GetButtonDown(button);
                bool tButtonHeld = CrossPlatformInputManager.GetButton(button);
                bool tButtonUp = CrossPlatformInputManager.GetButtonUp(button);

                if (tButtonDown)
                {
                    buttonDown = true;
                    buttonHeld = true;
                    buttonUp = false;
                }
                if (tButtonHeld)
                {
                }
                if (tButtonUp)
                {
                    buttonDown = false;
                    buttonHeld = false;
                    buttonUp = true;
                }
            }
            #endregion
            //==================================================
            #region GetKey
            keyDown = false;
            keyUp = false;
            foreach (KeyCode key in KeyCodes)
            {
                bool tKeyDown = Input.GetKeyDown(key);
                bool tKeyHeld = Input.GetKey(key);
                bool tKeyUp = Input.GetKeyUp(key);

                if (tKeyDown)
                {
                    keyDown = true;
                    keyHeld = true;
                    keyUp = false;
                }
                if (tKeyHeld)
                {
                }
                if (tKeyUp)
                {
                    keyDown = false;
                    keyHeld = false;
                    keyUp = true;
                }
            }
            #endregion
            //==================================================
        }
    }
}
