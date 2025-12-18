using System;
using UnityEngine;
using UnityEngine.UI;

public class UILose : BasePopup
{
    public override UIType Type => UIType.LOSE;

    [SerializeField] Button btnHome;
    [SerializeField] Button btnReplay;

    public static Action OnHomeClicked;
    public static Action OnReplayClicked;

    protected override void Awake()
    {
        base.Awake();
        btnHome.onClick.AddListener(OnClickHome);
        btnReplay.onClick.AddListener(OnClickReplay);   
    }

    void OnClickHome()
    {
        Hide(() => OnHomeClicked?.Invoke());
        
    }

    void OnClickReplay()
    {
        Hide(() => OnReplayClicked?.Invoke());
    }
}

