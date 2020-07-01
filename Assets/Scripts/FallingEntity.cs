using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FallingEntity : Entity
{
    [SerializeField]
    private float _acceleration = 0.02f;

    protected override void Start()
    {
        base.Start();
        _acceleration *= Random.value + 0.5f;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, -Acceleration, 0), ForceMode.Acceleration);
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        gameObject.AddComponent<BoxCollider>().isTrigger = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected float Acceleration
    {
        get { return _acceleration; }
    }
}
