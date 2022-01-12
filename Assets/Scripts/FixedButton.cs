using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler,IPointerClickHandler
{
    //[HideInInspector]
    [Tooltip("Don't interrupt this")]
    public bool pointerDown;

    public MyPlayer player;
    // Use this for initialization
    void Start()
    {

    }

    public void SetPlayer(MyPlayer _player)
    {
        player = _player;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       // player.Jump();
    }
}
