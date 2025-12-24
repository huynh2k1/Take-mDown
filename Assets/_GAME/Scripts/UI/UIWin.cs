using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : BasePopup
{
    public override UIType Type => UIType.WIN;

    [SerializeField] Button btnHome;
    [SerializeField] Button btnReplay;
    [SerializeField] Button btnNext;

    [SerializeField] GameObject[] _stars;

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

    public void UpdateStarUIs(int starCount)
    {
        DeActiveAllStart();
        for(int i = 0; i < starCount; i++)
        {
            _stars[i].SetActive(true);
        }
    }

    void DeActiveAllStart()
    {
        foreach(var star in _stars)
        {
            star.SetActive(false);
        }
    }
}
