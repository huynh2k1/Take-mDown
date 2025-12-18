using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : BaseUI
{
    public override UIType Type => UIType.HOME;

    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnHowtoplay;

    public static Action OnReplayClicked;
    public static Action OnHowToPlayClicked;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(OnClickPlay);
        _btnHowtoplay.onClick.AddListener(OnClickHowToPlay);
    }

    void OnClickPlay()
    {
        OnReplayClicked?.Invoke();    
    }

    void OnClickHowToPlay()
    {
        OnHowToPlayClicked?.Invoke();
    }
}
