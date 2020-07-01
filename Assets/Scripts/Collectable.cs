using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : FallingEntity
{
    public enum Types { points };
    [SerializeField]
    byte type;
    [SerializeField]
    byte minCollectables;
    [SerializeField]
    byte maxCollectables;
    [SerializeField]
    float cubeSize;
    [SerializeField]
    float radius;

    protected override void Start()
    {
        base.Start();
        for (byte i = 0; i < Random.Range(minCollectables, maxCollectables); i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.GetComponent<BoxCollider>().isTrigger = true;
            cube.transform.SetParent(gameObject.transform);
            cube.transform.localScale *= cubeSize;
            cube.transform.localPosition = Random.insideUnitSphere*radius;

            Material GetMaterial()
            {
                switch (type)
                {
                    case (byte)Types.points:
                        return Resources.Load("Materials/PointCollectable") as Material;
                    default:
                        return null;
                }
            }
            cube.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial();

            cube.AddComponent<CubeCollectable>().type = type;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnTriggerStay(Collider other)
    {
        if (GameManager.Player && other.gameObject == GameManager.Player.gameObject)
        {
            //enable collected behaviour in cube
            foreach (Transform cube in GetComponentInChildren<Transform>())
            {
                CubeCollectable ccComp = cube.GetComponent<CubeCollectable>();
                if (ccComp)
                {
                    ccComp.Collect();
                    //sometime compnent is randomly disabled so this is a quick fix
                    ccComp.enabled = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
