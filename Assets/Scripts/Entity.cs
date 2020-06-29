using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (transform.position.y < GameManager.MinHeight)
            Destroy(gameObject);
    }
}
