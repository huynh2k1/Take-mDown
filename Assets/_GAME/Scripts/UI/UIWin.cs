using DG.Tweening;
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

    public override void Show()
    {
        base.Show();
        UpdateStarUIs(GameController.I.GetCurHeartRemaining());
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
        DeActiveAllStar();

        float delay = 0f;
        for (int i = 0; i < starCount && i < _stars.Length; i++)
        {
            PlayStarTween(_stars[i], delay);
            delay += 0.2f;
        }
    }

    void PlayStarTween(GameObject star, float delay)
    {
        star.SetActive(true);

        RectTransform rect = star.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;

        rect.DOScale(Vector3.one, 0.4f)
            .SetDelay(delay)
            .SetEase(Ease.OutBack);
    }

    void DeActiveAllStar()
    {
        foreach (var star in _stars)
        {
            star.SetActive(false);
        }
    }
}
