using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Vishal.Networking
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        GameObject beams;

        bool isFiring;

        public float health = 1f;
        private void Awake()
        {
            if(beams == null)
            {
                Debug.Log("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }

        private void Update()
        {
            ProcessInputs();
            if(beams!=null && isFiring != beams.activeInHierarchy)
            {
                beams.SetActive(isFiring);
            }
        }
        void ProcessInputs()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                if(!isFiring)
                {
                    isFiring = true;
                }
            }
            if(Input.GetButtonUp("Fire1"))
            {
                if(isFiring)
                {
                    isFiring = false;
                }
            }
        }

        /// <summary>
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if(!photonView.IsMine)
            {
                return;
            }
            if(!other.name.Contains("Beams"))
            {
                return;
            }
            health -= 0.1f;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beams"))
            {
                return;
            }
            health -= 0.1f * Time.deltaTime;
        }
    }//class
}
