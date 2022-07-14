using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup))]
public class Minesweeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;

    [SerializeField]
    private GameObject _gameClearPanel;

    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private Cell _cellPrefab = null;

    [SerializeField]
    private int _rows = 10;

    [SerializeField]
    private int _columns = 10;

    [SerializeField, Range(0, 50)]
    private int _percentageOfMine;

    private int _mineCount;
    private Cell[,] _cells;
    private bool _isFirst = true;

    private void Start()
    {
        _mineCount = Mathf.FloorToInt(_rows * _columns * _percentageOfMine * 0.01f);
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

        //var mineCount = Mathf.Min(_mineCount, _cells.Length);

        //for(var i = 0; i < mineCount; i++)
        //{
        //    SetUpMine();
        //}
    }

    private void OnValidate()
    {
        // 最低でも1マスは地雷が置かれないマスにする
        if(_mineCount > (_rows * _columns) - 1)
        {
            _mineCount = (_rows * _columns) - 1;
        }
    }

    private void SetUpMine()
    {
        // ランダムで座標を指定する
        var row = Random.Range(0, _rows);
        var column = Random.Range(0, _columns);
        
        if (_cells[row,column].MineCounter == MineCounter.Mine || _cells[row, column].State == CellState.Open) // もしすでに地雷が設置されていたか開かれているならやり直す
        {
            SetUpMine();
        }
        else
        {
            _cells[row, column].MineCounter = MineCounter.Mine;

            // 周囲8マスのCellStateの数字を一つ上げる
            for (var r = -1; r <= 1; r++)
            {
                var a = row + r;
                if (a < 0 || a > _rows - 1) continue;

                for (var c = -1; c <= 1; c++)
                {
                    var b = column + c;
                    if (b < 0 || b > _columns - 1) continue;

                    if ((_cells[a, b].MineCounter == MineCounter.Mine) || (r == 0 && c == 0))
                    {
                        continue;
                    }

                    _cells[a, b].MineCounter++;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<Cell>();
        if(cell)
        {
            bool isMine = cell.OnClick(eventData.button);

            if(_isFirst)
            {
                _isFirst = false;
                var mineCount = Mathf.Min(_mineCount, _cells.Length);

                for (var i = 0; i < mineCount; i++)
                {
                    SetUpMine();
                }
            }

            CheckGameFinish(isMine);
        }
    }

    private void CheckGameFinish(bool isMine)
    {
        if(isMine)
        {
            GameOver();
        }
        else
        {
            foreach(var c in _cells)
            {
                if(c.MineCounter != MineCounter.Mine && (c.State == CellState.Close || c.State == CellState.Flag))
                {
                    return;
                }
            }
            GameClear();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        _gameOverPanel.SetActive(true);
    }

    private void GameClear()
    {
        _gameClearPanel.SetActive(true);
    }
}
