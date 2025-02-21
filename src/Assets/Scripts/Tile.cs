using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isVisited { get; private set; }

    public void Visit()
    {
        isVisited = true;
        if (transform.childCount > 0)
        {
            // Desactivar el primer hijo (si solo tienes uno, este sería el indicado)
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
