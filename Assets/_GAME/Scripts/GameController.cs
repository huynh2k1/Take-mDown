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
        Application.targetFrameRate = 120;
        Home();
    }

    private void OnEnable()
    {
        UIHome.OnReplayClicked += StartGame;
        UIHome.OnHowToPlayClicked += Setting;

        UIGame.OnPauseClicked += PauseGame;  

        UILose.OnHomeClicked += Home;
        UILose.OnReplayClicked += StartGame;

        UIWin.OnHomeClicked += Home;
        UIWin.OnReplayClicked += StartGame;
        UIWin.OnNextClicked += StartGame;

        UIPause.OnHomeClicked += Home;
        UIPause.OnResumeClicked += ResumeGame;

        PlayerController.OnPlayerDead += LoseGame;
    }

    void Home()
    {
        CurState = State.WAIT;
        uiCtrl.ShowHome();
    }

    void StartGame()
    {
        CurState = State.PLAYING;
        uiCtrl.ShowGame();
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
        if(GameData.CurLevel > 0)
        {
            GameData.CurLevel--;
        }
        else
        {
            GameData.CurLevel = 0;
        }
        StartGame();
    }

    void Setting()
    {
        uiCtrl.Show(UIType.SETTING);
    }
}
