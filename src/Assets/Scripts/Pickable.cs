using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private Canvas keycapCanvas;

    private HUD hud;

    void Start()
    {
        hud = FindObjectOfType<HUD>();
        keycapCanvas.enabled = false;
    }

    public void ShowKeycap(bool value)
    {
        keycapCanvas.enabled = value;
        if (hud != null)
        {
            if (value == true)
            {
                hud.ShowKeyCapQ();
            }
            else
            {
                hud.HideKeyCapQ();
            }
        }
    }
}
