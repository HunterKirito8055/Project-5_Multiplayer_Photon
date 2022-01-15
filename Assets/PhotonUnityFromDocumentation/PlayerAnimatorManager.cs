using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Vishal.Networking
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviour
    {
        Animator animator;
        public float directionDapmTime = 0.25f;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if(!animator)
            {
                Debug.Log("PlayerAnimatormnager is missing animator component", this);
            }    
        }

        private void Update()
        {
            if(!animator)
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if(v<0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDapmTime, Time.deltaTime);
        }


    }//class
}
