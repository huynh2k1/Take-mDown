using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BaseUICtrl : MonoBehaviour
{
    public BaseUI[] _arrUI;
    protected Dictionary<UIType, BaseUI> _uis = new Dictionary<UIType, BaseUI>();

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
