using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class ColourTransition : MonoBehaviour
{
    protected Material mat;

    protected Color normal;
    [SerializeField]
    protected Color otherColour;

    [SerializeField]
    protected float transSpeed;
    protected float tTrans;

    protected virtual void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        normal = mat.color;
    }

    void FixedUpdate()
    {
        if (tTrans > 0f)
        {
            tTrans = Mathf.Clamp(tTrans - transSpeed, 0f, 1f);
            mat.color = Color.Lerp(normal, otherColour, tTrans);
        }
    }
}
