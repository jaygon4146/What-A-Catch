using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour {

    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameObject throwingControlsObj;
    [SerializeField] private GameObject catchingControlsObj;
    
    public void AttachInputManager(InputManager m)
    {
        //print("AttachInputManager");
        inputManager = m;
    }

    public void enableThrowingControls()
    {
        throwingControlsObj.SetActive(true);
        catchingControlsObj.SetActive(false);
    }
    public void enableCatchingControls()
    {
        throwingControlsObj.SetActive(false);
        catchingControlsObj.SetActive(true);
    }

    public void AcceptThrowDelta(Vector2 deltaPointer)
    {
        //print("AcceptThrowDelta()");
        inputManager.AcceptThrowDelta(deltaPointer);
    }
}
