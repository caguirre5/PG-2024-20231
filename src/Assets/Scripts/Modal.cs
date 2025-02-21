using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public GameObject panel; // Referencia al panel que quieres mostrar/ocultar

    void Start()
    {
    }

    // Función para ocultar el panel
    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // Función para mostrar el panel
    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }
}
