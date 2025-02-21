using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    // Referencia al script NPCBrain
    public NPCBrain npcBrain; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Mostrar el chat box
            //npcBrain.ShowChatBox(true);  
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //npcBrain.ShowChatBox(false);  // Ocultar el chat box
        }
    }
}
