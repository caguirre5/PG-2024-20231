using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Inventory/NPC")]
public class NPCPartsCatalog : ScriptableObject
{
    [System.Serializable]
    public class HeadData
    {
        public Mesh head;
        public string genre;
        public string description;
    }
    [System.Serializable]
    public class BodyData
    {
        public Mesh meshes;
        public string description;
    }
    [System.Serializable]
    public class ExtData
    {
        public Mesh extLeft;
        public Mesh extRight;
        public string description;
    }

    public List<Material> materials;

    public List<HeadData> heads;
    public List<BodyData> bodies;
    public List<ExtData> arms;
    public List<ExtData> legs;
}
