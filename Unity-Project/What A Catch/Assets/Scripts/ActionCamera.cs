using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour {

    [SerializeField] private Transform CameraTransform;

    [Range (0, 1)]
    public float BezierProgress;
    
}
