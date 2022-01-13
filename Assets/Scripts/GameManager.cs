using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Camera sceneCam;
    public GameObject player;
    public Transform spawnPoint;
    public Button leaveRoomBtn;
    private void Start()
    {
        leaveRoomBtn.onClick.AddListener(LeaveRoom);
        sceneCam.gameObject.SetActive(false);
        if (player == null)
            player = Resources.Load("Player") as GameObject;
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation).SetActive(true); 
    }
    /// <summary>
    /// Called when the local player left the room. We need to load the lobby scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}//class
