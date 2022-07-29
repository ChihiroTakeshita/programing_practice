using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [SerializeField, Tooltip("¶‚«‚Ä‚¢‚é‚Æ‚«‚ÌF")]
    private Color32 _aliveColor;

    [SerializeField, Tooltip("Ž€‚ñ‚Å‚¢‚é‚Æ‚«‚ÌF")]
    private Color32 _deadColor;

    private Image _image;
    private bool _isAlive = false;
    private bool _isAliveNext = false;
    private int _aroundLivingCell;

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            _image.color = OnChageState(_isAlive);
        }
    }

    public int AroundLivingCell { get => _aroundLivingCell; set => _aroundLivingCell = value; }
    public bool IsAliveNext { get => _isAliveNext; set => _isAliveNext = value; }

    private void Start()
    {
        _image = GetComponent<Image>();
        IsAlive = false;
    }

    private Color32 OnChageState(bool isAlive)
    {
        Color32 color;

        if(isAlive)
        {
            color = _aliveColor;
        }
        else
        {
            color= _deadColor;
        }

        return color;
    }

    public bool DetermineAliveNext()
    {
        bool aliveNext = false;

        if(_isAlive)
        {
            if(_aroundLivingCell > 1 && _aroundLivingCell < 4)
            {
                aliveNext = true;
            }
        }
        else
        {
            if (_aroundLivingCell == 3)
            {
                aliveNext = true;
            }
        }

        return aliveNext;
    }
}
