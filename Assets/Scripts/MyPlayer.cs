using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 3f;
    public float smoothRotationTime = 0.125f;
    public bool enableMobile;
    public GameObject gunRayPoint, headPoint;
    public GameObject crosshair;

    public GameObject playerName3D;
    //jump and fall
    public float jumpForce;
    [Range(0, 5f)]
    public float fallMulti = 2.5f;

    public FixedJoystick joystick;
    public FixedButton jumpBtn, fireBtn;

    [Header("Chat System")]
    public GameObject chatSys;

    [Space(15)]

    public ParticleSystem leftGunMuzzleFlash, rightGunMuzzleFlash;


    public bool fire;
    //sounds
    public AudioSource shootSound, runSound;


    //health
    public Image healthBar;
    public Gradient healthGradient;
    public float playerHealth;
    public float PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            playerHealth = value;
            if (photonView.IsMine)
            {
                healthBar.fillAmount = value;
                healthBar.color = healthGradient.Evaluate(value);
            }
        }
    }


    public bool isFire
    {
        get
        {
            return fire;
        }
        set
        {
            fire = value;
            GunMuzzleFlash(fire);
            if (photonView.IsMine)
            {
                anim.SetBool("fire 0", value);
                shootSound.loop = value;
                if (value)
                {
                    shootSound.Play();
                }
                else
                {
                    shootSound.Stop();
                }
            }
        }
    }

    [Space(20)]
    [Header("Just There!")]
    public float currentVeclocity;
    public float currentSpeed;
    float speedVelocity;
    Transform myCamera;
    Actions actions;
    Animator anim;
    Rigidbody rb;
    PlayerController pc;
    MyCamera myCam;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
            pc = GetComponent<PlayerController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            myCam = GameObject.Find("PlayerCamera").GetComponent<MyCamera>();
            if (myCam)
            {
                myCam.gameObject.SetActive(true);
                myCam.player = headPoint.transform;
                myCam.playerPunView = this.photonView;
            }
            chatSys.gameObject.SetActive(true);
            playerName3D.SetActive(false);
        }
        else
        {
            chatSys.gameObject.SetActive(false);
            playerName3D.SetActive(true);
        }
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            myCamera = Camera.main.transform;
            if (jumpBtn == null) jumpBtn = GameObject.Find("JumpBtn").GetComponent<FixedButton>();
            if (fireBtn == null) fireBtn = GameObject.Find("FireBtn").GetComponent<FixedButton>();
            healthBar.transform.parent.gameObject.SetActive(true);
            jumpBtn.SetPlayer(this);
            fireBtn.SetPlayer(this);
            crosshair.SetActive(true);
        }
        else
        {
            crosshair.SetActive(false);
            healthBar.transform.parent.gameObject.SetActive(false);
        }
        //actions = GetComponent<Actions>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            LocalPlayerUpdate();
        }
    }
    Vector2 movementInputs;
    void LocalPlayerUpdate()
    {
        if (enableMobile)
        {
            movementInputs = new Vector2(joystick.input.x, joystick.input.y);
        }
        else
        {
            movementInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        movementInputs = movementInputs.normalized;
        if (movementInputs != Vector2.zero)
        {
            float rot = Mathf.Atan2(movementInputs.x, movementInputs.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rot, ref currentVeclocity, smoothRotationTime);
            if (!runSound.isPlaying)
            {
                runSound.Play();
            }
        }
        else
        {
            runSound.Stop();
        }
        float targetSpeed = (moveSpeed * movementInputs.magnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);

        if (!isFire)
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        else
        {
            float rot = Mathf.Atan2(movementInputs.x, movementInputs.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * rot;
        }
        if (movementInputs.magnitude > 0)
        {
            anim.SetBool("run", true);
        }
        else if (Mathf.Approximately(movementInputs.magnitude, 0))
        {
            anim.SetBool("run", false);
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity -= Vector3.up * Mathf.Abs(Physics.gravity.y) * Mathf.Abs(fallMulti) * Time.deltaTime;
        }
        CheckInputsfrombuttons();
    }
    void CheckInputsfrombuttons()
    {
        if (jumpBtn.pressing)
        {
            Jump();
        }
        if (fireBtn.pressing)
        {
            Fire();
        }
        else
        {
            FireUp();
        }
    }
    private void LateUpdate()
    {
        if (photonView.IsMine)
            PositionCrosshair();
        else
        {
            float roty = Mathf.Atan2(movementInputs.x, movementInputs.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
            float rotx = myCamera.eulerAngles.x;
            playerName3D.transform.eulerAngles = new Vector3(rotx, roty, 0);
        }
    }
    PhotonView otherPlayer;
    public void Fire()
    {
        //anim.SetTrigger("fire");
        if (!isFire)
        {
            Debug.DrawRay(gunRayPoint.transform.position, Camera.main.transform.forward * 25f, Color.green);
            isFire = true;
        }
        if (isFire)
        {
            RaycastHit hit;
            if (Physics.Raycast(gunRayPoint.transform.position, Camera.main.transform.forward, out hit, 25f))
            {
                otherPlayer = hit.transform.GetComponent<PhotonView>();
                if (otherPlayer != null && hit.transform.CompareTag("Player") && !otherPlayer.IsMine)
                {
                    otherPlayer.RPC("GetDamage", RpcTarget.AllBuffered, 0.01f);
                    print(hit.transform.name);
                }
            }
        }

    }
    public void Jump()
    {
        anim.SetTrigger("jump");
        rb.velocity = rb.angularVelocity = Vector3.zero;
        // rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    public void FireUp()
    {
        if (isFire)
        {
            isFire = false;
        }
    }
    [PunRPC]
    public void GetDamage(float amount)
    {
        PlayerHealth -= amount;
    }
    Vector3 crosshairVel;
    void PositionCrosshair()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one / 2f);
        int layerMask = LayerMask.GetMask("Default");
        crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, ray.GetPoint(10), Time.deltaTime * 20f);
        crosshair.transform.LookAt(myCamera.transform);
        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            // Debug.Log(hit.transform.name);
            // crosshair.transform.position = Vector3.SmoothDamp(crosshair.transform.position,ray.GetPoint(10),ref crosshairVel,0.05f );
        }
    }

    void GunMuzzleFlash(bool value)
    {
        if (rightGunMuzzleFlash != null)
        {
            if (value)
                rightGunMuzzleFlash.Play();
            else
            {
                rightGunMuzzleFlash.Stop();
            }
        }
        if (leftGunMuzzleFlash != null)
        {
            if (value)
                leftGunMuzzleFlash.Play();
            else
            {
                leftGunMuzzleFlash.Stop();
            }
        }

    }

    private void OnDestroy()
    {
        if (myCam)
            myCam.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFire);
        }
        else
        {
            isFire = (bool)stream.ReceiveNext();
        }
    }
}//class
