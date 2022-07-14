using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    [SerializeField]
    private Text _time;

    private float _startTime;
    private void Awake()
    {
        _startTime = Time.time;

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        var playTime = Time.time - _startTime;
        _time.text = $"Time  {(int)playTime / 60 : 00}:{(int)playTime % 60 : 00}";
    }
}
