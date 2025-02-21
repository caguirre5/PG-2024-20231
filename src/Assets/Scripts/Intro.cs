using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NextScene"; // Nombre de la escena a cargar

    void Update()
    {
        // Verifica si se ha presionado cualquier tecla
        if (Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    // Método para cargar la escena
    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
                    // Muestra un mensaje en la consola si está en el Editor
                    Debug.Log("Saliendo del juego (solo funciona en compilación).");
#else
        // Cierra la aplicación en una compilación
        Application.Quit();
#endif
    }
}
