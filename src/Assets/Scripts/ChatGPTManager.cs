using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using OpenAI;

public class ChatGPTManager : MonoBehaviour
{
    public static ChatGPTManager Instance { get; private set; }
    private OpenAIApi openAI = new OpenAIApi();
    private Dictionary<string, List<ChatMessage>> chatConversations = new Dictionary<string, List<ChatMessage>>();

    private Dictionary<string, NPCAttributes> npcAttributes = new Dictionary<string, NPCAttributes>();

    public List<string> worldData = new List<string>();

    [SerializeField]
    private PromptsCatalog promptsCatalog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task InitializeWorldData()
    {
        if (promptsCatalog != null && promptsCatalog.prompts.Count > 0)
        {
            Debug.Log("---------------Inicio de historia de juego -------------");

            // Crear una lista de tareas para ejecutar todas las solicitudes en paralelo
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (var prompt in promptsCatalog.prompts)
            {
                // Añadir las tareas a la lista en lugar de esperar inmediatamente
                tasks.Add(AskGPTResponse(prompt, "General"));
            }

            // Esperar a que todas las solicitudes terminen al mismo tiempo
            string[] responses = await Task.WhenAll(tasks);

            // Agregar todas las respuestas al worldData
            foreach (var response in responses)
            {
                worldData.Add(response);
                Debug.Log(response);
            }

            Debug.Log("-------------------------------------------------------------");
        }
    }


    public async Task<string> AskGPTResponse(string messageContent, string chatName, string model = "gpt-3.5-turbo")
    {

        //Debug.Log(messageContent);
        if (!chatConversations.ContainsKey(chatName))
        {
            chatConversations[chatName] = new List<ChatMessage>();
        }

        ChatMessage message = new ChatMessage { Content = messageContent, Role = "user" };
        chatConversations[chatName].Add(message);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest
        {
            Messages = chatConversations[chatName],
            Model = model
        };

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            chatConversations[chatName].Add(chatResponse);
            //Debug.Log(chatResponse.Content);
            return chatResponse.Content;
        }
        return string.Empty;
    }

    // M�todo para registrar un NPC
    public void RegisterNPC(NPCAttributes npc)
    {
        if (!npcAttributes.ContainsKey(npc.NPCname))
        {
            npcAttributes.Add(npc.NPCname, npc);
        }
    }

    // M�todo para obtener los atributos de todos los NPCs
    public Dictionary<string, NPCAttributes> GetNPCAttributes()
    {
        return npcAttributes;
    }
}
