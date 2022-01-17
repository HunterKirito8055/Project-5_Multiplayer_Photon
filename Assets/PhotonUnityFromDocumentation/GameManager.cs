using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Vishal.Networking
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        public GameObject playerPrefab;

        [SerializeField]
        private Button leaveRoomBtn;
        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            leaveRoomBtn.onClick.AddListener(LeaveRoom);
            if (PlayerManager.localPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting
            if (PhotonNetwork.IsMasterClient)
            {
                LoadArena();
            }
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);// seen when otherPlayer disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }

    }//class
}
