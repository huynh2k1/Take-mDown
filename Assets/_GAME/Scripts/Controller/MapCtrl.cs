using UnityEngine;

public class MapCtrl : MonoBehaviour
{
    [SerializeField] GameObject[] _maps;

    GameObject _curMap;

    public void RandomMap()
    {
        if(_curMap != null)
        {
            _curMap.SetActive(false);
            _curMap = null;
        }

        int result = Random.Range(0, _maps.Length);
        _curMap = _maps[result];
        _curMap.SetActive(true);
    }
}
