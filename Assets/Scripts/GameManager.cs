using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Camera sceneCam;
    public GameObject player;
    public Transform spawnPoint;
    public Button leaveRoomBtn;
    public TextMeshProUGUI pingTxt;
    private void Start()
    {
        PhotonNetwork.SerializationRate = 20;
        leaveRoomBtn.onClick.AddListener(LeaveRoom);
        sceneCam.gameObject.SetActive(false);
        if (player == null)
            player = Resources.Load("Player") as GameObject;
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation).SetActive(true); 
    }

    private void Update()
    {
        pingTxt.text = PhotonNetwork.GetPing().ToString();
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
