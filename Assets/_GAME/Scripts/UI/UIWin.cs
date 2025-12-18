using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : BasePopup
{
    public override UIType Type => UIType.WIN;

    [SerializeField] Button btnHome;
    [SerializeField] Button btnReplay;
    [SerializeField] Button btnNext;

    public static Action OnHomeClicked;
    public static Action OnReplayClicked;
    public static Action OnNextClicked;

    protected override void Awake()
    {
        base.Awake();
        btnHome.onClick.AddListener(OnClickHome);
        btnReplay.onClick.AddListener(OnClickReplay);
        btnNext.onClick.AddListener(OnClickNext);
    }

    void OnClickHome()
    {
        Hide(() =>
        {
            OnHomeClicked?.Invoke();
        });
    }

    void OnClickReplay()
    {
        Hide(() =>
        {
            OnReplayClicked?.Invoke();
        });
    }

    void OnClickNext()
    {
        Hide(() =>
        {
            OnNextClicked?.Invoke();
        });
    }
}
