using UnityEngine;

public class UIController : BaseUICtrl
{
    public void ShowHome()
    {
        Show(UIType.HOME);
        Hide(UIType.GAME);
    }

    public void OnLevelWin()
    {
        Hide(UIType.GAME);
        Show(UIType.WIN);
    }

    public void OnLevelLose()
    {
        Hide(UIType.GAME);
        Show(UIType.LOSE);
    }
}
