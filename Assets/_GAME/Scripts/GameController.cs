using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    public State CurState {  get; private set; }

    [Header("Refenrences")]
    [SerializeField] UIController uiCtrl;
    [SerializeField] LevelCtrl levelCtrl;
    //[SerializeField] SpawnController spawnCtrl;
    [SerializeField] PlayerController playerCtrl; 

    public enum State { WAIT, PLAYING }


    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        InitialGame();
    }

    private void OnEnable()
    {
        UIHome.OnReplayClicked += StartGame;
        UIHome.OnHowToPlayClicked += Setting;

        UIGame.OnPauseClicked += PauseGame;  

        UILose.OnHomeClicked += BackToHome;
        UILose.OnReplayClicked += ReplayGame;

        UIWin.OnHomeClicked += BackToHome;
        UIWin.OnReplayClicked += ReplayGame;
        UIWin.OnNextClicked += NextGame;

        UIPause.OnHomeClicked += BackToHome;
        UIPause.OnResumeClicked += ResumeGame;

        PlayerController.OnPlayerDead += LoseGame;
    }

    void InitialGame()
    {
        Application.targetFrameRate = 120;

        CurState = State.WAIT;
        uiCtrl.ShowHome();
    }

    void BackToHome()
    {
        CurState = State.WAIT;
        uiCtrl.SwitchUI(UIType.GAME, UIType.HOME);
    }

    void StartGame()
    {
        uiCtrl.SwitchUI(UIType.HOME, UIType.GAME);
        SetupGame();
    }

    void SetupGame()
    {
        CurState = State.PLAYING;
        levelCtrl.InitLevelByID(GameData.CurLevel);
        //spawnCtrl.StartSpawn();
        playerCtrl.Init();
    }

    public void WinGame()
    {
        CurState = State.WAIT;
        uiCtrl.Show(UIType.WIN);
        levelCtrl.OnLevelWin();
    }

    public void LoseGame()
    {
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
            if(GameData.CurLevel > 0)
            {
                GameData.CurLevel--;
            }
            else
            {
                GameData.CurLevel = 0;
            }
            SetupGame();
        });
    }

    void NextGame()
    {
        uiCtrl.TransitionFX(() =>
        {
            SetupGame();
        });
    }
    void Setting()
    {
        uiCtrl.Show(UIType.SETTING);
    }
}
