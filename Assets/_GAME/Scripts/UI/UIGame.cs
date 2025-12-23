using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    public override UIType Type => UIType.GAME;

    [SerializeField] Button _btnPause;
    [SerializeField] TMP_Text _txtLevel;
    [SerializeField] GameObject _tutorial;

    public static Action OnPauseClicked;

    private void Awake()
    {
        _btnPause.onClick.AddListener(OnClickPause);
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtLevel();
        //if (GameData.FirstPlayGame)
        //{
        //    GameData.FirstPlayGame = false;
        //    ShowTut(true);
        //}
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
}
