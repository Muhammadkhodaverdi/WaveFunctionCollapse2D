using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSOList", menuName = "ScriptableObject/TileSOList")]
public class TileListSO : ScriptableObject
{
    public List<TileSO> tileSOList;
}
