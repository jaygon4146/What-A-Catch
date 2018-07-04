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
        //print("ThrowBall()");
        Vector3 throwInput = kidInput.GetThrowVector();
        //print("ThrowInput = " + throwInput);

        throwVector = new Vector3(-throwInput.x, 10, -throwInput.y);
        throwVector *= throwForce;
        //print("ThrowVector = " + throwVector);
        //print("ThrowBall()");
        kidUnit.AttemptThrowBall(ballHolder.position, throwVector);
    }
    [ClientRpc]
    public void RpcAcceptBallThrow()
    {
        if (isLocalPlayer)
        {
            kidInput.ThrowBall();
            holdingBall = false;
        }
    }
    [ClientRpc]
    public void RpcAcceptBallGrab()
    {
        if (isLocalPlayer)
        {
            moveVector = Vector3.zero;
            rigidbody.velocity = moveVector;
            kidInput.GrabBall();
        }
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
