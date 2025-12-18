using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] Level[] _listLevel;

    Level _curLevel;

    public void InitLevelByID(int id)
    {
        DestroyCurLevel();
        if (_listLevel.Length == 0 || _listLevel[id] == null)
            return;
        _curLevel = Instantiate(_listLevel[id], transform);
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
            GameData.CurLevel++; 
        else
            GameData.CurLevel = 0;
    }
}
