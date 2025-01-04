using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bolt Skin List", menuName = "ScriptableObjects/Bolt Skin List", order = 1)]
public class BoltSkinListSO : ScriptableObject
{
    public List<BoltSkin> list;
}

[System.Serializable]
public struct BoltSkin
{
    public Sprite defaultSkin;
    public Sprite fullSkin;
}