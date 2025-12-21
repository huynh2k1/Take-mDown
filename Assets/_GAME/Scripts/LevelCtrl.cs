using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] Level[] _listLevel;

    Level _curLevel;

    public void InitLevelByID()
    {
        DestroyCurLevel();

        _curLevel = Instantiate(_listLevel[GameData.CurLevel], transform);
    }

    void DestroyCurLevel()
    {
        if (_curLevel == null)
            return;
        Destroy(_curLevel.gameObject);
    }

    public void OnLevelWin()
    {
        if(GameData.CurLevel < _listLevel.Length - 1)
        {
            GameData.CurLevel++; 
        }
        else
        {
            GameData.CurLevel = 0;
        }

        Debug.Log($"CurLevel: {GameData.CurLevel}");
    }

    public void OnLevelReplay()
    {
        if (GameData.CurLevel > 0)
        {
            GameData.CurLevel--;
        }
        else
        {
            GameData.CurLevel = 0;
        }
    }
}
