using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject npcPrefab; // Asigna el prefab del NPC desde el editor
    private ChatGPTManager chatGPTManager;
    private int numberOfNPCs = 3;
    private List<NPCAttributes> npcList = new List<NPCAttributes>();
    private List<GameObject> npcBrainsList = new List<GameObject>();

    //Scriptable object con la lista de objetos requeridos para la pocion
    public ItemsCatalog itemsCatalog;
    private List<ItemsCatalog.ItemsData> selectedItems;

    // Referencia al caldero en la escena
    private Cauldron cauldron;

    public List<GameObject> itemsToSpawn;

    [System.Serializable]
    public class ItemsData
    {
        public string Name;
        public GameObject Prefab;  // Prefab que corresponde a este item
    }

    public GameObject paperReceipe;

    public List<GameObject> objectsToAdd = new List<GameObject>();


    private async void Start()
    {
        chatGPTManager = FindObjectOfType<ChatGPTManager>();
        cauldron = FindObjectOfType<Cauldron>();


        SelectRandomItems();

        if (chatGPTManager != null)
        {
            // Inicializa los datos del mundo al comienzo del juego
            await chatGPTManager.InitializeWorldData();

            // Primer ciclo: Crear y registrar NPCs con sus atributos
            for (int i = 0; i < numberOfNPCs; i++)
            {
                // Generar los atributos para el NPC, incluyendo el Genre (puede ser generado aleatoriamente o con ChatGPT)
                string genre = Random.Range(0, 2) == 0 ? "Masculino" : "Femenino"; // Por ejemplo, aleatorio entre Masculino o Femenino

                string prompt = $"Genera atributos únicos para el NPC #{i + 1} con los siguientes campos:\n" +
                    "NPCname, NPCrole, NPCpersonality.\n" +
                    "Proporciona valores para cada campo en el formato 'NPCname: <name>, NPCrole: <role>, NPCpersonality: <personality>'. " +
                    $"Recuerda que todo debe ser en español. Es de suma importancia que unicamnete devuelvas el formato solicitado. Ten en cuenta que el genero de este NPC es {genre}";

                string response = await chatGPTManager.AskGPTResponse(prompt, $"NPC{i}");
                NPCAttributesData npcAttributesData = ProcessResponse(response);

                // Asignar el genre generado al NPCAttributesData
                npcAttributesData.NPCGenre = genre;

                // Instanciar el prefab del NPC y asignar los atributos
                GameObject npcObject = Instantiate(npcPrefab);
                npcBrainsList.Add(npcObject);
                NPCAttributes npcComponent = npcObject.GetComponent<NPCAttributes>();

                if (npcComponent != null)
                {
                    // Asignar los atributos generados al NPC
                    npcComponent.SetAttributes(npcAttributesData, $"NPC{i}");

                    // Registrar el NPC en el ChatGPTManager
                    chatGPTManager.RegisterNPC(npcComponent);

                    // Añadir el NPC a la lista para la posterior inicialización del cerebro
                    npcList.Add(npcComponent);
                }
            }

            // Segundo ciclo: Inicializar el cerebro de todos los NPCs registrados
            foreach (var npc in npcBrainsList)
            {
                npc.GetComponent<NPCAttributes>().InitializeMyBrain();
                npc.GetComponent<NPCBrain>().AddPromptToBrain("El jugador debe de explorar y enfrentarse a diversos desafios relacionados con la historia, el objetivo principal es completar la poción mágica.");
                npc.GetComponent<NPCBrain>().AddPromptToBrain("El bosque del mapa está cerrado, para abrirlo se debe encontrar una llave ubicada en laberinto que esta al este del pueblo.");
            }

            GenerateEnemyStory();
            GeneratePotionRecipe();
        }
        else
        {
            Debug.LogError("ChatGPTManager not found in the scene.");
        }
    }


    private NPCAttributesData ProcessResponse(string response)
    {
        NPCAttributesData npcAttributesData = new NPCAttributesData();

        if (!string.IsNullOrEmpty(response))
        {
            string[] attributes = response.Split(',');
            foreach (var attribute in attributes)
            {
                string[] keyValue = attribute.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    switch (key)
                    {
                        case "NPCname":
                            npcAttributesData.NPCname = value;
                            break;
                        case "NPCrole":
                            npcAttributesData.NPCrole = value;
                            break;
                        case "NPCpersonality":
                            npcAttributesData.NPCpersonality = value;
                            break;
                    }
                }
            }

            // Imprimir los atributos generados de manera amigable
            // Debug.Log($"NPC Atributos:\n" +
            //           $"Nombre: {npcAttributesData.NPCname}\n" +
            //           $"Rol: {npcAttributesData.NPCrole}\n" +
            //           $"Personalidad: {npcAttributesData.NPCpersonality}");
        }
        else
        {
            Debug.LogWarning("Response was empty or null.");
        }

        return npcAttributesData;
    }


    private void SelectRandomItems()
    {
        selectedItems = new List<ItemsCatalog.ItemsData>();

        int itemsToSelect = Random.Range(3, 7);

        // Lista temporal para evitar duplicados
        List<ItemsCatalog.ItemsData> availableItems = new List<ItemsCatalog.ItemsData>(itemsCatalog.items);

        // Selección aleatoria
        for (int i = 0; i < itemsToSelect; i++)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            selectedItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex);
        }

        // Después de seleccionar los ítems, pasamos la lista al caldero
        if (cauldron != null)
        {
            cauldron.ReceiveItems(selectedItems);
        }
        else
        {
            Debug.LogError("No se encontró el objeto Cauldron en la escena.");
        }

        // Instancia el primer elemento seleccionado en el mapa
        InstantiateAllSelectedItemsInMap();
    }

    private void InstantiateAllSelectedItemsInMap()
    {
        // Verifica si hay elementos seleccionados
        if (selectedItems != null && selectedItems.Count > 0)
        {
            // Posición inicial y un offset para instanciar los objetos separados entre sí
            Vector3 spawnPosition = new Vector3(-1, 1, 5);
            Vector3 offset = new Vector3(2, 0, 0); // Separación entre objetos

            // Itera sobre todos los elementos seleccionados e instáncialos en el mapa
            for (int i = 0; i < selectedItems.Count; i++)
            {
                ItemsCatalog.ItemsData item = selectedItems[i]; // Obtenemos cada ítem de la lista

                // Verifica si el ítem tiene un prefab asignado
                if (item.Item != null)
                {
                    // Instancia el prefab en la posición actual
                    Instantiate(item.Item, spawnPosition, Quaternion.identity);
                    objectsToAdd.Add(item.Item);
                    //Debug.Log($"Instanciado el objeto {item.Name} en la posición {spawnPosition}");

                    // Mueve la posición hacia la derecha para el siguiente objeto
                    spawnPosition += offset;
                }
                else
                {
                    Debug.LogWarning($"El item {item.Name} no tiene un prefab asignado.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No hay ítems seleccionados para instanciar.");
        }
    }



    private async void GeneratePotionRecipe()
    {
        string prompt = "Necesito que generes una receta para una pocion magica que requiere los siguientes ingredientes:\n";

        foreach (var item in selectedItems)
        {
            prompt += $"- {item.Name}\n";
        }

        prompt += "Es importante que no incluyas pasos adicionales mas alla de agregar cada ingrediente al caldero. " +
                  "Asegurate de mencionar claramente el nombre de cada ingrediente, unicamente indica que hay que agregar una unidad de cada ingrediente" +
                  "A�ade detalles a la receta que expliquen de manera creativa y misteriosa como cada ingrediente contribuye al poder m�gico de la poci�n. " +
                  "Debes ser creativo en tus explicaciones y utiliza un enfoque que evoque el misticismo propio de la creaci�n de una poci�n m�gica; no es necesario que estas explicaciones sean realistas.\n" +
                  "Recuerda mantener el contexto narrativo: estamos en un mundo antiguo y medieval, y esta receta pertenece a un antiguo y misterioso libro encontrado en las profundidades de un bosque. " +
                  "Por lo tanto, la receta debe conservar un aire de misterio. No reveles en ning�n momento el objetivo o la funci�n de la poci�n, ya que este misterio es parte fundamental del juego. " +
                  "La finalidad de la poci�n debe quedar abierta, pues el desconocido autor de este extra�o libro solo explica c�mo crearla, no sus prop�sitos." +
                  "Intenta redactar este texto a manera de dejar ambiguedad en el lector, ya que muchas de las cosas debe de descubrirlas por su cuenta a traves de la exploracion." +
                  "Que no sean mas de 1000 caracteres";

        string narrativa = await chatGPTManager.AskGPTResponse(prompt, "General");

        // Debug.Log("Narrativa generada: " + narrativa);

        if (paperReceipe != null)
        {
            // Define una posición y rotación donde quieres instanciar el fragmento de papel
            Vector3 spawnPosition = new Vector3(-3.3f, 5, 3.3f);
            Quaternion spawnRotation = Quaternion.identity;

            GameObject paperObject = Instantiate(paperReceipe, spawnPosition, spawnRotation);

            // Obtener el componente FragmentPaper del objeto instanciado
            PaperFragment fragmentPaper = paperObject.GetComponent<PaperFragment>();
            objectsToAdd.Add(paperObject);

            if (fragmentPaper != null)
            {
                // Asignar el texto de la receta al TMP_Text del FragmentPaper
                fragmentPaper.SetRecipeText(narrativa);
            }
            else
            {
                Debug.LogError("No se encontró el componente FragmentPaper en el prefab instanciado.");
            }
        }
        else
        {
            Debug.LogError("No se encontro receta.");
        }
    }

    private async void GenerateEnemyStory()
    {
        string[] storyFragments = new string[3];

        string basePrompt = "Quiero que generes un fragmento de una leyenda oscura sobre el enemigo principal de este mundo. " +
                            "Este ser no es un simple mago o hechicero, sino algo mucho m�s siniestro y complejo. Su historia " +
                            "est� impregnada de misticismo y oscuridad, y est� relacionada profundamente con la magia ancestral. " +
                            "Tiene cierta aficion por las pociones y hechizos, y le gusta atormentar para conseguir lo que quiere" +
                            "Explica levemente sobre eso en los fragmentos. " +
                            "Cada fragmento debe ser corto pero cargado de un estilo literario muy m�stico y oscuro. " +
                            "Estos fragmentos de historia deben complementarse entre s�, y aunque se pueden leer en cualquier orden, " +
                            "solo juntos revelan la totalidad de la leyenda. No repitas informaci�n, sino que ampl�a y profundiza " +
                            "en la naturaleza y origen de este ser. Cada fragmento no debe de ser extenso, mantenlo corto y simple, aprox 40 palabras.";

        for (int i = 0; i < 3; i++)
        {
            // Personalizar el prompt para cada fragmento
            string prompt = basePrompt + $" Genera el fragmento {i + 1} de 3.";

            // Usar ChatGPTManager para generar el fragmento
            storyFragments[i] = await chatGPTManager.AskGPTResponse(prompt, "General");
        }

        // Mostrar los fragmentos generados en la consola para verificar
        // for (int i = 0; i < storyFragments.Length; i++)
        // {
        //     Debug.Log($"Fragmento {i + 1}: {storyFragments[i]}");
        // }
    }

}

// Clase para almacenar los atributos de un NPC
public class NPCAttributesData
{
    public string NPCname;
    public string NPCrole;
    public string NPCpersonality;

    public string NPCGenre;
}
