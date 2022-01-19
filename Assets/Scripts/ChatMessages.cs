using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessages : MonoBehaviour
{
    public Button[] msgButtons;
    public Transform buttonContent;
    private void Start()
    {
        GameManager.Instance.chatMessages = this;
        msgButtons = buttonContent.GetComponentsInChildren<Button>(true);
        foreach (var item in msgButtons)
        {
            item.onClick.AddListener(()=>GameManager.Instance.chatMessageHandler.SendMessageToAll(item.GetComponentInChildren<Text>().text));
        }
    }
    
}
