using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform player;
    public FixedTouchField touch;
    public bool enableMobile;
    public float yAxis, xAxis ;
    public float rotationSensitive = 8f, distanceOffset = 2f;

    float minYRot = -40f, maxYRot = 85f;public float smoothTime = 0.12f;
   public  Vector3 currentVelocity;
   public Vector3 current,target;
    private void LateUpdate()
    {
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
        
        xAxis = Mathf.Clamp(xAxis, minYRot,maxYRot);
        target = new Vector3(xAxis, yAxis);

        current = Vector3.SmoothDamp(current, target, ref currentVelocity, smoothTime);
        transform.eulerAngles = current;
        transform.position = player.position - transform.forward * distanceOffset;

    }
}//class