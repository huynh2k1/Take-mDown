using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    public State CurState {  get; set; }

    [Header("Refenrences")]
    [SerializeField] UIController uiCtrl;
    [SerializeField] LevelCtrl levelCtrl;
    [SerializeField] PlayerController playerCtrl; 
    [SerializeField] PanelHeart panelHeart;
    [SerializeField] MapCtrl mapCtrl;
    [SerializeField] Tutorial tutorial;
    public enum State { WAIT, PLAYING }


    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
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
        uiCtrl.ShowHome();
    }

    void BackToHome()
    {
        CurState = State.WAIT;
        uiCtrl.SwitchUI(UIType.GAME, UIType.HOME);
    }

    void StartGame(int id)
    {
        uiCtrl.SwitchUI(UIType.HOME, UIType.GAME);
        SetupGame(id);
    }

    void SetupGame(int id)
    {
        levelCtrl.InitLevelByID(id);
        playerCtrl.Init();
        panelHeart.Init();
        mapCtrl.RandomMap();
        if(PrefData.CurLevel < 1)
        {
            tutorial.Show();
            return;
        }
        CurState = State.PLAYING;
    }

    public void WinGame()
    {
        if (CurState != State.PLAYING)
            return;
        SoundCtrl.I.PlaySFXByType(TypeSFX.WIN);
        CurState = State.WAIT;
        levelCtrl.OnLevelWin();
        uiCtrl.Show(UIType.WIN);
    }

    public void LoseGame()
    {
        SoundCtrl.I.PlaySFXByType(TypeSFX.LOSE);
        CurState = State.WAIT;
        uiCtrl.Show(UIType.LOSE);
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
