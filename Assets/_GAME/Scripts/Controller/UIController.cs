using UnityEngine;

public class UIController : BaseUICtrl
{
    public void ShowHome()
    {
        Show(UIType.HOME);
        Hide(UIType.GAME);
    }

    public void ShowGame()
    {
        Show(UIType.GAME);
        Hide(UIType.HOME);
    }

}
