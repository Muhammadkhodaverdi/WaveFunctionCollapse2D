using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    public Tile_Type type;
}
public enum Tile_Type
{
    Blank,
    DownLeft,
    DownRight,
    Horizontal,
    Plus,
    UpLeft,
    UpRight,
    Vertical
}
