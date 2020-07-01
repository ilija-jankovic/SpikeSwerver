using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spike : FallingEntity
{
    [SerializeField]
    protected byte value = 1;
    private bool valueGiven;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!valueGiven && GameManager.Player && transform.position.y <= GameManager.Platform.transform.position.y)
        {
            GameManager.AddPoints(value);
            valueGiven = true;
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (GameManager.Player && other.gameObject == GameManager.Player.gameObject)
        {
            GameManager.Player.Hit();
            Destroy(gameObject);
        }
    }
}
