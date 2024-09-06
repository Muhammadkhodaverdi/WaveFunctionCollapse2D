using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public event EventHandler<OnCollapseEventArgs> OnCollapse;
    public class OnCollapseEventArgs : EventArgs
    {
        public Tile_Type tileType;
    }

    public event EventHandler<OnOptionTileListChangeEventArgs> OnOptionTileListChange;
    public class OnOptionTileListChangeEventArgs : EventArgs
    {
        public List<TileSO> tileList;
    }

    public int x;
    public int y;
    public bool isCollapsed;
    public List<TileSO> optionTileList;
    public CellVisual cellVisual;

    public void Initialize(int x, int y, bool isCollapsed, List<TileSO> initialOptionTileList)
    {
        cellVisual.Initialize();

        this.x = x;
        this.y = y;
        this.isCollapsed = isCollapsed;
        this.optionTileList = new List<TileSO>(initialOptionTileList);
        OnOptionTileListChange?.Invoke(this, new OnOptionTileListChangeEventArgs { tileList = optionTileList });
    }

    public void UpdateOptionList(List<TileSO> newOptionTileList)
    {
        optionTileList = new List<TileSO>();
        optionTileList = newOptionTileList;
        OnOptionTileListChange?.Invoke(this, new OnOptionTileListChangeEventArgs { tileList = optionTileList });
    }

    public void Collapse(TileSO selectedTile)
    {
        isCollapsed = true;
        optionTileList = new List<TileSO> { selectedTile };
        OnCollapse?.Invoke(this, new OnCollapseEventArgs { tileType = optionTileList[0].type });
    }

    public int GetEntropy()
    {
        return optionTileList.Count;
    }
}
