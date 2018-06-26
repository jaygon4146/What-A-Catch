using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpinner : MonoBehaviour {

    public float spin = 1f;
    public float interval = 10f;
    public float countDown = 0f;

    private float x = 0;
    private float y = 0;
    private float z = 0;

    // Use this for initialization
    void Awake ()
    {
        RandomizeColor();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (countDown <= 0)
        {
            x = Random.Range(-spin, spin);
            y = Random.Range(-spin, spin);
            z = Random.Range(-spin, spin);

            countDown = interval;
        }
        transform.Rotate(new Vector3(x, y, z));

        countDown -= Time.deltaTime;
	}

    public void ClickThis()
    {
        print("ClickThis()");
        RandomizeColor();
    }

    private void RandomizeColor()
    {
        print("RandomizeColor()");
        Renderer rend = GetComponent<Renderer>();
        Color c = Random.ColorHSV();
        rend.material.SetColor("_Color", c);

    }
}
