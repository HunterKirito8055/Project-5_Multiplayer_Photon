using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform player;
    public FixedTouchField touch;
    public bool enableMobile;
    public float yAxis, xAxis;
    public float rotationSensitive = 8f;

    public Vector3 distanceOffset = new Vector3(0, 1.25f, 2f);

    float minYRot = -40f, maxYRot = 78f; public float smoothTime = 0.12f;
    public Vector3 currentVelocity;
    public Vector3 current, target;
    public float yVel;
    private void Awake()
    {
        transform.parent = null;
        touch = GameObject.Find("RightTouchPanel").GetComponent<FixedTouchField>();
    }
    private void LateUpdate()
    {
        if (enableMobile) rotationSensitive = 0.2f;

        if (enableMobile)
        {
            yAxis += touch.TouchDist.x * rotationSensitive;
            xAxis -= touch.TouchDist.y * rotationSensitive;
        }
        else
        {
            yAxis += Input.GetAxis("Mouse X") * rotationSensitive;
            xAxis -= Input.GetAxis("Mouse Y") * rotationSensitive;
        }

        xAxis = Mathf.Clamp(xAxis, minYRot, maxYRot);
        target = new Vector3(xAxis, yAxis);

        current = Vector3.SmoothDamp(current, target, ref currentVelocity, smoothTime);
        transform.eulerAngles = current;

        Vector3 dir = Vector3.zero;
        dir = player.position - transform.forward * Mathf.Abs(distanceOffset.z);
        transform.position = dir;

    }
}//class
