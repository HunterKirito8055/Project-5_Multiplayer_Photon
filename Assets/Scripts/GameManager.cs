using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Camera sceneCam;
    public GameObject player;
    public Transform[] spawnPoints;
    public Button leaveRoomBtn;
    public Text pingTxt;
    public int totalPlayers;
    public HudNotification hudNotification;
    private void Start()
    {
        totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        leaveRoomBtn.onClick.AddListener(LeaveRoom);
        sceneCam.gameObject.SetActive(false);
        if (player == null)
            player = Resources.Load("Player") as GameObject;

        player = PhotonNetwork.Instantiate(player.name, spawnPoints[totalPlayers - 1].position, spawnPoints[totalPlayers - 1].rotation);
        player.SetActive(true);
        player.name = PhotonNetwork.NickName;
        PhotonNetwork.SendRate = 25;
        PhotonNetwork.SerializationRate = 15;
    }
    /// <summary>
    /// Called when the local player left the room. We need to load the lobby scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

    }
  
    private void Update()
    {
        pingTxt.text = PhotonNetwork.GetPing().ToString();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}//class
