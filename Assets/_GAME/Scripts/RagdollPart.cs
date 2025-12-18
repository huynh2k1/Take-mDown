using System;
using UnityEngine;

public class RagdollPart : MonoBehaviour
{
    public Rigidbody rb;
    public Action<Collider, RagdollPart> onTriggerEnter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other, this);
    }
}
