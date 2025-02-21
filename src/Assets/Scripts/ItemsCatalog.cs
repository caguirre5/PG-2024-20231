using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCatalog", menuName = "Inventory/Items")]
public class ItemsCatalog : ScriptableObject
{
    [System.Serializable]
    public class ItemsData
    {
        public string Name;
        public GameObject Item;
    } 

    public List<ItemsData> items;
}
