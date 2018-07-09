using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KidUnit : NetworkBehaviour {

    private GameObject[] ballObjects;
    private List<BallBehaviour> ballList = new List<BallBehaviour>();
    private KidController myKid;

    private void Awake()
    {
        ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject obj in ballObjects)
        {
            BallBehaviour b = obj.GetComponent<BallBehaviour>();
            if (b != null)
            {
                ballList.Add(b);
            }
        }
    }
    public void AttachKidController(KidController kid)
    {
        myKid = kid;
    }

    //==================================================
    #region KidController Input
    /*Local Controller needs to communicate...
     * - Their attempts at changing the Ball State
     */
    public void AttemptThrowBall(Vector3 origin, Vector3 throwVector)
    {
        CmdThrowBall(origin, throwVector);
    }

    public void AttemptGrabBall(GameObject grabber)
    {
        //print("AttemptGrabBall()");
        CmdGrabBall(grabber);
    }
    #endregion
    //==================================================
    #region BallServerCommands
    /*Server is in charge of processing...
     * - Keeping the Ball State
     * - Confirming & Applying changes in Ball State
     */
    [Command]
    private void CmdThrowBall(Vector3 origin, Vector3 throwVector)
    {
        ballList[0].MoveTo(origin);
        ballList[0].Throw(throwVector);
    }
    [Command]
    private void CmdGrabBall(GameObject grabber)
    {
        //print("CmdGrabBall()");
        ballList[0].Grab(grabber);
    }

    public Vector3 GetBallPosition()
    {
        return myKid.GetBallPosition();
    }

    #endregion
    //================================================== 
    #region BallRemoteProcedureCalls
    /*Server tells clients to process...
     * - Update changes in Ball State
     */
    [ClientRpc]
    public void RpcAcceptBallThrow()
    {
        if (isLocalPlayer)
        {
            myKid.AcceptBallThrow();
        }
    }
    [ClientRpc]
    public void RpcAcceptBallGrab()
    {
        if (isLocalPlayer)
        {
            myKid.AcceptBallGrab();
        }
    }
    #endregion
    //==================================================
}
