using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallBehaviour : NetworkBehaviour {

    public enum BallState
    {
        Carried,
        Free,
    }
    public BallState ballState = BallState.Free;

    private Rigidbody rigidbody;
    private SphereCollider collider;

    private KidUnit myCarrier;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
    }
        
    private void FixedUpdate()
    {
        if (ballState == BallState.Free)
        {

        }

        if (ballState == BallState.Carried)
        {
            //transform.position = myCarrier.position;
            transform.position = myCarrier.GetBallPosition();
        }
    }

    public void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Throw(Vector3 throwForce)
    {
        RpcThrow(throwForce);
    }
    [ClientRpc]
    public void RpcThrow(Vector3 throwForce)
    {
        if (ballState != BallState.Carried)
            return;

        myCarrier.RpcAcceptBallThrow();

        //print("Throw()");
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true; ;
        rigidbody.AddForce(throwForce, ForceMode.Acceleration);
        collider.enabled = true;
        ballState = BallState.Free;
    }


    public void Grab(GameObject grabber)
    {
        RpcGrab(grabber);
    }
    [ClientRpc]
    public void RpcGrab(GameObject grabber)
    {
        //print("Grab()");
        if (ballState != BallState.Free)
            return;

        myCarrier = grabber.GetComponent<KidUnit>();
        myCarrier.RpcAcceptBallGrab();

        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = false;
        collider.enabled = false;
        ballState = BallState.Carried;
    }
}
