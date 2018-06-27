using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : InputUtilities {

    public InputAction MoveHorizontalAction = new InputAction("MoveHorizontal");
    public InputAction MoveVerticalAction = new InputAction("MoveVertical");

    public InputAction ReleaseBallAction = new InputAction("ReleaseBall");

    private void Awake()
    {
        SetUpInputs();
    }

    private void SetUpInputs()
    {
        MoveHorizontalAction.AddAxis("Horizontal");
        MoveVerticalAction.AddAxis("Vertical");

        ReleaseBallAction.AddButton("Jump");
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        MoveHorizontalAction.GetCrossPlatformInput();
        MoveVerticalAction.GetCrossPlatformInput();

        ReleaseBallAction.GetCrossPlatformInput();
    }
}
