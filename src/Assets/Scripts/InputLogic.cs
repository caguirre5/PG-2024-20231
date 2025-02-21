using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class InputLogic : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI outputText;
    public Button submitButton;

    public ChatGPTManager chatGPTManager;
    private string chatHistory = "";

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);

        // Asegura que el campo de entrada detecte la tecla Enter
        inputField.onSubmit.AddListener(delegate { OnSubmit(); });
    }

    private async void OnSubmit()
    {
        string inputText = inputField.text;

        if (!string.IsNullOrEmpty(inputText))
        {
            // Mostrar el texto del usuario en el chat
            chatHistory += "Tú: " + inputText + "\n";
            outputText.text = chatHistory;

            // Obtener la respuesta del ChatGPTManager
            string response = await AskGPTResponse(inputText);

            // Mostrar la respuesta en el chat
            chatHistory += "NPC: " + response + "\n";
            outputText.text = chatHistory;

            // Resetear el campo de entrada
            inputField.text = string.Empty;

            // Opcional: volver a enfocar el campo de entrada
            inputField.ActivateInputField();
        }
    }

    private async Task<string> AskGPTResponse(string inputText)
    {
        if (chatGPTManager != null)
        {
            // Llamar al método adecuado en ChatGPTManager
            string response = await chatGPTManager.AskGPTResponse(inputText, "NPCName"); // Asegúrate de que "NPCName" es el nombre correcto
            return response;
        }

        return "Error: No ChatGPTManager found.";
    }
}