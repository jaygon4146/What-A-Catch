﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReferee : MonoBehaviour {

    [SerializeField] private List<KidController> kidList = new List<KidController>();

    public int AddToKidList(KidController kid)
    {
        int r = 0;
        kidList.Add(kid);
        r = kidList.Count - 1;
        return r;
    }

    private void FixedUpdate()
    {

    }
}
