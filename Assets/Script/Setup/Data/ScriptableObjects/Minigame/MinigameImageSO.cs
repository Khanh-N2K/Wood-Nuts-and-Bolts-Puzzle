using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Images", menuName = "ScriptableObjects/Minigame Images", order = 1)]
public class MinigameImageSO: ScriptableObject
{
    public List<Texture2D> list;
}