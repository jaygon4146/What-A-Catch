using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour {

    private Rigidbody rb;

    public Vector3 launchVector = Vector3.forward + Vector3.up;

    public float launchMagnitude = 10f;

    [SerializeField]
    private Vector3 launchForce;


	void Awake () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchThis();
        }
	}


    public void ClickThis()
    {
        LaunchThis();

    }

    public void LaunchThis()
    {
        print("LaunchThis");
        rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        rb.useGravity = true;

        launchVector = transform.forward + Vector3.up;

        launchForce = launchVector.normalized * launchMagnitude;


        rb.AddForce(launchForce);
    }
}
