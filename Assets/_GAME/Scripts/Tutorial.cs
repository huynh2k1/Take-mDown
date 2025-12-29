using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    public bool IsShowing { get; set; } = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && IsShowing)
        {
            Hide();
        }
    }

    public void Show()
    {
        IsShowing = true;
        _canvasGroup.interactable = false;
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, 0.5f).From(0).SetUpdate(true).OnComplete(() =>
        {
            _canvasGroup.interactable = true;
        });
    }

    public void Hide()
    {
        IsShowing = false;
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            GameController.I.CurState = GameController.State.PLAYING;

        });
    }
}
