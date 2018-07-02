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

    private KidController myCarrier;
    
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
        if (ballState != BallState.Carried)
            return;

        //print("Throw()");
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true; ;
        rigidbody.AddForce(throwForce, ForceMode.Acceleration);
        ballState = BallState.Free;
    }

    public void Grab(GameObject grabber)
    {
        //print("Grab()");
        if (ballState != BallState.Free)
            return;

        myCarrier = grabber.GetComponent<KidController>();
        myCarrier.RpcAcceptBallGrab();

        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = false;
        ballState = BallState.Carried;
    }
}
