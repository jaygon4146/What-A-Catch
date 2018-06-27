using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour {

    public enum BallState
    {
        Carried,
        Free,
    }
    public BallState ballState = BallState.Free;

    private Rigidbody rigidbody;
    private SphereCollider collider;

    [SerializeField] private int carrierId = -1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
    }

    public bool AttemptPickUp(int kidId)
    {
        if (ballState != BallState.Free)
        {
            return false;
        }
        carrierId = kidId;
        ballState = BallState.Carried;
        print("Ball was picked up by Kid #" + carrierId);

        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;

        return true;
    }

    public bool AttemptRelease(int kidId)
    {
        if (ballState != BallState.Carried)
        {
            return false;
        }
        print("Ball was released by Kid #" + carrierId);
        carrierId = -1;
        ballState = BallState.Free;

        rigidbody.useGravity = true;

        return true;
    }

    private void FixedUpdate()
    {
        if (ballState == BallState.Free)
        {

        }

        if (ballState == BallState.Carried)
        {
            
        }
    }

    public bool AttemptCarryBall(int kidId, Transform transformCarrier)
    {
        if (ballState != BallState.Carried)
        {
            return false;
        }
        if (kidId != carrierId)
        {
            return false;
        }
        transform.position = transformCarrier.position;

        return true;
    }
    
    public int GetCarrierId()
    {
        return carrierId;
    }
}
