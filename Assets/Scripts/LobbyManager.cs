using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("--UI Screens--")]
    public GameObject roomUI;
    public GameObject connectingUi;
    [Header("--UI Texts--")]
    public TextMeshProUGUI statusTxt;
    public TextMeshProUGUI connectingtxt;
    [Header("--UI InputFields--")]
    public TMP_InputField createRoomIF;
    public TMP_InputField joinRoomIF;
    public TMP_InputField playerNameIF;

    [Header("--UI Buttons--")]
    public Button createBtn;
    public Button joinBtn;
    public Button playBtn;

    const string playerNamePrefKey = "PlayerName";
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusTxt.text = "Please wait...";
        Debug.Log("awake");
        roomUI.SetActive(false);
        connectingUi.SetActive(true);
    }
    private void Start()
    {
        CheckForSavedName();
        playerNameIF.onValueChanged.AddListener(SetPlayerName);
        createBtn.onClick.AddListener(() => OnCreateBtn());
        joinBtn.onClick.AddListener(() => OnJoinBtn());
        playBtn.onClick.AddListener(() => OnPlayBtn());
    }
    void CheckForSavedName()
    {
        string defaultName = string.Empty;
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            playerNameIF.text = defaultName;
        }
        PhotonNetwork.NickName = defaultName;
    }
    public override void OnConnectedToMaster()
    {
        connectingUi.SetActive(true);
        roomUI.SetActive(false);
        connectingtxt.text = "Joining Lobby...";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("connected to master");
    }
    public override void OnJoinedLobby()
    {
        statusTxt.text = "Lobby Joined!";
        connectingUi.SetActive(false);
        roomUI.SetActive(true);
        Debug.Log("joined lobby");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
        Debug.Log("joined room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        joinRoomIF.text = "Enter Room Name Again";
        statusTxt.text = "No Such Room name Exists! \t Please try again!";
        Debug.Log("Join room failed");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (connectingUi)
            connectingUi.SetActive(false);
        if (roomUI) roomUI.SetActive(true);
        connectingtxt.text = "Disconnected..." + cause.ToString();
        Debug.Log("Disconnected");
        statusTxt.text = "";
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        int roomNum = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("RoomName" + roomNum.ToString(), roomOptions: roomOptions, TypedLobby.Default);
        Debug.Log("join random failed");
    }

    #region Button methods

    public void OnCreateBtn()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        if (createRoomIF.text != string.Empty)
            PhotonNetwork.CreateRoom(createRoomIF.text, roomOptions: roomOptions, TypedLobby.Default);
        else
        {
            statusTxt.text = "Invalid Room Name";
        }
    }
    public void OnJoinBtn()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        if (joinRoomIF.text != string.Empty)
            PhotonNetwork.JoinRoom(joinRoomIF.text);
        else
        {
            statusTxt.text = "A roomname is required. If you don't know one, how will you join?";
        }
    }

    public void OnPlayBtn()
    {
        PhotonNetwork.JoinRandomRoom();
        statusTxt.text = "Creating room now...";
    }
    #endregion

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        PlayerPrefs.SetString(playerNamePrefKey, value);
        PhotonNetwork.NickName = value;

    }
}//class
