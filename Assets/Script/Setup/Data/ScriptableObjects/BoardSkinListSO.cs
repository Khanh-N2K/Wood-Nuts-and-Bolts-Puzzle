using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Skin List", menuName = "ScriptableObjects/Board Skin List", order = 1)]
public class BoardSkinListSO : ScriptableObject
{
    public List<Sprite> list;
}