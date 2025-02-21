using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject KeyCapE;
    [SerializeField] private GameObject KeyCapQ;
    [SerializeField] private GameObject pauseMenu; // Referencia al menú de pausa
    [SerializeField] private GameObject offIcon;
    [SerializeField] private TextMeshProUGUI timerText; // Texto para mostrar el tiempo
    [SerializeField] private float totalTime = 600;
    [SerializeField] private AudioSource backgroundMusic; // Referencia al AudioSource que reproduce la música de fondo

    private float timeRemaining;
    private bool isPaused = false; // Controla el estado de pausa del juego

    void Start()
    {
        KeyCapE.SetActive(false);
        KeyCapQ.SetActive(false);
        pauseMenu.SetActive(false); // Asegurarse de que el menú de pausa esté desactivado al inicio
        timeRemaining = totalTime; // Inicializa el tiempo restante con el total
        UpdateTimerDisplay();
        offIcon.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Usar Escape para activar/desactivar la pausa
        {
            TogglePause();
        }

        if (!isPaused) // Solo actualizar el temporizador si el juego no está en pausa
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Reduce el tiempo restante
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                // Lógica cuando el tiempo se acaba
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowKeyCapE()
    {
        KeyCapE.SetActive(true);
    }

    public void HideKeyCapE()
    {
        KeyCapE.SetActive(false);
    }

    public void ShowKeyCapQ()
    {
        KeyCapQ.SetActive(true);
    }

    public void HideKeyCapQ()
    {
        KeyCapQ.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Cambiar el estado de pausa
        pauseMenu.SetActive(isPaused); // Activar o desactivar el menú de pausa
        Time.timeScale = isPaused ? 0 : 1; // Detener o reanudar el tiempo del juego
    }

    public void ToggleMute()
    {
        // Mutear o desmutear la música cambiando el estado de mute del AudioSource
        backgroundMusic.mute = !backgroundMusic.mute;
        if (backgroundMusic.mute)
        {
            offIcon.SetActive(true); 
        }
        else
        {
            offIcon.SetActive(false); 
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Cargar una nueva escena por nombre
    }
}
