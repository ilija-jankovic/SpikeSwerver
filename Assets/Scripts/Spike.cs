using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spike : Entity
{
    [SerializeField]
    private float /*speed,*/ _acceleration = 0.02f;
    [SerializeField]
    protected byte value = 1;
    protected override void Start()
    {
        base.Start();
        gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(0,-Acceleration,0),ForceMode.Acceleration);
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().convex = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (FindObjectOfType<Player>() && (collision.gameObject == GameObject.Find("Platform") || collision.gameObject.GetComponent<Player>() != null))
            GameManager.AddPoints(value);
        CollisionEffects();
        Destroy(gameObject);
    }

    protected virtual void CollisionEffects()
    {

    }

    protected float Acceleration
    {
        get { return _acceleration * (Random.value + 0.5f); }
    }
}
