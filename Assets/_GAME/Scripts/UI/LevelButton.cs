using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int ID;
    [SerializeField] Button _btn;
    [SerializeField] Image _bgButton;
    [SerializeField] GameObject _lockObj;
    [SerializeField] TMP_Text _txtLevel;

    [SerializeField] Sprite _bgLock, _bgUnlock;

    public event Action<int> OnClickThis;

    private void Awake()
    {
        _btn.onClick.AddListener(() => OnClickThis?.Invoke(ID));
    }

    private void OnEnable()
    {
        CheckUnlock();
    }

    public void Init(int id)
    {
        ID = id;
        UpdateTextLevel(ID);
        CheckUnlock();
    }

    void CheckUnlock()
    {
        if(ID <= PrefData.LevelUnlocked)
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }

    void Lock()
    {
        _btn.interactable = false;
        _btn.targetGraphic.raycastTarget = false;

        _lockObj.SetActive(true);
        ShowTextLevel(false);
        UpdateBGButton(false);
    }

    void Unlock()
    {
        _btn.interactable = true;
        _btn.targetGraphic.raycastTarget = true;

        _lockObj.SetActive(false);
        ShowTextLevel(true);
        UpdateBGButton(true);
    }

    void ShowTextLevel(bool isShow)
    {
        _txtLevel.gameObject.SetActive(isShow);
    }

    void UpdateTextLevel(int id)
    {
        _txtLevel.text = (id + 1).ToString();
    }

    void UpdateBGButton(bool isUnlocked)
    {
        if (isUnlocked)
        {
            _bgButton.sprite = _bgUnlock;
        }else
        {
            _bgButton.sprite = _bgLock;
        }
    }
}
