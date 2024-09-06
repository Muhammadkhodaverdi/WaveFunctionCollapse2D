using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellVisual : MonoBehaviour
{
    [SerializeField] private Cell cell;
    [SerializeField] private List<TileVisual> optionTileVisualList;
    [SerializeField] private List<TileVisual> CollapsedTileVisualList;
    [SerializeField] private Outline outline;

    public void Initialize()
    {
        outline.enabled = true;

        cell.OnCollapse -= Cell_OnCollapse;
        cell.OnOptionTileListChange -= Cell_OnOptionTileListChange;

        cell.OnCollapse += Cell_OnCollapse;
        cell.OnOptionTileListChange += Cell_OnOptionTileListChange;

        DeActiveCollapsedTileVisual();
    }
    private void Cell_OnCollapse(object sender, Cell.OnCollapseEventArgs e)
    {
        outline.enabled = false;
        UpdateCollapsedTileVisual(e.tileType);
    }

    private void UpdateCollapsedTileVisual(Tile_Type tileType)
    {
        foreach (TileVisual tileVisual in CollapsedTileVisualList)
        {
            if (tileVisual.type == tileType)
            {
                tileVisual.gameObject.SetActive(true);
            }
        }
    }

    private void DeActiveCollapsedTileVisual()
    {
        foreach (TileVisual tileVisual in CollapsedTileVisualList)
        {
            tileVisual.gameObject.SetActive(false);
        }
    }

    private void Cell_OnOptionTileListChange(object sender, Cell.OnOptionTileListChangeEventArgs e)
    {
        List<Tile_Type> optionTileTypeList = GetOptionTileTypeList(e.tileList);
        UpdateOptionTileVisual(optionTileTypeList);
    }
    private void UpdateOptionTileVisual(List<Tile_Type> optionTileTypeList)
    {
        foreach (TileVisual tileVisual in optionTileVisualList)
        {
            if (optionTileTypeList.Contains(tileVisual.type))
            {
                tileVisual.gameObject.SetActive(true);
            }
            else
            {
                tileVisual.gameObject.SetActive(false);
            }
        }
    }

    private List<Tile_Type> GetOptionTileTypeList(List<TileSO> optionTileList)
    {
        List<Tile_Type> optionTileTypeList = new List<Tile_Type>();
        foreach (TileSO tile in optionTileList)
        {
            optionTileTypeList.Add(tile.type);
        }
        return optionTileTypeList;
    }
}
