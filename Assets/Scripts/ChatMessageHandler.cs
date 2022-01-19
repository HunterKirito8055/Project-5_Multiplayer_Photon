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
    public Text receivedMsg;

    public enum EventCodes
    {
        CHATMESSAGE = 0
    }
    private void OnEnable()
    {

    }

    public void OnEventRaised(EventData photonEvent)
    {

    }

    public void SendMessageToAll(string msg)
    {
        object[] datas = new object[] { msg };
        RaiseEventOptions options = new RaiseEventOptions();
        options.CachingOption = EventCaching.DoNotCache;
        options.Receivers = ReceiverGroup.All;


        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent((byte)EventCodes.CHATMESSAGE, datas, options, sendOptions);
    }
    private void OnDisable()
    {

    }
}
