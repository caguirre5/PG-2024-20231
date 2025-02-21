using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Cauldron : MonoBehaviour
{
    private List<ItemsCatalog.ItemsData> itemsInCauldron;
    private List<ItemsCatalog.ItemsData> addedItems = new List<ItemsCatalog.ItemsData>(); // Lista de objetos agregados

    [SerializeField] private TMP_Text itemsCounterText;  // Referencia al TMP_Text que muestra la cantidad de objetos agregados y requeridos

    [SerializeField] private Canvas keycapCanvas;

    [SerializeField] private AudioSource ItemAdded;

    [SerializeField] private AudioSource Bubbles;
    [SerializeField] private AudioSource ReciepeCompleted;

    [SerializeField] private AudioSource VictorySound;

    [SerializeField] private ParticleSystem ItemAddedEffect;
    [SerializeField] private ParticleSystem completedEffect;

    [SerializeField] private GameObject victoryScreen;

    // Start is called before the first frame update
    void Start()
    {
        keycapCanvas.enabled = false;
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false); // Asegúrate de que VictoryScreen esté inactivo al inicio
        }
    }

    public void ShowKeycap(bool value)
    {
        keycapCanvas.enabled = value;
    }

    // Corutina para mostrar VictoryScreen después de un minuto
    private IEnumerator ShowVictoryScreenAfterDelay()
    {
        yield return new WaitForSeconds(1); // Espera 1 segundo antes de mostrar la pantalla de victoria
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            VictorySound?.Play();
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Credits");
    }




    // Método para recibir la lista de items desde el GameController
    public void ReceiveItems(List<ItemsCatalog.ItemsData> selectedItems)
    {
        // Clonar o referenciar la lista
        itemsInCauldron = new List<ItemsCatalog.ItemsData>(selectedItems);

        // Mostrar los objetos en el caldero para depurar
        Debug.Log("Items recibidos en el caldero:");
        foreach (var item in itemsInCauldron)
        {
            Debug.Log(item.Name);  // Asume que ItemsData tiene un campo llamado Name
        }

        // Asegurarse de actualizar el texto del contador inmediatamente
        UpdateItemsCounterText();
    }

    // Método para agregar un item al caldero y verificar si es un objeto requerido
    public void AddItemToCauldron(Pickable pickable)
    {
        // Asumimos que el Pickable tiene una propiedad 'Name' que corresponde al nombre del objeto
        string itemName = pickable.gameObject.name;

        // Verificar si el item está en la lista de items requeridos (itemsInCauldron)
        bool isRequiredItem = false;
        ItemsCatalog.ItemsData matchedItem = null;

        foreach (var item in itemsInCauldron)
        {
            Debug.Log($"{item.Item.name} == {itemName}");
            if (item.Item.name + "(Clone)" == itemName) // Comparar el nombre del objeto con los requeridos
            {
                isRequiredItem = true;
                matchedItem = item;
                break;
            }
        }

        if (isRequiredItem && !addedItems.Contains(matchedItem))
        {
            // Agregar el item a la lista de items agregados
            addedItems.Add(matchedItem);
            Debug.Log($"El objeto {itemName} es un objeto requerido y ha sido agregado al caldero.");
            ItemAdded?.Play();
            Bubbles?.Play();
            ItemAddedEffect?.Play();

            // Actualizar el contador en el TMP_Text
            UpdateItemsCounterText();

            // Verificar si se han agregado todos los items requeridos
            if (addedItems.Count == itemsInCauldron.Count)
            {
                ReciepeCompleted?.Play();
                Debug.Log("¡Has ganado! Todos los objetos requeridos han sido agregados al caldero.");
                completedEffect?.Play();
                StartCoroutine(ShowVictoryScreenAfterDelay());
            }
        }
        else if (isRequiredItem)
        {
            Debug.Log($"El objeto {itemName} ya fue agregado previamente.");
        }
        else
        {
            Debug.Log($"El objeto {itemName} no es un objeto requerido para esta poción.");
        }
    }

    // Método para actualizar el TMP_Text con la cantidad de objetos agregados vs requeridos
    private void UpdateItemsCounterText()
    {
        if (itemsCounterText != null && itemsInCauldron != null)
        {
            // Mostrar "0/n" desde el principio, aunque no se hayan agregado objetos
            itemsCounterText.text = $"{addedItems.Count}/{itemsInCauldron.Count}";
        }
    }

    // Método para otros comportamientos del caldero
    public void BrewPotion()
    {
        Debug.Log("Preparando la poción con los items del caldero...");
        // Aquí puedes definir la lógica para preparar la poción
    }
}
