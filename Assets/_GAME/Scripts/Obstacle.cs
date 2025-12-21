using NaughtyAttributes;
using UnityEngine;

public class Obstacle : BaseObjectMove
{
    #region EDITOR
    [SerializeField] float _horizontalDistance = 0.7f;
    [SerializeField] float _verticalDistance = 7f;

    [Button("Move Left")]
    public void MoveLeft() => Move(Vector3.left * _horizontalDistance);

    [Button("Move Right")]
    public void MoveRight() => Move(Vector3.right * _horizontalDistance);

    [Button("Move Up")]
    public void MoveUp() => Move(Vector3.forward * _verticalDistance);

    [Button("Move Down")]
    public void MoveDown() => Move(Vector3.back * _verticalDistance);

    private void Move(Vector3 offset)
    {
        transform.position += offset;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyArea"))
        {
            Destroy(gameObject);
        }
    }
}
