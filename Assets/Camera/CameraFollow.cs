using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    Vector3 velocity = Vector3.zero;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, 0.05f);


    }
}
