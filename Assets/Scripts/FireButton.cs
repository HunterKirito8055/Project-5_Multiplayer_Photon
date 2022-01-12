using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MyPlayer player;
    public void SetPlayer(MyPlayer _player)
    {
        player = _player;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        player.Fire();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        player.FireUp();

    }
}
