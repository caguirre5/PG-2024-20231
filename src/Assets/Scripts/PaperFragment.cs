using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PaperFragment : MonoBehaviour
{
    [SerializeField] private Canvas keycapCanvas;
    [SerializeField] private Canvas fragmentCanvas;

    [SerializeField] private TMP_Text recipeText;
    [SerializeField] private AudioSource PaperSound;

    private HUD hud;

    void Start()
    {
        hud = FindObjectOfType<HUD>();
        keycapCanvas.enabled = false;
        fragmentCanvas.enabled = false;
    }

    public void ShowKeycap(bool value)
    {
        keycapCanvas.enabled = value;
        if (hud != null)
        {
            if (value == true)
            {
                hud.ShowKeyCapE();
            }
            else
            {
                hud.HideKeyCapE();
            }
        }
    }

    public void ShowFragment()
    {
        fragmentCanvas.enabled = true;
        PaperSound?.Play();
    }

    public void HideFragment()
    {
        fragmentCanvas.enabled = false;
    }

    public void SetRecipeText(string recipe)
    {
        if (recipeText != null)
        {
            recipeText.text = recipe;
        }
    }
}
