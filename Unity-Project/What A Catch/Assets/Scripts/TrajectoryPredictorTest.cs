using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPredictorTest : MonoBehaviour {

    public enum CameraState
    {
        Aiming,
        InFlight,
        Catching,
    }
    public CameraState cameraState = CameraState.Catching;

    [SerializeField] private GameObject MarkerObject;
    [SerializeField] private int NumberOfMarkers = 20;
    [SerializeField] private Vector3 throwVelocity;

    [SerializeField] private Vector3 inputDelta;
    [SerializeField] private float throwForce;
    [SerializeField] private Vector3 startingPosition;

    [SerializeField] private Vector3 peakPosition;
    [SerializeField] private float peakTime;
    [SerializeField] private Vector3 landPosition;
    [SerializeField] private float landTime;

    private List<GameObject> MarkerList = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < NumberOfMarkers; i++)
        {
            GameObject go = Instantiate<GameObject>(MarkerObject, Vector3.down * 5, Quaternion.identity);
            MarkerList.Add(go);
        }
    }
    public void SetToAiming()
    {
        cameraState = CameraState.Aiming;
    }
    public void SetToInFlight()
    {
        cameraState = CameraState.InFlight;
    }
    public void SetToCatching()
    {
        cameraState = CameraState.Catching;
    }

    private void Update()
    {
        if (cameraState == CameraState.Aiming)
        {
            FindPeakPosition();
            FindLandPosition();

            for (int i = 0; i < NumberOfMarkers; i++)
            {
                float timeFraction = i / landTime;
                float timeStep = timeFraction * landTime;

                Vector3 markerPos = FindPositionAtTime(timeStep);
                //print("markerPos#" + i + " :  " + markerPos);
                MarkerList[i].transform.position = markerPos;
            }
        }
    }

    private void FindPeakPosition()
    {
        float t = CalculatePeakTime();      //How long till velocity = 0
        float y = CalculateHeightAtTime(t); //How high is that

        float r = CalculateRange(t);        //How far is that
        Vector3 peak = FindPositionAtTime(t);
        peakPosition = peak;
        peakTime = t;
    }

    private void FindLandPosition()
    {
        float t = CalculateTimeTillYZero(); //How long till y = 0
        float r = CalculateRange(t);        //How far is that

        Vector3 position = FindPositionAtTime(t);
        landPosition = position;
        landTime = t;
    }
    
    private Vector3 FindPositionAtTime(float t)
    {
        float y = CalculateHeightAtTime(t); //How high is that
        float r = CalculateRange(t);        //How far is that

        Vector2 direction = new Vector2(throwVelocity.x, throwVelocity.z);
        direction = direction.normalized;

        Vector2 horizontal = direction * r;
        Vector3 pos = new Vector3(horizontal.x, y, horizontal.y);
        Vector3 startOffset = new Vector3(startingPosition.x, 0, startingPosition.y);
        pos += startOffset;
        return pos;
    }

    private float CalculateRange(float time)
    {
        float range = 0;
        /*
        Range of the throw, is the delta X when y = 0
        First, we need the time of the throw.               
        */
        //float time = CalculateTimeTillYZero();
        /*
        Using X Displacement
        X = V.0.x * t
        */
        float m = new Vector2(throwVelocity.x, throwVelocity.z).magnitude;
        range = m * time;
        //print("range: " + range);
        return range;
    }

    private float CalculateTimeTillYZero()
    {
        float time = 0;
        /*
        Using Y Displacement =
            0 = Ax^2        + Bx        +   C       <--- Quadratic Formula

            x = -B (+-)Sqr(B^2 - 4AC) /
                2A
        
            Y = 1/2 gt^2    + V.0.y * t   + Y.0     <--- Quadratic Formula
               [A  ]          [B   ]        [C]         
        */
        float g = Physics.gravity.y;

        float A = 0.5f * g;
        float B = throwVelocity.y;
        float C = startingPosition.y;

        float posT  = (-B + Mathf.Sqrt(B * B - 4 * (A * C))) /
                        (2 * A);
        float negT  = (-B - Mathf.Sqrt(B * B - 4 * (A * C))) /
                        (2 * A);

        //print("posT: " + posT + "        negT: " + negT);

        time = Mathf.Max(posT, negT);

        //print("time till y = 0: " + time);

        return time;
    }

    private float CalculateHeightAtTime(float time)
    {
        float height = 0;
        /*
        Height of the throw, is when Velocity.y = 0
        First, we need the time when this occurs
        */
        //float time = CalculatePeakTime();
        /*
        Using Y Displacement
        Y = 1/2gt^2     + V.0.y * t     + Y.0
        */
        float g = Physics.gravity.y;

        height =    (0.5f*g*time)*(0.5f*g*time) + 
                    throwVelocity.y * time      +
                    startingPosition.y;
        //print("peak height: " + height);
        return height;
    }

    private float CalculatePeakTime()
    {
        float time = 0;
        /*
        Using Y Velocity
        V.y = V.0.y + gt
        t = -V.0.y / g
        */
        float g = Physics.gravity.y;
        time = -throwVelocity.y / g;
        //print("time till y velocity = 0: " + time);
        return time;
    }
    public void AcceptThrowVelocity(Vector3 vector)
    {
        throwVelocity = vector;
    }
    public void AcceptStartPos(Vector3 pos)
    {
        startingPosition = pos;
    }
}


