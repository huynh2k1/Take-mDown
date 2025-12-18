using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : BasePopup
{
    public override UIType Type => UIType.PAUSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnResume;

    public static Action OnHomeClicked;
    public static Action OnResumeClicked;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnResume.onClick.AddListener(OnClickResume);
    }

    void OnClickHome()
    {
        Hide(() => OnHomeClicked?.Invoke());

    }

    void OnClickResume()
    {
        Hide(() => OnResumeClicked?.Invoke());
    }

}
