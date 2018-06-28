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

    private Transform myCarrier;
    
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
            transform.position = myCarrier.position;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Throw(Vector3 throwForce){
        print("Throw()");
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(throwForce, ForceMode.Acceleration);
    }
}
