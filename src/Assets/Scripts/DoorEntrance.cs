using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntrance : MonoBehaviour
{
    [SerializeField] private Canvas keycapCanvas;
    [SerializeField] private float rotationSpeed = 5f; // Velocidad de rotación al mirar al jugador
    public Transform playerTransform; // Referencia al transform del jugador

    [SerializeField] private Transform doorObject; // Objeto adicional que se moverá
    [SerializeField] private Vector3 closedPosition = new Vector3(-8.498f, 0.899f, 17.502f); // Posición cerrada de la puerta
    [SerializeField] private Vector3 openPosition = new Vector3(-8.498f, 0.899f, 37.502f);   // Posición abierta de la puerta
    [SerializeField] private float moveSpeed = 2f; // Velocidad de transición al abrir la puerta

    [SerializeField] private AudioSource slideRock;

    private bool isOpening = false; // Controla si la puerta se está abriendo

    void Start()
    {
        keycapCanvas.enabled = false;

        // Asegura que la posición inicial de doorObject sea la posición cerrada
        if (doorObject != null)
        {
            doorObject.position = closedPosition;
        }
    }

    void Update()
    {
        // Si el jugador está cerca, voltea hacia él
        if (playerTransform != null)
        {
            LookAtPlayer();
        }

        // Si la puerta se está abriendo, mueve el objeto adicional suavemente hacia la posición abierta
        if (isOpening && doorObject != null)
        {
            // Transición suave a la posición abierta
            doorObject.position = Vector3.MoveTowards(doorObject.position, openPosition, moveSpeed * Time.deltaTime);

            // Verifica si ha llegado a la posición de destino
            if (Vector3.Distance(doorObject.position, openPosition) < 0.01f)
            {
                isOpening = false; // Detiene la transición cuando llega a la posición abierta
            }
        }
    }

    private void LookAtPlayer()
    {
        // Calcula la dirección hacia el jugador sin cambiar el eje Y
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // Mantiene el eje Y sin cambios para evitar inclinación

        // Realiza una rotación suave hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void ShowKeycap(bool value)
    {
        keycapCanvas.enabled = value;
    }

    public void OpenDoor()
    {
        slideRock.Play();
        isOpening = true; // Inicia la transición hacia la posición abierta
    }
}
