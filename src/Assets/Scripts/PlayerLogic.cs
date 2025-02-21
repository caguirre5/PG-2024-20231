using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private NPCBrain lastNPCBrain = null;
    private Pickable lastPickable = null;
    private Cauldron lastCauldron = null;
    private PaperFragment lastPaper = null;
    private DoorEntrance lastDoorEntrance = null; // Último DoorEntrance detectado

    private Pickable heldPickable = null; // Pickable actualmente sostenido por el jugador

    [SerializeField] private float detectionRadius = 5f; // Radio de detección
    [SerializeField] private float fovAngle = 45f; // Ángulo del campo de visión
    [SerializeField] private Transform anchor; // Punto donde se va a mover el pickable

    private Quaternion originalRotation; // Rotación original del Pickable

    [SerializeField] private AudioSource GrabSound;

    void Update()
    {
        DetectObjects();

        // Lógica para agarrar, soltar el pickable, o agregarlo al Cauldron al presionar Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HandlePickable();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleObject();
        }

        // Si hay un pickable sostenido, mantener su posición en el anchor
        if (heldPickable != null)
        {
            heldPickable.transform.position = anchor.position;
        }
    }

    private void DetectObjects()
    {
        // Obtiene todos los colliders en el radio de detección
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

        // Variables para verificar si encontramos objetos dentro del campo de visión
        NPCBrain npcInView = null;
        Pickable pickableInView = null;
        Cauldron cauldronInView = null;
        PaperFragment paperFragmentInView = null;
        DoorEntrance doorEntranceInView = null; // Nuevo: para detectar DoorEntrance

        foreach (Collider col in objectsInRange)
        {
            if (col.CompareTag("InteractableObject"))
            {
                Vector3 directionToTarget = (col.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget <= fovAngle / 2)
                {
                    // Detección de NPCBrain
                    NPCBrain npcBrain = col.GetComponent<NPCBrain>();
                    if (npcBrain != null)
                    {
                        npcInView = npcBrain;
                        if (npcBrain != lastNPCBrain && lastNPCBrain != null)
                        {
                            lastNPCBrain.ShowKeycap(false);
                        }
                        npcBrain.ShowKeycap(true);
                        lastNPCBrain = npcBrain;
                    }

                    // Detección de Cauldron
                    Cauldron cauldron = col.GetComponent<Cauldron>();
                    if (cauldron != null)
                    {
                        cauldronInView = cauldron;
                        if (heldPickable != null)
                        {
                            cauldron.ShowKeycap(true);
                        }
                        if (cauldron != lastCauldron && lastCauldron != null)
                        {
                            lastCauldron.ShowKeycap(false);
                        }
                        lastCauldron = cauldron;
                    }

                    // Detección de Pickable
                    Pickable pickable = col.GetComponent<Pickable>();
                    if (pickable != null)
                    {
                        pickableInView = pickable;
                        if (pickable != lastPickable && lastPickable != null)
                        {
                            lastPickable.ShowKeycap(false);
                        }
                        pickable.ShowKeycap(true);
                        lastPickable = pickable;
                    }

                    // Detección de PaperFragment
                    PaperFragment paperFragment = col.GetComponent<PaperFragment>();
                    if (paperFragment != null)
                    {
                        paperFragmentInView = paperFragment;
                        paperFragment.ShowKeycap(true);
                        lastPaper = paperFragment;
                    }

                    // Detección de DoorEntrance
                    DoorEntrance doorEntrance = col.GetComponent<DoorEntrance>();
                    if (doorEntrance != null)
                    {
                        doorEntranceInView = doorEntrance;

                        // Mostrar el keycap solo si el jugador tiene un Pickable en la mano
                        if (heldPickable != null)
                        {
                            doorEntrance.playerTransform = transform;
                            doorEntrance.ShowKeycap(true);
                        }

                        // Ocultar el keycap del último DoorEntrance si es un nuevo DoorEntrance
                        if (doorEntrance != lastDoorEntrance && lastDoorEntrance != null)
                        {
                            doorEntrance.playerTransform = null;
                            lastDoorEntrance.ShowKeycap(false);
                        }

                        lastDoorEntrance = doorEntrance; // Actualizamos la referencia del DoorEntrance
                    }
                }
            }
        }

        // Ocultar el keycap de los objetos anteriores si ya no están en el campo de visión
        if (npcInView == null && lastNPCBrain != null)
        {
            lastNPCBrain.ShowKeycap(false);
            lastNPCBrain = null;
        }

        if (pickableInView == null && lastPickable != null)
        {
            lastPickable.ShowKeycap(false);
            lastPickable = null;
        }

        if (cauldronInView == null && lastCauldron != null)
        {
            lastCauldron.ShowKeycap(false);
            lastCauldron = null;
        }

        if (paperFragmentInView == null && lastPaper != null)
        {
            lastPaper.ShowKeycap(false);
            lastPaper = null;
        }

        if (doorEntranceInView == null && lastDoorEntrance != null)
        {
            lastDoorEntrance.ShowKeycap(false);
            lastDoorEntrance = null;
        }
    }

    private void HandleObject()
    {
        if (lastPaper != null)
        {
            lastPaper.ShowFragment();
        }

        if (lastDoorEntrance != null)
        {
            lastDoorEntrance.OpenDoor();
            Destroy(heldPickable.gameObject); // Destruye el gameObject de DoorEntrance
            heldPickable  = null;
        }
    }

    private void HandlePickable()
    {
        if (lastCauldron != null && heldPickable != null)
        {
            if (heldPickable.name != "Key")
            {
                lastCauldron.AddItemToCauldron(heldPickable);
                heldPickable.transform.parent = null;
                heldPickable.transform.position = lastCauldron.transform.position;
                heldPickable.transform.rotation = Quaternion.identity;
                heldPickable.gameObject.SetActive(false);
                heldPickable = null;
            }
            else
            {
                Debug.Log("The pickable is a Key and cannot be added to the Cauldron.");
            }
        }
        else if (heldPickable != null)
        {
            Rigidbody pickableRb = heldPickable.GetComponent<Rigidbody>();
            if (pickableRb != null)
            {
                pickableRb.isKinematic = false;
                pickableRb.useGravity = true;
            }
            heldPickable.transform.rotation = originalRotation;
            heldPickable.transform.parent = null;
            heldPickable = null;
        }
        else if (lastPickable != null)
        {
            GrabSound?.Play();
            heldPickable = lastPickable;
            heldPickable.transform.parent = anchor;
            originalRotation = heldPickable.transform.rotation;
            heldPickable.transform.localPosition = Vector3.zero;

            Rigidbody pickableRb = heldPickable.GetComponent<Rigidbody>();
            if (pickableRb != null)
            {
                pickableRb.isKinematic = true;
                pickableRb.useGravity = false;
            }
        }
    }

    // Dibuja el área de detección en la escena para depurar
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
