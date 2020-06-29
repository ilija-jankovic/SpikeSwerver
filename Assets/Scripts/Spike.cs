using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spike : Entity
{
    [SerializeField]
    private float _acceleration = 0.02f;
    [SerializeField]
    protected byte value = 1;
    private bool valueGiven; 
    protected override void Start()
    {
        base.Start();
        gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(0,-Acceleration,0),ForceMode.Acceleration);
        gameObject.AddComponent<BoxCollider>().isTrigger = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!valueGiven && GameManager.Player && transform.position.y <= GameManager.Platform.transform.position.y)
        {
            GameManager.AddPoints(value);
            valueGiven = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Player && other.gameObject == GameManager.Player.gameObject)
        {
            GameManager.Player.Hit();
            Destroy(gameObject);
        }
    }

    protected float Acceleration
    {
        get { return _acceleration * (Random.value + 0.5f); }
    }
}
