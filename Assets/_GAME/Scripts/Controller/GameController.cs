using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    public State CurState {  get; private set; }

    [Header("Refenrences")]
    [SerializeField] UIController uiCtrl;
    [SerializeField] LevelCtrl levelCtrl;
    [SerializeField] PlayerController playerCtrl; 
    [SerializeField] PanelHeart panelHeart;

    public enum State { WAIT, PLAYING }


    private void Awake()
    {
        I = this;
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        InitialGame();
    }

    private void OnEnable()
    {
        UIHome.OnPlayClicked += ShowSelectLevel;
        UIHome.OnHowToPlayClicked += Setting;

        UIGame.OnPauseClicked += PauseGame;  

        UILose.OnHomeClicked += BackToHome;
        UILose.OnReplayClicked += ReplayGame;

        UIWin.OnHomeClicked += BackToHome;
        UIWin.OnReplayClicked += PreviousGame;
        UIWin.OnNextClicked += NextGame;

        UIPause.OnHomeClicked += BackToHome;
        UIPause.OnResumeClicked += ResumeGame;

        PlayerController.OnPlayerDead += LoseGame;
        panelHeart.OnOutOfHeartAction += LoseGame;

        UISelectLevel.OnSelectLevelAction += StartGame;
    }

    void InitialGame()
    {
        CurState = State.WAIT;
        SoundCtrl.I.PlayMusic();
        uiCtrl.ShowHome();
    }

    void BackToHome()
    {
        CurState = State.WAIT;
        SoundCtrl.I.PlayMusic();
        uiCtrl.SwitchUI(UIType.GAME, UIType.HOME);
    }

    void StartGame(int id)
    {
        uiCtrl.TransitionFX(() =>
        {
            uiCtrl.Hide(UIType.SELECTLEVEL);
            uiCtrl.Hide(UIType.HOME);
            uiCtrl.Show(UIType.GAME);
        });
        SetupGame(id);
    }

    void SetupGame(int id)
    {
        CurState = State.PLAYING;
        levelCtrl.InitLevelByID(id);
        playerCtrl.Init();
        panelHeart.Init();
        SoundCtrl.I.PlayMusic();
    }

    public void WinGame()
    {
        if (CurState != State.PLAYING)
            return;

        SoundCtrl.I.StopMusic();
        SoundCtrl.I.PlaySFXByType(TypeSFX.WIN);
        CurState = State.WAIT;
        levelCtrl.OnLevelWin();
        uiCtrl.OnLevelWin();
    }

    public void LoseGame()
    {
        if (CurState != State.PLAYING)
            return;
        SoundCtrl.I.StopMusic();
        SoundCtrl.I.PlaySFXByType(TypeSFX.LOSE);
        CurState = State.WAIT;
        uiCtrl.Show(UIType.LOSE);
        uiCtrl.OnLevelLose();
    }

    void PauseGame()
    {
        CurState = State.WAIT;
        uiCtrl.Show(UIType.PAUSE);
    }

    void ResumeGame()
    {
        CurState = State.PLAYING;
    }

    void ReplayGame()
    {
        uiCtrl.TransitionFX(() =>
        {
            uiCtrl.Show(UIType.GAME);
            SetupGame(PrefData.CurLevel);
        });
    }

    void PreviousGame()
    {
        uiCtrl.TransitionFX(() =>
        {
            uiCtrl.Show(UIType.GAME);
            levelCtrl.OnLevelReplay();
            SetupGame(PrefData.CurLevel);
        });
    }

    void NextGame()
    {
        uiCtrl.TransitionFX(() =>
        {
            uiCtrl.Show(UIType.GAME);
            SetupGame(PrefData.CurLevel);
        });
    }
    void Setting()
    {
        uiCtrl.Show(UIType.SETTING);
    }

    public void ReduceHeart()
    {
        panelHeart.ReduceHeart();
    }


    public void ShowSelectLevel()
    {
        uiCtrl.Show(UIType.SELECTLEVEL);
    }

    public int GetCurHeartRemaining() => panelHeart.GetHeartRemaining();
}
