using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None = 0, // ‹ó
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,

    Mine = -1, // ’n—‹
}

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Text _view = null;

    [SerializeField]
    private CellState _cellState = CellState.None;

    public CellState CellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged();
        }
    }

    private void Start()
    {
        OnCellStateChanged();
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (_view == null) return;

        switch (_cellState)
        {
            case CellState.None:
                _view.text = "";
                break;
            case CellState.Mine:
                _view.color = Color.red;
                _view.text = "x";
                break;
            default:
                _view.color = Color.blue;
                _view.text = ((int)_cellState).ToString();
                break;
        }
    }
}
