using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;


public class TestPlayer : NetworkBehaviour {
    
    [SerializeField] public float rotation = 0f;
    [SerializeField] private float distance = 7.5f;

    public Camera myCamera;
    public Renderer rend;

    [SerializeField] TestNetworkToggle myNetToggles;

	// Use this for initialization
	void Awake () {

        transform.position = new Vector3 (0f, 1f, 0f);
        rotation = Random.Range(0f, 360f);
        transform.Rotate(new Vector3(0f, rotation, 0f));

        transform.Translate(Vector3.forward * distance);

        Vector3 toLook = new Vector3(0f, 4f, 0f);
        transform.LookAt(toLook);

        Vector3 newLooking = new Vector3(20, 0, 0);

        transform.Rotate(newLooking);

        if (isLocalPlayer)
        {            
            myCamera.enabled = true;
        }
	}

    public override void OnStartLocalPlayer()
    {
        //nderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.green);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
