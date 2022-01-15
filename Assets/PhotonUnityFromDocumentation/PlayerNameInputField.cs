using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Vishal.Networking
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviourPun
    {
        const string playerNamePrefKey = "PlayerName";
        [SerializeField]
        InputField _inputField;
        private void Start()
        {
            string defaultName = string.Empty;
            _inputField = GetComponent<InputField>();
            _inputField.onValueChanged.AddListener(SetPlayerName);
            if (_inputField !=null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }
            PhotonNetwork.NickName = defaultName;
        }


        public void SetPlayerName(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                Debug.Log("PlayerName is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }


    }//class
}
