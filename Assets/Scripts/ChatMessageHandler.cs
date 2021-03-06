using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ChatMessageHandler : MonoBehaviourPun
{
    public HudNotification receivedMsg;

    public enum PhotonEventCode
    {
        CHATMESSAGE = 0
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventRaised;
        GameManager.Instance.chatMessageHandler = this;
    }

    public void OnEventRaised(EventData photonEvent)
    {
        PhotonEventCode eventCode = (PhotonEventCode)photonEvent.Code;
        object content = photonEvent.CustomData;
        DecryptData(eventCode, content);
    }
    void DecryptData(PhotonEventCode code, object content)
    {
        if (code == PhotonEventCode.CHATMESSAGE)
        {
            object[] datas = content as object[];
            if ((int)datas[2] != this.photonView.ViewID)// this will let only other users in room see msgs but not us
                receivedMsg.ChatMessage = (string)datas[0] + " sent by " + (string)datas[1];
        }
    }
    public void SendMessageToAll(string msg)
    {
        object[] datas = new object[] { msg, PhotonNetwork.LocalPlayer.NickName, base.photonView.ViewID };
        RaiseEventOptions options = new RaiseEventOptions
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All,
            //options.TargetActors = new int[] { };
        };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.CHATMESSAGE, datas, options, sendOptions);
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventRaised;
    }
}
