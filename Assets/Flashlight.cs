using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeepXRotationLocked();
    }

    void KeepXRotationLocked()
    {
        rotation.x = 0f;
        rotation.y = transform.rotation.y;
        rotation.z = transform.rotation.z;
        transform.eulerAngles = rotation;
    }
}
