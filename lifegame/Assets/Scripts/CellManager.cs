using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CellManager : MonoBehaviour
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
}
