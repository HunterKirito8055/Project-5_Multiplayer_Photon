using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera sceneCam;
    public GameObject player;
    private void Start()
    {
        sceneCam.gameObject.SetActive(false);
        player = Resources.Load("Player") as GameObject;
        Instantiate(player);
    }
}//class
