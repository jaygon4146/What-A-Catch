using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : InputUtilities {
    //==================================================
    #region Input Modes
    public enum ControlMode
    {
        MenuNavigation,
        Throwing,
        Catching,
    }
    public ControlMode controlMode = ControlMode.Catching;
    #endregion
    //==================================================
    [SerializeField] private List<InputAction> MenuNavActionList = new List<InputAction>();
    [SerializeField] private List<InputAction> ThrowingActionList = new List<InputAction>();
    [SerializeField] private List<InputAction> CatchingActionList = new List<InputAction>();
    //==================================================
    public InputAction MoveHorizontalAction     = new InputAction("MoveHorizontal");
    public InputAction MoveVerticalAction       = new InputAction("MoveVertical");

    public InputAction HoldAimAction            = new InputAction("HoldAim");
    public InputAction AimHorizontalAction      = new InputAction("AimHorizontal");
    public InputAction AimVerticalAction        = new InputAction("AimVertical");

    public InputAction ReleaseBallAction        = new InputAction("ReleaseBall");
    //==================================================
    [SerializeField] private GameObject inputUIObj;
    [SerializeField] private InputUI inputUI;
    //==================================================
    [SerializeField] private Vector3 mouseDownAim = Vector3.zero;
    [SerializeField] private Vector3 throwInputVector = Vector3.zero;
    [SerializeField] private bool throwReleased = false;
    //==================================================
    private void Awake()
    {
        SetUpInputs();
        AddActionsToLists();
    }

    private void OnEnable()
    {
        AttachInputToUI();
    }

    public void AttachInputToUI()
    {
        //print("Attaching Input To UI");
        inputUIObj = GameObject.FindGameObjectWithTag("InputUI");
        inputUI = inputUIObj.GetComponent<InputUI>();
        inputUI.AttachInputManager(this);

        inputUI.enableCatchingControls();
    }

    private void SetUpInputs()
    {
        MoveHorizontalAction.AddAxis("Horizontal");
        MoveVerticalAction.AddAxis("Vertical");

        HoldAimAction.AddButton("Fire1");
        AimHorizontalAction.AddAxis("Mouse X");
        AimVerticalAction.AddAxis("Mouse Y");

        ReleaseBallAction.AddButton("Jump");
    }

    private void AddActionsToLists()
    {
        ThrowingActionList.Add(HoldAimAction);
        ThrowingActionList.Add(AimHorizontalAction);
        ThrowingActionList.Add(AimVerticalAction);
        ThrowingActionList.Add(ReleaseBallAction);

        CatchingActionList.Add(MoveHorizontalAction);
        CatchingActionList.Add(MoveVerticalAction);
    }
    
    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if (controlMode == ControlMode.MenuNavigation)
        {
            return;
        }
        if (controlMode == ControlMode.Throwing)
        {
            foreach(InputAction action in ThrowingActionList)
            {
                action.GetCrossPlatformInput();
                UpdateAiming();
            }
            return;
        }
        if (controlMode == ControlMode.Catching)
        {
            foreach (InputAction action in CatchingActionList)
            {
                action.GetCrossPlatformInput();
            }
            return;
        }
    }

    private void UpdateAiming()
    {
        if (HoldAimAction.buttonDown)
        {
            mouseDownAim = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    private void ResetInputs()
    {
        throwReleased = false;
        foreach (InputAction action in MenuNavActionList)
        {
            action.ResetCrossPlatformInput();
        }
        foreach (InputAction action in ThrowingActionList)
        {
            action.ResetCrossPlatformInput();
        }
        foreach (InputAction action in CatchingActionList)
        {
            action.ResetCrossPlatformInput();
        }
    }

    public bool isCatching()
    {
        if (controlMode == ControlMode.Catching)
        {
            return true;
        }
        return false;
    }

    public bool isThrowing()
    {
        if (controlMode == ControlMode.Throwing)
        {
            return true;
        }
        return false;
    }
    
    public bool throwWasReleased()
    {
        return throwReleased;
    }

    public void AcceptThrowDelta(Vector3 deltaPointer)
    {
        throwInputVector = deltaPointer;
        throwReleased = true;
    }

    public Vector3 GetThrowVector()
    {
        return throwInputVector;
    }

    public void ThrowBall()
    {
        controlMode = ControlMode.Catching;
        ResetInputs();
        if (inputUI == null)
            return;
        inputUI.enableCatchingControls();
    }

    public void GrabBall()
    {
        controlMode = ControlMode.Throwing;
        ResetInputs();
        if (inputUI == null)
            return;
        inputUI.enableThrowingControls();
    }
}
