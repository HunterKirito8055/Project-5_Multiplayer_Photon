using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[RequireComponent(typeof(CanvasGroup))]
public class HudNotification : MonoBehaviourPun
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
            if (!GameManager.Instance.player.GetPhotonView().IsMine)
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
}
