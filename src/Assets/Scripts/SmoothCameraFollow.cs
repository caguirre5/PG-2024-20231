using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f; // Tiempo para seguir al jugador
    [SerializeField] private float rotationSmoothTime = 5.0f; // Tiempo para suavizar la rotación
    [SerializeField] private float viewChangeLerpSpeed = 2.0f; // Velocidad de interpolación para cambio de vista

    private Vector3 _currentVelocity = Vector3.zero;
    private Quaternion _currentRotationVelocity;

    private Vector3 extraPosition = new Vector3(0, 0.3f, 0);
    private bool isTopDownView = false;
    private Vector3 topDownOffset = new Vector3(0, 10f, 0);
    private Vector3 defaultRotation = new Vector3(30, 45, 0);
    private Vector3 topDownRotation = new Vector3(90, 45, 0);

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isTransitioning = false;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip topDownViewClip;
    [SerializeField] private AudioClip normalViewClip;

    private void Start()
    {
        // Inicializar las posiciones y rotaciones
        targetPosition = target.position + extraPosition;
        targetRotation = Quaternion.Euler(defaultRotation);
    }

    private void LateUpdate()
    {
        // Verifica si estamos en transición entre vistas
        if (isTransitioning)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, viewChangeLerpSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                isTransitioning = false;
            }
        }
        else
        {
            Vector3 followTargetPosition = target.position + (isTopDownView ? topDownOffset : extraPosition);
            transform.position = Vector3.SmoothDamp(transform.position, followTargetPosition, ref _currentVelocity, smoothTime);
        }
    }

    public void SetTopDownView(bool activate)
    {
        isTopDownView = activate;
        isTransitioning = true;

        _currentVelocity = Vector3.zero;

        if (isTopDownView)
        {
            targetPosition = target.position + topDownOffset;
            targetRotation = Quaternion.Euler(topDownRotation);

            audioSource.clip = topDownViewClip;
            audioSource.Play();
        }
        else
        {
            targetPosition = target.position + extraPosition;
            targetRotation = Quaternion.Euler(defaultRotation);
            audioSource.clip = normalViewClip;
            audioSource.Play();
        }
    }
}
