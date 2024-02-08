using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CameraSetup : MonoBehaviourPun
{
    public CinemachineVirtualCamera followCam;

    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (photonView.IsMine)
            {
                followCam = GameObject.Find("FollowCam").GetComponent<CinemachineVirtualCamera>();
                followCam.Follow = transform;
            }
        }
        else
        {
            followCam = GameObject.Find("FollowCam").GetComponent<CinemachineVirtualCamera>();
            followCam.Follow = transform;
        }
    }
}
