using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSensor : MonoBehaviour
{
    [SerializeField] private SmoothCameraFollow smoothCameraFollow; // Referencia al script SmoothCameraFollow

    private void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que el objeto que entra en el trigger es el target
        if (other.CompareTag("CameraTopDownTrigger")) // O puedes cambiar "Player" por el tag de tu target
        {
            // Activa la vista top-down en SmoothCameraFollow
            smoothCameraFollow.SetTopDownView(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Asegúrate de que el objeto que sale del trigger es el target
        if (other.CompareTag("CameraTopDownTrigger")) // O puedes cambiar "Player" por el tag de tu target
        {
            // Desactiva la vista top-down en SmoothCameraFollow
            smoothCameraFollow.SetTopDownView(false);
        }
    }
}
