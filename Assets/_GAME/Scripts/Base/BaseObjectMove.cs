using UnityEngine;

public class BaseObjectMove : MonoBehaviour
{
    public float speed = 2f;

    protected virtual void Update()
    {
        if (GameController.I.CurState != GameController.State.PLAYING)
            return;
        Move();
    }

    public virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
