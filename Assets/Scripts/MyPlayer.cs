using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float smoothRotationTime = 0.25f;
    public bool enableMobile;
    public FixedJoystick joystick;

    [Space(20)]
    [Header("Just There!")]
    public float currentVeclocity;
    public float currentSpeed;
    public float speedVelocity;
    Transform myCamera;
    Actions actions;
    private void Start()
    {
        myCamera = Camera.main.transform;
        //actions = GetComponent<Actions>();
    }
    void Update()
    {
        Vector2 input;
        if (enableMobile)
        {
            input = new Vector2(joystick.input.x,joystick.input.y);
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        Vector2 inputDir = input.normalized;
        if(inputDir != Vector2.zero)
        {
            float rot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rot, ref currentVeclocity, smoothRotationTime);
        }
        float targetSpeed = (moveSpeed * inputDir.magnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity,0.1f);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime,Space.World);

    }
}
