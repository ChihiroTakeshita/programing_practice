using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class Minesweeper : MonoBehaviour
{
    [SerializeField]
    private int _rows = 10;

    [SerializeField]
    private int _columns = 10;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;

    [SerializeField]
    private Cell _cellPrefab = null;

    [SerializeField]
    private int _mineCount = 10;

    private Cell[,] _cells;

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _cells = new Cell[_rows, _columns];
        for(var r = 0; r < _rows; r++)
        {
            for(var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.name = $"({r}, {c})";
                _cells[r, c] = cell;
            }
        }

        for(var i = 0; i < _mineCount; i++)
        {
            SetUpMine();
        }
    }

    private void OnValidate()
    {
        if(_mineCount > (_rows * _columns) - 1)
        {
            _mineCount = (_rows * _columns) - 1;
        }
    }

    private void SetUpMine()
    {
        var row = Random.Range(0, _rows);
        var column = Random.Range(0, _columns);

        if (_cells[row,column].CellState == CellState.Mine)
        {
            SetUpMine();
        }
        else
        {
            _cells[row, column].CellState = CellState.Mine;

            for (var r = -1; r <= 1; r++)
            {
                var a = row + r;
                if (a < 0 || a > _rows - 1) continue;

                for (var c = -1; c <= 1; c++)
                {
                    var b = column + c;
                    if (b < 0 || b > _columns - 1) continue;

                    if ((_cells[a, b].CellState == CellState.Mine) || (r == 0 && c == 0))
                    {
                        continue;
                    }

                    _cells[a, b].CellState++;
                }
            }
        }
    }
}
