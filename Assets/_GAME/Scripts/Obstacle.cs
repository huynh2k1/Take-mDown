using UnityEngine;

public class Obstacle : BaseObjectMove
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyArea"))
        {
            Destroy(gameObject);
        }
    }
}
