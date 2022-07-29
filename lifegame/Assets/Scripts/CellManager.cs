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

    [SerializeField, RuntimeDisable]
    private uint _row;

    [SerializeField, RuntimeDisable]
    private uint _column;

    [SerializeField]
    private float _autoInterval;

    [SerializeField, Range(0, 100)]
    private float _percentageOfLiving;

    private GridLayoutGroup _gridLayoutGroup;
    private Cell[,] _cells;
    private IEnumerator _autoCoroutine = null;

    private void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
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

        Debug.LogWarning("óvëfÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÇ≈ÇµÇΩ");
        return new Vector2Int(-1, -1);
    }

    private void ChangeCellState(Cell cell)
    {
        cell.IsAlive = !cell.IsAlive;
        InformLiving(cell);
    }

    private void ChangeCellState(Cell cell, bool isAlive)
    {
        bool wasAlive = cell.IsAlive;
        cell.IsAlive = isAlive;
        if(wasAlive != isAlive)
        {
            InformLiving(cell);
        }
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

    private void AdvanceNextGeneration()
    {
        foreach(var cell in _cells)
        {
            cell.IsAliveNext = cell.DetermineAliveNext();
        }

        foreach(var cell in _cells)
        {
            ChangeCellState(cell, cell.IsAliveNext);
            //bool wasAlive = cell.IsAlive;
            //cell.IsAlive = cell.IsAliveNext;
            //if (wasAlive != cell.IsAliveNext)
            //{
            //    InformLiving(cell);
            //}
        }
    }

    public void OnClickStart()
    {
        if(_autoCoroutine == null)
        {
            _autoCoroutine = AutoAdvance(_autoInterval);
            StartCoroutine(_autoCoroutine);
        }
    }

    public void OnClickStop()
    {
        StopCoroutine(_autoCoroutine);
        _autoCoroutine = null;
    }

    public void OnClickNext()
    {
        if(_autoCoroutine == null)
        {
            AdvanceNextGeneration();
        }
    }
    public void OnClickRandom()
    {
        KillAllCells();
        SetRandom(_percentageOfLiving);
    }

    public void OnClickClear()
    {
        KillAllCells();
    }

    private IEnumerator AutoAdvance(float interval)
    {
        while(true)
        {
            AdvanceNextGeneration();
            yield return new WaitForSeconds(interval);
        }
    }

    private void SetRandom(float percentageOfLiving)
    {
        if(percentageOfLiving < 0 || percentageOfLiving > 100)
        {
            Debug.LogError("ïsê≥Ç»ílÇ≈Ç∑");
            return;
        }

        bool isAlive;
        int livingCount;
        if(percentageOfLiving < 50)
        {
            isAlive = true;
            livingCount = Mathf.FloorToInt((_row * _column) * (percentageOfLiving / 100));
        }
        else
        {
            isAlive = false;
            livingCount = Mathf.FloorToInt((_row * _column) * ((100 - percentageOfLiving) / 100));
            foreach (var cell in _cells)
            {
                ChangeCellState(cell, true);
            }
        }

        for(int i = 0; i < livingCount; i++)
        {
            while(true)
            {
                int r = Random.Range(0, (int)_row);
                int c = Random.Range(0, (int)_column);
                if (_cells[r, c].IsAlive != isAlive)
                {
                    ChangeCellState(_cells[r, c], isAlive);
                    break;
                }
            }
        }
    }

    private void KillAllCells()
    {
        foreach(var cell in _cells)
        {
            ChangeCellState(cell, false);
        }
    }
}
