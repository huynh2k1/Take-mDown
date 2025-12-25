using UnityEngine;

public static class PrefData
{
    public const string CUR_LEVEL = "CUR_LEVEL";
    public const string LEVEL_UNLOCKED = "LEVEL_UNLOCKED";
    public const string COIN = "COIN";
    public const string FIRSTPLAYGAME = "FIRSTPLAYGAME";

    public const string SOUND = "SOUND";
    public const string MUSIC = "MUSIC";

    public static int CurLevel
    {
        get => PlayerPrefs.GetInt(CUR_LEVEL, 0);
        set => PlayerPrefs.SetInt(CUR_LEVEL, value);
    }

    public static int LevelUnlocked
    {
        get => PlayerPrefs.GetInt(LEVEL_UNLOCKED, 0);
        set => PlayerPrefs.SetInt(LEVEL_UNLOCKED, value);
    }

    public static int Coin
    {
        get => PlayerPrefs.GetInt(COIN, 0);
        set => PlayerPrefs.SetInt(COIN, value);
    }

    public static float Sound
    {
        get => PlayerPrefs.GetFloat(SOUND, 1);
        set => PlayerPrefs.SetFloat(SOUND, value);
    }

    public static float Music
    {
        get => PlayerPrefs.GetFloat(MUSIC, 1);
        set => PlayerPrefs.SetFloat(MUSIC, value);
    }

    public static bool FirstPlayGame
    {
        get => PlayerPrefs.GetInt(FIRSTPLAYGAME, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(FIRSTPLAYGAME, value ? 0 : 1);
    }
}
