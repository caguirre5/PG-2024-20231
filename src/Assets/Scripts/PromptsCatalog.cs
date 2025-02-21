using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCatalog", menuName = "Inventory/Prompts")]
public class PromptsCatalog : ScriptableObject
{
    [TextArea(3, 10)]
    public List<string> prompts;
}
