using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLookAt : MonoBehaviour {

    public Transform target;

	void Update ()
    {
        Vector3 toLook = new Vector3(0f, 4f, 0f);
        transform.LookAt(toLook);
    }
}
