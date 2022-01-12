using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviour
{
    [Header("--UI Screens--")]
    public GameObject roomUI;
    public GameObject connectingUi;
    [Header("--UI Texts--")]
    public TextMeshProUGUI statusTxt;
    public TextMeshProUGUI connectingtxt;
    [Header("--UI InputFields--")]
    public TMP_InputField createRoomIF;
    public TMP_InputField joinRoomIF;


}//class
