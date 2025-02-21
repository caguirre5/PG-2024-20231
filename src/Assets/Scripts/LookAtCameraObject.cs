using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraObject : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        transform.LookAt(cam.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
