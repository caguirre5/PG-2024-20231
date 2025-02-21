using System.Collections.Generic;
using UnityEngine;

public class NPCAttributes : MonoBehaviour
{
    public string NPCname { get; private set; }
    public string NPCrole { get; private set; }
    public string NPCpersonality { get; private set; }
    public string NPCGenre { get; private set; }

    public string NPCID { get; private set; }

    private ChatGPTManager chatGPTManager;

    private void Awake()
    {
        chatGPTManager = FindAnyObjectByType<ChatGPTManager>();

        if (chatGPTManager == null)
        {
            Debug.LogError("ChatGPTManager not found in the scene.");
        }
    }

    // M�todo para asignar los atributos del NPC
    public void SetAttributes(NPCAttributesData attributesData, string id)
    {
        NPCname = attributesData.NPCname;
        NPCrole = attributesData.NPCrole;
        NPCpersonality = attributesData.NPCpersonality;
        NPCGenre = attributesData.NPCGenre; // Asignar el género generado
        NPCID = id;
    }


    public async void InitializeMyBrain()
    {
        Dictionary<string, NPCAttributes> allNPCs = chatGPTManager.GetNPCAttributes();
        string worldInfo = "";
        string npcsInfo = "Aqui estan los atributos de los otros NPCs:\n";

        // Alimentar el chat con la informaci�n previamente generada en GPTManager
        List<string> worldData = ChatGPTManager.Instance.worldData;
        if (worldData != null && worldData.Count > 0)
        {
            foreach (var data in worldData)
            {
                worldInfo += data + "\n";
            }
        }

        foreach (var npc in allNPCs.Values)
        {
            npcsInfo += $"{npc.NPCname} es un {npc.NPCrole} con la siguiente personalidad {npc.NPCpersonality}.\n";
        }


        string initialPrompt = $"De ahora en adelante eres un habitante de Valle Sereno y esta es la información que conoces: \n{worldInfo}\n." +
                    $"Tu nombre es {NPCname}, tu género es {NPCGenre}, tu oficio es {NPCrole} y tu personalidad es {NPCpersonality}." +
                    $"Esta es la lista de todos los habitantes de Valle Sereno, incluyéndote a ti: \n{npcsInfo}, recuerda que esta lista representa a todas las personas que conoces en el pueblo.\n" +
                    "Cuando respondas preguntas, basa tus respuestas únicamente en esta información. Si te hacen una pregunta que no se alinea con lo que sabes o es demasiado extraña, evítala o responde que no sabes o no puedes contestarla.\n" +
                    "El bosque mágico se ubica al norte del pueblo y se rumorea que se necesita una llave del laberinto para acceder. El laberinto está al este del pueblo y también se rumorea que allí se encuentra la llave necesaria, pero nadie que ha entrado ha salido.\n" +
                    "Si te preguntan '¿Qué debo hacer?' o '¿Cómo puedo ayudar?', indica que como habitante de Valle Sereno necesitas un héroe que ayude al pueblo a preparar una poción mágica para alejar al mal.\n" +
                    "Este prompt es solo para inicializar este chat, para asegurar que hayas entendido, responde 'ok'.";

        Debug.Log("Prompt" + initialPrompt);
        // Enviar el prompt inicial para iniciar la conversaci�n

        await chatGPTManager.AskGPTResponse(initialPrompt, NPCID);
    }
}
