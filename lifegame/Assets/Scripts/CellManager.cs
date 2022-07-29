using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup))]
public class CellManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Cell _cellPrefab;

    [SerializeField]
    private uint _row;

    [SerializeField]
    private uint _column;

    private GridLayoutGroup _gridLayoutGroup;
    private Cell[,] _cells;

    private void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        _gridLayoutGroup.cellSize = new Vector2(15, 15);
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = (int)_column;
        _cells = SetUpCells(_cellPrefab, (int)_row, (int)_column);
    }

    private Cell[,] SetUpCells(Cell cellPrefab, int row, int column)
    {
        Cell[,] cells = new Cell[row, column];
        for(int r = 0; r < row; r++)
        {
            for(int c = 0; c < column; c++)
            {
                var cell = Instantiate(cellPrefab, this.transform);
                cell.name = $"{r}, {c}";
                cells[r, c] = cell;
            }
        }

        return cells;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            ChangeCellState(cell);
        }
    }

    private Vector2Int GetCellIndex(Cell[,] cells, Cell cell)
    {
        var row = cells.GetLength(0);
        var column = cells.GetLength(1);
        for(int r = 0; r < row; r++)
        {
            for(int c = 0; c < column; c++)
            {
                if (cells[r, c] == cell)
                {
                    return new Vector2Int(r, c);
                }
            }
        }

        Debug.LogWarning("—v‘f‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        return new Vector2Int(-1, -1);
    }

    private void ChangeCellState(Cell cell)
    {
        cell.IsAlive = !cell.IsAlive;
        InformLiving(cell);
    }

    private void InformLiving(Cell cell)
    {
        var index = GetCellIndex(_cells, cell);
        if (index.x == -1) return;
        for(int r = -1; r <= 1; r++)
        {
            if (index.x + r < 0 || index.x + r > _row - 1) continue;

            for(int c = -1; c <= 1; c++)
            {
                if(index.y + c < 0 || index.y + c > _column - 1 || (r == 0 && c == 0)) continue;
                _cells[index.x + r, index.y + c].AroundLivingCell += cell.IsAlive ? 1 : -1;
            }
        }
    }

    public void AdvanceNextGeneration()
    {
        foreach(var cell in _cells)
        {
            cell.IsAliveNext = cell.DetermineAliveNext();
        }

        foreach(var cell in _cells)
        {
            bool wasAlive = cell.IsAlive;
            cell.IsAlive = cell.IsAliveNext;
            if (wasAlive != cell.IsAliveNext)
            {
                InformLiving(cell);
            }
        }
    }
}
