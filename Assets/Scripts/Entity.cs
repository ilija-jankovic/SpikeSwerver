using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (transform.position.y < GameManager.MinHeight)
            Destroy(gameObject);
        foreach (Transform transform in GetComponentInChildren<Transform>())
            if (transform.position.y < GameManager.MinHeight)
                Destroy(transform.gameObject);
    }
}
