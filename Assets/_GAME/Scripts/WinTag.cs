using System;
using UnityEngine;

public class WinTag : BaseObjectMove
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameTag.DEADZONE))
        {
            GameController.I.WinGame();
            Destroy(gameObject);
        }
    }
}
