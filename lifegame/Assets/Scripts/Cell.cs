using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [SerializeField, Tooltip("�����Ă���Ƃ��̐F")]
    private Color32 _aliveColor;

    [SerializeField, Tooltip("����ł���Ƃ��̐F")]
    private Color32 _deadColor;

    private Image _image;
    private bool _isAlive = false;

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            _image.color = OnChageState(_isAlive);
        }
    }

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
}