using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollectable : Entity
{
    public byte type;
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!Collected)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, Time.deltaTime * 300f);
            return;
        }
        switch (type)
        {
            case (byte)Collectable.Types.points:
                Vector3 pos = transform.position;
                Vector3 pointsTextPos = GameManager.PointsText.transform.position;

                //move towards points text
                transform.position = Vector3.MoveTowards(pos, pointsTextPos, 0.2f);

                //check if close enough to points text
                if (Vector3.Distance(pos, pointsTextPos) < 0.1f)
                {
                    Destroy(gameObject);
                    //add points if player still alive
                    if (GameManager.Player)
                        GameManager.AddPoints(1);
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void Collect()
    {
        Destroy(GetComponent<BoxCollider>());
        transform.SetParent(null);
    }

    bool Collected
    {
        get { return transform.parent == null; }
    }
}
