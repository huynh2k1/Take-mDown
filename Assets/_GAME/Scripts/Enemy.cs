using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : BaseObjectMove
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
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Animator _animator;
    Rigidbody[] _rbRagdoll;
    Collider[] _colliders;

    public event Action<Enemy> OnEnemyTriggerDestroyArea;

    public float pushForce = 300f; // tùy chỉnh

    bool _isDead;

    public bool IsDead => _isDead;

    [Button("Setup Ragdoll")]
    public void SetupRagdoll()
    {
        _rbRagdoll = transform.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        foreach (var r in _rbRagdoll)
        {
            var old = r.transform.GetComponents<RagdollPart>();
            foreach(var o in old)
            {
                DestroyImmediate(o); // dùng trong Editor / Setup
            }

            r.transform.AddComponent<RagdollPart>();
        }

        foreach (var c in _colliders)
        {
            c.isTrigger = true;
        }
    }

 
    private void Awake()
    {
        Init();
    }
    void Init()
    {
        SetupRagdoll();
        var parts = GetComponentsInChildren<RagdollPart>();

        foreach (var part in parts)
        {
            part.onTriggerEnter += (other, part)=> OnRagdollTrigger(other, part);
        }

        DisableRagdoll();
    }

    void Dead()
    {
        //_animator.enabled = false;
        _animator.SetBool("FallForward", true);
        //EnableRagdoll();
        _isDead = true;               
    }

    public void EnableRagdoll()
    {
        foreach (var c in _colliders)
        {
            c.isTrigger = false;
        }
        foreach (Rigidbody rb in _rbRagdoll)
        {
            rb.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Rigidbody rb in _rbRagdoll)
        {
            rb.isKinematic = true;
        }
    }
    private void OnRagdollTrigger(Collider other, RagdollPart part)
    {
        if (other.CompareTag(GameTag.DEADZONE))
        {
            OnEnemyTriggerDestroyArea?.Invoke(this);
            Destroy(gameObject);
        }

        if (other.CompareTag(GameTag.PLAYER) && !_isDead)
        {
            _isDead = true;
            _audioSource.Play();


            if (part.transform.position.y < 1f)
            {
                Dead(); // Bật ragdoll
            }
            else
            {
                _animator.SetBool("SweepFall", true);
                _isDead = true;
            }

        }
    }
}
