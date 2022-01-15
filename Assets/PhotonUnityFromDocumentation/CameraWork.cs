using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Vishal.Networking
{
    public class CameraWork : MonoBehaviourPun
    {
        [SerializeField]
        float distance = 7f, height = 3f, smoothSpeed = 0.125f;
        [SerializeField]
        Vector3 centerOffset = Vector3.zero;

        [SerializeField]
        bool followOnStart = false;


        Transform cameraTrans;
        bool isFollowing;
        Vector3 cameraOffset = Vector3.zero;
        private void Start()
        {
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }

        private void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (cameraTrans == null && isFollowing)
            {
                OnStartFollowing();
            }
            // only follow is explicitly declared
            if (isFollowing)
            {
                Follow();
            }
        }
        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTrans = Camera.main.transform;
            isFollowing = true;
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }
        void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;
            cameraTrans.position = Vector3.Lerp(cameraTrans.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

            cameraTrans.LookAt(this.transform.position + centerOffset);
        }
        void Cut()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTrans.position = this.transform.position + this.transform.TransformVector(centerOffset);
            cameraTrans.LookAt(this.transform.position + centerOffset);
        }
    }//class
}
