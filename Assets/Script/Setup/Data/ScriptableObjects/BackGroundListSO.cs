using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Background List", menuName = "ScriptableObjects/BackGround List", order = 1)]
public class BackGroundListSO : ScriptableObject
{
    public List<Sprite> backGroundList;
}