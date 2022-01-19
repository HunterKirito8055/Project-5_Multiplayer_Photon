using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public Camera sceneCam;
    public GameObject player;
    public Camera playerCam;
    public Transform[] spawnPoints;
    public Button leaveRoomBtn;
    public Text pingTxt;
    public int totalPlayers;
    public HudNotification hudNotification;
    public ChatMessageHandler chatMessageHandler;
    public ChatMessages chatMessages;
    public List<MyPlayer> allPlayers;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        leaveRoomBtn.onClick.AddListener(LeaveRoom);
        sceneCam.gameObject.SetActive(false);
        if (player == null)
            player = Resources.Load("Player") as GameObject;
        object[] data = new object[] { PhotonNetwork.NickName };
        player = PhotonNetwork.Instantiate(player.name, spawnPoints[totalPlayers - 1].position, spawnPoints[totalPlayers - 1].rotation, data: data);
        MyPlayer mp = player.GetComponent<MyPlayer>();
        allPlayers.Add(mp);
        player.SetActive(true);
        player.name = PlayerPrefs.GetString("PlayerName");
        playerCam = mp.myCam.GetComponent<Camera>();
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
        hudNotification.Notification = newPlayer.NickName + " has entered the room";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        hudNotification.Notification = otherPlayer.NickName + " has left the room";
        GameManager.Instance.allPlayers.RemoveAll(x => x.photonView.Controller == otherPlayer);
        Debug.Log("player removed");
    }

    private void Update()
    {
        pingTxt.text = PhotonNetwork.GetPing().ToString();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        if (PhotonNetwork.PlayerListOthers.Length > 0)
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
        PhotonNetwork.LeaveRoom(false);
    }

}//class
