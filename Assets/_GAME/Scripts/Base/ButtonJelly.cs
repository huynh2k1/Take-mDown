using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonJelly : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Button _btn;

    [Header("Jelly Settings")]
    public float pressedScale = 0.85f;
    public float pressDuration = 0.12f;
    public float releaseDuration = 0.5f;

    [Header("Anti Spam")]
    public float clickDelay = 0.2f;
    public bool canClick = true;

    RectTransform rect;
    Vector3 originalScale;
    Tween tween;
    Tween delayTween;   // <-- tween thay cho coroutine

    void Awake()
    {
        _btn = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;

        _btn.onClick.AddListener(() =>
        {
            SoundCtrl.I.PlaySFXByType(TypeSFX.CLICK);
            StartDelay();
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick) return;

        tween?.Kill();

        rect.DOScale(originalScale * pressedScale, pressDuration)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canClick) return;

        tween?.Kill();

        tween = rect.DOScale(originalScale, releaseDuration)
            .SetEase(Ease.OutElastic, 1.2f, 0.5f);
    }

    void StartDelay()
    {
        canClick = false;
        if (_btn) _btn.interactable = false;

        delayTween?.Kill();
        // ❗ Thay coroutine bằng DOTween delay
        delayTween = DOVirtual.DelayedCall(clickDelay, () =>
        {
            canClick = true;
            if (_btn) _btn.interactable = true;
        }).SetAutoKill(true);
    }
}
