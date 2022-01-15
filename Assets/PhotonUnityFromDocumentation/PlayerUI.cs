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

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }
        public void SetTarget(PlayerManager _playerManager)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            target = _playerManager;
            if (playerNametxt != null)
            {
                playerNametxt.text = target.photonView.Owner.NickName;
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
    }//class
}
