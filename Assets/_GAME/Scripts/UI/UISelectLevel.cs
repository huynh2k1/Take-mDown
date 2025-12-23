using System;
using UnityEngine;

public class UISelectLevel : BasePopup
{
    public override UIType Type => UIType.SELECTLEVEL;

    private LevelButton[] _levelButtons;

    public static Action<int> OnSelectLevelAction;

    protected override void Awake()
    {
        base.Awake();
        _levelButtons = transform.GetComponentsInChildren<LevelButton>(true);

        foreach(var b in _levelButtons)
        {
            b.OnClickThis += OnSelectLevel;
        }
    }

    private void Start()
    {
        for(int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].Init(i);
        }
    }

    void OnSelectLevel(int id)
    {
        Hide();
        OnSelectLevelAction?.Invoke(id);
    }
}
