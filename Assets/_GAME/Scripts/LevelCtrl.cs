using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] Level[] _listLevel;

    Level _curLevel;

    public void InitLevelByID(int id)
    {
        DestroyCurLevel();
        PrefData.CurLevel = id;
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
        if(PrefData.CurLevel < _listLevel.Length - 1)
        {
            PrefData.CurLevel++; 
        }
        else
        {
            PrefData.CurLevel = 0;
        }

        Debug.Log($"CurLevel: {PrefData.CurLevel}");
    }

    public void OnLevelReplay()
    {
        if (PrefData.CurLevel > 0)
        {
            PrefData.CurLevel--;
        }
        else
        {
            PrefData.CurLevel = 0;
        }
    }
}
