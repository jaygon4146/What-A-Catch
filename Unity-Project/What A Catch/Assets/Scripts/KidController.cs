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
    #endregion
    //==================================================
    #region Components
    [SerializeField] private GameManager     gameManager;
    private InputManager    kidInput;
    private Rigidbody       rigidbody;
    #endregion
    //==================================================
    #region GamePieces
    public Transform ballHolder;
    #endregion
    //==================================================
    #region Attributes
    [SerializeField] private int kidListId;
    [SerializeField] private BallBehaviour ballBehaviour;
    #endregion
    //==================================================
    //==================================================
    //==================================================
    #region Setup
    void Awake()
    {
        GetComponents();
        kidListId = gameManager.AddToKidList(this);
    }

    void GetComponents()
    {
        GameObject obj = GameObject.FindWithTag("GameManager");
        gameManager = obj.GetComponent<GameManager>();

        kidInput = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody>();
    }
    #endregion
    //==================================================
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
        float horz = kidInput.MoveHorizontalAction.axisFloat;
        float vert = kidInput.MoveVerticalAction.axisFloat;

        Vector3 inputVector = new Vector3(horz, 0f, vert);
        moveVector = inputVector * moveForce;

        rigidbody.velocity = moveVector;
    }

    void ThrowingFixedUpdate()
    {
        float horz = kidInput.MoveHorizontalAction.axisFloat;
        float vert = kidInput.MoveVerticalAction.axisFloat;

        Vector3 inputVector = new Vector3(horz, 0f, vert);
        moveVector = inputVector * moveForce;

        rigidbody.velocity = moveVector;

        bool carried = ballBehaviour.AttemptCarryBall(kidListId, ballHolder);

        if (kidInput.ReleaseBallAction.buttonHeld)
        {
            bool success = ballBehaviour.AttemptRelease(kidListId);
            if (success)
            {
                ThrowBall();
            }
        }
    }
    //==================================================
    #region BallActions
    public void PickUpBall()
    {        
        controlType = ControlType.Throwing;
    }
    private void ThrowBall()
    {
        controlType = ControlType.Catching;
    }
    #endregion
    //==================================================
    #region ColliderEvents
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            BallBehaviour ball = collision.gameObject.GetComponent<BallBehaviour>();
            bool success = ball.AttemptPickUp(kidListId);
            if (success)
            {
                PickUpBall();
                ballBehaviour = ball;
            }
        }
    }
    #endregion
    //==================================================


}
