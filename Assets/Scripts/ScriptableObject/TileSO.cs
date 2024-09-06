using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "ScriptableObject/TileSO")]
public class TileSO : ScriptableObject
{
    public Tile_Type type;

    [Header("Sockets")]
    public int PX = 0;
    public int NX = 0;
    public int PY = 0;
    public int NY = 0;
}
