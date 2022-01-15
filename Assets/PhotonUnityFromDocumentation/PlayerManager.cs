using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.Vishal.Networking
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static PlayerManager localPlayerInstance;

        [SerializeField]
        GameObject beams;

        public GameObject playerUiPrefab;

        bool isFiring;

        public float health = 1f;
        private void Awake()
        {
            if (beams == null)
            {
                Debug.Log("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.localPlayerInstance = this;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                    _cameraWork.OnStartFollowing();
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif

            if(playerUiPrefab !=null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab);
                PlayerUI pui = _uiGo.GetComponent<PlayerUI>();
                pui.SetTarget(this);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }
        private void Update()
        {
            if (photonView.IsMine)
                ProcessInputs();

            if (beams != null && isFiring != beams.activeInHierarchy)
            {
                beams.SetActive(isFiring);
            }
            if (health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode)
        {
            CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
#if !UNITY_5_4_OR_NEWER
        private void OnLevelWasLoaded(int level)
        {
            CalledOnLevelWasLoaded(level);
        }
#endif
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5))
            {
                transform.position = new Vector3(0f, 5, 0);
            }
            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            PlayerUI pui = _uiGo.GetComponent<PlayerUI>();
            pui.SetTarget(this);
        }
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!isFiring)
                {
                    isFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (isFiring)
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
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beams"))
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isFiring);
                stream.SendNext(health);
            }
            else
            {
                // Network player, receive data
                this.isFiring = (bool)stream.ReceiveNext();
                this.health = (float)stream.ReceiveNext();
            }
        }
#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif

    }//class
}
