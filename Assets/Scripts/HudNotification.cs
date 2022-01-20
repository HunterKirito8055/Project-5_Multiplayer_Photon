using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

[RequireComponent(typeof(CanvasGroup))]
public class HudNotification : MonoBehaviourPun, IChatClientListener
{
    public Text notificationTxt;
    CanvasGroup canvasGroup;

    public string Notification
    {
        set
        {
            notificationTxt.text = value;
            gameObject.SetActive(true);
        }
    }
    public string ChatMessage
    {
        set
        {
            notificationTxt.text = value;
            {
                gameObject.SetActive(true);
            }
        }
    }
    private void OnEnable()
    {
        canvasGroup.alpha = 1;
        StartCoroutine(FadeOut());
    }
    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    IEnumerator FadeOut()
    {
        float t = 1.5f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, 1f);
        }
        gameObject.SetActive(false);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
