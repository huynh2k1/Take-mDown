using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    public override UIType Type => UIType.GAME;

    [SerializeField] Button _btnPause;
    [SerializeField] TMP_Text _txtLevel;
    [SerializeField] GameObject _tutorial;
    [SerializeField] Image _warning;

    public static Action OnPauseClicked;

    private void Awake()
    {
        _btnPause.onClick.AddListener(OnClickPause);

        GameController.OnHeartReduce += Warning;
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtLevel();
    }

    void OnClickPause()
    {
        OnPauseClicked?.Invoke();
    }

    void UpdateTxtLevel()
    {
        _txtLevel.text = $"LEVEL {PrefData.CurLevel + 1}";
    }

    public void ShowTut(bool isShow)
    {
        _tutorial.SetActive(isShow);
    }

    public void Warning()
    {
        _warning.DOKill();
        Handheld.Vibrate();
        _warning.DOFade(1f, 0.2f).SetLoops(2).OnComplete(() =>
        {
            _warning.DOFade(0, 0f);
        });
    }
}
