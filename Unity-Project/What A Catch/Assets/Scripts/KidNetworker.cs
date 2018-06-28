using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class KidNetworker : NetworkBehaviour
{
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    GameObject mainCamera;

    [SerializeField] private static List<KidNetworker> kidList = new List<KidNetworker>();

    private void Start()
    {
        mainCamera = Camera.main.gameObject;

        EnablePlayer();    
    }

    [ServerCallback]
    private void OnEnable()
    {
        if (!kidList.Contains(this))
        {
            kidList.Add(this);
        }
    }

    [ServerCallback]
    private void OnDisable()
    {
        if (kidList.Contains(this))
        {
            kidList.Remove(this);
        }
    }
    
    void DisablePlayer()
    {
        if (isLocalPlayer)
        {
            mainCamera.SetActive(true);
        }

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
        {
            mainCamera.SetActive(false);
        }

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }
    }
    
}
