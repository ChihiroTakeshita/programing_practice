using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum MineCounter
{
    None = 0, // ��
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,

    Mine = -1, // �n��
}

public enum CellState
{
    Close,
    Open,
    Flag
}

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Text _view = null;

    [SerializeField]
    private Image _cellImage;

    [SerializeField, DisableInInspector]
    private MineCounter _mineCounter = MineCounter.None;

    private CellState _state = CellState.Close;
    private int _indexR;
    private int _indexC;

    public MineCounter MineCounter
    {
        get => _mineCounter;
        set
        {
            _mineCounter = value;
            OnStateChanged();
        }
    }

    public CellState State 
    { 
        get => _state; 
        set
        {
            _state = value;
            OnStateChanged();
        }
    }

    public int IndexR { get => _indexR; set => _indexR = value; }
    public int IndexC { get => _indexC; set => _indexC = value; }

    private void Start()
    {
        _cellImage = GetComponent<Image>();
        OnStateChanged();
    }

    private void OnValidate()
    {
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        if (_view == null) return;
        switch(State)
        {
            case CellState.Close:
                _view.text = "";
                _cellImage.color = Color.cyan;
                break;
            case CellState.Open:
                _cellImage.color = Color.white;
                switch (_mineCounter)
                {
                    case MineCounter.None:
                        _view.text = "";
                        break;
                    case MineCounter.Mine:
                        _view.color = Color.red;
                        _view.text = "x";
                        break;
                    default:
                        _view.color = Color.blue;
                        _view.text = ((int)_mineCounter).ToString();
                        break;
                }
                break;
            case CellState.Flag:
                _view.color = Color.black;
                _view.text = "F";
                _cellImage.color = Color.cyan;
                break;
            default:
                break;
        }
    }

    //public bool OnClick(PointerEventData.InputButton button)
    //{
    //    bool isMine = false;
    //    switch (State)
    //    {
    //        case CellState.Close:
    //            if (button == PointerEventData.InputButton.Left)
    //            {
    //                State = CellState.Open;
    //                if(MineCounter == MineCounter.Mine)
    //                {
    //                    isMine = true;
    //                }
    //            }
    //            else if (button == PointerEventData.InputButton.Right)
    //            {
    //                State = CellState.Flag;
    //            }
    //            break;
    //        case CellState.Flag:
    //            if (button == PointerEventData.InputButton.Right)
    //            {
    //                State = CellState.Close;
    //            }
    //            break;
    //        case CellState.Open:
    //            break;
    //        default:
    //            break;
    //    }
    //    return isMine;
    //}
}
