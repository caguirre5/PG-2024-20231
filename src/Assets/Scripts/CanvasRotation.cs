using UnityEngine;

public class CanvasRotation : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Encontrar la c�mara principal
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Hacer que el canvas mire siempre hacia la c�mara
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}

