using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int ID;
    [SerializeField] Button _btn;
    [SerializeField] Image _bgButton;
    [SerializeField] GameObject _lockObj;
    public event Action<int> OnClickThis;

    private void Awake()
    {
        _btn.onClick.AddListener(() => OnClickThis?.Invoke(ID));
    }

    public void Init(int id)
    {
        ID = id;
    }

    void CheckUnlock()
    {

    }

    void Lock()
    {

    }

    void Unlock()
    {

    }
}
