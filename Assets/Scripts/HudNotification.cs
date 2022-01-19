using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudNotification : MonoBehaviour
{
    public Text notificationTxt;
    CanvasGroup canvasGroup;
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
            t -= Time.deltaTime ;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while(canvasGroup.alpha >0)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, 1f);
        }
        gameObject.SetActive(false);
    }
}
