using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class KidController : NetworkBehaviour {
    
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
    private NetworkIdentity networkIdentity;
    private InputManager    kidInput;
    private Rigidbody       rigidbody;
    #endregion
    //==================================================
    #region GamePieces
    public Transform ballHolder;
    #endregion
    //==================================================
    #region Attributes
    [SerializeField] private bool holdingBall = false;
    [SerializeField] private string netId;
    #endregion
    //==================================================
    //==================================================
    //==================================================
    #region Setup
    void Awake()
    {
        GetComponents();
        netId = "#" + networkIdentity.netId;
        kidUnit.AttachKidController(this);      
    }
    void GetComponents()
    {
        kidUnit         = GetComponent<KidUnit>();
        kidNetworker    = GetComponent<KidNetworker>();
        networkIdentity = GetComponent<NetworkIdentity>();
        kidInput        = GetComponent<InputManager>();
        rigidbody       = GetComponent<Rigidbody>();
    }
    #endregion
    //==================================================
    #region PhysicsUpdates
    void FixedUpdate ()
    {
        if (kidInput.isCatching())
        {
            CatchingFixedUpdate();
        }
        if (kidInput.isThrowing())
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
        if (kidInput.throwWasReleased())
        {
            ThrowBall();
        }

        CalculateThrowVector();
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
    private void CalculateThrowVector()
    {
        Vector3 throwInput = kidInput.GetThrowVector();

        throwVector = new Vector3(-throwInput.x, 10, -throwInput.y);
        throwVector *= throwForce;


    }
    private void ThrowBall()
    {
        CalculateThrowVector();
        kidUnit.AttemptThrowBall(ballHolder.position, throwVector);
    }
    public void AcceptBallThrow()
    {
        kidInput.ThrowBall();
        holdingBall = false;
    }
    public void AcceptBallGrab()
    {
        moveVector = Vector3.zero;
        rigidbody.velocity = moveVector;
        kidInput.GrabBall();
    }
    public Vector3 GetBallPosition()
    {
        return ballHolder.transform.position;
    }
    
    #endregion
    //==================================================
    #region ColliderEvents
    private void OnCollisionEnter(Collision collision)
    {
        if (kidInput.isCatching())
        {
            if (collision.gameObject.tag == "Ball")
            {
                //print("Touched Ball()");
                kidUnit.AttemptGrabBall(gameObject);
            }
        }
    }
    #endregion
    //==================================================
}
