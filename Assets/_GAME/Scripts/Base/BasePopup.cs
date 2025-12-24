using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : BaseUI
{
    public override UIType Type => throw new System.NotImplementedException();

    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Image mask;
    [SerializeField] protected GameObject main;
    [SerializeField] protected Button btnClose;

    [Header("Tween Setup")]
    public float timeTween = 0.3f;

    [Button("Show Popup")]
    public void ShowPopup()
    {
        main.SetActive(true);
        canvasGroup.alpha = 1;
    }

    [Button("Setup")]
    public void Setup()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if(mask == null)
        {
            mask = GetComponentsInChildren<Image>(true).FirstOrDefault(img => img.name == "Mask");
        }

        if (main == null)
        {
            main = transform.Find("Main")?.gameObject;
        }

        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;

        mask.raycastTarget = false;
        main.SetActive(false);
    }

    protected virtual void Awake()
    {
        if (btnClose)
            btnClose.onClick.AddListener(OnClickClose);

        Setup();
        Prewarm();
    }

    void Prewarm()
    {
        main.SetActive(true);
        Canvas.ForceUpdateCanvases();
        main.SetActive(false);
    }

    public virtual void OnClickClose()
    {
        Hide();
    }

    public override void Show()
    {
        base.Show();
        canvasGroup.DOKill();
        mask.raycastTarget = true;
        canvasGroup.interactable = true;
        main.SetActive(true);
        canvasGroup.DOFade(1f, timeTween).From(0).SetEase(Ease.Linear);
    }

    public void Hide(Action actionDone = default)
    {
        base.Hide();
        canvasGroup.DOKill();
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0f, timeTween).From(1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            main.SetActive(false);
            mask.raycastTarget = false;
            actionDone?.Invoke();
        });
    }
}
