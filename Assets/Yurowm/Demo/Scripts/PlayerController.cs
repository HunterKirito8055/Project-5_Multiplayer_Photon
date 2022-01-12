﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;
    public Arsenal currentArsenal;

    private Animator animator;

    MyPlayer myPlayer;

    void Awake()
    {
        myPlayer = GetComponent<MyPlayer>();
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
        {
            SetArsenal(arsenal[1].name);
        }
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                currentArsenal = hand;
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    myPlayer.rightGunMuzzleFlash = newRightGun.GetComponentInChildren<ParticleSystem>();
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    myPlayer.rightGunMuzzleFlash = newLeftGun.GetComponentInChildren<ParticleSystem>();
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
