using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
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
