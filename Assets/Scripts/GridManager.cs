using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Button solveButton;
    [SerializeField] private Button resetButton;

    [SerializeField] private Transform cellTransform;
    [SerializeField] private TileListSO tileListSO;

    [SerializeField] private int width = 9;
    [SerializeField] private int height = 9;

    [SerializeField] private int solveSpeed = 0;

    private Cell[,] cellArray;

    private int collapseCount;

    private void Start()
    {
        CreateGrid();

        SpeedSliderUI.Instance.OnSliderValueChanage += SpeedSliderUI_OnSliderValueChanage;

        solveButton.onClick.AddListener(() =>
        {
            InitializeGrid();
            FindCellWithLowestEntropy();
        });

        resetButton.onClick.AddListener(() =>
        {
            InitializeGrid();
        });
    }

    private void SpeedSliderUI_OnSliderValueChanage(object sender, SpeedSliderUI.OnSliderValueChanageEventArgs e)
    {
        solveSpeed = (int)((1 - e.value) * 1000);
    }

    private void InitializeGrid()
    {
        collapseCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cellArray[x, y].Initialize(x, y, false, tileListSO.tileSOList);
            }
        }
    }

    private void CreateGrid()
    {
        cellArray = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform cellTransform = Instantiate(this.cellTransform, transform);
                cellArray[x, y] = cellTransform.GetComponent<Cell>();
            }
        }
        InitializeGrid();
    }

    private void FindCellWithLowestEntropy()
    {
        if (collapseCount >= GetGridSize())
        {
            return;
        }

        List<Cell> lowestEntropyCellList = new List<Cell>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!cellArray[x, y].isCollapsed)
                {
                    lowestEntropyCellList.Add(cellArray[x, y]);
                }
            }
        }
        lowestEntropyCellList.Sort((a, b) => { return a.GetEntropy() - b.GetEntropy(); });

        int lowestEntropy = lowestEntropyCellList[0].GetEntropy();
        int stopIndex = 0;
        for (int i = 1; i < lowestEntropyCellList.Count; i++)
        {
            if (lowestEntropyCellList[i].GetEntropy() > lowestEntropy)
            {
                stopIndex = i;
                break;
            }
        }

        if (stopIndex > 0)
        {
            lowestEntropyCellList.RemoveRange(stopIndex, lowestEntropyCellList.Count - stopIndex);
        }

        CollapseCell(lowestEntropyCellList);
    }

    private void CollapseCell(List<Cell> lowestEntropyCellList)
    {
        collapseCount++;

        int randomIndex = Random.Range(0, lowestEntropyCellList.Count);

        Cell randomCell = lowestEntropyCellList[randomIndex];

        randomIndex = Random.Range(0, randomCell.optionTileList.Count);

        TileSO randomTile = randomCell.optionTileList[randomIndex];

        randomCell.Collapse(randomTile);

        UpdateGrid(randomCell);
    }

    private async void UpdateGrid(Cell collapsedCell)
    {
        Vector2Int colapsedCellCoordinates = new Vector2Int(collapsedCell.x, collapsedCell.y);
        TileSO colapsedCellTile = collapsedCell.optionTileList[0];

        //Update Up Cell

        Vector2Int upCellCoordinates = new Vector2Int(colapsedCellCoordinates.x, colapsedCellCoordinates.y + 1);

        if (ValidateCoordinates(upCellCoordinates))
        {
            Cell upCell = cellArray[upCellCoordinates.x, upCellCoordinates.y];
            List<TileSO> optionTileList = upCell.optionTileList;
            List<TileSO> newOptionTileList = new List<TileSO>();
            foreach (TileSO optionTile in optionTileList)
            {
                if ((optionTile.NY == colapsedCellTile.PY))
                {
                    newOptionTileList.Add(optionTile);
                }
            }
            upCell.UpdateOptionList(newOptionTileList);
        }

        //Update Right Cell

        Vector2Int rightCellCoordinates = new Vector2Int(colapsedCellCoordinates.x + 1, colapsedCellCoordinates.y);

        if (ValidateCoordinates(rightCellCoordinates))
        {
            Cell rightCell = cellArray[rightCellCoordinates.x, rightCellCoordinates.y];
            List<TileSO> optionTileList = rightCell.optionTileList;
            List<TileSO> newOptionTileList = new List<TileSO>();
            foreach (TileSO optionTile in optionTileList)
            {
                if ((optionTile.NX == colapsedCellTile.PX))
                {
                    newOptionTileList.Add(optionTile);
                }
            }
            rightCell.UpdateOptionList(newOptionTileList);
        }

        //Update Down Cell

        Vector2Int downCellCoordinates = new Vector2Int(colapsedCellCoordinates.x, colapsedCellCoordinates.y - 1);

        if (ValidateCoordinates(downCellCoordinates))
        {
            Cell downCell = cellArray[downCellCoordinates.x, downCellCoordinates.y];
            List<TileSO> optionTileList = downCell.optionTileList;
            List<TileSO> newOptionTileList = new List<TileSO>();
            foreach (TileSO optionTile in optionTileList)
            {
                if ((optionTile.PY == colapsedCellTile.NY))
                {
                    newOptionTileList.Add(optionTile);
                }
            }
            downCell.UpdateOptionList(newOptionTileList);
        }

        //Update Left Cell

        Vector2Int leftCellCoordinates = new Vector2Int(colapsedCellCoordinates.x - 1, colapsedCellCoordinates.y);

        if (ValidateCoordinates(leftCellCoordinates))
        {
            Cell leftCell = cellArray[leftCellCoordinates.x, leftCellCoordinates.y];
            List<TileSO> optionTileList = leftCell.optionTileList;
            List<TileSO> newOptionTileList = new List<TileSO>();
            foreach (TileSO optionTile in optionTileList)
            {
                if ((optionTile.PX == colapsedCellTile.NX))
                {
                    newOptionTileList.Add(optionTile);
                }
            }
            leftCell.UpdateOptionList(newOptionTileList);
        }


        await Task.Delay(solveSpeed);

        //Find Lowest Entropy to Collapse
        FindCellWithLowestEntropy();

    }
    private bool ValidateCoordinates(Vector2Int coordinates)
    {
        if (coordinates.x >= 0 && coordinates.x < width && coordinates.y >= 0 && coordinates.y < height) return true;
        return false;
    }

    private int GetGridSize()
    {
        return width * height;
    }
}


