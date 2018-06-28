using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class KidController : NetworkBehaviour {
    //==================================================
    #region Inputs
    public enum ControlType
    {
        MenuNavigation,
        Throwing,
        Catching,
    }
    public ControlType controlType = ControlType.Catching;
    #endregion
    //==================================================
    #region Physics
    public float moveForce = 5f;
    [SerializeField] Vector3 moveVector = new Vector3();
    public float throwForce = 10f;
    [SerializeField] Vector3 throwVector = Vector3.up;
    #endregion
    //==================================================
    #region Components
    private KidUnit         kidUnit;
    private KidNetworker    kidNetworker;
    private InputManager    kidInput;
    private Rigidbody       rigidbody;
    #endregion
    //==================================================
    #region GamePieces
    public Transform ballHolder;
    #endregion
    //==================================================
    #region Attributes
    #endregion
    //==================================================
    //==================================================
    //==================================================
    #region Setup
    void Awake()
    {
        GetComponents();
    }
    void GetComponents()
    {
        kidUnit         = GetComponent<KidUnit>();
        kidInput        = GetComponent<InputManager>();
        rigidbody       = GetComponent<Rigidbody>();
    }
    #endregion
    //==================================================
    #region PhysicsUpdates
    void FixedUpdate ()
    {
        if (controlType == ControlType.Catching)
        {
            CatchingFixedUpdate();
        }
        if (controlType == ControlType.Throwing)
        {
            ThrowingFixedUpdate();
        }
    }
    void CatchingFixedUpdate()
    {
        ApplyMovement();
    }

    void ThrowingFixedUpdate()
    {
        ApplyMovement();

        if (kidInput.ReleaseBallAction.buttonDown)
        {
            ThrowBall();
        }
    }

    void ApplyMovement()
    {
        float horz = kidInput.MoveHorizontalAction.axisFloat;
        float vert = kidInput.MoveVerticalAction.axisFloat;

        Vector3 inputVector = new Vector3(horz, 0f, vert);
        moveVector = inputVector * moveForce;

        rigidbody.velocity = moveVector;
    }
    #endregion
    //==================================================
    #region BallCommands
    private void ThrowBall()
    {
        print("ThrowBall()");
        throwVector = Vector3.up * throwForce;
        kidUnit.AttemptThrowBall(ballHolder.position, throwVector);
    }
    #endregion
    //==================================================
    #region ColliderEvents
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
        }
    }
    #endregion
    //==================================================
}
