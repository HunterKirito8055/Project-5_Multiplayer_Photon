using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.Vishal.Networking
{

    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] Text playerNametxt;
        [SerializeField] Slider playerHealthSlider;

        PlayerManager target;

        [SerializeField]
        Vector3 screenOffset = new Vector3(0, 30, 0);

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup canvasGroup;
        Vector3 targetPosition;

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            canvasGroup = this.GetComponent<CanvasGroup>();
        }
        public void SetTarget(PlayerManager _playerManager)
        {
            if (_playerManager == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            target = _playerManager;
            if (playerNametxt != null)
            {
                playerNametxt.text = target.photonView.Owner.NickName;
            }

            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController cc = target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (cc != null)
            {
                characterControllerHeight = cc.height;
            }
        }

        private void Update()
        {
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.health;
            }

        }

        private void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }
            // #Critical
            // Follow the Target GameObject on screen.
            if (target.photonView.IsMine)
                if (targetTransform != null)
                {
                    targetPosition = targetTransform.position;
                    targetPosition.y += characterControllerHeight;
                    this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
                }
        }
    }//class
}
