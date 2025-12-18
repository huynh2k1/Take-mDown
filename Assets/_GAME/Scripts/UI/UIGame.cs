using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    public override UIType Type => UIType.GAME;

    [SerializeField] Button _btnPause;

    public static Action OnPauseClicked;

    private void Awake()
    {
        _btnPause.onClick.AddListener(OnClickPause);
    }

    void OnClickPause()
    {
        OnPauseClicked?.Invoke();
    }
}
