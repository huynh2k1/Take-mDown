using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class BaseUICtrl : MonoBehaviour
{
    public BaseUI[] _arrUI;
    protected Dictionary<UIType, BaseUI> _uis = new Dictionary<UIType, BaseUI>();

    [Header("UI FADE TRANSITITION")]
    [SerializeField] protected Image _mask;
    [SerializeField] float _fadeDuration;

    [Button("Load All UI")]
    public void LoadAllUI()
    {
        _arrUI = GetComponentsInChildren<BaseUI>(true); 
    }

    protected virtual void Awake()
    {
        foreach (var ui in _arrUI)
        {
            _uis[ui.Type] = ui;
        }
    }

    public virtual void Show(UIType type)
    {
        if (!_uis.ContainsKey(type))
        {
            return;
        }
        _uis[type].Show();
    }

    public virtual void Hide(UIType type)
    {
        if (!_uis.ContainsKey(type))
        {
            return;
        }
        _uis[type].Hide();
    }

    public void TransitionFX(Action actionDone = default)
    {
        FadeOut(() =>
        {
            actionDone?.Invoke();
            FadeIn();
        });
    }

    public void SwitchUI(UIType fromUI, UIType toUI)
    {
        FadeOut(() =>
        {
            Hide(fromUI);
            Show(toUI);
            FadeIn();
        });
    }

    public void FadeOut(Action onComplete = null)
    {
        _mask.raycastTarget = true;
        _mask.DOFade(1, _fadeDuration)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void FadeIn(Action onComplete = null)
    {
        _mask.DOFade(0, _fadeDuration)
            .OnComplete(() =>
            {
                _mask.raycastTarget = false;
                onComplete?.Invoke();
            });
    }
}

public enum UIType
{
    HOME,
    GAME,
    HOWTOPLAY,
    SELECTLEVEL,
    SETTING,
    PAUSE,
    WIN,
    LOSE,
}
