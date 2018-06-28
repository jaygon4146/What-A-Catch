using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour {

    public GameObject ballPrefab;

    //[SerializeField] private List<KidController> kidList = new List<KidController>();

    public override void OnStartServer()
    {
        Vector3 spawnPos = new Vector3(0f, 5f, 5f);
        Quaternion spawnRot = Quaternion.identity;
        GameObject ballObj = (GameObject)Instantiate(ballPrefab, spawnPos, spawnRot);
        NetworkServer.Spawn(ballObj);
    }




}
