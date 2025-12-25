using System;
using UnityEngine;

public class PanelHeart : MonoBehaviour
{
    [SerializeField] HeartUI[] _heartUI;
    const int MAX_HEART = 3;
    int _curHeart;

    public event Action OnOutOfHeartAction;

    public void Init()
    {
        _curHeart = MAX_HEART;
        foreach(HeartUI h in _heartUI)
        {
            h.Active();
        }
    }

    public void ReduceHeart()
    {
        if (_curHeart <= 0)
            return;
        _curHeart--;
        UpdateHeartRemaing(_curHeart);
        if(_curHeart <= 0)
        {
            OnOutOfHeartAction?.Invoke();
        }
            
    }

    public void UpdateHeartRemaing(int remaining)
    {
        for(int i = remaining; i < _heartUI.Length; i++)
        {
            _heartUI[i].InActive();
        }
    }

    public int GetHeartRemaining() => _curHeart;
}
