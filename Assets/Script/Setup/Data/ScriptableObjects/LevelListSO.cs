using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/LevelList", order = 1)]
public class LevelListSO : ScriptableObject
{
    public List<GameObject> levelList;
}