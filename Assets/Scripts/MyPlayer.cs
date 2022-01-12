using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MyPlayer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float smoothRotationTime = 0.25f;
    public bool enableMobile;
    public GameObject gunRayPoint, headPoint;
    public GameObject crosshair;
    //jump and fall
    public float jumpForce;
    [Range(0, 5f)]
    public float fallMulti = 2.5f;
    public FixedJoystick joystick;
    public FixedButton jumpBtn, fireBtn;

    public ParticleSystem leftGunMuzzleFlash, rightGunMuzzleFlash;


    public bool fire;
    //sounds
    public AudioSource shootSound, runSound;

    public bool isFire
    {
        get
        {
            return fire;
        }
        set
        {
            fire = value;
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
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        pc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        myCam = GameObject.Find("PlayerCamera").GetComponent<MyCamera>();
        if (myCam)
            myCam.player = headPoint.transform;
        myCamera = Camera.main.transform;
        if (jumpBtn == null) jumpBtn = GameObject.Find("JumpBtn").GetComponent<FixedButton>();
        if (fireBtn == null) fireBtn = GameObject.Find("FireBtn").GetComponent<FixedButton>();
        jumpBtn.SetPlayer(this);
        fireBtn.SetPlayer(this);
        //actions = GetComponent<Actions>();
    }
    void Update()
    {
        Vector2 inputDir;
        if (enableMobile)
        {
            inputDir = new Vector2(joystick.input.x, joystick.input.y);
        }
        else
        {
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        inputDir = inputDir.normalized;
        if (inputDir != Vector2.zero)
        {
            float rot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
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
        float targetSpeed = (moveSpeed * inputDir.magnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);

        if (!isFire)
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        if (inputDir.magnitude > 0)
        {
            anim.SetBool("run", true);
        }
        else if (Mathf.Approximately(inputDir.magnitude, 0))
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
        if (Input.GetKeyDown(KeyCode.Space))
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
        PositionCrosshair();
    }

    public void Fire()
    {
        //anim.SetTrigger("fire");
        if (!isFire)
        {
            isFire = true;
            RaycastHit hit;
            if (Physics.Raycast(gunRayPoint.transform.position, Camera.main.transform.forward, out hit, 25f))
            {
                print(hit.transform.name);
            }
            Debug.DrawRay(gunRayPoint.transform.position, Camera.main.transform.forward * 25f, Color.green);
            GunMuzzleFlash(true);
        }
    }
    public void Jump()
    {
        //  anim.SetTrigger("jump");
        rb.velocity = rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    public void FireUp()
    {
        if (isFire)
        {
            isFire = false;
            GunMuzzleFlash(false);
        }
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
            Debug.Log(hit.transform.name);
            // crosshair.transform.position = Vector3.SmoothDamp(crosshair.transform.position,ray.GetPoint(10),ref crosshairVel,0.05f );
        }
    }

    void GunMuzzleFlash(bool value)
    {
        if (rightGunMuzzleFlash)
        {
            if (value)
                rightGunMuzzleFlash.Play();
            else
            {
                rightGunMuzzleFlash.Stop();
            }
        }
        if (leftGunMuzzleFlash)
        {
            if (value)
                leftGunMuzzleFlash.Play();
            else
            {
                leftGunMuzzleFlash.Stop();
            }
        }

    }
}//class
