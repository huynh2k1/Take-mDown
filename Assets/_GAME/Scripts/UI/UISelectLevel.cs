using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISelectLevel : BasePopup
{
    public override UIType Type => UIType.SELECTLEVEL;

    [Header("Pages")]
    [SerializeField] RectTransform[] _pages;
    [SerializeField] float _pageWidth = 2500f;
    [SerializeField] float _tweenTime = 0.35f;

    [Header("Buttons")]
    [SerializeField] Button _btnPrev;
    [SerializeField] Button _btnNext;

    private LevelButton[] _levelButtons;
    private int _currentPage = 0;
    private bool _isTweening;

    public static Action<int> OnSelectLevelAction;

    protected override void Awake()
    {
        base.Awake();

        _levelButtons = transform.GetComponentsInChildren<LevelButton>(true);
        foreach (var b in _levelButtons)
            b.OnClickThis += OnSelectLevel;

        _btnPrev.onClick.AddListener(PrevPage);
        _btnNext.onClick.AddListener(NextPage);
    }

    private void Start()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
            _levelButtons[i].Init(i);

        UpdateButtonState();
    }

    #region Page Logic

    void NextPage()
    {
        if (_isTweening || _currentPage >= _pages.Length - 1) return;
        MoveToPage(_currentPage + 1);
    }

    void PrevPage()
    {
        if (_isTweening || _currentPage <= 0) return;
        MoveToPage(_currentPage - 1);
    }

    void MoveToPage(int targetPage)
    {
        _isTweening = true;
        _currentPage = targetPage;

        for (int i = 0; i < _pages.Length; i++)
        {
            float targetX = (i - _currentPage) * _pageWidth;

            _pages[i]
                .DOAnchorPosX(targetX, _tweenTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _isTweening = false);
        }

        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        _btnPrev.gameObject.SetActive(_currentPage > 0);
        _btnNext.gameObject.SetActive(_currentPage < _pages.Length - 1);
    }

    #endregion

    #region Level Select

    void OnSelectLevel(int id)
    {
        Hide();
        OnSelectLevelAction?.Invoke(id);
    }

    private void OnDestroy()
    {
        foreach (var b in _levelButtons)
            b.OnClickThis -= OnSelectLevel;
    }

    #endregion
}
