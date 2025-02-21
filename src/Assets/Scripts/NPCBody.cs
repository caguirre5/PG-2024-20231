using UnityEngine;
using System.Collections.Generic;

public class NPCBody : MonoBehaviour
{
    public NPCPartsCatalog npcPartsCatalog; // Asigna aqu� el ScriptableObject en el editor

    [SerializeField] private GameObject headObject;   // Asigna aqu� el GameObject que representa la cabeza
    [SerializeField] private GameObject bodyObject;   // Asigna aqu� el GameObject que representa el cuerpo
    [SerializeField] private GameObject leftArmObject;  // Asigna aqu� el GameObject que representa el brazo izquierdo
    [SerializeField] private GameObject rightArmObject; // Asigna aqu� el GameObject que representa el brazo derecho
    [SerializeField] private GameObject leftLegObject;  // Asigna aqu� el GameObject que representa la pierna izquierda
    [SerializeField] private GameObject rightLegObject; // Asigna aqu� el GameObject que representa la pierna derecha
    [SerializeField] private GameObject cape;

    void Start()
    {
        if (npcPartsCatalog != null)
        {
            NPCAttributes npcAttributes = GetComponent<NPCAttributes>();
            // Asigna la cabeza
            if (npcAttributes != null)
            {
                string npcGenre = npcAttributes.NPCGenre;

                // Asigna la cabeza con el género correcto
                List<NPCPartsCatalog.HeadData> headsOfSameGenre = npcPartsCatalog.heads.FindAll(h => h.genre == npcGenre);
                if (headsOfSameGenre.Count > 0)
                {
                    Debug.Log($"{headsOfSameGenre.Count} heads found for genre {npcGenre}!");
                    int headIndex = Random.Range(0, headsOfSameGenre.Count);
                    Mesh newHeadMesh = headsOfSameGenre[headIndex].head;
                    AssignMesh(headObject, newHeadMesh);
                }
                else
                {
                    Debug.Log($"No heads found for genre {npcGenre}!");
                }

            }

            // Asigna el cuerpo
            if (npcPartsCatalog.bodies.Count > 0)
            {
                int bodyIndex = Random.Range(0, npcPartsCatalog.bodies.Count);
                Mesh newBodyMesh = npcPartsCatalog.bodies[bodyIndex].meshes;
                AssignMesh(bodyObject, newBodyMesh);
            }

            // Asigna las extremidades (brazos y piernas)
            if (npcPartsCatalog.arms.Count > 0)
            {
                int armsIndex = Random.Range(0, npcPartsCatalog.arms.Count);
                Mesh newLeftArmMesh = npcPartsCatalog.arms[armsIndex].extLeft;
                Mesh newRightArmMesh = npcPartsCatalog.arms[armsIndex].extRight;
                AssignMesh(leftArmObject, newLeftArmMesh);
                AssignMesh(rightArmObject, newRightArmMesh);
            }

            if (npcPartsCatalog.legs.Count > 0)
            {
                int legsIndex = Random.Range(0, npcPartsCatalog.legs.Count);
                Mesh newLeftLegMesh = npcPartsCatalog.legs[legsIndex].extLeft;
                Mesh newRightLegMesh = npcPartsCatalog.legs[legsIndex].extRight;
                AssignMesh(leftLegObject, newLeftLegMesh);
                AssignMesh(rightLegObject, newRightLegMesh);
            }

            // Asigna un color aleatorio a la capa

            // Asigna un material aleatorio a todas las partes
            if (npcPartsCatalog.materials.Count > 0)
            {
                int materialIndex = Random.Range(0, npcPartsCatalog.materials.Count);
                Material newMaterial = npcPartsCatalog.materials[materialIndex];

                AssignMaterial(headObject, newMaterial);
                AssignMaterial(bodyObject, newMaterial);
                AssignMaterial(leftArmObject, newMaterial);
                AssignMaterial(rightArmObject, newMaterial);
                AssignMaterial(leftLegObject, newMaterial);
                AssignMaterial(rightLegObject, newMaterial);
                AssignMaterial(cape, newMaterial);
            }
        }
        else
        {
            Debug.LogWarning("NPCPartsCatalog is not assigned!");
        }
    }

    private void AssignMesh(GameObject obj, Mesh newMesh)
    {
        if (obj != null && newMesh != null)
        {
            SkinnedMeshRenderer meshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sharedMesh = newMesh;
            }
            else
            {
                Debug.LogWarning($"SkinnedMeshRenderer component is missing on {obj.name}!");
            }
        }
    }

    private void AssignMaterial(GameObject obj, Material newMaterial)
    {
        if (obj != null && newMaterial != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
            else
            {
                Debug.LogWarning($"Renderer component is missing on {obj.name}!");
            }
        }
    }
}
