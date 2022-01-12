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
    public FixedJoystick joystick;
    public GameObject gunRayPoint;
    public GameObject crosshair;
    public float jumpForce;
    [Range(0,5f)]
    public float fallMulti = 2.5f;
    public bool fire;

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
    FireButton fireBtn;
    FixedButton fixedButton;
    Rigidbody rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        myCamera = Camera.main.transform;
        fixedButton = GameObject.Find("JumpBtn").GetComponent<FixedButton>();
        fireBtn = GameObject.Find("FireBtn").GetComponent<FireButton>();
        fixedButton.SetPlayer(this);
        fireBtn.SetPlayer(this);
        rb = GetComponent<Rigidbody>();
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
        }
        float targetSpeed = (moveSpeed * inputDir.magnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        if (inputDir.magnitude > 0)
        {
            anim.SetBool("run", true);
        }
        else if (Mathf.Approximately(inputDir.magnitude, 0))
        {
            anim.SetBool("run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(rb.velocity.y < 0)
        {
            rb.velocity -= Vector3.up * Mathf.Abs(Physics.gravity.y) * Mathf.Abs(fallMulti) * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        PositionCrosshair();
    }

    public void Fire()
    {
        //anim.SetTrigger("fire");
        isFire = true;
        RaycastHit hit;
        if (Physics.Raycast(gunRayPoint.transform.position, Camera.main.transform.forward, out hit, 25f))
        {
            print(hit.transform.name);
        }
        Debug.DrawRay(gunRayPoint.transform.position, Camera.main.transform.forward * 25f, Color.green);
    }
    public void Jump()
    {
        //  anim.SetTrigger("jump");
        rb.velocity =rb.angularVelocity =  Vector3.zero;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    public void FireUp()
    {
        isFire = false;
    }
    Vector3 crosshairVel;
    void PositionCrosshair()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one / 2f);
        int layerMask = LayerMask.GetMask("Default");
        crosshair.transform.position = ray.GetPoint(10);
        crosshair.transform.LookAt(myCamera.transform);
        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            Debug.Log(hit.transform.name);
            // crosshair.transform.position = Vector3.SmoothDamp(crosshair.transform.position,ray.GetPoint(10),ref crosshairVel,0.05f );
        }
    }
}//class
