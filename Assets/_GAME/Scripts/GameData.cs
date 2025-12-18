using UnityEngine;

public class GameData : MonoBehaviour
{
    public const string CUR_LEVEL = "CUR_LEVEL";
    public const string COIN = "COIN";

    public const string SOUND = "SOUND";
    public const string MUSIC = "MUSIC";

    public static int CurLevel
    {
        get => PlayerPrefs.GetInt(CUR_LEVEL, 0);
        set => PlayerPrefs.SetInt(CUR_LEVEL, value);
    }

    public static int Coin
    {
        get => PlayerPrefs.GetInt(COIN, 0);
        set => PlayerPrefs.SetInt(COIN, value);
    }

    public static bool Sound
    {
        get => PlayerPrefs.GetInt(SOUND, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(SOUND, value ? 0 : 1);
    }

    public static bool Music
    {
        get => PlayerPrefs.GetInt(MUSIC, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(MUSIC, value ? 0 : 1);
    }
}
