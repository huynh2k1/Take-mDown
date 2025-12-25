using UnityEngine;

public class HeartUI : MonoBehaviour
{
    [SerializeField] GameObject _active;

    public void Active()
    {
        _active.SetActive(true);
    }

    public void InActive()
    {
        _active.SetActive(false);
    }
}
